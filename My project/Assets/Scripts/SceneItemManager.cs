using UnityEngine;

/// <summary>
/// 场景物品管理器 - 确保每个场景都有正确的物品收集器
/// </summary>
public class SceneItemManager : MonoBehaviour
{
    [Header("场景设置")]
    [Tooltip("当前场景需要收集的物品数量")]
    public int requiredItems = 3;
    [Tooltip("收集完成后延迟切换场景的秒数")]
    public float delaySeconds = 2f;
    [Tooltip("收集完成后要切换到的目标场景")]
    public string targetScene;

    [Header("调试")]
    [Tooltip("是否在收集完成后自动切换场景")]
    public bool autoSwitchScene = true;

    private SceneItemCollector collector;
    private int collectedCount = 0;
    private bool isComplete = false;

    void Start()
    {
        // 确保场景中有SceneItemCollector
        collector = FindObjectOfType<SceneItemCollector>();
        if (collector == null)
        {
            // 如果没有找到，创建一个
            GameObject collectorObj = new GameObject("SceneItemCollector");
            collector = collectorObj.AddComponent<SceneItemCollector>();
            
            // 设置参数
            collector.requiredItems = requiredItems;
            collector.delaySeconds = delaySeconds;
            collector.targetScene = targetScene;
            
            Debug.Log("自动创建了SceneItemCollector");
        }
        else
        {
            // 如果找到了，同步设置
            collector.requiredItems = requiredItems;
            collector.delaySeconds = delaySeconds;
            collector.targetScene = targetScene;
        }

        // 重置所有物品状态
        ResetAllItems();
    }

    /// <summary>
    /// 重置场景中所有物品的收集状态
    /// </summary>
    private void ResetAllItems()
    {
        CollectibleItem[] items = FindObjectsOfType<CollectibleItem>();
        foreach (var item in items)
        {
            // 通过反射重置私有字段，或者添加公共重置方法
            item.gameObject.SetActive(true);
        }
        Debug.Log($"重置了 {items.Length} 个物品的显示状态");
    }

    /// <summary>
    /// 手动触发物品收集（用于测试）
    /// </summary>
    [ContextMenu("测试物品收集")]
    public void TestItemCollection()
    {
        if (collector != null)
        {
            collector.OnItemCollected();
        }
    }

    /// <summary>
    /// 手动重置所有物品
    /// </summary>
    [ContextMenu("重置所有物品")]
    public void ManualResetAllItems()
    {
        ResetAllItems();
        collectedCount = 0;
        isComplete = false;
    }
}

