using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TutorialScreen : MonoBehaviour
{    
    [SerializeField] private TextMeshProUGUI _textMesh = null;

    [Tooltip("Time in seconds to appear next lyric")]
    [SerializeField] private float _lyricTime = 0.2f;

    [Tooltip("Time in seconds to remove screen")]
    [SerializeField] private float _disableScreenTime = 1f;

    [Space]
    [SerializeField] private UnityEvent _OnScreenEnd = null;

    private string _message;
    private int _indexText;

    private Coroutine _writeTextCoroutine;
    
    public bool IsEnable { get; private set; }
    
    private bool _isWriting;
    private bool _canManager;    

    private void Awake()
    {
        _message = _textMesh.text;

        _textMesh.text = "";        
                
        gameObject.SetActive(IsEnable);        
    }
               
    public void EnableScreen()
    {
        if(!IsEnable)
        {
            IsEnable = true;
            _isWriting = true;

            gameObject.SetActive(IsEnable);
            
            _writeTextCoroutine = StartCoroutine(WriteTextCoroutine());
        }
    }
    public void ScreenManager()
    {
        if(_canManager)
        {
            if(_isWriting)
            {
                StopCoroutine(_writeTextCoroutine);
                WriteText();
                Invoke("DisableScreen", _disableScreenTime);
                _isWriting = false;
            }
            else if(IsEnable)
            {                
                DisableScreen();
                IsEnable = false;
                _canManager = false;

            }
        }
    }

    private IEnumerator WriteTextCoroutine()
    {                
        while(_isWriting)
        {
            _textMesh.text += _message[_indexText];
            
            _indexText++;
            
            if (_indexText >= _message.Length)
            {                
                Invoke("DisableScreen", _disableScreenTime);                
                _isWriting = false;
            }
            
            yield return new WaitForSeconds(_lyricTime);

            _canManager = true;
        }
    }    

    private void WriteText() => _textMesh.text = _message;
    private void DisableScreen()
    {       
        if(IsEnable)
        {
            _OnScreenEnd?.Invoke();

            IsEnable = false;
            _canManager = false;
            _isWriting = false;

            gameObject.SetActive(IsEnable);
        }
    }

}
