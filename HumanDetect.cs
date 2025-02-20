using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDetect : MonoBehaviour
{
    [SerializeField] private GameObject helper; // helper

    private HumanSpawner humanSpawner; // ������ ������ NPC

    private void Start()
    {
        humanSpawner = helper.GetComponent<HumanSpawner>();
    }

    /// <summary>
    /// ����� ������� ����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Human")
        {
            humanSpawner.linePosition++;
        }
    }

    /// <summary>
    /// ����� ������� �����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Human")
        {
            humanSpawner.linePosition--;
            helper.GetComponent<HumanSpawner>().HumanMove();
        }
    }
}
