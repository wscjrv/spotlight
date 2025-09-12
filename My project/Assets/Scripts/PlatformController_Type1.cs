using UnityEngine;

public class PlatformController_Type1 : MonoBehaviour
{
    [Header("下降参数")]
    public float dropDistance = 5f; // 下降总距离
    public float dropSpeed = 2f;   // 下降速度

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool hasDropped = false; // 标记是否已下降（防止重复触发）

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
                dropSpeed * Time.deltaTime
            );
        }
    }

    // 触发下降（仅执行一次）
    public void TriggerDrop()
    {
        hasDropped = true; // 一旦触发，永久保持下降状态
    }
}
