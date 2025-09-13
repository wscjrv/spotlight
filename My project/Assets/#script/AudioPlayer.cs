using UnityEngine;

public enum TriggerType { Touch, Key }

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }

    [Header("中断时间设置（秒）")]
    public float timekey = 0.5f;   // 按键音频可被中断的最短时间
    public float timetouch = 0.3f; // 触摸音频可被中断的最短时间

    private AudioSource audioSource;
    private TriggerType currentType; // 当前播放的音频类型
    private float currentPlayTime;   // 当前音频已播放时间
    private bool isPlaying => audioSource.isPlaying;

    private void Awake()
    {
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

    private void Update()
    {
        // 更新当前音频播放时间
        if (isPlaying)
        {
            currentPlayTime += Time.deltaTime;
        }
    }

    /// <summary>播放音频（根据类型处理中断）</summary>
    public void PlayAudio(click_eft click, TriggerType type)
    {
        if (click == null) return;

        // 选择对应的音频
        AudioClip clip = type == TriggerType.Key
            ? (click.audiokey ?? click.audiotouch)
            : click.audiotouch;

        if (clip == null) return;

        // 触摸音频：最高优先级，立即中断所有
        if (type == TriggerType.Touch)
        {
            PlayImmediately(clip, type);
            return;
        }

        // 按键音频：检查是否可中断当前音频
        if (!isPlaying || CanInterruptCurrent(type))
        {
            PlayImmediately(clip, type);
        }
    }

    /// <summary>立即播放音频（中断当前）</summary>
    private void PlayImmediately(AudioClip clip, TriggerType type)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
        currentType = type;
        currentPlayTime = 0; // 重置播放时间
        Debug.Log($"播放音频: {clip.name}（类型: {type}）");
    }

    /// <summary>判断按键音频是否可中断当前音频</summary>
    private bool CanInterruptCurrent(TriggerType newType)
    {
        if (currentType == TriggerType.Key)
        {
            // 中断已播放超过timekey的按键音频（包括自身）
            return currentPlayTime >= timekey;
        }
        else if (currentType == TriggerType.Touch)
        {
            // 中断已播放超过timetouch的触摸音频
            return currentPlayTime >= timetouch;
        }
        return false;
    }

    /// <summary>播放完成音频（强制中断所有）</summary>
    public void PlayCompleteAudio(AudioClip completeClip)
    {
        if (completeClip == null) return;

        audioSource.Stop();
        audioSource.clip = completeClip;
        audioSource.Play();
        Debug.Log($"播放完成音频: {completeClip.name}");
    }
}
