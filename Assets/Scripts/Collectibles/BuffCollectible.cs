using UnityEngine;

// Hito 9 — Power-up temporal

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject en el nivel que represente el power-up.
  Se recomienda crear un prefab por cada tipo de buff con sprites diferentes.

Componentes necesarios:
- Collider2D configurado como Trigger.
- SpriteRenderer con el sprite correspondiente al tipo de buff.

Referencias del Inspector:
- buffType: tipo de estadística que se potencia (Speed, Damage o AttackSpeed).
- multiplier: valor del multiplicador (ej: 2.0 = el doble de la estadística base).
- duration: duración del buff en segundos.

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- El mismo script sirve para los tres tipos de buff.
  Crear tres prefabs distintos cambiando el buffType en el Inspector.
  Ejemplos de configuración:
    * Speed Buff:       buffType=Speed,       multiplier=2.0, duration=5
    * Damage Buff:      buffType=Damage,      multiplier=2.0, duration=5
    * AttackSpeed Buff: buffType=AttackSpeed, multiplier=2.0, duration=5
*/
public class BuffCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float duration = 5f;

    public BuffType BuffType => buffType;
    public float Multiplier => multiplier;
    public float Duration => duration;

    // Activa el buff en el jugador y desactiva el objeto del nivel.
    public void Collect(PlayerCollector collector)
    {
        collector.PowerUpController.ActivateBuff(buffType, multiplier, duration);
        gameObject.SetActive(false);
    }
}
