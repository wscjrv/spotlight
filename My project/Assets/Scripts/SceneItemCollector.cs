using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneItemCollector : MonoBehaviour
{
    [Header("��ǰ��������")]
    [Tooltip("��ǰ������Ҫ�ռ��ĵ�������")]
    public int requiredItems = 3; // ÿ�������ɵ������ã��糡��1��3������2��2��

    [Tooltip("�ռ���ɺ��ӳټ�����ת")]
    public float delaySeconds = 2f;

    [Tooltip("��ǰ�����ռ���ɺ���ת��Ŀ�곡����")]
    public string targetScene; // ÿ�����������ò�ͬĿ�꣨�糡��1��Scene2������2��Scene3��

    private int collectedCount = 0; // ��ǰ�������ռ�����
    private bool isComplete = false; // �Ƿ��ռ����
    private bool isSwitching = false; // ��ֹ�ظ���ת

    // ���߱��ռ�ʱ���ã��ɵ��߽ű�������
    public void OnItemCollected()
    {
        if (isComplete) return; // ��������ټ���

        collectedCount++;
        Debug.Log($"��ǰ�������ռ���{collectedCount}/{requiredItems}");

        // ����Ƿ�ﵽĿ������
        if (collectedCount >= requiredItems)
        {
            isComplete = true;
            StartCoroutine(DelaySwitchScene());
        }
    }

    // �ӳ���ת����
    private System.Collections.IEnumerator DelaySwitchScene()
    {
        if (isSwitching || string.IsNullOrEmpty(targetScene)) yield break;

        isSwitching = true;
        Debug.Log($"�ռ���ɣ�{delaySeconds}�����ת��{targetScene}");

        yield return new WaitForSeconds(delaySeconds);

        try
        {
            SceneManager.LoadScene(targetScene);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"��תʧ�ܣ�{e.Message}�����鳡�����Ƿ���ȷ");
            isSwitching = false;
        }
    }
}