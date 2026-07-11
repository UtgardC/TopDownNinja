using UnityEngine;

// Portal simple para terminar el tutorial y cargar el nivel principal.
public class LevelExitTrigger : MonoBehaviour
{
    [SerializeField] private LevelFlowController levelFlow;
    [SerializeField] private LayerMask playerLayer;

    private bool used;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used || levelFlow == null) return;
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

        used = true;
        levelFlow.LoadMainLevel();
    }
}
