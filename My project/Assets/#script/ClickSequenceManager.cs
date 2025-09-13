using System.Collections.Generic;
using UnityEngine;

public enum DisplayMode
{
    Sequential, // 顺序显示
    AllAtOnce   // 同时显示
}

public class ClickSequenceManager : MonoBehaviour
{
    public Animator maskanimation;

    [Header("显示设置")]
    public DisplayMode displayMode = DisplayMode.Sequential;
    public List<click_eft> clickList = new List<click_eft>();
    public List<clickse_eft> clickseList = new List<clickse_eft>();
    public GameObject player;

    [Header("完成音频设置")]
    public AudioClip completionAudio; // 所有交互完成时播放的音频

    // 顺序模式变量
    private int currentSequenceIndex = 0;
    // 同时模式变量
    private int completedCount = 0;
    private int totalCount = 0;

    private void Start()
    {
        InitializeAllElements();

        if (displayMode == DisplayMode.Sequential)
        {
            ShowCurrentSequence();
        }
        else
        {
            ShowAllElements();
        }
    }

    /// <summary>初始化元素</summary>
    private void InitializeAllElements()
    {
        totalCount = clickList.Count;

        // 初始化click元素
        for (int i = 0; i < totalCount; i++)
        {
            var click = clickList[i];
            if (click != null)
            {
                click.SetManager(this);
                click.SetSequenceIndex(i);
                click.Hide();
            }
        }

        // 初始化clickse元素
        foreach (var clickse in clickseList)
        {
            clickse?.Hide();
        }

        currentSequenceIndex = 0;
        completedCount = 0;
    }

    #region 顺序模式
    private void ShowCurrentSequence()
    {
        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex]?.Show();
            clickseList[currentSequenceIndex]?.Show();
        }
    }

    private void HideCurrentSequence()
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

        HideCurrentSequence();
        currentSequenceIndex++;

        if (IsIndexValid(currentSequenceIndex))
        {
            ShowCurrentSequence();
        }
        else
        {
            OnAllCompleted();
        }
    }
    #endregion

    #region 同时模式
    private void ShowAllElements()
    {
        for (int i = 0; i < totalCount; i++)
        {
            clickList[i]?.Show();
            clickseList[i]?.Show();
        }
    }

    public void OnElementTriggered(int index)
    {
        if (displayMode != DisplayMode.AllAtOnce || !IsIndexValid(index)) return;

        clickList[index]?.Hide();
        clickseList[index]?.Hide();

        completedCount++;
        if (completedCount >= totalCount)
        {
            OnAllCompleted();
        }
    }
    #endregion

    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < totalCount;
    }

    /// <summary>所有交互完成处理</summary>
    private void OnAllCompleted()
    {
        Debug.Log("所有元素处理完成");
        maskanimation?.SetTrigger("ifmaskout");

        // 中断所有音频并播放完成音频
        AudioQueueManager.Instance.StopAllAndClear();
        if (completionAudio != null)
        {
            AudioQueueManager.Instance.EnqueueAudio(completionAudio, null, true);
        }
    }
}
