using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerscript : MonoBehaviour
{
    [Header("playermove")]
    public Rigidbody2D rb;//移动组件
    private float xinput;//水平输入
    public float xspeed = 10;//移动速度
    public Animator playeranimator;//角色动画控制器
    public bool isright = true;//是否朝右
    public SpriteRenderer playersprite;//角色渲染器（用于翻转）

    [Header("playerjump")]
    public float jumpforce = 300;
    public bool isground = true;//是否在地面
    public GameObject playergo;//角色对象
    public LayerMask groundlayer;//地面图层
    private bool isJumping = false;//是否正在跳跃中

    void Start()
    {
        Application.targetFrameRate = 60;//设置帧率
        Debug.Log("hello,game!");
    }

    void Update()
    {
        playermove();
        playerjump();
        checkisground();
        UpdateAnimationStates();//更新动画状态
    }

    private void FixedUpdate()//物理更新
    {
        fixplayermove();
    }

    public void playermove()
    {
        xinput = Input.GetAxisRaw("Horizontal");//获取水平输入

        //控制角色朝向
        if (xinput > 0)
        {
            isright = true;
            playersprite.flipX = false;
        }
        else if (xinput < 0)
        {
            isright = false;
            playersprite.flipX = true;
        }

        //跑步动画控制
        playeranimator.SetBool("isrun", xinput != 0);
    }

    public void fixplayermove()
    {
        rb.velocity = new Vector2(xinput * xspeed, rb.velocity.y);
    }

    public void playerjump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isground)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpforce));
            isJumping = true;
            isground = false; //立即标记为不在地面
        }
    }

    //更新动画状态的核心逻辑
    private void UpdateAnimationStates()
    {
        //地面状态动画
        if (isground)
        {
            playeranimator.SetBool("isjump", false);
            playeranimator.SetBool("isair", false);
            playeranimator.SetBool("isground", true);
            isJumping = false;
        }
        //空中状态动画
        else
        {
            playeranimator.SetBool("isground", false);

            //上升阶段 - 播放跳跃动画
            if (rb.velocity.y > 0.1f)
            {
                playeranimator.SetBool("isjump", true);
                playeranimator.SetBool("isair", false);
            }
            //下落阶段 - 播放下落动画
            else if (rb.velocity.y < -0.1f)
            {
                playeranimator.SetBool("isjump", false);
                playeranimator.SetBool("isair", true);
                isJumping = false;
            }
        }
    }

    public void checkisground()
    {
        Vector2 startpos = playergo.transform.position;
        Vector2 endpos = playergo.transform.position + Vector3.down * 1.2f;
        Debug.DrawLine(startpos, endpos, Color.red, 1f);//调试射线

        RaycastHit2D hit = Physics2D.Linecast(startpos, endpos, groundlayer);
        isground = hit.collider != null;
    }
}

//if(Input.GetKey(KeyCode.A))//检测输入按键A