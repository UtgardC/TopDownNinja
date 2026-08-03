using System.Collections;
using UnityEngine;

// Efecto visual de tinte rojo al recibir daño. Añadir a cualquier enemigo.

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir a MeleeEnemy, RangedEnemy, BossEnemy o cualquier objeto con Health.

Componentes necesarios:
- Health en el mismo GameObject (o en el padre).
- SpriteRenderer en el mismo GameObject o en un hijo.

Referencias del Inspector:
- health: se autodetecta en Awake si no se asigna.
- spriteRenderer: se autodetecta en hijos si no se asigna.
- hitColor: color del tinte al recibir daño (por defecto rojo).
- flashDuration: cuántos segundos dura el tinte antes de volver al color normal.
*/
public class HitFlash : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = new Color(1f, 0.2f, 0.2f, 1f);
    [SerializeField] private float flashDuration = 0.12f;

    private Color normalColor = Color.white;

    private void Awake()
    {
        if (health == null) health = GetComponent<Health>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) normalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        if (health != null) health.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        if (health != null) health.OnDamaged -= HandleDamaged;
    }

    private void HandleDamaged(int amount)
    {
        if (spriteRenderer == null) return;
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = normalColor;
    }
}
