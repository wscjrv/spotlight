using UnityEngine;

public class PlatformTrigger_Type2 : MonoBehaviour
{
    public PlatformController_Type2 platform; // 拖入平台对象

    // 角色触碰时触发下降（仅在平台处于初始状态时允许触发）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null)
        {
            // 只有当平台不在下降、上升、停留状态时，才允许再次触发
            if (!platform.IsInMotion())
            {
                platform.StartDrop();
            }
        }
    }
}