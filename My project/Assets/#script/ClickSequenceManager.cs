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
    public List<click_eft> clickList = new List<click_eft>(); // �����˳��洢��click�б�
    public List<clickse_eft> clickseList = new List<clickse_eft>();
    public GameObject player;
    public Animator maskanimation;

    [Header("�����Ƶ")]
    public AudioClip completionAudio;

    [Header("��ʼ����Ƶ����")]
    public bool playInitAudio = true; // �Ƿ��ڳ�ʼ��ʱ������Ƶ
    public TriggerType initAudioType = TriggerType.Key; // ��ʼ��ʱ���ŵ���Ƶ����

    // ˳��ģʽ����
    private int currentSequenceIndex = 0;

    // ͬʱģʽ���ı�����������Ƶ˳��
    private int totalCount = 0;
    private int completedCount = 0;
    private int currentAudioGroupIndex = 0; // ��¼��ǰӦ���ŵ���Ƶ�������������˳��
    private List<bool> isClickCompleted; // ���ÿ��click�Ƿ�����ʧ

    private void Start()
    {
        InitializeAllElements();
        StartModeLogic();

        // ��ʼ����ɺ󲥷����пɼ�click����Ƶ
        if (playInitAudio)
        {
            PlayInitialVisibleClicksAudio();
        }
    }

    /// <summary>��ʼ������Ԫ�أ��ص��ʼ��ͬʱģʽ��״̬����</summary>
    private void InitializeAllElements()
    {
        totalCount = clickList.Count;
        isClickCompleted = new List<bool>(totalCount); // ��ʼ��״̬�б�

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

            // ��ʼ�����״̬��Ĭ��δ��ɣ�
            isClickCompleted.Add(false);
        }

        // ����״̬����
        currentSequenceIndex = 0;
        completedCount = 0;
        currentAudioGroupIndex = 0; // �ӵ�һ����Ƶ�鿪ʼ
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
            ShowAllAtOnce(); // ͬʱ��ʾ����click
        }
    }

    /// <summary>���ų�ʼ���׶����пɼ�click����Ƶ</summary>
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

    /// <summary>����ͬʱģʽ�µİ������������ŵ�ǰ��Ƶ���audiokey��</summary>
    public void HandleKeyTriggerInAllAtOnce()
    {
        if (displayMode != DisplayMode.AllAtOnce) return;

        // ���ڵ�ǰ��Ƶ����Ч��δ���ʱ����
        if (IsAudioGroupValid())
        {
            click_eft currentClick = clickList[currentAudioGroupIndex];
            if (currentClick != null && currentClick.IsVisible)
            {
                // ���ŵ�ǰ��Ƶ��İ�����Ƶ
                AudioPlayer.Instance.PlayAudio(currentClick, TriggerType.Key);
            }
        }
    }

    /// <summary>����ͬʱģʽ�µĴ������������ŵ�ǰ��Ƶ���audiotouch��</summary>
    /// <param name="triggeredIndex">��������click���������������أ�����Ƶ˳���޹أ�</param>
    public void OnElementTriggered(int triggeredIndex)
    {
        if (displayMode != DisplayMode.AllAtOnce || !IsIndexValid(triggeredIndex) || isClickCompleted[triggeredIndex])
            return;

        // ���ŵ�ǰ��Ƶ��Ĵ�����Ƶ����ʵ�ʴ�����click�޹أ�
        if (IsAudioGroupValid())
        {
            click_eft currentClick = clickList[currentAudioGroupIndex];
            if (currentClick != null)
            {
                AudioPlayer.Instance.PlayAudio(currentClick, TriggerType.Touch);
            }
        }

        // ���ر�������click�����Ϊ����ɣ�
        clickList[triggeredIndex]?.Hide();
        clickseList[triggeredIndex]?.Hide();
        isClickCompleted[triggeredIndex] = true;
        completedCount++;

        // �ƽ�����һ����Ƶ�飨���۴��������ĸ�click��
        currentAudioGroupIndex++;

        // ����Ƿ�����click�������
        if (completedCount >= totalCount)
        {
            OnAllCompleted();
        }
    }

    /// <summary>��鵱ǰ��Ƶ���Ƿ���Ч��δԽ���Ҷ�Ӧclickδ��������</summary>
    private bool IsAudioGroupValid()
    {
        return currentAudioGroupIndex < totalCount && clickList[currentAudioGroupIndex] != null;
    }
    #endregion

    /// <summary>������Ч�Լ��</summary>
    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < totalCount;
    }

    /// <summary>���н�����ɴ���</summary>
    private void OnAllCompleted()
    {
        maskanimation?.SetTrigger("ifmaskout");
        AudioPlayer.Instance.PlayCompleteAudio(completionAudio);
    }
}
