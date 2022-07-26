﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
	private AudioSource _audioSource;

	public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

	private void Awake()
	{
		_audioSource = this.GetComponent<AudioSource>();
		_audioSource.playOnAwake = false;
	}

	/// <summary>
	/// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
	/// </summary>
	/// <param name="clip"></param>
	/// <param name="settings"></param>
	/// <param name="hasToLoop"></param>
	/// <param name="position"></param>
	public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
	{
		_audioSource.clip = clip;
		settings.ApplyTo(_audioSource);
		_audioSource.transform.position = position;
		_audioSource.loop = hasToLoop;
		_audioSource.time = 0f; //Reset in case this AudioSource is being reused for a short SFX after being used for a long music track
        _audioSource.volume = settings.Volume;
		_audioSource.Play();

		if (!hasToLoop)
		{
			StartCoroutine(FinishedPlaying(clip.length));
		}
	}

	/// <summary>
	/// Used to check which music track is being played.
	/// </summary>
	public AudioClip GetClip()
	{
		return _audioSource.clip;
	}


	/// <summary>
	/// Used when the game is unpaused, to pick up SFX from where they left.
	/// </summary>
	public void Resume()
	{
		_audioSource.Play();
	}

	/// <summary>
	/// Used when the game is paused.
	/// </summary>
	public void Pause()
	{
		_audioSource.Pause();
	}

	public void Stop()
	{
		_audioSource.Stop();
	}

	public void Finish()
    {
        if (!_audioSource.loop)
        {
            return;
        }

        _audioSource.loop = false;
        var timeRemaining = _audioSource.clip.length - _audioSource.time;
        StartCoroutine(FinishedPlaying(timeRemaining));
    }

	public bool IsPlaying()
	{
		return _audioSource.isPlaying;
	}

	public bool IsLooping()
	{
		return _audioSource.loop;
	}

    private IEnumerator FinishedPlaying(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);

		NotifyBeingDone();
	}

	private void NotifyBeingDone()
    {
        OnSoundFinishedPlaying?.Invoke(this); // The AudioManager will pick this up
    }
}
