using UnityEngine;

// Hito 9 — Buff recolectable

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un objeto en la escena con SpriteRenderer.

Componentes necesarios:
- Collider2D (ej: CircleCollider2D) con "Is Trigger" marcado = true.

Referencias del Inspector:
- buffType: tipo de estadística que modificará (Speed, Damage, AttackSpeed).
- multiplier: multiplicador que se aplicará a la estadística base (ej: 2f para duplicar).
- duration: cuánto tiempo en segundos durará el efecto antes de desvanecerse.
*/
public class BuffCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float duration = 5f;

    public BuffType BuffType => buffType;
    public float Multiplier => multiplier;
    public float Duration => duration;

    // Se ejecuta al ser tocado por el PlayerCollector. Delega al PowerUpController.
    public void Collect(PlayerCollector collector)
    {
        collector.PowerUpController.ActivateBuff(buffType, multiplier, duration);
        gameObject.SetActive(false);
    }
}
