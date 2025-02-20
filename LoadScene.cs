using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private int sceneID; // ID сцены

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Загрузка сцены
    /// </summary>
    /// <param name="numberScne"></param>
    public void LoadNumberScene(int numberScne)
    {
        sceneID = numberScne;
        StartCoroutine(AsyncLoad());
    }

    /// <summary>
    /// Ассинхронная загрузка сцены
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
