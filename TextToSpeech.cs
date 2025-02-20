using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;

public class TextToSpeech : MonoBehaviour
{
    public bool canTalk; // ����� �� ���������� ���� � �����
    private SpVoice voice; // �����
    private Queue<string> messageQueue; // �������
    private bool isSpeaking; // ������� �� ������

    void Start()
    {
        voice = new SpVoice();
        messageQueue = new Queue<string>();
        isSpeaking = false;
    }

    /// <summary>
    /// ���������� ��������� � �������
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
    /// ���������������� ���������������
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
