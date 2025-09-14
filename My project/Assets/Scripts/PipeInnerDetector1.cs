using UnityEngine;
public class PipeInnerDetector : MonoBehaviour
{
    private PipeTransparency pipeTransparency; // ���ùܵ���͸���ȿ��ƽű�
    void Start()
    {
        // �ҵ�������PipeChannel���ϵ�͸���Ƚű�
        pipeTransparency = GetComponentInParent<PipeTransparency>();
        if (pipeTransparency == null)
        {
            Debug.LogError("�ܵ���������δ���� PipeTransparency �ű���");
        }
    }
    // ���ǽ���ܵ��ڲ���������������
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && pipeTransparency != null)
        {
            pipeTransparency.SetPlayerInInner(true); // ֪ͨ�ܵ����������ڲ�
        }
    }
    // �����뿪�ܵ��ڲ�
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && pipeTransparency != null)
        {
            pipeTransparency.SetPlayerInInner(false); // ֪ͨ�ܵ��������뿪
        }
    }
}