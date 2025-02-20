using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private TextToSpeech textToSpeech;
    [SerializeField] private TextMeshProUGUI textCanTalk;

    private void Start()
    {
        textToSpeech = this.GetComponent<TextToSpeech>();    
    }

    public void SetTalk()
    {
        if (textToSpeech.canTalk) 
        {
            textToSpeech.canTalk = false;
            textCanTalk.text = "���. �������";
        } else
        {
            textToSpeech.canTalk = true;
            textCanTalk.text = "����. �������";
        }
    }
}
