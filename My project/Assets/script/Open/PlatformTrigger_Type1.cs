using UnityEngine;

public class PlatformTrigger_Type1 : MonoBehaviour
{
    public PlatformController_Type1 platform; // �����Ӧƽ̨
    private bool hasTriggered = false; // ��ֹ�ظ�����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null && !hasTriggered)
        {
            platform.TriggerDrop();
            hasTriggered = true; // ������һ��
        }
    }

    // ��ʵ��OnTriggerExit2D��ȷ���뿪��ƽ̨����λ
}
