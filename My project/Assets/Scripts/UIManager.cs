using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ������ӣ�������ת�����������ռ�

public class UIManager : MonoBehaviour
{
    public Button startBtn;  // ��ʼ��ť
    public Button quitBtn;   // �˳���ť

    void Start()
    {
        // �󶨰�ť�¼�
        startBtn.onClick.AddListener(OnStartClick); // ��ʼ��ť����ת��ѧ��
        quitBtn.onClick.AddListener(OnQuitClick);   // �˳���ť���˳���Ϸ
    }

    // ��ʼ��ť�������ת��Teach��ѧ��
    public void OnStartClick()
    {
        // ������Ϊ"Teach"�ĳ��������Ʊ����뱣��ĳ�������ȫһ�£�
        SceneManager.LoadScene("Teach");
    }

    // �˳���ť������˳���Ϸ������֮ǰ���߼���
    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �༭��ֹͣ
#else
        Application.Quit(); // ������˳�
#endif
    }
}
