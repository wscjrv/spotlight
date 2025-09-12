using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_eft : MonoBehaviour
{
    [Header("��Ч����")]
    public Animator effectAnimator; // ��Ч����������
    public GameObject model; // Click��ģ�ͣ���ѡ��

    private click_eft_base manager;
    private Collider2D clickCollider; // ��ײ��

    private void Awake()
    {
        // ��ȡ��ײ�����
        clickCollider = GetComponent<Collider2D>();
        if (clickCollider != null)
        {
            clickCollider.isTrigger = true; // ����Ϊ������
        }

        // ���δָ��ģ�ͣ�Ĭ��ʹ������
        if (model == null)
        {
            model = gameObject;
        }
    }

    // ���ù���������
    public void SetManager(click_eft_base manager)
    {
        this.manager = manager;
    }

    // ��ʾClick
    public void Show()
    {
        if (model != null)
        {
            model.SetActive(true);
        }

        if (clickCollider != null)
        {
            clickCollider.enabled = true;
        }
    }

    // ����Click
    public void Hide()
    {
        if (model != null)
        {
            model.SetActive(false);
        }

        if (clickCollider != null)
        {
            clickCollider.enabled = false;
        }
    }

    // ������Ч
    public void PlayEffect()
    {
        if (effectAnimator != null)
        {
            effectAnimator.SetTrigger("click");
        }
    }

    // ����ɫ��ײ
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ����Ƿ����ɫ��ײ
        if (manager != null && other.gameObject == manager.player)
        {
            // ֪ͨ�������л�����һ��Click
            manager.NextClick();
        }
    }

    // �ո��������Ч
    private void Update()
    {
        // ֻ�е�Click�ɼ�ʱ����Ӧ�ո��
        if (model != null && model.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            PlayEffect();
        }
    }
}
