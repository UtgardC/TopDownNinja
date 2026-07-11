using UnityEngine;

// Hito 10 — Pergaminos y habilidades

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al mismo GameObject del jugador junto a ScrollLoadout.

Componentes necesarios:
- ScrollLoadout en el mismo GameObject.

Referencias del Inspector:
- cooldown (de ScrollAbility): tiempo entre usos del fuego (recomendado: 1.5).
- damage (de ScrollAbility): daño de cada proyectil de fuego (recomendado: 15).
- fireProjectilePrefab: arrastrar el prefab del proyectil de fuego.
  Puede ser el mismo Projectile o uno con sprite diferente.

Layers y Tags:
- Ninguno adicional (usa la Layer del Projectile).

Notas:
- Demuestra: Herencia (extiende ScrollAbility) y Polimorfismo (redefine Execute).
- El proyectil de fuego se lanza en la dirección que pasa ScrollLoadout.
*/
public class FireAbility : ScrollAbility
{
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float spawnOffset = 0.6f;

    public override ScrollType AbilityType => ScrollType.Fire;

    // Lanza un proyectil de fuego en la dirección indicada.
    protected override bool Execute(Vector2 direction)
    {
        if (fireProjectilePrefab == null) return false;

        Vector2 normalizedDirection = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.down;
        Vector2 spawnPosition = (Vector2)transform.position + normalizedDirection * spawnOffset;
        GameObject projectileGO = Instantiate(fireProjectilePrefab, spawnPosition, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile == null)
        {
            Destroy(projectileGO);
            Debug.LogError("El prefab de fuego necesita un componente Projectile.", fireProjectilePrefab);
            return false;
        }

        projectile.SetDamage(damage);
        projectile.SetTargetLayers(targetLayers);
        projectile.Launch(normalizedDirection);
        return true;
    }
}
