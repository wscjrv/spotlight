using System.Collections.Generic;
using UnityEngine;

public enum DisplayMode
{
    Sequential, // 顺序模式
    AllAtOnce   // 同时模式
}

public class ClickSequenceManager : MonoBehaviour
{
    [Header("基础设置")]
    public DisplayMode displayMode = DisplayMode.Sequential;
    public List<click_eft> clickList = new List<click_eft>(); // 按添加顺序存储的click列表
    public List<clickse_eft> clickseList = new List<clickse_eft>();
    public GameObject player;
    public Animator maskanimation;

    [Header("完成音频")]
    public AudioClip completionAudio;

    [Header("初始化音频设置")]
    public bool playInitAudio = true; // 是否在初始化时播放音频
    public TriggerType initAudioType = TriggerType.Key; // 初始化时播放的音频类型

    // 顺序模式变量
    private int currentSequenceIndex = 0;

    // 同时模式核心变量（控制音频顺序）
    private int totalCount = 0;
    private int completedCount = 0;
    private int currentAudioGroupIndex = 0; // 记录当前应播放的音频组索引（按添加顺序）
    private List<bool> isClickCompleted; // 标记每个click是否已消失

    private void Start()
    {
        InitializeAllElements();
        StartModeLogic();

        // 初始化完成后播放所有可见click的音频
        if (playInitAudio)
        {
            PlayInitialVisibleClicksAudio();
        }
    }

    /// <summary>初始化所有元素，重点初始化同时模式的状态跟踪</summary>
    private void InitializeAllElements()
    {
        totalCount = clickList.Count;
        isClickCompleted = new List<bool>(totalCount); // 初始化状态列表

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

            // 初始化完成状态（默认未完成）
            isClickCompleted.Add(false);
        }

        // 重置状态变量
        currentSequenceIndex = 0;
        completedCount = 0;
        currentAudioGroupIndex = 0; // 从第一个音频组开始
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
            ShowAllAtOnce(); // 同时显示所有click
        }
    }

    /// <summary>播放初始化阶段所有可见click的音频</summary>
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

    /// <summary>处理同时模式下的按键触发（播放当前音频组的audiokey）</summary>
    public void HandleKeyTriggerInAllAtOnce()
    {
        if (displayMode != DisplayMode.AllAtOnce) return;

        // 仅在当前音频组有效且未完成时播放
        if (IsAudioGroupValid())
        {
            click_eft currentClick = clickList[currentAudioGroupIndex];
            if (currentClick != null && currentClick.IsVisible)
            {
                // 播放当前音频组的按键音频
                AudioPlayer.Instance.PlayAudio(currentClick, TriggerType.Key);
            }
        }
    }

    /// <summary>处理同时模式下的触碰触发（播放当前音频组的audiotouch）</summary>
    /// <param name="triggeredIndex">被触碰的click索引（仅用于隐藏，与音频顺序无关）</param>
    public void OnElementTriggered(int triggeredIndex)
    {
        if (displayMode != DisplayMode.AllAtOnce || !IsIndexValid(triggeredIndex) || isClickCompleted[triggeredIndex])
            return;

        // 播放当前音频组的触碰音频（与实际触碰的click无关）
        if (IsAudioGroupValid())
        {
            click_eft currentClick = clickList[currentAudioGroupIndex];
            if (currentClick != null)
            {
                AudioPlayer.Instance.PlayAudio(currentClick, TriggerType.Touch);
            }
        }

        // 隐藏被触碰的click（标记为已完成）
        clickList[triggeredIndex]?.Hide();
        clickseList[triggeredIndex]?.Hide();
        isClickCompleted[triggeredIndex] = true;
        completedCount++;

        // 推进到下一个音频组（无论触碰的是哪个click）
        currentAudioGroupIndex++;

        // 检查是否所有click都已完成
        if (completedCount >= totalCount)
        {
            OnAllCompleted();
        }
    }

    /// <summary>检查当前音频组是否有效（未越界且对应click未被跳过）</summary>
    private bool IsAudioGroupValid()
    {
        return currentAudioGroupIndex < totalCount && clickList[currentAudioGroupIndex] != null;
    }
    #endregion

    /// <summary>索引有效性检查</summary>
    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < totalCount;
    }

    /// <summary>所有交互完成处理</summary>
    private void OnAllCompleted()
    {
        maskanimation?.SetTrigger("ifmaskout");
        AudioPlayer.Instance.PlayCompleteAudio(completionAudio);
    }
}
