using UnityEngine;

public class click_eft : MonoBehaviour
{
    [Header("特效设置")]
    public Animator effectAnimator; // 点击特效动画器
    private Collider2D clickCollider; // 碰撞组件

    private ClickSequenceManager manager; // 管理器引用
    private int sequenceIndex; // 组索引

    private void Awake()
    {
        // 初始化碰撞组件
        clickCollider = GetComponent<Collider2D>();
        if (clickCollider != null)
        {
            clickCollider.isTrigger = true;
        }
    }

    /// <summary>设置管理器引用</summary>
    public void SetManager(ClickSequenceManager manager)
    {
        this.manager = manager;
    }

    /// <summary>设置组索引</summary>
    public void SetSequenceIndex(int index)
    {
        sequenceIndex = index;
    }

    /// <summary>显示元素（启用碰撞）</summary>
    public void Show()
    {
        if (clickCollider != null)
            clickCollider.enabled = true;
    }

    /// <summary>隐藏元素（禁用碰撞）</summary>
    public void Hide()
    {
        if (clickCollider != null)
            clickCollider.enabled = false;
    }

    /// <summary>播放特效</summary>
    public void PlayEffect()
    {
        effectAnimator?.SetTrigger("click");
    }

    /// <summary>判断是否可见（碰撞体启用即为可见）</summary>
    public bool IsVisible => clickCollider != null && clickCollider.enabled;

    /// <summary>玩家触碰触发</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (manager == null || other.gameObject != manager.player)
            return;

        if (manager.displayMode == DisplayMode.Sequential)
        {
            manager.NextSequence();
        }
        else
        {
            manager.OnElementTriggered(sequenceIndex);
        }
    }

    /// <summary>S键播放特效（仅可见时）</summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && IsVisible)
        {
            PlayEffect();
        }
    }
}