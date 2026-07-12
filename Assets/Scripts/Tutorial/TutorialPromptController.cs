using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialPromptController : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string initialMessage = "WASD / Flechas: moverse    J / Click: atacar";

    private Coroutine hideCoroutine;

    private void Start()
    {
        ShowMessage(initialMessage, 0f);
    }

    public void ShowMessage(string message, float duration)
    {
        if (promptText == null) return;
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);

        promptText.gameObject.SetActive(true);
        promptText.text = message;

        if (duration > 0f) hideCoroutine = StartCoroutine(HideAfter(duration));
    }

    private IEnumerator HideAfter(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        promptText.gameObject.SetActive(false);
        hideCoroutine = null;
    }
}
