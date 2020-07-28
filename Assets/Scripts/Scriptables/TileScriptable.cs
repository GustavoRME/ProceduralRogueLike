using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Scriptables/Tiles", order = 0)]    
public class TileScriptable : ScriptableObject
{   
    public bool isBreakable;
    public bool isWalkable;
    
    [Space]
    
    public string sortingLayerName;
    public int sortingOrder;

    [Space]

    public Sprite[] sprites;
}
