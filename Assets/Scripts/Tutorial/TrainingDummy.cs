using System.Collections;
using UnityEngine;

// Objetivo reutilizable para practicar ataques sin bloquear el tutorial.
public class TrainingDummy : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float resetDelay = 1f;
    [SerializeField] private Color hitColor = new Color(1f, 0.35f, 0.35f, 1f);

    private Color normalColor = Color.white;

    private void Awake()
    {
        if (health == null) health = GetComponent<Health>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) normalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        if (health == null) return;
        health.OnDamaged += HandleDamaged;
        health.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        if (health == null) return;
        health.OnDamaged -= HandleDamaged;
        health.OnDied -= HandleDied;
    }

    private void HandleDamaged(int amount)
    {
        if (spriteRenderer != null)
        {
            StopCoroutine(nameof(Flash));
            StartCoroutine(nameof(Flash));
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.12f);
        spriteRenderer.color = normalColor;
    }

    private void HandleDied()
    {
        StartCoroutine(ResetDummy());
    }

    private IEnumerator ResetDummy()
    {
        yield return new WaitForSeconds(resetDelay);
        health.RestoreToFull();
        if (spriteRenderer != null) spriteRenderer.color = normalColor;
    }
}
