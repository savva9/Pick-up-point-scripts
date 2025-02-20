using NUnit.Framework;
using SpeechLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField] public Transform player; // Игрок

    public float minSeconds; // Минимальное количество секунд до спавна NPC
    public float maxSeconds; // Максимальное количество секунд до спавна NPC

    public int maxHumans; // Максимальное число NPC
    [SerializeField] private List<Transform> spawners; // Спавнеры
    [SerializeField] private List<GameObject> peoplePrefabs; // Префабы NPC
    [SerializeField] public List<GameObject> peopleList; // Список людей

    [SerializeField] public List<Transform> humanPoints; // Цели
    [SerializeField] public List<Transform> quitPoints; // Точки выхода

    [SerializeField] private Transform humanContainer; // Контейнер людей

    [SerializeField] public GameObject currentHuman; // Текущий NPC на выдачи заказа

    public List<string> phrases; // Приветственные фразы
    public List<string> sadPhrases; // Грустные фразы
    public List<string> positivePhrases; // Позитивные фразы
    public List<string> refundPhrases; // Фраза возврата товара

    public int linePosition = 0; // Длина очереди

    private void Start()
    {
        StartCoroutine(humanSpawner(Random.Range(minSeconds, maxSeconds)));
    }

    /// <summary>
    /// Спавнер людей
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator humanSpawner(float sec)
    {
        if (peopleList.Count < maxHumans)
        {
            GameObject spawnHuman = Instantiate(peoplePrefabs[Random.Range(0, peoplePrefabs.Count)], spawners[Random.Range(0, spawners.Count)].position, Quaternion.identity);
            spawnHuman.GetComponent<NPCAI>().helper = GetComponent<HumanSpawner>().gameObject;
            spawnHuman.transform.SetParent(humanContainer);
            spawnHuman.GetComponent<NPCAI>().player = player;
            peopleList.Add(spawnHuman);
        }
        yield return new WaitForSeconds(sec);
        StartCoroutine(humanSpawner(Random.Range(minSeconds, maxSeconds)));
    }

    /// <summary>
    /// Передвижение людей в очереди
    /// </summary>
    public void HumanMove()
    {
        foreach (GameObject people in peopleList)
        {
            if(currentHuman != people && people.GetComponent<NPCAI>().pickUpStage != 0 && people.GetComponent<NPCAI>().pickUpStage != 5)
            {
                people.GetComponent<NPCAI>().pickUpStage--;
                people.GetComponent<NPCAI>().pathUpdate();
            }
        }
    }
}
