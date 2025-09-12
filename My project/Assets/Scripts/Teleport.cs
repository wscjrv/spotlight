using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Header("Ŀ�곡������")]
    // ������Ϊ��ȷ�ĳ�������SampleScene
    public string targetSceneName = "SampleScene";

    // ��ѡ�����������⣬����д������Build Settings�е���������1��
    public int targetSceneIndex = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �����ҽ���
        if (other.CompareTag("Player"))
        {
            Debug.Log($"��⵽��ң�׼�������� {targetSceneName}");
            TeleportToScene();
        }
    }

    private void TeleportToScene()
    {
        try
        {
            // ����ʹ���������أ����ɿ�������ƴд���⣩
            if (targetSceneIndex != -1)
            {
                SceneManager.LoadScene(targetSceneIndex);
                Debug.Log($"ͨ������ {targetSceneIndex} ���ͳɹ�");
            }
            else
            {
                // �ó��������أ���ȷ�����ƺ͹���������ȷ��
                SceneManager.LoadScene(targetSceneName);
                Debug.Log($"ͨ������ {targetSceneName} ���ͳɹ�");
            }
        }
        catch (System.Exception e)
        {
            // ����������飬�����Ų�
            Debug.LogError($"����ʧ�ܣ�{e.Message}\n���鳡�����͹�������");
        }
    }
}
