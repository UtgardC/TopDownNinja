using System;
using System.Collections;
using UnityEngine;

// Hito 9 — Power-up temporal

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador junto a PlayerStats.

Componentes necesarios:
- PlayerStats en el mismo GameObject.

Referencias del Inspector:
- Ninguna adicional (lee PlayerStats desde GetComponent en Awake).

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Suscribir OnBuffStarted y OnBuffEnded desde HUDController para mostrar el estado del buff.
- Si el jugador recoge un buff del mismo tipo mientras ya está activo,
  el buff anterior se cancela y comienza uno nuevo con la nueva duración.
- Cada tipo de buff se maneja de forma independiente; pueden estar activos simultáneamente.
*/
public class TemporaryPowerUpController : MonoBehaviour
{
    private PlayerStats stats;

    // Coroutines activas por tipo de buff (permite cancelarlas individualmente).
    private Coroutine speedCoroutine;
    private Coroutine damageCoroutine;
    private Coroutine attackSpeedCoroutine;

    // Notifica cuando un buff comienza: tipo y duración en segundos.
    public event Action<BuffType, float> OnBuffStarted;

    // Notifica cuando un buff termina.
    public event Action<BuffType> OnBuffEnded;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    // Activa un buff temporal del tipo indicado. Si ya había uno activo del mismo tipo, lo reinicia.
    public void ActivateBuff(BuffType type, float multiplier, float duration)
    {
        switch (type)
        {
            case BuffType.Speed:
                if (speedCoroutine != null) StopCoroutine(speedCoroutine);
                speedCoroutine = StartCoroutine(RunBuff(
                    type, duration,
                    () => stats.ApplySpeedMultiplier(multiplier),
                    () => stats.ResetSpeedMultiplier(),
                    () => speedCoroutine = null
                ));
                break;

            case BuffType.Damage:
                if (damageCoroutine != null) StopCoroutine(damageCoroutine);
                damageCoroutine = StartCoroutine(RunBuff(
                    type, duration,
                    () => stats.ApplyDamageMultiplier(multiplier),
                    () => stats.ResetDamageMultiplier(),
                    () => damageCoroutine = null
                ));
                break;

            case BuffType.AttackSpeed:
                if (attackSpeedCoroutine != null) StopCoroutine(attackSpeedCoroutine);
                attackSpeedCoroutine = StartCoroutine(RunBuff(
                    type, duration,
                    () => stats.ApplyAttackSpeedMultiplier(multiplier),
                    () => stats.ResetAttackSpeedMultiplier(),
                    () => attackSpeedCoroutine = null
                ));
                break;
        }
    }

    // Aplica el buff, espera la duración y luego lo revierte.
    private IEnumerator RunBuff(BuffType type, float duration, Action apply, Action remove, Action clearReference)
    {
        apply();
        OnBuffStarted?.Invoke(type, duration);

        yield return new WaitForSeconds(Mathf.Max(0f, duration));

        remove();
        clearReference();
        OnBuffEnded?.Invoke(type);
    }

    // Cancela todos los buffs activos y restaura los multiplicadores base.
    public void CancelAllBuffs()
    {
        if (speedCoroutine != null)     { StopCoroutine(speedCoroutine);      stats.ResetSpeedMultiplier();       OnBuffEnded?.Invoke(BuffType.Speed); }
        if (damageCoroutine != null)    { StopCoroutine(damageCoroutine);     stats.ResetDamageMultiplier();      OnBuffEnded?.Invoke(BuffType.Damage); }
        if (attackSpeedCoroutine != null){ StopCoroutine(attackSpeedCoroutine); stats.ResetAttackSpeedMultiplier(); OnBuffEnded?.Invoke(BuffType.AttackSpeed); }

        speedCoroutine = null;
        damageCoroutine = null;
        attackSpeedCoroutine = null;
    }
}
