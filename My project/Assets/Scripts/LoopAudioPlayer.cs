using UnityEngine;

public class LoopAudioPlayer : MonoBehaviour
{
    [Header("循环音频设置")]
    public AudioClip audioClip;
    public float volume = 0.5f;
    public bool playOnStart = true;
    
    private AudioSource audioSource;
    
    private void Awake()
    {
        // 获取或添加AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 配置音频源
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = true; // 设置为循环播放
        audioSource.playOnAwake = false; // 不自动播放，由脚本控制
    }
    
    private void Start()
    {
        if (playOnStart && audioClip != null)
        {
            PlayLoop();
        }
    }
    
    /// <summary>开始循环播放</summary>
    public void PlayLoop()
    {
        if (audioClip != null)
        {
            audioSource.Play();
            Debug.Log("开始循环播放音频: " + audioClip.name);
        }
    }
    
    /// <summary>停止循环播放</summary>
    public void StopLoop()
    {
        audioSource.Stop();
        Debug.Log("停止循环播放音频");
    }
    
    /// <summary>设置音频剪辑</summary>
    public void SetAudioClip(AudioClip newClip)
    {
        audioClip = newClip;
        audioSource.clip = newClip;
    }
    
    /// <summary>设置音量</summary>
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }
}
