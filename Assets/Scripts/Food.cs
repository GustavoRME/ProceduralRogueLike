using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Food : MonoBehaviour
{
    [SerializeField] private int _amount = 1;

    [Space]
    [SerializeField] private AudioClip[] _eatFoodClips = null;

    private AudioPlayer _audioPlayer;

    private void Awake() => _audioPlayer = new AudioPlayer(GetComponent<AudioSource>(), _eatFoodClips);
        
    public int Eat()
    {
        _audioPlayer.PlayRandomAudio();
        gameObject.SetActive(false);

        return _amount;
    }    
}
