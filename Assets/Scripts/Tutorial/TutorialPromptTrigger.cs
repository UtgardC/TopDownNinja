using UnityEngine;

public class TutorialPromptTrigger : MonoBehaviour
{
    [SerializeField] private TutorialPromptController promptController;
    [SerializeField, TextArea] private string message;
    [SerializeField] private float duration = 6f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool showOnlyOnce = true;

    private bool wasShown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;
        if (showOnlyOnce && wasShown) return;

        wasShown = true;
        if (promptController != null) promptController.ShowMessage(message, duration);
    }
}
