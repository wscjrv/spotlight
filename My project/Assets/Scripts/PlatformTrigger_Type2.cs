using UnityEngine;

public class PlatformTrigger_Type2 : MonoBehaviour
{
    public PlatformController_Type2 platform; // �����Ӧƽ̨
    public float delayTime = 2f; // �ӳ�ʱ�䣨�룩������Inspector����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null)
        {
            platform.Drop(); // ����ʱ�����½�
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null)
        {
            // �뿪���ӳ�ָ��ʱ��������
            Invoke("TriggerRise", delayTime);
        }
    }

    // �ӳٺ�ִ������
    private void TriggerRise()
    {
        platform.Rise();
    }
}
