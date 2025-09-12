using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerscript : MonoBehaviour
{
    [Header("playermove")]
    public Rigidbody2D rb;//�ƶ����
    private float xinput;//ˮƽ����
    public float xspeed = 10;//�ƶ��ٶ�
    public Animator playeranimator;//��ɫ����������
    public bool isright = true;//�Ƿ���
    public SpriteRenderer playersprite;//��ɫ��Ⱦ�������ڷ�ת��

    [Header("playerjump")]
    public float jumpforce = 300;
    public bool isground = true;//�Ƿ��ڵ���
    public GameObject playergo;//��ɫ����
    public LayerMask groundlayer;//����ͼ��
    private bool isJumping = false;//�Ƿ�������Ծ��

    void Start()
    {
        Application.targetFrameRate = 60;//����֡��
        Debug.Log("hello,game!");
    }

    void Update()
    {
        playermove();
        playerjump();
        checkisground();
        UpdateAnimationStates();//���¶���״̬
    }

    private void FixedUpdate()//�������
    {
        fixplayermove();
    }

    public void playermove()
    {
        xinput = Input.GetAxisRaw("Horizontal");//��ȡˮƽ����

        //���ƽ�ɫ����
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

        //�ܲ���������
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
            isground = false; //�������Ϊ���ڵ���
        }
    }

    //���¶���״̬�ĺ����߼�
    private void UpdateAnimationStates()
    {
        //����״̬����
        if (isground)
        {
            playeranimator.SetBool("isjump", false);
            playeranimator.SetBool("isair", false);
            playeranimator.SetBool("isground", true);
            isJumping = false;
        }
        //����״̬����
        else
        {
            playeranimator.SetBool("isground", false);

            //�����׶� - ������Ծ����
            if (rb.velocity.y > 0.1f)
            {
                playeranimator.SetBool("isjump", true);
                playeranimator.SetBool("isair", false);
            }
            //����׶� - �������䶯��
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
        Debug.DrawLine(startpos, endpos, Color.red, 1f);//��������

        RaycastHit2D hit = Physics2D.Linecast(startpos, endpos, groundlayer);
        isground = hit.collider != null;
    }
}

//if(Input.GetKey(KeyCode.A))//������밴��A