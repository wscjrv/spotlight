using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShowSpriteWithMask : MonoBehaviour
{
    [Header("核心配置")]
    [Tooltip("要显示的图片（Sprite格式）")]
    public Sprite targetSprite;

    [Tooltip("图片显示时长（秒），0表示持续显示")]
    public float displayDuration = 3f;

    [Tooltip("是否只触发一次")]
    public bool triggerOnce = true;

    [Header("显示位置配置")]
    [Tooltip("相对于触发器的偏移位置（世界坐标）")]
    public Vector2 offsetFromTrigger = new Vector2(0, 1);

    [Tooltip("图片缩放比例")]
    public Vector3 spriteScale = new Vector3(1, 1, 1);

    [Header("渲染与遮罩配置")]
    [Tooltip("渲染层级名称（避免被遮挡）")]
    public string sortingLayerName = "Foreground";

    [Tooltip("同层级内的显示优先级")]
    public int sortingOrder = 10;

    [Tooltip("遮罩交互模式（默认跟随触发器所在遮罩）")]
    public SpriteMaskInteraction maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

    private SpriteRenderer spriteRenderer;
    private Collider2D triggerCollider;
    private bool hasTriggered = false;


    private void Awake()
    {
        // 初始化触发器
        triggerCollider = GetComponent<Collider2D>();
        triggerCollider.isTrigger = true;

        // 创建并初始化显示用的精灵（包含遮罩配置）
        CreateDisplaySprite();
    }


    // 动态创建场景中的精灵对象（支持遮罩同步）
    private void CreateDisplaySprite()
    {
        GameObject spriteObject = new GameObject("TriggerDisplaySprite");
        spriteObject.transform.parent = transform;
        spriteObject.transform.localPosition = offsetFromTrigger;
        spriteObject.transform.localScale = spriteScale;

        // 添加精灵渲染组件并配置基础属性
        spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = targetSprite;
        spriteRenderer.sortingLayerName = sortingLayerName;
        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.enabled = false;

        // 关键：同步遮罩设置
        SyncMaskSettings();
    }


    // 同步遮罩配置（让图片受触发器所在的遮罩影响）
    private void SyncMaskSettings()
    {
        // 1. 继承触发器所在物体的遮罩层（如果有）
        SpriteRenderer triggerRenderer = GetComponent<SpriteRenderer>();
        if (triggerRenderer != null)
        {
            // 若触发器本身有精灵渲染器，直接复用其遮罩层
            spriteRenderer.maskInteraction = triggerRenderer.maskInteraction;
        }
        else
        {
            // 否则使用自定义的遮罩交互模式
            spriteRenderer.maskInteraction = maskInteraction;
        }

        // 2. 确保图片与遮罩在同一渲染层级（遮罩只影响同层级物体）
        // （已通过sortingLayerName保证）
    }


    // 检测玩家进入触发区域
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            (!triggerOnce || !hasTriggered) &&
            targetSprite != null &&
            spriteRenderer != null)
        {
            ShowSprite();
            hasTriggered = true;
        }
    }


    // 显示图片
    private void ShowSprite()
    {
        spriteRenderer.enabled = true;

        if (displayDuration > 0)
        {
            StartCoroutine(HideAfterDelay(displayDuration));
        }
    }


    // 延迟隐藏图片
    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }


    // 编辑模式下绘制辅助线
    private void OnDrawGizmos()
    {
        // 绘制触发器范围
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Gizmos.DrawCube(transform.position + (Vector3)box.offset, (Vector3)box.size);
        }
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            Gizmos.DrawSphere(transform.position + (Vector3)circle.offset, circle.radius);
        }

        // 绘制图片显示位置标记
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)offsetFromTrigger, 0.1f);
    }
}
