using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    [SerializeField] private GameObject helper; // helper
     
    [SerializeField] private bool deleteOnExit; // Удалять из выхода заказа из склада

    /// <summary>
    /// Вход заказа
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box" && 
            !helper.GetComponent<BoxSpawner>().boxesInWarehouse.Contains(other.gameObject) && 
            !helper.GetComponent<BoxSpawner>().onRefund.Contains(other.gameObject))
        {
            helper.GetComponent<BoxSpawner>().boxesInWarehouse.Add(other.gameObject);
        }
    }

    /// <summary>
    /// Выход заказа
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Box" && deleteOnExit && helper.GetComponent<BoxSpawner>().boxesInWarehouse.Contains(other.gameObject))
        {
            helper.GetComponent<BoxSpawner>().boxesInWarehouse.Remove(other.gameObject);
        }
    }
}
