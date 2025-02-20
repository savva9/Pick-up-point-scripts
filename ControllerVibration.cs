using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerVibration : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Vibration hapticAction; // Действие вибрации
    
    [SerializeField] private float duration = 0.02f; // Длительность вибрации в секундах
    [SerializeField] private float frequency = 50.0f; // Частота вибрации
    [SerializeField] private float amplitude = 2.0f; // Амплитуда вибрации

    private Coroutine vibrationCoroutine;

    /// <summary>
    /// Начало вибрации
    /// </summary>
    /// <param name="hand"></param>
    public void TriggerVibration(SteamVR_Input_Sources hand)
    {
        if (vibrationCoroutine != null)
        {
            StopCoroutine(vibrationCoroutine);
        }
        vibrationCoroutine = StartCoroutine(VibrateController(hand, duration));
    }

    /// <summary>
    /// Вибрация
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator VibrateController(SteamVR_Input_Sources hand, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            hapticAction.Execute(0, Time.deltaTime, frequency, amplitude, hand);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
