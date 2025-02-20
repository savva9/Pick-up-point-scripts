using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPickUpTrigger : MonoBehaviour
{
    [SerializeField] public GameObject helper; // helper

    /// <summary>
    /// Выдача коробки 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box" && 
            helper.GetComponent<BoxSpawner>().currentBox == other.gameObject && 
            other.transform.parent == helper.GetComponent<BoxSpawner>().boxContainer)
        {
            BoxSpawner boxSpawner = helper.GetComponent<BoxSpawner>();
            if(boxSpawner.boxesInWarehouse.Contains(other.gameObject))
            {
                boxSpawner.boxesInWarehouse.Remove(other.gameObject);
            }
            boxSpawner.boxesList.Remove(other.gameObject);
            boxSpawner.currentBox = null;

            Destroy(other.gameObject);

            HumanSpawner humanSpawner = helper.GetComponent<HumanSpawner>();

            NPCAI NPCAI = humanSpawner.currentHuman.GetComponent<NPCAI>();
            NPCAI.pickUpStage = 5;
            NPCAI.pathUpdate();
            int randomTextNumber = Random.Range(0, humanSpawner.positivePhrases.Count);
            NPCAI.text.text = humanSpawner.positivePhrases[randomTextNumber];
            helper.GetComponent<TextToSpeech>().AddMessageToQueue(humanSpawner.positivePhrases[randomTextNumber]);;

            KeyBoard keyBoard = helper.GetComponent<KeyBoard>();
            keyBoard.getBox = false;
            keyBoard.monitorText.text = "Заказ выдан";
        }
    }
}
