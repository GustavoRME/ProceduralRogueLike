using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer
{
    private readonly AudioSource _source;
    private readonly AudioClip[] _clips;

    public AudioPlayer(AudioSource source, params AudioClip[] clips)
    {
        _source = source;
        _clips = clips;
    }

    public void PlayRandomAudio()
    {
        if(HaveClips())
        {
            var clip = _clips[Random.Range(0, _clips.Length)];
            
            _source.PlayOneShot(clip);
        }
    }

    private bool HaveClips() => _clips != null && _clips.Length > 0;
}
