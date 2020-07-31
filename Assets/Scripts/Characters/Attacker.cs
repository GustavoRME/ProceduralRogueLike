using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Attacker : MonoBehaviour
{
    [SerializeField] private int _damage = 2;

    [Space]
    [SerializeField] private AudioClip[] _attackClips = null;

    private PlayerController _player;
    private AudioPlayer _audioPlayer;

    private void Awake() 
    {
        _player = FindObjectOfType<PlayerController>();
        _audioPlayer = new AudioPlayer(GetComponent<AudioSource>(), _attackClips);
    } 

    public void Attack()
    {
        _player.TakeDamage(_damage);
        _audioPlayer.PlayRandomAudio();
    }
}
