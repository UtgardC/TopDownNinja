using UnityEngine;

// Hito 7 — Proyectil para enemigos ranged y pergamino de fuego

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un objeto en la escena con el Sprite del proyectil (ej. shuriken).
- Puede crearse como Prefab para ser instanciado dinámicamente.

Componentes necesarios:
- Rigidbody2D: Body Type = Dynamic, Gravity Scale = 0, Collision Detection = Continuous.
- Collider2D (ej: CircleCollider2D) con "Is Trigger" marcado = true.

Referencias del Inspector:
- speed: velocidad de vuelo lineal del proyectil.
- playerLayer: seleccionar la Layer del objetivo al que puede dañar.
               (Para shuriken de enemigos = Player; para pergamino de fuego del Player = Enemy).
- lifetime: tiempo de autodestrucción automática por si no choca con nada.
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

    // Configura el daño que infligirá el proyectil (llamado desde el script que lo lanza).
    public void SetDamage(int amount)
    {
        damage = amount;
    }

    // Aplica impulso físico al proyectil y programa su autodestrucción.
    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    // Detecta impacto. Si choca con el objetivo correcto, le hace daño y se destruye.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target != null && target.IsAlive())
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
