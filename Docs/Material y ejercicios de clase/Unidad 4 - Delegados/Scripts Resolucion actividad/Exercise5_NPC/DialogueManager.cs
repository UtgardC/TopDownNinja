using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private NPC npc;

    [Header("UI de Diálogo")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void OnEnable()
    {
        npc.OnInteract += ShowDialogue;
    }

    private void OnDisable()
    {
        npc.OnInteract -= ShowDialogue;
    }

    private void ShowDialogue(string line)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = line;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = string.Empty;
    }
}
