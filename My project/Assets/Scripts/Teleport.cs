using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Header("目标场景设置")]
    // 已修正为正确的场景名：SampleScene
    public string targetSceneName = "SampleScene";

    // 可选：若仍有问题，可填写场景在Build Settings中的索引（如1）
    public int targetSceneIndex = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测玩家进入
        if (other.CompareTag("Player"))
        {
            Debug.Log($"检测到玩家，准备传送至 {targetSceneName}");
            TeleportToScene();
        }
    }

    private void TeleportToScene()
    {
        try
        {
            // 优先使用索引加载（更可靠，避免拼写问题）
            if (targetSceneIndex != -1)
            {
                SceneManager.LoadScene(targetSceneIndex);
                Debug.Log($"通过索引 {targetSceneIndex} 传送成功");
            }
            else
            {
                // 用场景名加载（需确保名称和构建设置正确）
                SceneManager.LoadScene(targetSceneName);
                Debug.Log($"通过名称 {targetSceneName} 传送成功");
            }
        }
        catch (System.Exception e)
        {
            // 输出错误详情，方便排查
            Debug.LogError($"传送失败：{e.Message}\n请检查场景名和构建设置");
        }
    }
}
