using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public float minSeconds; // Минимальное количество секунд до спавна коробки
    public float maxSeconds; // Максимальное количество секунд до спавна коробки
    public float delayBoxSpawn; // Время спавна коробок с задержкой

    public int maxBoxes; // Максимальное количество коробок
    public int maxSpawnBoxes; // Максимальное количество коробо заспавненых за раз
    
    public List<Transform> spawners; // Спавнера
    [SerializeField] private Transform refundSpawer; // Спавнер возврата заказа
    [SerializeField] private List<GameObject> boxesPrefabs; // Префабы коробок

    [SerializeField] public List<GameObject> boxesList; // Список коробок
    [SerializeField] public List<GameObject> onRefund; // Коробки на возврат
    [SerializeField] public List<GameObject> boxesInWarehouse; // Список коробок на складе

    [SerializeField] public Transform boxContainer; // Контейнер коробок

    [SerializeField] public GameObject currentBox; // Нанешняя коробка на выдачь

    [SerializeField] public Material outlineCurrentBox; // Подсветка коробки при выдачи
    [SerializeField] public Material outlineRefundBox; // Подсветка коробки при возврате

    [SerializeField] public List<AudioClip> DropMusic; // Звуки падения коробки

    private void Start()
    {
        maxSpawnBoxes++;
        StartCoroutine(boxSpawner(Random.Range(minSeconds, maxSeconds)));
    }

    /// <summary>
    /// Спавнер коробок
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator boxSpawner(float sec)
    {
        if (boxesList.Count < maxBoxes)
        {
            this.GetComponent<TVHelper>().SetTVText("Отнеси товары которые приехали на склад");
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
    /// Коробки на возврат
    /// </summary>
    /// <returns></returns>
    public IEnumerator refundBoxSpawner()
    {
        int boxToSpawn = Random.Range(1, maxSpawnBoxes);

        this.GetComponent<TVHelper>().SetTVText("Отнесите товары на возврат");
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
    /// Обводка коробки
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
