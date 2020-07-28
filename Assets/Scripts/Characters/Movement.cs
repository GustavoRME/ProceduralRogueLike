using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footStepAudios = null;
    
    private AudioPlayer _audioPlayer;

    private void Awake() => _audioPlayer = new AudioPlayer(GetComponent<AudioSource>(), _footStepAudios);

    public void Move(int x, int y)
    {        
        transform.position = new Vector3(x, y, 0f);

        _audioPlayer.PlayRandomAudio();
    }
}
