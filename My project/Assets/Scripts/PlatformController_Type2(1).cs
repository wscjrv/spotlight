using UnityEngine;

public class PlatformController_Type2 : MonoBehaviour
{
    [Header("移动参数")]
    public float dropDistance = 5f;
    public float moveSpeed = 2f;
    public float stayTime = 2f;

    private Vector2 startPos;
    private Vector2 dropPos;
    private bool isDropping = false;
    private bool isRising = false;
    private bool isStaying = false;
    private float stayTimer = 0;

    void Start()
    {
        startPos = transform.position;
        dropPos = startPos - new Vector2(0, dropDistance);
    }

    void Update()
    {
        if (isDropping)
        {
            transform.position = Vector2.MoveTowards(transform.position, dropPos, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, dropPos) < 0.1f)
            {
                isDropping = false;
                isStaying = true;
                stayTimer = 0;
            }
        }
        else if (isStaying)
        {
            stayTimer += Time.deltaTime;
            if (stayTimer >= stayTime)
            {
                isStaying = false;
                isRising = true;
            }
        }
        else if (isRising)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, startPos) < 0.1f)
            {
                isRising = false;
            }
        }
    }

    public void StartDrop()
    {
        if (!isDropping && !isRising && !isStaying)
        {
            isDropping = true;
        }
    }

    // 供触发器判断：平台是否处于“可再次触发”状态（不在运动/停留中）
    public bool IsInMotion()
    {
        return isDropping || isRising || isStaying;
    }
}