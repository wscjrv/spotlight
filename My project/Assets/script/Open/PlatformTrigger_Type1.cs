using UnityEngine;

public class PlatformTrigger_Type1 : MonoBehaviour
{
    public PlatformController_Type1 platform; // 拖入对应平台
    private bool hasTriggered = false; // 防止重复触发

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null && !hasTriggered)
        {
            platform.TriggerDrop();
            hasTriggered = true; // 仅触发一次
        }
    }

    // 不实现OnTriggerExit2D，确保离开后平台不复位
}
