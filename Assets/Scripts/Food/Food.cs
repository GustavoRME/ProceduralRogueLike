using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Food : MonoBehaviour
{
    [SerializeField] private int _amount = 1;

    [Space]
    [SerializeField] private AudioClip[] _eatFoodClips = null;

    private AudioPlayer _audioPlayer;
    private SpriteRenderer _renderer2D;
    private Collider2D _boxCollider2D;

    private void Awake()
    {
        _renderer2D = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<Collider2D>();

        _audioPlayer = new AudioPlayer(GetComponent<AudioSource>(), _eatFoodClips);
    }
        
    public int Eat()
    {
        _audioPlayer.PlayRandomAudio();
        _renderer2D.enabled = false;
        _boxCollider2D.enabled = false;

        StartCoroutine(WaitToDestroy());

        return _amount;
    }   
    
    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }
}
