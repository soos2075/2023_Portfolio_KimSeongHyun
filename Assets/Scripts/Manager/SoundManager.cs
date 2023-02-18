using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
	public enum SoundType
	{
		Bgm,
		Effect,
	}


	AudioSource audio_bgm;
	AudioSource audio_effect;

	Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

	public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

			GameObject go = new GameObject { name = "AudioSource_BGM" };
			audio_bgm = go.AddComponent<AudioSource>();
			go.transform.parent = root.transform;

			GameObject go2 = new GameObject { name = "AudioSource_Effect" };
			audio_effect = go2.AddComponent<AudioSource>();
			go2.transform.parent = root.transform;
		}
    }


	public void Play(string path, SoundType type = SoundType.Effect, float volume = 1.0f)
	{
		AudioClip audioClip = GetOrAddAudioClip(path, type);

        switch (type)
        {
            case SoundType.Bgm:
				audio_bgm.clip = audioClip;
				audio_bgm.volume = volume;
				audio_bgm.Play();
				break;
            case SoundType.Effect:
				audio_effect.PlayOneShot(audioClip, volume);
				break;
        }
	}

	AudioClip GetOrAddAudioClip(string path, SoundType type = SoundType.Effect)
	{
		if (path.Contains("Sounds/") == false)
			path = $"Sounds/{path}";

		AudioClip audioClip = null;

		if (type == SoundType.Bgm)
		{
			audioClip = Managers.Resource.Load<AudioClip>(path);
		}
		else
		{
			if (_audioClips.TryGetValue(path, out audioClip) == false)
			{
				audioClip = Managers.Resource.Load<AudioClip>(path);
				_audioClips.Add(path, audioClip);
			}
		}

		if (audioClip == null)
			Debug.Log($"AudioClip Missing ! {path}");

		return audioClip;
	}

}
