using UnityEngine;

public class PassThroughBlock : MonoBehaviour
{
    [Header("��ײ����")]
    [Tooltip("�������Ƿ�������·������߳���")]
    public float detectionDistance = 2f;
    [Tooltip("�������Ƿ�������·������߿���")]
    public float detectionWidth = 1f;
    [Tooltip("���㼶")]
    public LayerMask playerLayerMask = -1;

    [Header("����")]
    [Tooltip("��ʾ�������")]
    public bool showDetectionArea = true;
    [Tooltip("��ʾ�������")]
    public bool showDetectionRay = true;

    private Collider2D blockCollider;
    private Transform playerTransform;
    private bool isPlayerBelow = false;
    private bool isPassingThrough = false;

    void Start()
    {
        // ��ȡ������ײ��
        blockCollider = GetComponent<Collider2D>();
        if (blockCollider == null)
        {
            Debug.LogWarning($"PassThroughBlock: {gameObject.name} û���ҵ�Collider2D�����");
            return;
        }

        // �������
        FindPlayer();
    }

    void Update()
    {
        if (blockCollider == null || playerTransform == null) return;

        // �������Ƿ���������·�
        bool playerBelowNow = IsPlayerBelow();

        // �����Ҹոս����·�����
        if (playerBelowNow && !isPlayerBelow)
        {
            OnPlayerEnterBelow();
        }
        // �����Ҹո��뿪�·�����
        else if (!playerBelowNow && isPlayerBelow)
        {
            OnPlayerExitBelow();
        }

        isPlayerBelow = playerBelowNow;

        // ������ڴ�����ң�����Ƿ��Ѿ���ȫͨ��
        if (isPassingThrough)
        {
            CheckIfPassedThrough();
        }
    }

    private void FindPlayer()
    {
        // ����ͨ����ǩ�������
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            return;
        }

        // ���û�ҵ�Player��ǩ������ͨ��playerscript�������
        playerscript playerScript = FindObjectOfType<playerscript>();
        if (playerScript != null)
        {
            playerTransform = playerScript.transform;
            return;
        }

        Debug.LogWarning("PassThroughBlock: δ�ҵ���Ҷ�����ȷ�������Player��ǩ�����playerscript�����");
    }

    private bool IsPlayerBelow()
    {
        if (playerTransform == null) return false;

        // �������ĵײ����ĵ�
        Vector2 blockBottomCenter = new Vector2(
            blockCollider.bounds.center.x,
            blockCollider.bounds.min.y
        );

        // ����������������յ�
        Vector2 rayStart = blockBottomCenter;
        Vector2 rayEnd = blockBottomCenter + Vector2.down * detectionDistance;

        // ʹ��BoxCast�������Ƿ����·�
        Vector2 boxSize = new Vector2(detectionWidth, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(rayStart, boxSize, 0f, Vector2.down, detectionDistance, playerLayerMask);

        // ���Ի���
        if (showDetectionRay)
        {
            Debug.DrawLine(rayStart, rayEnd, hit.collider != null ? Color.red : Color.green, 0.1f);
        }

        if (showDetectionArea)
        {
            // ���Ƽ������
            Vector2 boxCenter = (rayStart + rayEnd) * 0.5f;
            Debug.DrawLine(
                new Vector2(boxCenter.x - boxSize.x * 0.5f, boxCenter.y),
                new Vector2(boxCenter.x + boxSize.x * 0.5f, boxCenter.y),
                Color.blue, 0.1f
            );
        }

        // ����Ƿ�������
        if (hit.collider != null)
        {
            // ��֤���е�ȷʵ�����
            return hit.collider.transform == playerTransform ||
                   hit.collider.CompareTag("Player") ||
                   hit.collider.GetComponent<playerscript>() != null;
        }

        return false;
    }

    private void OnPlayerEnterBelow()
    {
        if (isPassingThrough) return; // ����Ѿ��ڴ���״̬�����ظ�����

        // ������ײ���
        SetCollisionEnabled(false);
        isPassingThrough = true;

        Debug.Log($"PassThroughBlock: {gameObject.name} ��ʼ�������");
    }

    private void OnPlayerExitBelow()
    {
        // ����뿪�·����򣬵����ܻ��ڴ�����
        Debug.Log($"PassThroughBlock: {gameObject.name} ����뿪�·�����");
    }

    private void CheckIfPassedThrough()
    {
        if (playerTransform == null) return;

        // �������Ƿ��Ѿ���ȫͨ�����
        // ͨ���Ƚ����ĵײ�����ҵĶ������ж�
        float blockBottom = blockCollider.bounds.min.y;
        float playerBottom = playerTransform.GetComponent<Collider2D>()?.bounds.min.y ??
                         playerTransform.position.y - 1f; // ���û����ײ����ʹ��Ĭ�ϸ߶�

        // ������ײ��Ѿ�������Ҷ�����˵���Ѿ���ȫͨ��
        if (blockBottom < playerBottom - 0.2f) // ����һ�㻺�����
        {
            // ����������ײ���
            SetCollisionEnabled(true);
            isPassingThrough = false;

            Debug.Log($"PassThroughBlock: {gameObject.name} ����ȫͨ����ң��ָ���ײ���");
        }
    }

    private void SetCollisionEnabled(bool enabled)
    {
        if (blockCollider == null) return;

        blockCollider.enabled = enabled;
    }

    // �����������ֶ����ô���״̬
    public void SetPassingThrough(bool passing)
    {
        isPassingThrough = passing;
        SetCollisionEnabled(!passing);
    }

    // ����������ǿ�����¼�����λ��
    public void RefreshPlayerDetection()
    {
        FindPlayer();
    }

    void OnDrawGizmosSelected()
    {
        if (blockCollider == null) return;

        // ���Ƽ������
        Vector2 blockBottomCenter = new Vector2(
            blockCollider.bounds.center.x,
            blockCollider.bounds.min.y
        );

        // ���Ƽ������
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            blockBottomCenter,
            blockBottomCenter + Vector2.down * detectionDistance
        );

        // ���Ƽ�������
        Gizmos.color = Color.cyan;
        Vector2 boxCenter = blockBottomCenter + Vector2.down * detectionDistance * 0.5f;
        Vector2 boxSize = new Vector2(detectionWidth, detectionDistance);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}