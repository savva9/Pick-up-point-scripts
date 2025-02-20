using NUnit.Framework;
using SpeechLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField] public Transform player; // �����

    public float minSeconds; // ����������� ���������� ������ �� ������ NPC
    public float maxSeconds; // ������������ ���������� ������ �� ������ NPC

    public int maxHumans; // ������������ ����� NPC
    [SerializeField] private List<Transform> spawners; // ��������
    [SerializeField] private List<GameObject> peoplePrefabs; // ������� NPC
    [SerializeField] public List<GameObject> peopleList; // ������ �����

    [SerializeField] public List<Transform> humanPoints; // ����
    [SerializeField] public List<Transform> quitPoints; // ����� ������

    [SerializeField] private Transform humanContainer; // ��������� �����

    [SerializeField] public GameObject currentHuman; // ������� NPC �� ������ ������

    public List<string> phrases; // �������������� �����
    public List<string> sadPhrases; // �������� �����
    public List<string> positivePhrases; // ���������� �����
    public List<string> refundPhrases; // ����� �������� ������

    public int linePosition = 0; // ����� �������

    private void Start()
    {
        StartCoroutine(humanSpawner(Random.Range(minSeconds, maxSeconds)));
    }

    /// <summary>
    /// ������� �����
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
    /// ������������ ����� � �������
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
