using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_eft : MonoBehaviour
{
    [Header("特效设置")]
    public Animator effectAnimator; // 点击特效动画器
    public GameObject model; // click的显示模型

    private ClickSequenceManager manager; // 改为新的序列管理器
    private Collider2D clickCollider; // 碰撞检测组件

    private void Awake()
    {
        // 获取碰撞组件并设置为触发器
        clickCollider = GetComponent<Collider2D>();
        if (clickCollider != null)
        {
            clickCollider.isTrigger = true;
        }

        // 若未指定模型，默认使用自身
        if (model == null)
        {
            model = gameObject;
        }
    }

    // 设置管理器（修改参数类型为新管理器）
    public void SetManager(ClickSequenceManager manager)
    {
        this.manager = manager;
    }

    // 显示当前click
    public void Show()
    {
        if (model != null)
            model.SetActive(true);
        if (clickCollider != null)
            clickCollider.enabled = true;
    }

    // 隐藏当前click
    public void Hide()
    {
        if (model != null)
            model.SetActive(false);
        if (clickCollider != null)
            clickCollider.enabled = false;
    }

    // 播放点击特效
    public void PlayEffect()
    {
        if (effectAnimator != null)
        {
            effectAnimator.SetTrigger("click");
        }
    }

    // 玩家触碰时切换序列
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (manager != null && other.gameObject == manager.player)
        {
            manager.NextSequence(); // 调用新管理器的切换方法
        }
    }

    // 键播放特效（仅对可见状态有效）
    private void Update()
    {
        if (model != null && model.activeSelf && Input.GetKeyDown(KeyCode.S))
        {
            PlayEffect();
        }
    }
}