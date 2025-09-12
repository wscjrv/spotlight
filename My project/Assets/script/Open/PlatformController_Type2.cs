using UnityEngine;

public class PlatformController_Type2 : MonoBehaviour
{
    [Header("ÒÆ¶¯²ÎÊý")]
    public float dropDistance = 5f;
    public float moveSpeed = 2f;

    private Vector2 startPos;
    private Vector2 dropPos;
    private bool isDropping = false;
    private bool isRising = false;

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
                isDropping = false;
        }
        else if (isRising)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, startPos) < 0.1f)
                isRising = false;
        }
    }

    public void Drop()
    {
        isRising = false;
        isDropping = true;
    }

    public void Rise()
    {
        isDropping = false;
        isRising = true;
    }
}
