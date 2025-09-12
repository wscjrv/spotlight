using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_eft_base : MonoBehaviour
{
    [Header("配置")]
    public List<click_eft> clickList = new List<click_eft>(); // Click列表（按出现顺序排列）
    public GameObject player; // 角色对象

    private int currentIndex = 0; // 当前显示的Click索引

    private void Start()
    {
        InitializeClicks();
        ShowCurrentClick();
    }

    // 初始化所有Click
    private void InitializeClicks()
    {
        if (clickList.Count == 0)
        {
            Debug.LogWarning("未设置Click列表，请在Inspector中添加Click对象");
            return;
        }

        // 隐藏所有Click并设置管理器引用
        foreach (var click in clickList)
        {
            click.SetManager(this);
            click.Hide();
        }
    }

    // 显示当前索引的Click
    private void ShowCurrentClick()
    {
        if (IsIndexValid(currentIndex))
        {
            clickList[currentIndex].Show();
        }
    }

    // 切换到下一个Click
    public void NextClick()
    {
        // 隐藏当前Click
        if (IsIndexValid(currentIndex))
        {
            clickList[currentIndex].Hide();
        }

        // 移动到下一个索引
        currentIndex++;

        // 检查是否还有下一个Click
        if (IsIndexValid(currentIndex))
        {
            ShowCurrentClick();
        }
        else
        {
            // 所有Click处理完毕，触发后续功能
            OnAllClicksCompleted();
        }
    }

    // 检查索引是否有效
    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < clickList.Count;
    }

    // 所有Click完成后的回调（留白，供扩展）
    private void OnAllClicksCompleted()
    {
        Debug.Log("所有Click已处理完毕，可以在此处添加后续功能");
        // TODO: 实现所有Click完成后的逻辑
    }
}
