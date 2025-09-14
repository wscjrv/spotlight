using UnityEngine;

public class PlatformController_Type1 : MonoBehaviour
{
    [Header("下降参数")]
    public float dropDistance = 5f; // 下降总距离
    public float dropSpeed = 2f;   // 下降速度

    [Header("控制选项")]
    [Tooltip("是否允许外部程序控制移动")]
    public bool allowExternalControl = true;
    [Tooltip("是否在到达目标位置后停止移动")]
    public bool stopAtTarget = true;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isDropping = false; // 是否正在下降
    private bool hasReachedTarget = false; // 是否已到达目标位置

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition - new Vector2(0, dropDistance);
    }

    void Update()
    {
        // 只有在允许外部控制且正在下降且未到达目标时才移动
        if (allowExternalControl && isDropping && !hasReachedTarget)
        {
            Vector2 newPosition = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                dropSpeed * Time.deltaTime);
            
            transform.position = newPosition;

            // 检查是否到达目标位置
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                hasReachedTarget = true;
                if (stopAtTarget)
                {
                    isDropping = false; // 停止下降
                }
                Debug.Log("平台已到达目标位置");
            }
        }
    }

    // 触发下降
    public void TriggerDrop()
    {
        if (!hasReachedTarget) // 只有在未到达目标时才允许触发
        {
            isDropping = true;
            Debug.Log("平台开始下降");
        }
    }

    // 停止下降
    public void StopDrop()
    {
        isDropping = false;
        Debug.Log("平台停止下降");
    }

    // 重置平台到初始位置
    public void ResetPlatform()
    {
        transform.position = startPosition;
        isDropping = false;
        hasReachedTarget = false;
        Debug.Log("平台已重置到初始位置");
    }

    // 设置新的目标位置
    public void SetTargetPosition(Vector2 newTarget)
    {
        targetPosition = newTarget;
        hasReachedTarget = false;
        Debug.Log($"设置新的目标位置: {newTarget}");
    }

    // 设置下降速度
    public void SetDropSpeed(float newSpeed)
    {
        dropSpeed = newSpeed;
        Debug.Log($"设置新的下降速度: {newSpeed}");
    }

    // 获取当前状态信息
    public bool IsDropping => isDropping;
    public bool HasReachedTarget => hasReachedTarget;
    public Vector2 StartPosition => startPosition;
    public Vector2 TargetPosition => targetPosition;
    public float DropProgress => hasReachedTarget ? 1f : Vector2.Distance(startPosition, transform.position) / dropDistance;

    // 强制移动平台到指定位置（忽略其他限制）
    public void ForceMoveTo(Vector2 position)
    {
        transform.position = position;
        Debug.Log($"强制移动平台到位置: {position}");
    }

    // 设置是否允许外部控制
    public void SetAllowExternalControl(bool allow)
    {
        allowExternalControl = allow;
        if (!allow)
        {
            isDropping = false; // 如果不允许外部控制，停止移动
        }
    }
}