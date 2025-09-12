using System.Collections.Generic;
using UnityEngine;

public class ClickSequenceManager : MonoBehaviour
{
    public Animator maskanimation;
    [Header("序列配置")]
    [Tooltip("click对象列表，需与clickse列表数量一致且顺序对应")]
    public List<click_eft> clickList = new List<click_eft>();

    [Tooltip("clickse对象列表，需与click列表数量一致且顺序对应")]
    public List<clickse_eft> clickseList = new List<clickse_eft>();

    public GameObject player; // 玩家角色

    private int currentSequenceIndex = 0; // 当前激活的序列索引

    private void Start()
    {
        if (InitializeSequence())
        {
            ShowCurrentSequence();
        }
    }

    // 初始化序列（检查合法性并隐藏所有对象）
    private bool InitializeSequence()
    {
        // 检查两组数量是否一致
        if (clickList.Count != clickseList.Count)
        {
            Debug.LogError("Error: click与clickse数量不匹配！");
            return false;
        }

        // 检查列表是否为空
        if (clickList.Count == 0)
        {
            Debug.LogWarning("Warning: 未添加任何click或clickse对象！");
            return false;
        }

        // 初始化所有click（设置管理器并隐藏）
        foreach (var click in clickList)
        {
            click.SetManager(this);
            click.Hide();
        }

        // 隐藏所有clickse
        foreach (var clickse in clickseList)
        {
            clickse.Hide();
        }

        return true;
    }

    // 显示当前索引的click和clickse
    private void ShowCurrentSequence()
    {

        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex].Show();
            clickseList[currentSequenceIndex].Show();
            clickList[currentSequenceIndex].PlayEffect();
        }
    }

    // 隐藏当前索引的click和clickse
    private void HideCurrentSequence()
    {
        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex].Hide();
            clickseList[currentSequenceIndex].Hide();
        }
    }

    // 切换到下一组序列
    public void NextSequence()
    {
        HideCurrentSequence();
        currentSequenceIndex++;

        if (IsIndexValid(currentSequenceIndex))
        {
            ShowCurrentSequence();
        }
        else
        {
            OnAllSequencesCompleted();
        }
    }

    // 检查索引是否合法
    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < clickList.Count;
    }

    // 所有序列完成后的回调（留白扩展）
    private void OnAllSequencesCompleted()
    {
        Debug.Log("所有click/clickse序列已完成！");
        maskanimation.SetTrigger("ifmaskout");
        // TODO: 此处添加所有序列完成后的逻辑
    }
}