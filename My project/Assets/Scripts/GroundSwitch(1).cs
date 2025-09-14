// GroundSwitch.cs 修改后
using UnityEngine;

public class GroundSwitch : MonoBehaviour
{
    [Header("关联对象")]
    public SwitchableGround targetGround; // 需要控制的地面

    [Header("开关状态精灵")]
    public Sprite inactiveSprite;  // 未激活时的精灵
    public Sprite activeSprite;    // 激活时的精灵

    private SpriteRenderer switchRenderer;
    private bool isTriggered = false; // 是否已触发（防止重复触发）

    void Start()
    {
        switchRenderer = GetComponent<SpriteRenderer>();
        // 初始化显示与地面初始状态匹配的精灵
        if (switchRenderer != null && targetGround != null)
        {
            switchRenderer.sprite = targetGround.IsActive ? activeSprite : inactiveSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测到玩家且未触发过
        if (other.CompareTag("Player") && !isTriggered && targetGround != null)
        {
            targetGround.ToggleGround(); // 切换地面碰撞状态
            isTriggered = true; // 标记为已触发

            // 更新开关精灵
            if (switchRenderer != null)
            {
                switchRenderer.sprite = targetGround.IsActive ? activeSprite : inactiveSprite;
            }
        }
    }
}