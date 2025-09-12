using UnityEngine;

public class clickse_eft : MonoBehaviour
{
    [Header("显示设置")]
    public GameObject model; // clickse的显示模型

    private void Awake()
    {
        // 若未指定模型，默认使用自身 GameObject
        if (model == null)
        {
            model = gameObject;
        }
    }

    // 显示当前clickse
    public void Show()
    {
        if (model != null)
            model.SetActive(true);
    }

    // 隐藏当前clickse
    public void Hide()
    {
        if (model != null)
            model.SetActive(false);
    }
}