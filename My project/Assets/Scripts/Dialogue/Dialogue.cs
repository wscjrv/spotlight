using UnityEngine;

// 这行代码决定了在右键菜单中的位置
[CreateAssetMenu(
    fileName = "NewDialogue",    // 默认文件名
    menuName = "Dialogue/Create Dialogue",  // 菜单路径
    order = 0                   // 菜单显示优先级
)]
public class Dialogue : ScriptableObject
{
    [Header("说话人")]
    public string speakerName;  // 比如“村长”“士兵”

    [Header("对话内容")]
    [TextArea(3, 10)]  // 多行文本框，方便输入长对话
    public string[] sentences;  // 对话句子数组（一句一句显示）
}