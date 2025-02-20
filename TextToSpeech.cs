using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;

public class TextToSpeech : MonoBehaviour
{
    public bool canTalk; // Могут ли озвучивать речь в текст
    private SpVoice voice; // Голос
    private Queue<string> messageQueue; // Очередь
    private bool isSpeaking; // Говорят ли сейчас

    void Start()
    {
        voice = new SpVoice();
        messageQueue = new Queue<string>();
        isSpeaking = false;
    }

    /// <summary>
    /// Добавление сообщений в очередь
    /// </summary>
    /// <param name="messages"></param>
    public void AddMessageToQueue(params string[] messages)
    {
        if (canTalk)
        {
            foreach (string message in messages)
            {
                messageQueue.Enqueue(message);
            }

            if (!isSpeaking)
            {
                StartCoroutine(PlayMessages());
            }
        }
    }

    /// <summary>
    /// Последовательное воспроизведение
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayMessages()
    {
        while (messageQueue.Count > 0)
        {
            isSpeaking = true;

            string message = messageQueue.Dequeue();
            voice.Speak(message, SpeechVoiceSpeakFlags.SVSFlagsAsync);

            while (voice.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
            {
                yield return null;
            }
        }
        isSpeaking = false;
    }
}
