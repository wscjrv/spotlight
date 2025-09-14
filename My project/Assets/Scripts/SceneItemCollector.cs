using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneItemCollector : MonoBehaviour
{
    [Header("当前场景配置")]
    [Tooltip("当前场景需要收集的道具总数")]
    public int requiredItems = 3; // 每个场景可单独设置（如场景1填3，场景2填2）

    [Tooltip("收集完成后延迟几秒跳转")]
    public float delaySeconds = 2f;

    [Tooltip("当前场景收集完成后跳转的目标场景名")]
    public string targetScene; // 每个场景可设置不同目标（如场景1跳Scene2，场景2跳Scene3）

    private int collectedCount = 0; // 当前场景已收集数量
    private bool isComplete = false; // 是否收集完成
    private bool isSwitching = false; // 防止重复跳转

    // 道具被收集时调用（由道具脚本触发）
    public void OnItemCollected()
    {
        if (isComplete) return; // 已完成则不再计数

        collectedCount++;
        Debug.Log($"当前场景已收集：{collectedCount}/{requiredItems}");

        // 检查是否达到目标数量
        if (collectedCount >= requiredItems)
        {
            isComplete = true;
            StartCoroutine(DelaySwitchScene());
        }
    }

    // 延迟跳转场景
    private System.Collections.IEnumerator DelaySwitchScene()
    {
        if (isSwitching || string.IsNullOrEmpty(targetScene)) yield break;

        isSwitching = true;
        Debug.Log($"收集完成！{delaySeconds}秒后跳转到{targetScene}");

        yield return new WaitForSeconds(delaySeconds);

        try
        {
            SceneManager.LoadScene(targetScene);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"跳转失败：{e.Message}，请检查场景名是否正确");
            isSwitching = false;
        }
    }
}