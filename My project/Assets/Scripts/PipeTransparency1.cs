using UnityEngine;
public class PipeTransparency : MonoBehaviour
{
    [Header("透明度设置")]
    public float normalAlpha = 1f; // 正常状态（外部）：不透明（alpha=1）
    public float innerAlpha = 0.5f; // 内部状态：半透明（alpha=0.5，可调整）
    public float fadeSpeed = 5f; // 透明度切换速度（避免生硬）
    [Header("引用配置")]
    public SpriteRenderer pipeRenderer; // 管道的 SpriteRenderer（控制透明度）
    private Color originalColor; // 管道原始颜色（记录 RGB 值）
    void Start()
    {
        // 初始化：获取管道原始颜色，设置初始不透明
        if (pipeRenderer == null)
            pipeRenderer = GetComponent<SpriteRenderer>();
        originalColor = pipeRenderer.color;
        originalColor.a = normalAlpha;
        pipeRenderer.color = originalColor;
    }
    // 主角进入管道内部（触发 PipeInnerTrigger）
    public void OnPlayerEnterInner()
    {
        // 目标颜色：半透明
        Color targetColor = originalColor;
        targetColor.a = innerAlpha;
        // 平滑过渡到半透明
        pipeRenderer.color = Color.Lerp(pipeRenderer.color, targetColor, fadeSpeed * Time.deltaTime);
    }
    // 主角离开管道内部
    public void OnPlayerExitInner()
    {
        // 目标颜色：不透明
        Color targetColor = originalColor;
        targetColor.a = normalAlpha;
        // 平滑过渡到不透明
        pipeRenderer.color = Color.Lerp(pipeRenderer.color, targetColor, fadeSpeed * Time.deltaTime);
    }
    // 每帧更新透明度过渡（确保平滑）
    void Update()
    {
        // 若管道 Renderer 为空，跳过
        if (pipeRenderer == null) return;
        // 检查是否需要过渡（避免 Update 空转）
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
    // 标记主角是否在内部（供触发器调用）
    private bool IsPlayerInInner = false;
    public void SetPlayerInInner(bool isIn)
    {
        IsPlayerInInner = isIn;
    }
}