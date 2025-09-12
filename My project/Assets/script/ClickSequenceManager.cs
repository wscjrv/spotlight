using System.Collections.Generic;
using UnityEngine;

public class ClickSequenceManager : MonoBehaviour
{
    public Animator maskanimation;
    [Header("��������")]
    [Tooltip("click�����б�����clickse�б�����һ����˳���Ӧ")]
    public List<click_eft> clickList = new List<click_eft>();

    [Tooltip("clickse�����б�����click�б�����һ����˳���Ӧ")]
    public List<clickse_eft> clickseList = new List<clickse_eft>();

    public GameObject player; // ��ҽ�ɫ

    private int currentSequenceIndex = 0; // ��ǰ�������������

    private void Start()
    {
        if (InitializeSequence())
        {
            ShowCurrentSequence();
        }
    }

    // ��ʼ�����У����Ϸ��Բ��������ж���
    private bool InitializeSequence()
    {
        // ������������Ƿ�һ��
        if (clickList.Count != clickseList.Count)
        {
            Debug.LogError("Error: click��clickse������ƥ�䣡");
            return false;
        }

        // ����б��Ƿ�Ϊ��
        if (clickList.Count == 0)
        {
            Debug.LogWarning("Warning: δ����κ�click��clickse����");
            return false;
        }

        // ��ʼ������click�����ù����������أ�
        foreach (var click in clickList)
        {
            click.SetManager(this);
            click.Hide();
        }

        // ��������clickse
        foreach (var clickse in clickseList)
        {
            clickse.Hide();
        }

        return true;
    }

    // ��ʾ��ǰ������click��clickse
    private void ShowCurrentSequence()
    {

        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex].Show();
            clickseList[currentSequenceIndex].Show();
            clickList[currentSequenceIndex].PlayEffect();
        }
    }

    // ���ص�ǰ������click��clickse
    private void HideCurrentSequence()
    {
        if (IsIndexValid(currentSequenceIndex))
        {
            clickList[currentSequenceIndex].Hide();
            clickseList[currentSequenceIndex].Hide();
        }
    }

    // �л�����һ������
    public void NextSequence()
    {
        HideCurrentSequence();
        currentSequenceIndex++;

        if (IsIndexValid(currentSequenceIndex))
        {
            ShowCurrentSequence();
        }
        else
        {
            OnAllSequencesCompleted();
        }
    }

    // ��������Ƿ�Ϸ�
    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < clickList.Count;
    }

    // ����������ɺ�Ļص���������չ��
    private void OnAllSequencesCompleted()
    {
        Debug.Log("����click/clickse��������ɣ�");
        maskanimation.SetTrigger("ifmaskout");
        // TODO: �˴��������������ɺ���߼�
    }
}