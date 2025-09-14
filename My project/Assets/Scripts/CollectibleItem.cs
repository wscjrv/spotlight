using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Tooltip("�ռ�ʱ����Ч����ѡ��")]
    public ParticleSystem collectEffect;

    private bool isCollected = false;
    private Collider2D itemCollider;

    private void Start()
    {
        // ȷ����ײ���Ǵ�����
        itemCollider = GetComponent<Collider2D>();
        if (itemCollider != null && !itemCollider.isTrigger)
        {
            Debug.LogWarning($"����{gameObject.name}���Զ�����Is Trigger");
            itemCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��Ҵ�����δ�ռ�
        if (other.CompareTag("Player") && !isCollected)
        {
            Collect();
        }
    }

    private void Collect()
    {
        isCollected = true;

        // ������Ч����ѡ��
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        // �ҵ���ǰ�����Ĺ�������֪ͨ�ռ��ɹ�
        SceneItemCollector collector = FindObjectOfType<SceneItemCollector>();
        if (collector != null)
        {
            collector.OnItemCollected();
        }
        else
        {
            Debug.LogError("��ǰ����δ�ҵ�SceneItemCollector�������ӹ�����");
        }

        // ���ص��ߣ������ظ��ռ���
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 重置物品状态（用于场景重新加载或调试）
    /// </summary>
    public void ResetItem()
    {
        isCollected = false;
        gameObject.SetActive(true);
    }
}