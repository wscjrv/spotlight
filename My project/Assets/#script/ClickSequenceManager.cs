using System.Collections.Generic;
using UnityEngine;

public enum DisplayMode
{
    Sequential, // ˳��ģʽ
    AllAtOnce   // ͬʱģʽ
}

public class ClickSequenceManager : MonoBehaviour
{
    [Header("��������")]
    public DisplayMode displayMode = DisplayMode.Sequential;
    public List<click_eft> clickList = new List<click_eft>(); // �洢������е�click�б�
    public List<clickse_eft> clickseList = new List<clickse_eft>();
    public GameObject player;
    public Animator maskanimation;

    [Header("������ɺ���ʾ������")]
    public GameObject objectToReveal; // �û���ѡ������

    [Header("�����Ч")]
    public AudioClip completionAudio;

    [Header("��ʼ�����Ч����")]
    public bool playInitAudio = true; // �Ƿ��ڳ�ʼ��ʱ������Ч
    public TriggerType initAudioType = TriggerType.Key; // ��ʼ��ʱ��������Ч����

    // ˳��ģʽ����
    private int currentSequenceIndex = 0;

    // ͬʱģʽ����
    private int totalCount = 0;
    private int completedCount = 0;
    private int currentAudioGroupIndex = 0;
    private List<bool> isClickCompleted;

    private void Start()
    {
        InitializeAllElements();
        StartModeLogic();

        // ��ʼ��ʱ���ſɼ��������Ч
        if (playInitAudio)
        {
            PlayInitialVisibleClicksAudio();
        }
    }

    /// <summary>��ʼ������Ԫ�أ�������ʼ����Ŀ������</summary>
    private void InitializeAllElements()
    {
        totalCount = clickList.Count;
        isClickCompleted = new List<bool>(totalCount);

        for (int i = 0; i < totalCount; i++)
        {
            // ��ʼ��clickԪ��
            if (clickList[i] != null)
            {
                clickList[i].SetManager(this);
                clickList[i].SetSequenceIndex(i);
                clickList[i].Hide();
            }

            // ��ʼ��clickseԪ��
            clickseList[i]?.Hide();

            isClickCompleted.Add(false);
        }

        // ��ʼ��ʱ�����û�ѡ�������
        if (objectToReveal != null)
        {
            objectToReveal.SetActive(false);
        }

        // ����״̬����
        currentSequenceIndex = 0;
        completedCount = 0;
        currentAudioGroupIndex = 0;
    }

    /// <summary>������Ӧģʽ���߼�</summary>
    private void StartModeLogic()
    {
        if (displayMode == DisplayMode.Sequential)
        {
            ShowCurrentSequential();
        }
        else
        {
            ShowAllAtOnce();
        }
    }

    /// <summary>���ų�ʼ�׶οɼ��������Ч</summary>
    private void PlayInitialVisibleClicksAudio()
    {
        foreach (var click in clickList)
        {
            if (click != null && click.IsVisible)
            {
                AudioPlayer.Instance.PlayAudio(click, initAudioType);
            }
        }
    }

    #region ˳��ģʽ�߼�
    private void ShowCurrentSequential()
    {
        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex]?.Show();
            clickseList[currentSequenceIndex]?.Show();
            clickList[currentSequenceIndex]?.PlayEffect();
        }
    }

    private void HideCurrentSequential()
    {
        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex]?.Hide();
            clickseList[currentSequenceIndex]?.Hide();
        }
    }

    public void NextSequence()
    {
        if (displayMode != DisplayMode.Sequential) return;

        HideCurrentSequential();
        currentSequenceIndex++;

        if (IsIndexValid(currentSequenceIndex))
        {
            ShowCurrentSequential();
        }
        else
        {
            OnAllCompleted();
        }
    }
    #endregion

    #region ͬʱģʽ�߼�
    private void ShowAllAtOnce()
    {
        for (int i = 0; i < totalCount; i++)
        {
            clickList[i]?.Show();
            clickseList[i]?.Show();
            clickList[i]?.PlayEffect();
        }
    }

    public void HandleKeyTriggerInAllAtOnce()
    {
        if (displayMode != DisplayMode.AllAtOnce) return;

        if (IsAudioGroupValid())
        {
            click_eft currentClick = clickList[currentAudioGroupIndex];
            if (currentClick != null && currentClick.IsVisible)
            {
                AudioPlayer.Instance.PlayAudio(currentClick, TriggerType.Key);
            }
        }
    }

    public void OnElementTriggered(int triggeredIndex)
    {
        if (displayMode != DisplayMode.AllAtOnce || !IsIndexValid(triggeredIndex) || isClickCompleted[triggeredIndex])
            return;

        if (IsAudioGroupValid())
        {
            click_eft currentClick = clickList[currentAudioGroupIndex];
            if (currentClick != null)
            {
                AudioPlayer.Instance.PlayAudio(currentClick, TriggerType.Touch);
            }
        }

        clickList[triggeredIndex]?.Hide();
        clickseList[triggeredIndex]?.Hide();
        isClickCompleted[triggeredIndex] = true;
        completedCount++;

        currentAudioGroupIndex++;

        if (completedCount >= totalCount)
        {
            OnAllCompleted();
        }
    }

    private bool IsAudioGroupValid()
    {
        return currentAudioGroupIndex < totalCount && clickList[currentAudioGroupIndex] != null;
    }
    #endregion

    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < totalCount;
    }

    /// <summary>���н�����ɺ�ִ�еĲ���</summary>
    private void OnAllCompleted()
    {
        maskanimation?.SetTrigger("ifmaskout");
        AudioPlayer.Instance.PlayCompleteAudio(completionAudio);

        // ��ʾ�û�ѡ�������
        if (objectToReveal != null)
        {
            objectToReveal.SetActive(true);
        }
    }
}