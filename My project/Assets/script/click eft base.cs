using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_eft_base : MonoBehaviour
{
    [Header("����")]
    public List<click_eft> clickList = new List<click_eft>(); // Click�б�������˳�����У�
    public GameObject player; // ��ɫ����

    private int currentIndex = 0; // ��ǰ��ʾ��Click����

    private void Start()
    {
        InitializeClicks();
        ShowCurrentClick();
    }

    // ��ʼ������Click
    private void InitializeClicks()
    {
        if (clickList.Count == 0)
        {
            Debug.LogWarning("δ����Click�б�����Inspector�����Click����");
            return;
        }

        // ��������Click�����ù���������
        foreach (var click in clickList)
        {
            click.SetManager(this);
            click.Hide();
        }
    }

    // ��ʾ��ǰ������Click
    private void ShowCurrentClick()
    {
        if (IsIndexValid(currentIndex))
        {
            clickList[currentIndex].Show();
        }
    }

    // �л�����һ��Click
    public void NextClick()
    {
        // ���ص�ǰClick
        if (IsIndexValid(currentIndex))
        {
            clickList[currentIndex].Hide();
        }

        // �ƶ�����һ������
        currentIndex++;

        // ����Ƿ�����һ��Click
        if (IsIndexValid(currentIndex))
        {
            ShowCurrentClick();
        }
        else
        {
            // ����Click������ϣ�������������
            OnAllClicksCompleted();
        }
    }

    // ��������Ƿ���Ч
    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < clickList.Count;
    }

    // ����Click��ɺ�Ļص������ף�����չ��
    private void OnAllClicksCompleted()
    {
        Debug.Log("����Click�Ѵ�����ϣ������ڴ˴���Ӻ�������");
        // TODO: ʵ������Click��ɺ���߼�
    }
}
