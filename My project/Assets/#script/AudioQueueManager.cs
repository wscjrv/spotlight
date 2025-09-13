using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioQueueManager : MonoBehaviour
{
    // 单例实例
    public static AudioQueueManager Instance { get; private set; }

    private AudioSource audioSource;
    private Queue<AudioPlayData> audioQueue = new Queue<AudioPlayData>();
    private bool isPlaying = false;

    private void Awake()
    {
        // 确保单例唯一性
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
    /// 将音频加入播放队列
    /// </summary>
    /// <param name="clip">音频片段</param>
    /// <param name="sourceClick">来源click</param>
    /// <param name="isPriority">是否为优先级音频（音频1）</param>
    public void EnqueueAudio(AudioClip clip, click_eft sourceClick, bool isPriority = false)
    {
        if (clip == null) return;

        var data = new AudioPlayData
        {
            clip = clip,
            sourceClick = sourceClick,
            isPriority = isPriority
        };

        // 优先级音频插入队列头部
        if (isPriority)
        {
            var tempQueue = new Queue<AudioPlayData>();
            tempQueue.Enqueue(data);
            while (audioQueue.Count > 0)
                tempQueue.Enqueue(audioQueue.Dequeue());
            audioQueue = tempQueue;

            // 立即中断当前播放，开始新队列
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
    /// 处理音频队列
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        isPlaying = true;

        while (audioQueue.Count > 0)
        {
            var current = audioQueue.Dequeue();

            // 检查来源click是否已隐藏
            if (current.sourceClick != null && !current.sourceClick.IsVisible)
                continue;

            // 播放音频
            audioSource.clip = current.clip;
            audioSource.Play();

            // 等待播放完成
            while (audioSource.isPlaying)
                yield return null;
        }

        isPlaying = false;
    }

    /// <summary>
    /// 强制中断所有音频并清空队列
    /// </summary>
    public void StopAllAndClear()
    {
        audioSource.Stop();
        audioQueue.Clear();
        StopAllCoroutines();
        isPlaying = false;
    }

    // 音频播放数据结构
    private struct AudioPlayData
    {
        public AudioClip clip;
        public click_eft sourceClick;
        public bool isPriority;
    }
}
