using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        // �л�����ʱ�������������
        DontDestroyOnLoad(gameObject);
    }
}

    // ���س����ķ������ⲿ���ã�
