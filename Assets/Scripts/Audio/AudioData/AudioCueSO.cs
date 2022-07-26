using UnityEngine;

/// <summary>
/// A collection of audio clips that are played in parallel, and support randomisation.
/// </summary>
[CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
public class AudioCueSO : ScriptableObject
{
	public bool Looping = false;
	[SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

	public AudioClip[] GetClips()
	{
		var numberOfClips = _audioClipGroups.Length;
		var resultingClips = new AudioClip[numberOfClips];

		for (var i = 0; i < numberOfClips; i++)
		{
			resultingClips[i] = _audioClipGroups[i].GetNextClip();
		}

		return resultingClips;
	}
}