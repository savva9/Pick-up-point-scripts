using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TVHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TVText; // Текст телевизора

    public void SetTVText(string text)
    {
        TVText.text = text;
    }
}
