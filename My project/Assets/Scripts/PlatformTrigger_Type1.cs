using UnityEngine;

public class PlatformTrigger_Type1 : MonoBehaviour
{
    [Header("开关状态精灵")]
    public Sprite inactiveSprite;  // 未激活时的精灵
    public Sprite activeSprite;    // 激活时的精灵

    [Header("关联平台")]
    public PlatformController_Type1 platform;    // 关联的平台

    private SpriteRenderer switchRenderer;  // 开关的精灵渲染器
    private bool hasTriggered = false;      // 防止重复触发

    private void Start()
    {
        // 初始化获取精灵渲染器组件
        switchRenderer = GetComponent<SpriteRenderer>();

        // 初始显示未激活精灵（如果组件存在）
        if (switchRenderer != null)
        {
            switchRenderer.sprite = inactiveSprite;
        }
        else
        {
            Debug.LogWarning("PlatformTx: 未找到SpriteRenderer组件，请添加！");
        }
    }
     // 拖入对应平台
    

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
