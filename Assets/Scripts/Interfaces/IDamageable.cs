// Hito 1 — Interfaces del sistema de combate y recolección

/*
CONFIGURACIÓN EN UNITY
- No se añaden a ningún GameObject directamente (son interfaces).

Notas:
- Cualquier objeto destructible (jugador, enemigos, barriles) debe implementar IDamageable.
- Cualquier objeto recogible (monedas, comida, power-ups) debe implementar ICollectible.
*/

// Interfaz para entidades que pueden recibir daño y morir.
public interface IDamageable
{
    // Aplica daño a la entidad.
    void TakeDamage(int amount);

    // Devuelve true si la entidad sigue con vida.
    bool IsAlive();
}
