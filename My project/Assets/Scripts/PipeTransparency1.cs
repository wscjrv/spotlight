using UnityEngine;
public class PipeTransparency : MonoBehaviour
{
    [Header("͸��������")]
    public float normalAlpha = 1f; // ����״̬���ⲿ������͸����alpha=1��
    public float innerAlpha = 0.5f; // �ڲ�״̬����͸����alpha=0.5���ɵ�����
    public float fadeSpeed = 5f; // ͸�����л��ٶȣ�������Ӳ��
    [Header("��������")]
    public SpriteRenderer pipeRenderer; // �ܵ��� SpriteRenderer������͸���ȣ�
    private Color originalColor; // �ܵ�ԭʼ��ɫ����¼ RGB ֵ��
    void Start()
    {
        // ��ʼ������ȡ�ܵ�ԭʼ��ɫ�����ó�ʼ��͸��
        if (pipeRenderer == null)
            pipeRenderer = GetComponent<SpriteRenderer>();
        originalColor = pipeRenderer.color;
        originalColor.a = normalAlpha;
        pipeRenderer.color = originalColor;
    }
    // ���ǽ���ܵ��ڲ������� PipeInnerTrigger��
    public void OnPlayerEnterInner()
    {
        // Ŀ����ɫ����͸��
        Color targetColor = originalColor;
        targetColor.a = innerAlpha;
        // ƽ�����ɵ���͸��
        pipeRenderer.color = Color.Lerp(pipeRenderer.color, targetColor, fadeSpeed * Time.deltaTime);
    }
    // �����뿪�ܵ��ڲ�
    public void OnPlayerExitInner()
    {
        // Ŀ����ɫ����͸��
        Color targetColor = originalColor;
        targetColor.a = normalAlpha;
        // ƽ�����ɵ���͸��
        pipeRenderer.color = Color.Lerp(pipeRenderer.color, targetColor, fadeSpeed * Time.deltaTime);
    }
    // ÿ֡����͸���ȹ��ɣ�ȷ��ƽ����
    void Update()
    {
        // ���ܵ� Renderer Ϊ�գ�����
        if (pipeRenderer == null) return;
        // ����Ƿ���Ҫ���ɣ����� Update ��ת��
        if (IsPlayerInInner)
        {
            Color target = originalColor;
            target.a = innerAlpha;
            pipeRenderer.color = Color.Lerp(pipeRenderer.color, target, fadeSpeed * Time.deltaTime);
        }
        else
        {
            Color target = originalColor;
            target.a = normalAlpha;
            pipeRenderer.color = Color.Lerp(pipeRenderer.color, target, fadeSpeed * Time.deltaTime);
        }
    }
    // ��������Ƿ����ڲ��������������ã�
    private bool IsPlayerInInner = false;
    public void SetPlayerInInner(bool isIn)
    {
        IsPlayerInInner = isIn;
    }
}