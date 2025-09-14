using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        // 切换场景时不销毁这个物体
        DontDestroyOnLoad(gameObject);
    }
}

    // 加载场景的方法（外部调用）
