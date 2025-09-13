using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioQueueManager : MonoBehaviour
{
    // ����ʵ��
    public static AudioQueueManager Instance { get; private set; }

    private AudioSource audioSource;
    private Queue<AudioPlayData> audioQueue = new Queue<AudioPlayData>();
    private bool isPlaying = false;

    private void Awake()
    {
        // ȷ������Ψһ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ����Ƶ���벥�Ŷ���
    /// </summary>
    /// <param name="clip">��ƵƬ��</param>
    /// <param name="sourceClick">��Դclick</param>
    /// <param name="isPriority">�Ƿ�Ϊ���ȼ���Ƶ����Ƶ1��</param>
    public void EnqueueAudio(AudioClip clip, click_eft sourceClick, bool isPriority = false)
    {
        if (clip == null) return;

        var data = new AudioPlayData
        {
            clip = clip,
            sourceClick = sourceClick,
            isPriority = isPriority
        };

        // ���ȼ���Ƶ�������ͷ��
        if (isPriority)
        {
            var tempQueue = new Queue<AudioPlayData>();
            tempQueue.Enqueue(data);
            while (audioQueue.Count > 0)
                tempQueue.Enqueue(audioQueue.Dequeue());
            audioQueue = tempQueue;

            // �����жϵ�ǰ���ţ���ʼ�¶���
            if (isPlaying)
            {
                audioSource.Stop();
                StopAllCoroutines();
                StartCoroutine(ProcessQueue());
            }
        }
        else
        {
            audioQueue.Enqueue(data);
        }

        if (!isPlaying)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    /// <summary>
    /// ������Ƶ����
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        isPlaying = true;

        while (audioQueue.Count > 0)
        {
            var current = audioQueue.Dequeue();

            // �����Դclick�Ƿ�������
            if (current.sourceClick != null && !current.sourceClick.IsVisible)
                continue;

            // ������Ƶ
            audioSource.clip = current.clip;
            audioSource.Play();

            // �ȴ��������
            while (audioSource.isPlaying)
                yield return null;
        }

        isPlaying = false;
    }

    /// <summary>
    /// ǿ���ж�������Ƶ����ն���
    /// </summary>
    public void StopAllAndClear()
    {
        audioSource.Stop();
        audioQueue.Clear();
        StopAllCoroutines();
        isPlaying = false;
    }

    // ��Ƶ�������ݽṹ
    private struct AudioPlayData
    {
        public AudioClip clip;
        public click_eft sourceClick;
        public bool isPriority;
    }
}
