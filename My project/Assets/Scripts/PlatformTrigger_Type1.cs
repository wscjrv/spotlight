using UnityEngine;

public class PlatformTrigger_Type1 : MonoBehaviour
{
    [Header("����״̬����")]
    public Sprite inactiveSprite;  // δ����ʱ�ľ���
    public Sprite activeSprite;    // ����ʱ�ľ���

    [Header("����ƽ̨")]
    public PlatformController_Type1 platform;    // ������ƽ̨

    private SpriteRenderer switchRenderer;  // ���صľ�����Ⱦ��
    private bool hasTriggered = false;      // ��ֹ�ظ�����

    private void Start()
    {
        // ��ʼ����ȡ������Ⱦ�����
        switchRenderer = GetComponent<SpriteRenderer>();

        // ��ʼ��ʾδ����飨���������ڣ�
        if (switchRenderer != null)
        {
            switchRenderer.sprite = inactiveSprite;
        }
        else
        {
            Debug.LogWarning("PlatformTx: δ�ҵ�SpriteRenderer���������ӣ�");
        }
    }
     // �����Ӧƽ̨
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platform != null && !hasTriggered)
        {
            platform.TriggerDrop();
            hasTriggered = true; // ������һ��
        }
    }

    // ��ʵ��OnTriggerExit2D��ȷ���뿪��ƽ̨����λ
}
