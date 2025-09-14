// SwitchableGround.cs 修改后
using UnityEngine;

public class SwitchableGround : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private SpriteRenderer groundRenderer;

    [Header("初始状态设置")]
    public bool isActiveByDefault = false; // 默认是否有碰撞体

    [Header("状态颜色")]
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f); // 无碰撞体时颜色
    public Color activeColor = new Color(0.8f, 0.8f, 0.8f);   // 有碰撞体时颜色

    // 公开属性用于获取当前状态
    public bool IsActive { get; private set; }

    void Start()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundRenderer = GetComponent<SpriteRenderer>();

        // 根据初始设置初始化状态
        IsActive = isActiveByDefault;
        UpdateGroundState();
    }

    // 切换地面碰撞状态
    public void ToggleGround()
    {
        IsActive = !IsActive;
        UpdateGroundState();
    }

    // 更新地面的碰撞体和颜色状态
    private void UpdateGroundState()
    {
        if (groundCollider != null)
        {
            groundCollider.enabled = IsActive;
        }
        if (groundRenderer != null)
        {
            groundRenderer.color = IsActive ? activeColor : inactiveColor;
        }
    }
}