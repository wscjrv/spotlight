using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShowSpriteWithMask : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("Ҫ��ʾ��ͼƬ��Sprite��ʽ��")]
    public Sprite targetSprite;

    [Tooltip("ͼƬ��ʾʱ�����룩��0��ʾ������ʾ")]
    public float displayDuration = 3f;

    [Tooltip("�Ƿ�ֻ����һ��")]
    public bool triggerOnce = true;

    [Header("��ʾλ������")]
    [Tooltip("����ڴ�������ƫ��λ�ã��������꣩")]
    public Vector2 offsetFromTrigger = new Vector2(0, 1);

    [Tooltip("ͼƬ���ű���")]
    public Vector3 spriteScale = new Vector3(1, 1, 1);

    [Header("��Ⱦ����������")]
    [Tooltip("��Ⱦ�㼶���ƣ����ⱻ�ڵ���")]
    public string sortingLayerName = "Foreground";

    [Tooltip("ͬ�㼶�ڵ���ʾ���ȼ�")]
    public int sortingOrder = 10;

    [Tooltip("���ֽ���ģʽ��Ĭ�ϸ��津�����������֣�")]
    public SpriteMaskInteraction maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

    private SpriteRenderer spriteRenderer;
    private Collider2D triggerCollider;
    private bool hasTriggered = false;


    private void Awake()
    {
        // ��ʼ��������
        triggerCollider = GetComponent<Collider2D>();
        triggerCollider.isTrigger = true;

        // ��������ʼ����ʾ�õľ��飨�����������ã�
        CreateDisplaySprite();
    }


    // ��̬���������еľ������֧������ͬ����
    private void CreateDisplaySprite()
    {
        GameObject spriteObject = new GameObject("TriggerDisplaySprite");
        spriteObject.transform.parent = transform;
        spriteObject.transform.localPosition = offsetFromTrigger;
        spriteObject.transform.localScale = spriteScale;

        // ��Ӿ�����Ⱦ��������û�������
        spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = targetSprite;
        spriteRenderer.sortingLayerName = sortingLayerName;
        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.enabled = false;

        // �ؼ���ͬ����������
        SyncMaskSettings();
    }


    // ͬ���������ã���ͼƬ�ܴ��������ڵ�����Ӱ�죩
    private void SyncMaskSettings()
    {
        // 1. �̳д�����������������ֲ㣨����У�
        SpriteRenderer triggerRenderer = GetComponent<SpriteRenderer>();
        if (triggerRenderer != null)
        {
            // �������������о�����Ⱦ����ֱ�Ӹ��������ֲ�
            spriteRenderer.maskInteraction = triggerRenderer.maskInteraction;
        }
        else
        {
            // ����ʹ���Զ�������ֽ���ģʽ
            spriteRenderer.maskInteraction = maskInteraction;
        }

        // 2. ȷ��ͼƬ��������ͬһ��Ⱦ�㼶������ֻӰ��ͬ�㼶���壩
        // ����ͨ��sortingLayerName��֤��
    }


    // �����ҽ��봥������
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


    // ��ʾͼƬ
    private void ShowSprite()
    {
        spriteRenderer.enabled = true;

        if (displayDuration > 0)
        {
            StartCoroutine(HideAfterDelay(displayDuration));
        }
    }


    // �ӳ�����ͼƬ
    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }


    // �༭ģʽ�»��Ƹ�����
    private void OnDrawGizmos()
    {
        // ���ƴ�������Χ
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

        // ����ͼƬ��ʾλ�ñ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)offsetFromTrigger, 0.1f);
    }
}
