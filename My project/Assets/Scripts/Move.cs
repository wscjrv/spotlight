using UnityEngine;
using System.Collections;

public class MoveRight : MonoBehaviour
{
    [Header("移动设置")]
    public float moveDistance = 1200f;  // 移动距离
    public float moveSpeed = 5f;        // 移动速度
    public float delayBetweenMoves = 0.5f; // 组件之间的移动延迟

    [Header("组件引用")]
    [Tooltip("第一个要移动的组件")]
    public Transform component1;
    [Tooltip("第二个要移动的组件")]
    public Transform component2;
    [Tooltip("第三个要移动的组件")]
    public Transform component3;

    [Header("控制选项")]
    [Tooltip("是否自动开始移动")]
    public bool autoStart = true;
    [Tooltip("是否循环移动")]
    public bool loopMovement = false;

    private Vector3[] startPositions = new Vector3[3];
    private Vector3[] targetPositions = new Vector3[3];
    private bool[] isMoving = new bool[3];
    private int currentMovingIndex = -1; // 当前正在移动的组件索引
    private bool allComponentsCompleted = false;

    void Start()
    {
        InitializeComponents();
        
        if (autoStart)
        {
            StartSequentialMovement();
        }
    }

    void Update()
    {
        // 更新每个组件的移动状态
        for (int i = 0; i < 3; i++)
        {
            if (isMoving[i] && GetComponentTransform(i) != null)
            {
                UpdateComponentMovement(i);
            }
        }
    }

    /// <summary>
    /// 初始化所有组件
    /// </summary>
    private void InitializeComponents()
    {
        // 如果没有指定组件，使用当前对象
        if (component1 == null) component1 = transform;
        if (component2 == null) component2 = transform;
        if (component3 == null) component3 = transform;

        // 记录初始位置和计算目标位置
        for (int i = 0; i < 3; i++)
        {
            Transform comp = GetComponentTransform(i);
            if (comp != null)
            {
                startPositions[i] = comp.position;
                targetPositions[i] = startPositions[i] + Vector3.right * moveDistance;
            }
        }

        Debug.Log("组件初始化完成");
    }

    /// <summary>
    /// 获取指定索引的组件Transform
    /// </summary>
    private Transform GetComponentTransform(int index)
    {
        switch (index)
        {
            case 0: return component1;
            case 1: return component2;
            case 2: return component3;
            default: return null;
        }
    }

    /// <summary>
    /// 开始顺序移动
    /// </summary>
    public void StartSequentialMovement()
    {
        if (allComponentsCompleted && !loopMovement)
        {
            Debug.Log("所有组件已完成移动，且未开启循环模式");
            return;
        }

        // 重置状态
        ResetAllComponents();
        currentMovingIndex = 0;
        allComponentsCompleted = false;

        // 开始移动第一个组件
        StartCoroutine(SequentialMoveCoroutine());
    }

    /// <summary>
    /// 顺序移动协程
    /// </summary>
    private IEnumerator SequentialMoveCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            currentMovingIndex = i;
            isMoving[i] = true;
            
            Debug.Log($"开始移动第 {i + 1} 个组件");
            
            // 等待当前组件移动完成
            yield return new WaitUntil(() => !isMoving[i]);
            
            Debug.Log($"第 {i + 1} 个组件移动完成");
            
            // 如果不是最后一个组件，等待延迟时间
            if (i < 2)
            {
                yield return new WaitForSeconds(delayBetweenMoves);
            }
        }

        // 所有组件移动完成
        allComponentsCompleted = true;
        OnAllMovesComplete();

        // 如果开启循环，重新开始
        if (loopMovement)
        {
            yield return new WaitForSeconds(1f); // 循环间隔
            StartSequentialMovement();
        }
    }

    /// <summary>
    /// 更新指定组件的移动
    /// </summary>
    private void UpdateComponentMovement(int index)
    {
        Transform comp = GetComponentTransform(index);
        if (comp == null) return;

        // 平滑移动到目标位置
        comp.position = Vector3.MoveTowards(
            comp.position, 
            targetPositions[index], 
            moveSpeed * Time.deltaTime
        );

        // 检查是否到达目标位置
        if (Vector3.Distance(comp.position, targetPositions[index]) < 0.01f)
        {
            comp.position = targetPositions[index];
            isMoving[index] = false;
            
            OnComponentMoveComplete(index);
        }
    }

    /// <summary>
    /// 单个组件移动完成的回调
    /// </summary>
    private void OnComponentMoveComplete(int index)
    {
        Debug.Log($"组件 {index + 1} 移动完成");
    }

    /// <summary>
    /// 所有组件移动完成的回调
    /// </summary>
    private void OnAllMovesComplete()
    {
        Debug.Log("所有组件移动完成！");
        // 在这里添加移动完成后需要执行的代码
    }

    /// <summary>
    /// 重置所有组件到初始位置
    /// </summary>
    public void ResetAllComponents()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform comp = GetComponentTransform(i);
            if (comp != null)
            {
                comp.position = startPositions[i];
            }
            isMoving[i] = false;
        }
        
        currentMovingIndex = -1;
        allComponentsCompleted = false;
        
        Debug.Log("所有组件已重置到初始位置");
    }

    /// <summary>
    /// 停止所有移动
    /// </summary>
    public void StopAllMovement()
    {
        for (int i = 0; i < 3; i++)
        {
            isMoving[i] = false;
        }
        
        StopAllCoroutines();
        Debug.Log("已停止所有移动");
    }

    /// <summary>
    /// 设置移动速度
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
        Debug.Log($"设置移动速度为: {speed}");
    }

    /// <summary>
    /// 设置移动距离
    /// </summary>
    public void SetMoveDistance(float distance)
    {
        moveDistance = distance;
        
        // 重新计算目标位置
        for (int i = 0; i < 3; i++)
        {
            targetPositions[i] = startPositions[i] + Vector3.right * moveDistance;
        }
        
        Debug.Log($"设置移动距离为: {distance}");
    }

    /// <summary>
    /// 获取当前移动状态
    /// </summary>
    public bool IsMoving => currentMovingIndex >= 0;
    public int CurrentMovingComponent => currentMovingIndex + 1;
    public bool AllCompleted => allComponentsCompleted;

    // 在Scene视图中绘制调试信息
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < 3; i++)
            {
                Transform comp = GetComponentTransform(i);
                if (comp != null)
                {
                    // 绘制起始位置到目标位置的连线
                    Gizmos.color = isMoving[i] ? Color.red : Color.green;
                    Gizmos.DrawLine(startPositions[i], targetPositions[i]);
                    
                    // 绘制目标位置
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(targetPositions[i], 0.5f);
                }
            }
        }
    }
}