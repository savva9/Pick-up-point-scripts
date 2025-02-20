using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent; // AI

    [SerializeField] private Transform target; // Цель
    public int pickUpStage = 0; // Номер цели

    [SerializeField] public GameObject helper; // helper

    public int orderNumber; // Номер заказа
    private string phrases; // Фраза
    public bool isRefund; // Товар на возврат
    [SerializeField] private GameObject textGameObject; // Объект текста
    [SerializeField] public TextMeshProUGUI text; // Текст
    private string textForVoice; // Текст для голоса

    [SerializeField] public Transform player; // Игрок
    public bool lookAtPlayer; // Надо ли смотреть на игрока

    public int id;
    public List<int> pickUpStages;

    private void Start()
    {
        this.GetComponent<HumanAnimator>().agent = agent;
        text = textGameObject.GetComponent<TextMeshProUGUI>();


        orderNumber = Random.Range(100000, 1000000);
        id = Random.Range(100000, 1000000);

        string orderNumberForVoice = "";
        for(int i = 0; i < orderNumber.ToString().Length; i++)
        {
            orderNumberForVoice += orderNumber.ToString()[i] + " ";
        }

        int randomSituation = Random.Range(0, 6);
        if (randomSituation != 5)
        {
            List<string> thisPhrases = helper.GetComponent<HumanSpawner>().phrases;
            int randomPhraseNumber = Random.Range(0, thisPhrases.Count);
            phrases = thisPhrases[randomPhraseNumber].Replace("{x}", orderNumber.ToString());
            textForVoice = thisPhrases[randomPhraseNumber].Replace("{x}", orderNumberForVoice);
        } else
        {
            isRefund = true;
            List<string> thisPhrases = helper.GetComponent<HumanSpawner>().refundPhrases;
            int randomPhraseNumber = Random.Range(0, thisPhrases.Count);
            phrases = thisPhrases[randomPhraseNumber].Replace("{x}", orderNumber.ToString());
            textForVoice = thisPhrases[randomPhraseNumber].Replace("{x}", orderNumberForVoice);
        }

        pathUpdate();
    }

    /// <summary>
    /// Поворачивание текста NPC к игроку
    /// </summary>
    private void Update()
    {
        if (lookAtPlayer)
        {
            Vector3 playerPos = player.transform.position;
            textGameObject.GetComponent<Transform>().LookAt(new Vector3(playerPos.x, playerPos.y, playerPos.z));
        }
    }

    /// <summary>
    /// Обновление пути
    /// </summary>
    public void pathUpdate()
    {
        pickUpStages.Add(pickUpStage);
        if (pickUpStage != 5)
        {
            target = helper.GetComponent<HumanSpawner>().humanPoints[pickUpStage];
        } else
        {
            List<Transform> targetsList = helper.GetComponent<HumanSpawner>().quitPoints;
            target = targetsList[Random.Range(0, targetsList.Count)];
        }
        agent.SetDestination(target.position);
        this.GetComponent<HumanAnimator>().MoveToObject(target.gameObject, false);
    }

    /// <summary>
    /// Выбор рандомной каробки
    /// </summary>
    public void takeBox()
    {
        List<GameObject> boxesList = helper.GetComponent<BoxSpawner>().boxesInWarehouse;
        GameObject randomBox = boxesList[Random.Range(0, boxesList.Count)];
        helper.GetComponent<BoxSpawner>().currentBox = randomBox;

        helper.GetComponent<BoxSpawner>().colorBox(randomBox, helper.GetComponent<BoxSpawner>().outlineCurrentBox);
    }

    /// <summary>
    /// Когда косается, любого тригера
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // Получить заказ
        if (other.gameObject.tag == "PickUp" && pickUpStage == 1)
        {
            lookAtPlayer = true;
            helper.GetComponent<HumanSpawner>().currentHuman = this.gameObject;
            text.text = phrases;
            helper.GetComponent<TextToSpeech>().AddMessageToQueue(textForVoice);
            helper.GetComponent<TVHelper>().SetTVText("Напечатайте на клавиатуре номер заказа");
        }

        // Выход
        if(other.gameObject.tag == "HumanQuit" && pickUpStage == 5)
        {
            helper.GetComponent<HumanSpawner>().peopleList.Remove(this.gameObject);
            Destroy(this.gameObject);
        }

        // Встать в очередь
        if(other.gameObject.tag == "HumanDetecter" && pickUpStage == 0)
        {
            int linePosition = helper.GetComponent<HumanSpawner>().linePosition;
            pickUpStage = linePosition;
            pathUpdate();
        }
    }
}