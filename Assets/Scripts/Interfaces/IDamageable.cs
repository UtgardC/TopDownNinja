// Hito 1 — Contratos y bases

// Contrato para objetos que pueden recibir daño.
// Implementado por: Health (jugador, enemigos y objetos destructibles).
// Permite que PlayerAttack y Projectile dañen sin conocer la clase concreta.
public interface IDamageable
{
    // Aplica daño al objeto. amount debe ser un valor positivo.
    void TakeDamage(int amount);

    // Devuelve verdadero si el objeto continúa con vida.
    bool IsAlive();
}
