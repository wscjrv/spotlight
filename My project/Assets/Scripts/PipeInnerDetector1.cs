using UnityEngine;
public class PipeInnerDetector : MonoBehaviour
{
    private PipeTransparency pipeTransparency; // 引用管道的透明度控制脚本
    void Start()
    {
        // 找到父对象（PipeChannel）上的透明度脚本
        pipeTransparency = GetComponentInParent<PipeTransparency>();
        if (pipeTransparency == null)
        {
            Debug.LogError("管道父对象上未挂载 PipeTransparency 脚本！");
        }
    }
    // 主角进入管道内部（触发触发器）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && pipeTransparency != null)
        {
            pipeTransparency.SetPlayerInInner(true); // 通知管道：主角在内部
        }
    }
    // 主角离开管道内部
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && pipeTransparency != null)
        {
            pipeTransparency.SetPlayerInInner(false); // 通知管道：主角离开
        }
    }
}