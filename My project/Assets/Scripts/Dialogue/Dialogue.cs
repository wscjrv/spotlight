using UnityEngine;

// ���д�����������Ҽ��˵��е�λ��
[CreateAssetMenu(
    fileName = "NewDialogue",    // Ĭ���ļ���
    menuName = "Dialogue/Create Dialogue",  // �˵�·��
    order = 0                   // �˵���ʾ���ȼ�
)]
public class Dialogue : ScriptableObject
{
    [Header("˵����")]
    public string speakerName;  // ���硰�峤����ʿ����

    [Header("�Ի�����")]
    [TextArea(3, 10)]  // �����ı��򣬷������볤�Ի�
    public string[] sentences;  // �Ի��������飨һ��һ����ʾ��
}