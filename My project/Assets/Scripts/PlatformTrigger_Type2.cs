using UnityEngine;
using System.Collections;

public class PlatformTrigger_Type2 : MonoBehaviour
{
    [Header("平台控制")]
    public PlatformController_Type2 platform; // 拖入平台对象

    [Header("贴图设置")]
    [Tooltip("触发前显示的贴图")]
    public Sprite normalSprite;
    [Tooltip("触发后显示的贴图")]
    public Sprite triggeredSprite;
    [Tooltip("触发后贴图显示时长（秒）")]
    public float displayTime = 2f;

    [Header("组件引用")]
    [Tooltip("用于显示贴图的SpriteRenderer组件")]
    public SpriteRenderer spriteRenderer;

    private bool isTriggered = false;
    private Coroutine spriteChangeCoroutine;

    void Start()
    {
        // 如果没有指定SpriteRenderer，尝试获取当前对象的组件
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // 初始化显示正常贴图
        SetSprite(normalSprite);
    }

    // 角色触碰时触发下降（仅在平台处于初始状态时允许触发）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null && !isTriggered)
        {
            // 只有当平台不在下降、上升、停留状态时，才允许再次触发
            if (!platform.IsInMotion())
            {
                // 标记为已触发
                isTriggered = true;

                // 触发平台下降
                platform.StartDrop();

                // 开始贴图切换序列
                if (spriteChangeCoroutine != null)
                {
                    StopCoroutine(spriteChangeCoroutine);
                }
                spriteChangeCoroutine = StartCoroutine(SpriteChangeSequence());
            }
        }
    }

    private IEnumerator SpriteChangeSequence()
    {
        // 显示触发后的贴图
        SetSprite(triggeredSprite);

        // 等待指定时间
        yield return new WaitForSeconds(displayTime);

        // 恢复正常贴图
        SetSprite(normalSprite);

        // 重置触发状态
        isTriggered = false;

        spriteChangeCoroutine = null;
    }

    private void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    // 公共方法：手动重置触发器状态
    public void ResetTrigger()
    {
        isTriggered = false;
        if (spriteChangeCoroutine != null)
        {
            StopCoroutine(spriteChangeCoroutine);
            spriteChangeCoroutine = null;
        }
        SetSprite(normalSprite);
    }

    // 公共方法：手动触发（用于测试或外部调用）
    public void ManualTrigger()
    {
        if (platform != null && !isTriggered)
        {
            if (!platform.IsInMotion())
            {
                isTriggered = true;
                platform.StartDrop();
                
                if (spriteChangeCoroutine != null)
                {
                    StopCoroutine(spriteChangeCoroutine);
                }
                spriteChangeCoroutine = StartCoroutine(SpriteChangeSequence());
            }
        }
    }

    void OnDestroy()
    {
        // 清理协程
        if (spriteChangeCoroutine != null)
        {
            StopCoroutine(spriteChangeCoroutine);
        }
    }
}

