using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Breaker : MonoBehaviour
{
    [SerializeField] private ProceduralBoardGenerate _proceduralBoard = null;
    [SerializeField] private AudioClip[] _chopClips = null;

    private AudioPlayer _audioPlayer;

    private void Awake() => _audioPlayer = new AudioPlayer(GetComponent<AudioSource>(), _chopClips);

    public void BreakTile(int x, int y)
    {
        Tile tile = _proceduralBoard.GetTile(x, y);

        if(tile != null)
        {
            tile.Hit();
            _audioPlayer.PlayRandomAudio();
        }
    }       
}
