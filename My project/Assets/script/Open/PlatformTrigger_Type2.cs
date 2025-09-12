using UnityEngine;

public class PlatformTrigger_Type2 : MonoBehaviour
{
    public PlatformController_Type2 platform; // 拖入对应平台
    public float delayTime = 2f; // 延迟时间（秒），可在Inspector调整

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null)
        {
            platform.Drop(); // 进入时立即下降
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null)
        {
            // 离开后延迟指定时间再上升
            Invoke("TriggerRise", delayTime);
        }
    }

    // 延迟后执行上升
    private void TriggerRise()
    {
        platform.Rise();
    }
}
