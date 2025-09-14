using UnityEngine;

public class PlatformTx : MonoBehaviour
{
    [Header("开关状态精灵")]
    public Sprite inactiveSprite;  // 未激活时的精灵
    public Sprite activeSprite;    // 激活时的精灵

    [Header("关联平台")]
    public PlatformCx platform;    // 关联的平台

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查条件：玩家触发 + 关联平台存在 + 未触发过
        if (other.CompareTag("Player") && platform != null && !hasTriggered && switchRenderer != null)
        {
            switchRenderer.sprite = activeSprite;  // 切换为激活状态精灵
            platform.TriggerDrop();       // 触发平台动作
            hasTriggered = true;          // 标记为已触发，防止重复
        }
    }
}
