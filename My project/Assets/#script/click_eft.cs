using UnityEngine;

public class click_eft : MonoBehaviour
{
    [Header("特效设置")]
    public Animator effectAnimator; // 点击特效动画器
    private Collider2D clickCollider; // 碰撞组件

    [Header("音频设置")]
    public AudioClip audiotouch; // 玩家触碰时播放的音频（优先级高）
    public AudioClip audiokey; // 按键触发时播放的音频（优先级低）

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

    /// <summary>播放特效动画</summary>
    private void PlayEffect()
    {
        effectAnimator?.SetTrigger("click");
    }

    /// <summary>通过玩家触碰播放特效和音频1（优先级）</summary>
    public void PlayByTouch()
    {
        PlayEffect();
        if (audiotouch != null)
        {
            AudioQueueManager.Instance.EnqueueAudio(audiotouch, this, true);
        }
    }

    /// <summary>通过按键播放特效和音频2（普通优先级）</summary>
    public void PlayByKey()
    {
        PlayEffect();
        // 音频2为null时使用音频1
        AudioClip targetAudio = audiokey ?? audiotouch;
        if (targetAudio != null)
        {
            AudioQueueManager.Instance.EnqueueAudio(targetAudio, this, false);
        }
    }

    /// <summary>判断是否可见（碰撞体启用即为可见）</summary>
    public bool IsVisible => clickCollider != null && clickCollider.enabled;

    /// <summary>玩家触碰触发</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (manager == null || other.gameObject != manager.player)
            return;

        PlayByTouch(); // 播放音频1（优先级）

        // 执行序列切换逻辑
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
            PlayByKey(); // 播放音频2或音频1（普通优先级）
        }
    }
}
