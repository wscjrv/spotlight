using UnityEngine;

/// <summary>
/// 物品层级修复器 - 修复被错误放入DontDestroyOnLoad的物品
/// </summary>
public class ItemHierarchyFixer : MonoBehaviour
{
    [Header("修复设置")]
    [Tooltip("是否在Start时自动修复物品层级")]
    public bool autoFixOnStart = true;
    [Tooltip("是否在每次Update时检查并修复")]
    public bool continuousFix = false;
    
    [Header("调试")]
    [Tooltip("显示修复日志")]
    public bool showDebugLogs = true;

    void Start()
    {
        if (autoFixOnStart)
        {
            FixItemHierarchy();
        }
    }

    void Update()
    {
        if (continuousFix)
        {
            FixItemHierarchy();
        }
    }

    /// <summary>
    /// 修复物品层级结构
    /// </summary>
    [ContextMenu("修复物品层级")]
    public void FixItemHierarchy()
    {
        // 查找所有CollectibleItem
        CollectibleItem[] allItems = FindObjectsOfType<CollectibleItem>();
        int fixedCount = 0;

        foreach (var item in allItems)
        {
            if (IsItemInWrongHierarchy(item))
            {
                MoveItemToCorrectHierarchy(item);
                fixedCount++;
            }
        }

        if (showDebugLogs && fixedCount > 0)
        {
            Debug.Log($"修复了 {fixedCount} 个物品的层级结构");
        }
    }

    /// <summary>
    /// 检查物品是否在错误的层级结构中
    /// </summary>
    private bool IsItemInWrongHierarchy(CollectibleItem item)
    {
        if (item == null) return false;
        
        Transform root = item.transform.root;
        return root.name == "DontDestroyOnLoad" || 
               root.name == "0" || 
               root.name.StartsWith("Music");
    }

    /// <summary>
    /// 将物品移动到正确的层级结构
    /// </summary>
    private void MoveItemToCorrectHierarchy(CollectibleItem item)
    {
        if (item == null) return;

        // 创建或找到场景根节点
        GameObject sceneRoot = GetOrCreateSceneRoot();
        
        // 移动物品到场景根节点
        item.transform.SetParent(sceneRoot.transform, true);
        
        // 确保物品是激活状态
        item.gameObject.SetActive(true);
        
        // 重置物品状态
        item.ResetItem();

        if (showDebugLogs)
        {
            Debug.Log($"已将物品 {item.gameObject.name} 移动到正确的层级结构");
        }
    }

    /// <summary>
    /// 获取或创建场景根节点
    /// </summary>
    private GameObject GetOrCreateSceneRoot()
    {
        GameObject sceneRoot = GameObject.Find("SceneRoot");
        if (sceneRoot == null)
        {
            sceneRoot = new GameObject("SceneRoot");
            if (showDebugLogs)
            {
                Debug.Log("创建了场景根节点 SceneRoot");
            }
        }
        return sceneRoot;
    }

    /// <summary>
    /// 清理DontDestroyOnLoad中的物品
    /// </summary>
    [ContextMenu("清理DontDestroyOnLoad中的物品")]
    public void CleanupDontDestroyOnLoadItems()
    {
        GameObject dontDestroyRoot = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyRoot == null) return;

        // 查找DontDestroyOnLoad中的所有物品
        CollectibleItem[] items = dontDestroyRoot.GetComponentsInChildren<CollectibleItem>();
        
        foreach (var item in items)
        {
            if (showDebugLogs)
            {
                Debug.Log($"发现DontDestroyOnLoad中的物品: {item.gameObject.name}");
            }
            
            // 销毁这些物品，因为它们不应该跨场景存在
            DestroyImmediate(item.gameObject);
        }

        if (showDebugLogs)
        {
            Debug.Log($"清理了 {items.Length} 个不应该在DontDestroyOnLoad中的物品");
        }
    }

    /// <summary>
    /// 完全重置场景中的物品
    /// </summary>
    [ContextMenu("重置所有物品")]
    public void ResetAllItems()
    {
        // 先修复层级结构
        FixItemHierarchy();
        
        // 然后重置所有物品状态
        CollectibleItem[] allItems = FindObjectsOfType<CollectibleItem>();
        foreach (var item in allItems)
        {
            item.ResetItem();
        }

        if (showDebugLogs)
        {
            Debug.Log($"重置了 {allItems.Length} 个物品的状态");
        }
    }
}
