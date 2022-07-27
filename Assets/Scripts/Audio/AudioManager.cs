﻿using Assets.Scripts.Audio.SoundEmitters;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[Header("SoundEmitters pool")]
	[SerializeField] private SoundEmitterPoolSO _pool = default;
	[SerializeField] private int _initialSize = 10;

	[Header("Listening on channels")]
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
	[SerializeField] private AudioCueEventChannelSO _SFXEventChannel = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
	[SerializeField] private AudioCueEventChannelSO _musicEventChannel = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
	[SerializeField] private FloatEventChannelSO _SFXVolumeEventChannel = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
	[SerializeField] private FloatEventChannelSO _musicVolumeEventChannel = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
	[SerializeField] private FloatEventChannelSO _masterVolumeEventChannel = default;


	[Header("Audio control")]
	[SerializeField] private AudioMixer _audioMixer = default;
	[Range(0f, 1f)]
	[SerializeField] private float _masterVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _musicVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _sfxVolume = 1f;

	private SoundEmitterVault _soundEmitterVault;
	private SoundEmitter _musicSoundEmitter;

	private void Awake()
	{
		//TODO: Get the initial volume levels from the settings
		_soundEmitterVault = new SoundEmitterVault();

		_pool.Prewarm(_initialSize);
		_pool.SetParent(transform);
	}

	private void OnEnable()
	{
		_SFXEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
		_SFXEventChannel.OnAudioCueStopRequested += StopAudioCue;
		_SFXEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

		_musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
		_musicEventChannel.OnAudioCueStopRequested += StopMusic;

		_masterVolumeEventChannel.OnEventRaised += ChangeMasterVolume;
		_musicVolumeEventChannel.OnEventRaised += ChangeMusicVolume;
		_SFXVolumeEventChannel.OnEventRaised += ChangeSFXVolume;

	}

	private void OnDestroy()
	{
		_SFXEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
		_SFXEventChannel.OnAudioCueStopRequested -= StopAudioCue;

		_SFXEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;
		_musicEventChannel.OnAudioCuePlayRequested -= PlayMusicTrack;

		_musicVolumeEventChannel.OnEventRaised -= ChangeMusicVolume;
		_SFXVolumeEventChannel.OnEventRaised -= ChangeSFXVolume;
		_masterVolumeEventChannel.OnEventRaised -= ChangeMasterVolume;
	}

	/// <summary>
	/// This is only used in the Editor, to debug volumes.
	/// It is called when any of the variables is changed, and will directly change the value of the volumes on the AudioMixer.
	/// </summary>
	void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        SetGroupVolume("MasterVolume", _masterVolume);
        SetGroupVolume("MusicVolume", _musicVolume);
        SetGroupVolume("SFXVolume", _sfxVolume);
    }
	void ChangeMasterVolume(float newVolume)
	{
		_masterVolume = newVolume;
		SetGroupVolume("MasterVolume", _masterVolume);
	}
	void ChangeMusicVolume(float newVolume)
	{
		_musicVolume = newVolume;
		SetGroupVolume("MusicVolume", _musicVolume);
	}
	void ChangeSFXVolume(float newVolume)
	{
		_sfxVolume = newVolume;
		SetGroupVolume("SFXVolume", _sfxVolume);
	}
	public void SetGroupVolume(string parameterName, float normalizedVolume)
	{
		var volumeSet = _audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
		if (!volumeSet)
			Debug.LogError("The AudioMixer parameter was not found");
	}

	public float GetGroupVolume(string parameterName)
	{
		if (_audioMixer.GetFloat(parameterName, out float rawVolume))
		{
			return MixerValueToNormalized(rawVolume);
		}
		else
		{
			Debug.LogError("The AudioMixer parameter was not found");
			return 0f;
		}
	}

	// Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
	/// when using UI sliders normalized format
	private float MixerValueToNormalized(float mixerValue)
	{
		// We're assuming the range [-80dB to 0dB] becomes [0 to 1]
		return 1f + (mixerValue / 80f);
	}
	private float NormalizedToMixerValue(float normalizedValue)
	{
		// We're assuming the range [0 to 1] becomes [-80dB to 0dB]
		// This doesn't allow values over 0dB
		return (normalizedValue - 1f) * 80f;
	}

	private AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
	{
        if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
        {
            var songToPlay = audioCue.GetClips()[0];
            if (_musicSoundEmitter.GetClip() == songToPlay)
                return AudioCueKey.Invalid;

			StopMusicEmitter(_musicSoundEmitter);
        }

		_musicSoundEmitter = _pool.Request();
		_musicSoundEmitter.PlayAudioClip(audioCue.GetClips()[0], audioConfiguration, true);
		_musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;

		return AudioCueKey.Invalid; //No need to return a valid key for music
	}

	private bool StopMusic(AudioCueKey key)
	{
		if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
		{
			_musicSoundEmitter.Stop();
			return true;
		}
		else
			return false;
	}

	/// <summary>
	/// Only used by the timeline to stop the gameplay music during cutscenes.
	/// Called by the SignalReceiver present on this same GameObject.
	/// </summary>
	public void TimelineInterruptsMusic()
	{
		StopMusic(AudioCueKey.Invalid);
	}

	/// <summary>
	/// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
	/// </summary>
	public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
	{
		var clipsToPlay = audioCue.GetClips();
		var soundEmitterArray = new SoundEmitter[clipsToPlay.Length];

		var nOfClips = clipsToPlay.Length;
		for (var i = 0; i < nOfClips; i++)
		{
			soundEmitterArray[i] = _pool.Request();

            if (soundEmitterArray[i] == null)
            {
                continue;
            }

            soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.Looping, position);
            if (!audioCue.Looping)
                soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
        }

		return _soundEmitterVault.Add(audioCue, soundEmitterArray);
	}

	public bool FinishAudioCue(AudioCueKey audioCueKey)
	{
		var isFound = _soundEmitterVault.Get(audioCueKey, out var soundEmitters);

		if (isFound)
        {
            foreach (var soundEmitter in soundEmitters)
            {
                soundEmitter.Finish();
                soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
            }
        }
		else
		{
			Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");
		}

		return isFound;
	}

	public bool StopAudioCue(AudioCueKey audioCueKey)
	{
		var isFound = _soundEmitterVault.Get(audioCueKey, out var soundEmitters);

        if (!isFound)
        {
            return false;
        }

        foreach (var soundEmitter in soundEmitters)
        {
            StopAndCleanEmitter(soundEmitter);
        }

        _soundEmitterVault.Remove(audioCueKey);

        return true;
	}

	private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
	{
		StopAndCleanEmitter(soundEmitter);
	}

	private void StopAndCleanEmitter(SoundEmitter soundEmitter)
	{
		if (!soundEmitter.IsLooping())
			soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;

		soundEmitter.Stop();
		_pool.Return(soundEmitter);
	}

	private void StopMusicEmitter(SoundEmitter soundEmitter)
	{
		soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
		_pool.Return(soundEmitter);
	}
}