using UnityEngine;

public class PlatformController_Type1 : MonoBehaviour
{
    [Header("�½�����")]
    public float dropDistance = 5f; // �½��ܾ���
    public float dropSpeed = 2f;   // �½��ٶ�

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool hasDropped = false; // ����Ƿ����½�����ֹ�ظ�������

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition - new Vector2(0, dropDistance);
    }

    void Update()
    {
        if (hasDropped)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                dropSpeed * Time.deltaTime);
        }
    }

    // �����½�����ִ��һ�Σ�
    public void TriggerDrop()
    {
        hasDropped = true; // һ�����������ñ����½�״̬
        Debug.Log("123");
    }
}
