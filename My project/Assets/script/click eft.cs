using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_eft : MonoBehaviour
{
    [Header("特效设置")]
    public Animator effectAnimator; // 特效动画控制器
    public GameObject model; // Click的模型（可选）

    private click_eft_base manager;
    private Collider2D clickCollider; // 碰撞体

    private void Awake()
    {
        // 获取碰撞体组件
        clickCollider = GetComponent<Collider2D>();
        if (clickCollider != null)
        {
            clickCollider.isTrigger = true; // 设置为触发器
        }

        // 如果未指定模型，默认使用自身
        if (model == null)
        {
            model = gameObject;
        }
    }

    // 设置管理器引用
    public void SetManager(click_eft_base manager)
    {
        this.manager = manager;
    }

    // 显示Click
    public void Show()
    {
        if (model != null)
        {
            model.SetActive(true);
        }

        if (clickCollider != null)
        {
            clickCollider.enabled = true;
        }
    }

    // 隐藏Click
    public void Hide()
    {
        if (model != null)
        {
            model.SetActive(false);
        }

        if (clickCollider != null)
        {
            clickCollider.enabled = false;
        }
    }

    // 播放特效
    public void PlayEffect()
    {
        if (effectAnimator != null)
        {
            effectAnimator.SetTrigger("click");
        }
    }

    // 检测角色碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否与角色碰撞
        if (manager != null && other.gameObject == manager.player)
        {
            // 通知管理器切换到下一个Click
            manager.NextClick();
        }
    }

    // 空格键触发特效
    private void Update()
    {
        // 只有当Click可见时才响应空格键
        if (model != null && model.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            PlayEffect();
        }
    }
}
