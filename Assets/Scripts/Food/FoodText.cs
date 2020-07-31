using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textPro = null;
        
    public void UpdateText(int food) => _textPro.text = food.ToString();
}
