using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 必须添加：场景跳转依赖的命名空间

public class UIManager : MonoBehaviour
{
    public Button startBtn;  // 开始按钮
    public Button quitBtn;   // 退出按钮

    void Start()
    {
        // 绑定按钮事件
        startBtn.onClick.AddListener(OnStartClick); // 开始按钮→跳转教学关
        quitBtn.onClick.AddListener(OnQuitClick);   // 退出按钮→退出游戏
    }

    // 开始按钮点击：跳转到Teach教学关
    public void OnStartClick()
    {
        // 加载名为"Teach"的场景（名称必须与保存的场景名完全一致）
        SceneManager.LoadScene("Teach");
    }

    // 退出按钮点击：退出游戏（保持之前的逻辑）
    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 编辑器停止
#else
        Application.Quit(); // 打包后退出
#endif
    }
}
