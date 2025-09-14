// GroundSwitch.cs �޸ĺ�
using UnityEngine;

public class GroundSwitch : MonoBehaviour
{
    [Header("��������")]
    public SwitchableGround targetGround; // ��Ҫ���Ƶĵ���

    [Header("����״̬����")]
    public Sprite inactiveSprite;  // δ����ʱ�ľ���
    public Sprite activeSprite;    // ����ʱ�ľ���

    private SpriteRenderer switchRenderer;
    private bool isTriggered = false; // �Ƿ��Ѵ�������ֹ�ظ�������

    void Start()
    {
        switchRenderer = GetComponent<SpriteRenderer>();
        // ��ʼ����ʾ������ʼ״̬ƥ��ľ���
        if (switchRenderer != null && targetGround != null)
        {
            switchRenderer.sprite = targetGround.IsActive ? activeSprite : inactiveSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��⵽�����δ������
        if (other.CompareTag("Player") && !isTriggered && targetGround != null)
        {
            targetGround.ToggleGround(); // �л�������ײ״̬
            isTriggered = true; // ���Ϊ�Ѵ���

            // ���¿��ؾ���
            if (switchRenderer != null)
            {
                switchRenderer.sprite = targetGround.IsActive ? activeSprite : inactiveSprite;
            }
        }
    }
}