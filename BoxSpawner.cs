using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public float minSeconds; // ����������� ���������� ������ �� ������ �������
    public float maxSeconds; // ������������ ���������� ������ �� ������ �������
    public float delayBoxSpawn; // ����� ������ ������� � ���������

    public int maxBoxes; // ������������ ���������� �������
    public int maxSpawnBoxes; // ������������ ���������� ������ ����������� �� ���
    
    public List<Transform> spawners; // ��������
    [SerializeField] private Transform refundSpawer; // ������� �������� ������
    [SerializeField] private List<GameObject> boxesPrefabs; // ������� �������

    [SerializeField] public List<GameObject> boxesList; // ������ �������
    [SerializeField] public List<GameObject> onRefund; // ������� �� �������
    [SerializeField] public List<GameObject> boxesInWarehouse; // ������ ������� �� ������

    [SerializeField] public Transform boxContainer; // ��������� �������

    [SerializeField] public GameObject currentBox; // �������� ������� �� ������

    [SerializeField] public Material outlineCurrentBox; // ��������� ������� ��� ������
    [SerializeField] public Material outlineRefundBox; // ��������� ������� ��� ��������

    [SerializeField] public List<AudioClip> DropMusic; // ����� ������� �������

    private void Start()
    {
        maxSpawnBoxes++;
        StartCoroutine(boxSpawner(Random.Range(minSeconds, maxSeconds)));
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator boxSpawner(float sec)
    {
        if (boxesList.Count < maxBoxes)
        {
            this.GetComponent<TVHelper>().SetTVText("������ ������ ������� �������� �� �����");
            int boxToSpawn = Random.Range(1, maxSpawnBoxes);
            for (int i = 0; i < boxToSpawn; i++)
            {
                GameObject spawnedBox = Instantiate(boxesPrefabs[Random.Range(0, boxesPrefabs.Count)], spawners[Random.Range(0, spawners.Count)].position, Quaternion.identity);
                spawnedBox.transform.SetParent(boxContainer);
                boxesList.Add(spawnedBox);
                spawnedBox.GetComponent<BoxContainer>().helper = this.gameObject;
                spawnedBox.GetComponent<BoxContainer>().Player = this.GetComponent<HumanSpawner>().player;
                yield return new WaitForSeconds(delayBoxSpawn);
            }
        }
        yield return new WaitForSeconds(sec);
        StartCoroutine(boxSpawner(Random.Range(minSeconds, maxSeconds)));
    }

    /// <summary>
    /// ������� �� �������
    /// </summary>
    /// <returns></returns>
    public IEnumerator refundBoxSpawner()
    {
        int boxToSpawn = Random.Range(1, maxSpawnBoxes);

        this.GetComponent<TVHelper>().SetTVText("�������� ������ �� �������");
        for (int i = 0; i < boxToSpawn; i++)
        {
            GameObject spawnedBox = Instantiate(boxesPrefabs[Random.Range(0, boxesPrefabs.Count)], refundSpawer.position, Quaternion.identity);
            spawnedBox.transform.SetParent(boxContainer);
            onRefund.Add(spawnedBox);
            colorBox(spawnedBox, outlineRefundBox);
            spawnedBox.GetComponent<BoxContainer>().helper = this.gameObject;
            spawnedBox.GetComponent<BoxContainer>().Player = this.GetComponent<HumanSpawner>().player;
            yield return new WaitForSeconds(delayBoxSpawn);
        }
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="randomBox"></param>
    /// <param name="material"></param>
    public void colorBox(GameObject randomBox, Material material)
    {
        Renderer renderer = randomBox.GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        Material[] newMaterials = new Material[materials.Length + 1];
        newMaterials[0] = materials[0];
        newMaterials[materials.Length] = material;
        renderer.materials = newMaterials;
    }
}
