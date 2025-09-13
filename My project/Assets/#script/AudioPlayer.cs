using UnityEngine;

public enum TriggerType { Touch, Key }

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }

    [Header("�ж�ʱ�����ã��룩")]
    public float timekey = 0.5f;   // ������Ƶ�ɱ��жϵ����ʱ��
    public float timetouch = 0.3f; // ������Ƶ�ɱ��жϵ����ʱ��

    private AudioSource audioSource;
    private TriggerType currentType; // ��ǰ���ŵ���Ƶ����
    private float currentPlayTime;   // ��ǰ��Ƶ�Ѳ���ʱ��
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
        // ���µ�ǰ��Ƶ����ʱ��
        if (isPlaying)
        {
            currentPlayTime += Time.deltaTime;
        }
    }

    /// <summary>������Ƶ���������ʹ����жϣ�</summary>
    public void PlayAudio(click_eft click, TriggerType type)
    {
        if (click == null) return;

        // ѡ���Ӧ����Ƶ
        AudioClip clip = type == TriggerType.Key
            ? (click.audiokey ?? click.audiotouch)
            : click.audiotouch;

        if (clip == null) return;

        // ������Ƶ��������ȼ��������ж�����
        if (type == TriggerType.Touch)
        {
            PlayImmediately(clip, type);
            return;
        }

        // ������Ƶ������Ƿ���жϵ�ǰ��Ƶ
        if (!isPlaying || CanInterruptCurrent(type))
        {
            PlayImmediately(clip, type);
        }
    }

    /// <summary>����������Ƶ���жϵ�ǰ��</summary>
    private void PlayImmediately(AudioClip clip, TriggerType type)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
        currentType = type;
        currentPlayTime = 0; // ���ò���ʱ��
        Debug.Log($"������Ƶ: {clip.name}������: {type}��");
    }

    /// <summary>�жϰ�����Ƶ�Ƿ���жϵ�ǰ��Ƶ</summary>
    private bool CanInterruptCurrent(TriggerType newType)
    {
        if (currentType == TriggerType.Key)
        {
            // �ж��Ѳ��ų���timekey�İ�����Ƶ����������
            return currentPlayTime >= timekey;
        }
        else if (currentType == TriggerType.Touch)
        {
            // �ж��Ѳ��ų���timetouch�Ĵ�����Ƶ
            return currentPlayTime >= timetouch;
        }
        return false;
    }

    /// <summary>���������Ƶ��ǿ���ж����У�</summary>
    public void PlayCompleteAudio(AudioClip completeClip)
    {
        if (completeClip == null) return;

        audioSource.Stop();
        audioSource.clip = completeClip;
        audioSource.Play();
        Debug.Log($"���������Ƶ: {completeClip.name}");
    }
}
