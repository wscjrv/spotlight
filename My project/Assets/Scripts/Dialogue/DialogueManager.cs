using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    private int currentSentenceIndex;
    private Dialogue currentDialogue;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // 开始对话
    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentSentenceIndex = 0;
        dialoguePanel.SetActive(true);
        speakerText.text = dialogue.speakerName;
        ShowNextSentence();
    }

    // 显示下一句
    public void ShowNextSentence()
    {
        if (currentSentenceIndex < currentDialogue.sentences.Length)
        {
            dialogueText.text = currentDialogue.sentences[currentSentenceIndex];
            currentSentenceIndex++;
        }
        else
        {
            // 对话结束
            dialoguePanel.SetActive(false);
        }
    }
}