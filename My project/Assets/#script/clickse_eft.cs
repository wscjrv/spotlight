using UnityEngine;

public class clickse_eft : MonoBehaviour
{
    [Header("显示设置")]
    public GameObject model; // 显示模型

    private void Awake()
    {
        if (model == null)
        {
            model = gameObject;
        }
    }

    /// <summary>显示元素</summary>
    public void Show()
    {
        model?.SetActive(true);
    }

    /// <summary>隐藏元素</summary>
    public void Hide()
    {
        model?.SetActive(false);
    }
}
