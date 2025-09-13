using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public enum DisplayMode
{
    Sequential, // ˳����ʾ
    AllAtOnce   // ͬʱ��ʾ
}

public class ClickSequenceManager : MonoBehaviour
{
    public Animator maskanimation;
    [Header("��ʾ����")]
    public DisplayMode displayMode = DisplayMode.Sequential;
    public List<click_eft> clickList = new List<click_eft>();
    public List<clickse_eft> clickseList = new List<clickse_eft>();
    public GameObject player;

    // ˳��ģʽ����
    private int currentSequenceIndex = 0;
    // ͬʱģʽ����
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

    /// <summary>��ʼ��Ԫ�أ����������ã���У�飩</summary>
    private void InitializeAllElements()
    {
        totalCount = clickList.Count;

        // ��ʼ��clickԪ��
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

        // ��ʼ��clickseԪ��
        foreach (var clickse in clickseList)
        {
            clickse?.Hide();
        }

        currentSequenceIndex = 0;
        completedCount = 0;
    }

    #region ˳��ģʽ
    private void ShowCurrentSequence()
    {
        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex]?.Show();
            clickseList[currentSequenceIndex]?.Show();
            clickList[currentSequenceIndex]?.PlayEffect();
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

    #region ͬʱģʽ
    private void ShowAllElements()
    {
        for (int i = 0; i < totalCount; i++)
        {
            clickList[i]?.Show();
            clickseList[i]?.Show();
            clickList[i]?.PlayEffect();
        }
    }

    public void OnElementTriggered(int index)
    {
        if (displayMode != DisplayMode.AllAtOnce || !IsIndexValid(index)) return;

        clickList[index]?.Hide();
        clickseList[index]?.Hide();
        clickList[index]?.PlayEffect();

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

    private void OnAllCompleted()
    {
        Debug.Log("����Ԫ�ش������");
        maskanimation?.SetTrigger("ifmaskout");
    }
}