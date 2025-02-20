using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent; // AI

    [SerializeField] private Transform target; // ����
    public int pickUpStage = 0; // ����� ����

    [SerializeField] public GameObject helper; // helper

    public int orderNumber; // ����� ������
    private string phrases; // �����
    public bool isRefund; // ����� �� �������
    [SerializeField] private GameObject textGameObject; // ������ ������
    [SerializeField] public TextMeshProUGUI text; // �����
    private string textForVoice; // ����� ��� ������

    [SerializeField] public Transform player; // �����
    public bool lookAtPlayer; // ���� �� �������� �� ������

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
    /// ������������� ������ NPC � ������
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
    /// ���������� ����
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
    /// ����� ��������� �������
    /// </summary>
    public void takeBox()
    {
        List<GameObject> boxesList = helper.GetComponent<BoxSpawner>().boxesInWarehouse;
        GameObject randomBox = boxesList[Random.Range(0, boxesList.Count)];
        helper.GetComponent<BoxSpawner>().currentBox = randomBox;

        helper.GetComponent<BoxSpawner>().colorBox(randomBox, helper.GetComponent<BoxSpawner>().outlineCurrentBox);
    }

    /// <summary>
    /// ����� ��������, ������ �������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // �������� �����
        if (other.gameObject.tag == "PickUp" && pickUpStage == 1)
        {
            lookAtPlayer = true;
            helper.GetComponent<HumanSpawner>().currentHuman = this.gameObject;
            text.text = phrases;
            helper.GetComponent<TextToSpeech>().AddMessageToQueue(textForVoice);
            helper.GetComponent<TVHelper>().SetTVText("����������� �� ���������� ����� ������");
        }

        // �����
        if(other.gameObject.tag == "HumanQuit" && pickUpStage == 5)
        {
            helper.GetComponent<HumanSpawner>().peopleList.Remove(this.gameObject);
            Destroy(this.gameObject);
        }

        // ������ � �������
        if(other.gameObject.tag == "HumanDetecter" && pickUpStage == 0)
        {
            int linePosition = helper.GetComponent<HumanSpawner>().linePosition;
            pickUpStage = linePosition;
            pathUpdate();
        }
    }
}