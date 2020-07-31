using UnityEngine;

[CreateAssetMenu(fileName = "Counter", menuName = "Scriptables/Countables")]
public class CountScriptable : ScriptableObject
{
    public int minimum = 1;
    public int maximum = 1;
    
    public int Count { get; set; }
    
}
