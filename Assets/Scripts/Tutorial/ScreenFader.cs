using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh = null;
    [SerializeField] private string text = "";

    [SerializeField] private UnityEvent _OnFadeInEnd = null;

    private Animator anim = null;

    private void Start() => anim = GetComponent<Animator>();
        
    public void FadeOut(int level)
    {        
        _textMesh.text = text + " " + level.ToString("D2");
        anim.SetTrigger("StartFade");
    }

    public void FadeInEnd() => _OnFadeInEnd?.Invoke();
}
