using UnityEngine;

public class click_eft : MonoBehaviour
{
    [Header("特效设置")]
    public Animator effectAnimator;
    [Header("音频设置")]
    public AudioClip audiokey;   // 按键音频（当前音频组）
    public AudioClip audiotouch; // 触碰音频（当前音频组）

    private Collider2D clickCollider;
    private ClickSequenceManager manager;
    private int sequenceIndex; // 自身索引（仅用于隐藏）

    private void Awake()
    {
        clickCollider = GetComponent<Collider2D>();
        if (clickCollider != null)
            clickCollider.isTrigger = true;
    }

    public void SetManager(ClickSequenceManager manager)
    {
        this.manager = manager;
    }

    public void SetSequenceIndex(int index)
    {
        sequenceIndex = index;
    }

    public void Show()
    {
        if (clickCollider != null)
            clickCollider.enabled = true;
    }

    public void Hide()
    {
        if (clickCollider != null)
            clickCollider.enabled = false;
    }

    public void PlayEffect()
    {
        effectAnimator?.SetTrigger("click");
    }

    public bool IsVisible => clickCollider != null && clickCollider.enabled;

    /// <summary>触碰触发：通知管理器处理（使用当前音频组）</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (manager == null || other.gameObject != manager.player)
            return;

        PlayEffect(); // 播放自身动画（与音频组无关）

        if (manager.displayMode == DisplayMode.Sequential)
        {
            // 顺序模式：使用自身音频
            AudioPlayer.Instance.PlayAudio(this, TriggerType.Touch);
            manager.NextSequence();
        }
        else
        {
            // 同时模式：由管理器决定播放哪个音频组
            manager.OnElementTriggered(sequenceIndex);
        }
    }

    /// <summary>按键触发：通知管理器处理（使用当前音频组）</summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && IsVisible)
        {
            PlayEffect(); // 播放自身动画（与音频组无关）

            if (manager.displayMode == DisplayMode.Sequential)
            {
                // 顺序模式：使用自身音频
                AudioPlayer.Instance.PlayAudio(this, TriggerType.Key);
            }
            else
            {
                // 同时模式：由管理器决定播放哪个音频组
                manager.HandleKeyTriggerInAllAtOnce();
            }
        }
    }
}
