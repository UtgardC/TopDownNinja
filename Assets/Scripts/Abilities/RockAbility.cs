using System.Collections.Generic;
using UnityEngine;

// Golpe de roca de corto alcance y daño alto.
public class RockAbility : ScrollAbility
{
    [SerializeField] private GameObject rockEffectPrefab;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float effectOffset = 0.9f;
    [SerializeField] private float effectRadius = 0.8f;
    [SerializeField] private float effectLifetime = 0.6f;
    [SerializeField] private float damageDelay = 0.2f;

    public override ScrollType AbilityType => ScrollType.Rock;

    protected override void Execute(Vector2 direction)
    {
        Vector2 normalizedDirection = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.down;
        Vector2 effectPosition = (Vector2)transform.position + normalizedDirection * effectOffset;

        if (rockEffectPrefab != null)
        {
            GameObject effect = Instantiate(rockEffectPrefab, effectPosition, Quaternion.identity);
            Destroy(effect, effectLifetime);
        }

        StartCoroutine(DealDamageAfterDelay(effectPosition));
    }

    private System.Collections.IEnumerator DealDamageAfterDelay(Vector2 position)
    {
        if (damageDelay > 0f)
        {
            yield return new WaitForSeconds(damageDelay);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, effectRadius, targetLayers);
        HashSet<IDamageable> damagedTargets = new HashSet<IDamageable>();

        foreach (Collider2D hit in hits)
        {
            IDamageable target = hit.GetComponentInParent<IDamageable>();
            if (target != null && target.IsAlive() && damagedTargets.Add(target))
            {
                target.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 direction = Vector2.down;
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) direction = movement.GetFacingDirection();

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere((Vector2)transform.position + direction.normalized * effectOffset, effectRadius);
    }
}
