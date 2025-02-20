using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refund : MonoBehaviour
{
    [SerializeField] private GameObject helper; // helper

    /// <summary>
    /// Возврат заказа
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box" && 
            helper.GetComponent<BoxSpawner>().onRefund.Contains(other.gameObject) && 
            other.gameObject.transform.parent == helper.GetComponent<BoxSpawner>().boxContainer)
        {
            helper.GetComponent<BoxSpawner>().onRefund.Remove(other.gameObject);
            Destroy(other.gameObject);

            if(helper.GetComponent<BoxSpawner>().onRefund.Count == 0)
            {
                HumanSpawner humanSpawner = helper.GetComponent<HumanSpawner>();

                NPCAI NPCAI = humanSpawner.currentHuman.GetComponent<NPCAI>();
                NPCAI.pickUpStage = 5;
                NPCAI.pathUpdate();
                int randomTextNumber = Random.Range(0, humanSpawner.positivePhrases.Count);
                NPCAI.text.text = humanSpawner.positivePhrases[randomTextNumber];
                helper.GetComponent<TextToSpeech>().AddMessageToQueue(humanSpawner.positivePhrases[randomTextNumber]);

                KeyBoard keyBoard = helper.GetComponent<KeyBoard>();
                keyBoard.getBox = false;
                keyBoard.monitorText.text = "Заказ возвращен";
            }
        }
    }
}
