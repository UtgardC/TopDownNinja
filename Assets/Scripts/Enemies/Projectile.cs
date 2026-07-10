using UnityEngine;

// Hito 7 — Segundo enemigo (ranged)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject "Projectile" y convertirlo en prefab.

Componentes necesarios:
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0.
- Collider2D configurado como Trigger.
- SpriteRenderer con el sprite del proyectil.

Referencias del Inspector:
- speed: velocidad de desplazamiento del proyectil.
- playerLayer: capa del jugador (para detectar impacto).

Layers y Tags:
- El jugador debe tener Layer "Player"; asignarla en el campo playerLayer.

Notas:
- Launch(direction, damage) es llamado por RangedEnemy al disparar.
- El proyectil se destruye al impactar al jugador o al salir de pantalla (por tiempo).
- SetDamage permite que RangedEnemy configure el daño antes de lanzar.
*/
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float lifetime = 4f;

    private int damage;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Configura el daño del proyectil antes de lanzarlo.
    public void SetDamage(int amount)
    {
        damage = amount;
    }

    // Lanza el proyectil en la dirección indicada y lo destruye después de su tiempo de vida.
    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto pertenece a la capa del jugador.
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null && target.IsAlive())
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
