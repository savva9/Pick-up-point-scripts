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
    public string text; // Текст
    [SerializeField] public TextMeshProUGUI monitorText; // Текст монитора
    [SerializeField] private GameObject helper; // helper

    public bool getBox; // Получили ли мы коробку

    [SerializeField] private ControllerVibration controllerVibration; // Контроллер
    [SerializeField] private GameObject rightHand; // Правая рука
    [SerializeField] private GameObject leftHand; // Левая рука

    [SerializeField] private List<GameObject> buttons; // Кнопки клавиатуры

    [SerializeField] private AudioSource ClicksAudioSource; // Звук клика
    [SerializeField] private List<AudioClip> AudioClicks; // Звуки клика

    [SerializeField] private AudioSource ComputerAudioSource; // Звук компьютера
    [SerializeField] private AudioClip errorSound; // Звук ошибки
    [SerializeField] private AudioClip completeSound; // Звук выполнения заказа

    private TVHelper TVHelper;

    private void Start()
    {
        helper = this.gameObject;
        TVHelper = helper.GetComponent<TVHelper>();
    }

    /// <summary>
    /// Воспроизведение звука компьютера
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
    /// Когда коробка не приехала
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
    /// Нажатие кнопки на клавиатуре
    /// </summary>
    /// <param name="key"></param>
    public void keyAdd(int key)
    {
        // Вибрация
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
                {"NowCompleteOrder", "Выдайте сначало заказ"},
                {"IncorrectCode", "Неправильный код"},
                {"OrderInWay", "Заказ в пути"},
                {"RightOrder", "Заказ найден"},
            };
        } else
        {
            phrases = new Dictionary<string, string>()
            {
                {"NowCompleteOrder", "Сначала сделайте возврат"},
                {"IncorrectCode", "Неправильный код"},
                {"OrderInWay", "Отказ в возврате"},
                {"RightOrder", "Сделайте возврат заказа"},
            };
        }

        // Звук нажатия калавиш
        ClicksAudioSource.resource = AudioClicks[Random.Range(0, AudioClicks.Count)];
        ClicksAudioSource.Play();

        // Нажатие
        if (!getBox)
        {
            if (key == -2)
            {
                GameObject currentHuman = helper.GetComponent<HumanSpawner>().currentHuman;
                string orderNumber = currentHuman.GetComponent<NPCAI>().orderNumber.ToString();
                // Проверка правильности кода
                if (orderNumber == text)
                {
                    // Проверка количества коробок
                    if (helper.GetComponent<BoxSpawner>().boxesInWarehouse.Count > 0 || isRefund)
                    {
                        // Рандомная ситуация
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
                                TVHelper.SetTVText("Принесите подсвеченную коробку");
                            }
                            else {
                                StartCoroutine(helper.GetComponent<BoxSpawner>().refundBoxSpawner());
                                TVHelper.SetTVText("Отнесите все коробки на возврат");
                            }

                            PlaySound(completeSound);
                        }
                    }
                    else
                    {
                        text = "";
                        monitorText.text = phrases["OrderInWay"];
                        BoxNotArrived();
                        TVHelper.SetTVText("Вы не можите выдать заказ, из-за того что заказов нет на складе");
                    }
                }
                else
                {
                    text = "";
                    monitorText.text = phrases["IncorrectCode"];
                    TVHelper.SetTVText("Вы ввели неправильный код");

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
