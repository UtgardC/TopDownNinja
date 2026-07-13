using UnityEngine;

// Hito 10 — Habilidad concreta: Pergamino de Fuego (FireAbility)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir como componente al GameObject del jugador ("Player").

Componentes necesarios:
- Todos los requeridos por ScrollLoadout.

Referencias del Inspector:
- fireProjectilePrefab: arrastrar el Prefab del proyectil (con script Projectile).
- cooldown: tiempo de espera específico de esta habilidad.
- damage: daño que tendrá el proyectil de fuego.
*/
public class FireAbility : ScrollAbility
{
    [SerializeField] private GameObject fireProjectilePrefab;

    public override ScrollType AbilityType => ScrollType.Fire;

    // Lanza un proyectil de fuego en la dirección que mira el jugador.
    protected override void Execute(Vector2 direction)
    {
        if (fireProjectilePrefab == null) return;

        GameObject projectileGO = Instantiate(fireProjectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetDamage(damage);
            projectile.Launch(direction);
        }
    }
}
