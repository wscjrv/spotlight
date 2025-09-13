using System.Collections.Generic;
using UnityEngine;

public enum DisplayMode
{
    Sequential, // 顺序模式
    AllAtOnce   // 同时模式
}

public class ClickSequenceManager : MonoBehaviour
{
    [Header("基本设置")]
    public DisplayMode displayMode = DisplayMode.Sequential;
    public List<click_eft> clickList = new List<click_eft>(); // 存储点击序列的click列表
    public List<clickse_eft> clickseList = new List<clickse_eft>();
    public GameObject player;
    public Animator maskanimation;

    [Header("交互完成后显示的物体")]
    public GameObject objectToReveal; // 用户自选的物体

    [Header("完成音效")]
    public AudioClip completionAudio;

    [Header("初始点击音效设置")]
    public bool playInitAudio = true; // 是否在初始化时播放音效
    public TriggerType initAudioType = TriggerType.Key; // 初始化时触发的音效类型

    // 顺序模式变量
    private int currentSequenceIndex = 0;

    // 同时模式变量
    private int totalCount = 0;
    private int completedCount = 0;
    private int currentAudioGroupIndex = 0;
    private List<bool> isClickCompleted;

    private void Start()
    {
        InitializeAllElements();
        StartModeLogic();

        // 初始化时播放可见点击的音效
        if (playInitAudio)
        {
            PlayInitialVisibleClicksAudio();
        }
    }

    /// <summary>初始化所有元素，包括初始隐藏目标物体</summary>
    private void InitializeAllElements()
    {
        totalCount = clickList.Count;
        isClickCompleted = new List<bool>(totalCount);

        for (int i = 0; i < totalCount; i++)
        {
            // 初始化click元素
            if (clickList[i] != null)
            {
                clickList[i].SetManager(this);
                clickList[i].SetSequenceIndex(i);
                clickList[i].Hide();
            }

            // 初始化clickse元素
            clickseList[i]?.Hide();

            isClickCompleted.Add(false);
        }

        // 初始化时隐藏用户选择的物体
        if (objectToReveal != null)
        {
            objectToReveal.SetActive(false);
        }

        // 重置状态变量
        currentSequenceIndex = 0;
        completedCount = 0;
        currentAudioGroupIndex = 0;
    }

    /// <summary>启动对应模式的逻辑</summary>
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

    /// <summary>播放初始阶段可见点击的音效</summary>
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

    #region 顺序模式逻辑
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

    #region 同时模式逻辑
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

    /// <summary>所有交互完成后执行的操作</summary>
    private void OnAllCompleted()
    {
        maskanimation?.SetTrigger("ifmaskout");
        AudioPlayer.Instance.PlayCompleteAudio(completionAudio);

        // 显示用户选择的物体
        if (objectToReveal != null)
        {
            objectToReveal.SetActive(true);
        }
    }
}