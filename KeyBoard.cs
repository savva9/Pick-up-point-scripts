using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Random = UnityEngine.Random;

public class KeyBoard : MonoBehaviour
{
    public string text; // �����
    [SerializeField] public TextMeshProUGUI monitorText; // ����� ��������
    [SerializeField] private GameObject helper; // helper

    public bool getBox; // �������� �� �� �������

    [SerializeField] private ControllerVibration controllerVibration; // ����������
    [SerializeField] private GameObject rightHand; // ������ ����
    [SerializeField] private GameObject leftHand; // ����� ����

    [SerializeField] private List<GameObject> buttons; // ������ ����������

    [SerializeField] private AudioSource ClicksAudioSource; // ���� �����
    [SerializeField] private List<AudioClip> AudioClicks; // ����� �����

    [SerializeField] private AudioSource ComputerAudioSource; // ���� ����������
    [SerializeField] private AudioClip errorSound; // ���� ������
    [SerializeField] private AudioClip completeSound; // ���� ���������� ������

    private TVHelper TVHelper;

    private void Start()
    {
        helper = this.gameObject;
        TVHelper = helper.GetComponent<TVHelper>();
    }

    /// <summary>
    /// ��������������� ����� ����������
    /// </summary>
    /// <param name="AudioClip"></param>
    private void PlaySound(AudioClip AudioClip)
    {
        if(!ComputerAudioSource.isPlaying)
        {
            ComputerAudioSource.resource = AudioClip;
            ComputerAudioSource.Play();
        }
    }

    /// <summary>
    /// ����� ������� �� ��������
    /// </summary>
    private void BoxNotArrived()
    {
        //helper.GetComponent<HumanSpawner>().HumanMove();
        NPCAI npcai = helper.GetComponent<HumanSpawner>().currentHuman.GetComponent<NPCAI>();
        HumanSpawner humanspawner = helper.GetComponent<HumanSpawner>();
        npcai.pickUpStage = 5;
        npcai.pathUpdate();

        int randomTextNumber = Random.Range(0, humanspawner.sadPhrases.Count);
        npcai.text.text = humanspawner.sadPhrases[randomTextNumber];
        helper.GetComponent<TextToSpeech>().AddMessageToQueue(humanspawner.sadPhrases[randomTextNumber]);
    }

    /// <summary>
    /// ������� ������ �� ����������
    /// </summary>
    /// <param name="key"></param>
    public void keyAdd(int key)
    {
        // ��������
        Vector3 currentButton = helper.GetComponent<KeyBoard>().buttons[key + 2].GetComponentInChildren<TextMeshProUGUI>().transform.position;
        
        float distanceRight = Vector3.Distance(currentButton, rightHand.transform.position);
        float distanceLeft = Vector3.Distance(currentButton, leftHand.transform.position);

        if(distanceRight < distanceLeft)
            controllerVibration.TriggerVibration(SteamVR_Input_Sources.RightHand);
        else if (distanceLeft < distanceRight)
            controllerVibration.TriggerVibration(SteamVR_Input_Sources.LeftHand);


        Dictionary<string, string> phrases;

        bool isRefund = helper.GetComponent<HumanSpawner>().currentHuman.GetComponent<NPCAI>().isRefund;
        if(!isRefund)
        {
            phrases = new Dictionary<string, string>()
            {
                {"NowCompleteOrder", "������� ������� �����"},
                {"IncorrectCode", "������������ ���"},
                {"OrderInWay", "����� � ����"},
                {"RightOrder", "����� ������"},
            };
        } else
        {
            phrases = new Dictionary<string, string>()
            {
                {"NowCompleteOrder", "������� �������� �������"},
                {"IncorrectCode", "������������ ���"},
                {"OrderInWay", "����� � ��������"},
                {"RightOrder", "�������� ������� ������"},
            };
        }

        // ���� ������� �������
        ClicksAudioSource.resource = AudioClicks[Random.Range(0, AudioClicks.Count)];
        ClicksAudioSource.Play();

        // �������
        if (!getBox)
        {
            if (key == -2)
            {
                GameObject currentHuman = helper.GetComponent<HumanSpawner>().currentHuman;
                string orderNumber = currentHuman.GetComponent<NPCAI>().orderNumber.ToString();
                // �������� ������������ ����
                if (orderNumber == text)
                {
                    // �������� ���������� �������
                    if (helper.GetComponent<BoxSpawner>().boxesInWarehouse.Count > 0 || isRefund)
                    {
                        // ��������� ��������
                        int randomSitoation = Random.Range(1, 6);
                        if (randomSitoation == 5)
                        {
                            text = "";
                            monitorText.text = phrases["OrderInWay"];
                            BoxNotArrived();
                        }
                        else
                        {
                            getBox = true;
                            text = "";
                            monitorText.text = phrases["RightOrder"];

                            if (!isRefund) { 
                                currentHuman.GetComponent<NPCAI>().takeBox();
                                TVHelper.SetTVText("��������� ������������ �������");
                            }
                            else {
                                StartCoroutine(helper.GetComponent<BoxSpawner>().refundBoxSpawner());
                                TVHelper.SetTVText("�������� ��� ������� �� �������");
                            }

                            PlaySound(completeSound);
                        }
                    }
                    else
                    {
                        text = "";
                        monitorText.text = phrases["OrderInWay"];
                        BoxNotArrived();
                        TVHelper.SetTVText("�� �� ������ ������ �����, ��-�� ���� ��� ������� ��� �� ������");
                    }
                }
                else
                {
                    text = "";
                    monitorText.text = phrases["IncorrectCode"];
                    TVHelper.SetTVText("�� ����� ������������ ���");

                    PlaySound(errorSound);
                }
            }
            else if (key != -1)
            {
                text += key;
                monitorText.text = text;
            }
            else if (text.Length > 0)
            {
                text = text.Remove(text.Length - 1);
                monitorText.text = text;
            }
        } else
        {
            monitorText.text = phrases["NowCompleteOrder"];
        }
    }
}
