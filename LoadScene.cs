using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private int sceneID; // ID �����

    /// <summary>
    /// ����� �� ����
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// �������� �����
    /// </summary>
    /// <param name="numberScne"></param>
    public void LoadNumberScene(int numberScne)
    {
        sceneID = numberScne;
        StartCoroutine(AsyncLoad());
    }

    /// <summary>
    /// ������������ �������� �����
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
