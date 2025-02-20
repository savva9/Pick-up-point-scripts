using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class BoxContainer : MonoBehaviour
{
    [SerializeField] public GameObject helper; // helper
    [SerializeField] public Transform Player; // Игрок

    [SerializeField] private AudioSource AudioSource; // Компонент звука

    private bool isFalling;

    private void Start()
    {
        AudioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Проверка надо ли переместь коробку в контейнер
    /// </summary>
    private void Update()
    {
        if (GetComponent<Renderer>().transform.parent == null)
        {
            GetComponent<Renderer>().transform.parent = helper.GetComponent<BoxSpawner>().boxContainer;
        }
    }

    /// <summary>
    /// Звук падения
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!AudioSource.isPlaying)
        {
            float distance = Vector3.Distance(this.gameObject.transform.position, Player.position);
            List<AudioClip> AudioClips = helper.GetComponent<BoxSpawner>().DropMusic;
            AudioSource.resource = AudioClips[Random.Range(0, AudioClips.Count)];
            AudioSource.volume = 0.5f / (distance * 1f);
            AudioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TeleportBox")
        {
            List<Transform> teleports = helper.GetComponent<BoxSpawner>().spawners;
            this.transform.position = teleports[Random.Range(0, teleports.Count)].position;
        }
    }
}
