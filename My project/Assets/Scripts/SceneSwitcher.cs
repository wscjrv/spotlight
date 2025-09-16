using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [Header("ESC跳转设置")]
    public string targetSceneName; // 目标场景名（需在Build Settings中添加）
    public float pressWindow = 1.0f; // 三次按键的时间窗口（秒），超出则重置计数
    public bool stopAudioOnSwitch = true; // 跳转时是否停止当前音频

    private int escPressCount = 0; // ESC按键计数
    private float lastPressTime = 0; // 最后一次按键时间

    private void Update()
    {
        // 监听ESC键按下
        if (Input.GetKeyDown(KeyCode.Escape) && !string.IsNullOrEmpty(targetSceneName))
        {
            HandleEscPress();
        }

        // 超过时间窗口未完成三次按键，重置计数
        if (escPressCount > 0 && Time.time - lastPressTime > pressWindow)
        {
            ResetEscCount();
        }
    }

    /// <summary>处理ESC按键逻辑</summary>
    private void HandleEscPress()
    {
        escPressCount++;
        lastPressTime = Time.time; // 更新最后一次按键时间

        // 连按三次且在时间窗口内，触发跳转
        if (escPressCount >= 3)
        {
            SwitchToTargetScene();
            ResetEscCount(); // 跳转后重置计数
        }
        else
        {
            // 提示当前按键次数（可选，用于调试）
            Debug.Log($"已按ESC {escPressCount}/3次（{pressWindow}秒内完成）");
        }
    }

    /// <summary>重置ESC按键计数</summary>
    private void ResetEscCount()
    {
        escPressCount = 0;
        lastPressTime = 0;
    }

    /// <summary>跳转到指定场景</summary>
    private void SwitchToTargetScene()
    {
        // 跳转前停止音频（利用现有AudioPlayer逻辑）
        if (stopAudioOnSwitch && AudioPlayer.Instance != null)
        {
            AudioPlayer.Instance.PlayCompleteAudio(null); // 传null不播放完成音效，仅停止当前音频
        }

        try
        {
            SceneManager.LoadScene(targetSceneName);
            Debug.Log($"连按3次ESC成功，跳转至场景：{targetSceneName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"场景跳转失败：{e.Message}\n请检查场景名是否正确且已添加到Build Settings");
        }
    }
}
