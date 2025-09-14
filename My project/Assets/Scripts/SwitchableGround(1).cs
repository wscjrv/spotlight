// SwitchableGround.cs �޸ĺ�
using UnityEngine;

public class SwitchableGround : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private SpriteRenderer groundRenderer;

    [Header("��ʼ״̬����")]
    public bool isActiveByDefault = false; // Ĭ���Ƿ�����ײ��

    [Header("״̬��ɫ")]
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f); // ����ײ��ʱ��ɫ
    public Color activeColor = new Color(0.8f, 0.8f, 0.8f);   // ����ײ��ʱ��ɫ

    // �����������ڻ�ȡ��ǰ״̬
    public bool IsActive { get; private set; }

    void Start()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundRenderer = GetComponent<SpriteRenderer>();

        // ���ݳ�ʼ���ó�ʼ��״̬
        IsActive = isActiveByDefault;
        UpdateGroundState();
    }

    // �л�������ײ״̬
    public void ToggleGround()
    {
        IsActive = !IsActive;
        UpdateGroundState();
    }

    // ���µ������ײ�����ɫ״̬
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