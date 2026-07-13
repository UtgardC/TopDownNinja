using System;
using System.Collections;
using UnityEngine;

// Hito 9 — Controlador de Power-ups temporales

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador ("Player").

Componentes necesarios:
- PlayerStats en el mismo GameObject.

Referencias del Inspector:
- Ninguna requerida (se enlaza automáticamente a PlayerStats en Awake).

Notas:
- Maneja la duración de los efectos mediante Corrutinas.
- Evita solapamientos: si se recoge un buff activo del mismo tipo,
  cancela la corrutina anterior y la inicia de nuevo con la duración fresca.
*/
public class TemporaryPowerUpController : MonoBehaviour
{
    private PlayerStats stats;

    private Coroutine speedCoroutine;
    private Coroutine damageCoroutine;
    private Coroutine attackSpeedCoroutine;

    // Eventos para que el HUD sepa cuándo mostrar u ocultar la UI del buff.
    public event Action<BuffType, float> OnBuffStarted;
    public event Action<BuffType> OnBuffEnded;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    // Activa un buff multiplicador específico por una duración en segundos.
    public void ActivateBuff(BuffType type, float multiplier, float duration)
    {
        switch (type)
        {
            case BuffType.Speed:
                if (speedCoroutine != null) StopCoroutine(speedCoroutine);
                speedCoroutine = StartCoroutine(RunBuff(
                    type, duration,
                    () => stats.ApplySpeedMultiplier(multiplier),
                    () => stats.ResetSpeedMultiplier()
                ));
                break;

            case BuffType.Damage:
                if (damageCoroutine != null) StopCoroutine(damageCoroutine);
                damageCoroutine = StartCoroutine(RunBuff(
                    type, duration,
                    () => stats.ApplyDamageMultiplier(multiplier),
                    () => stats.ResetDamageMultiplier()
                ));
                break;

            case BuffType.AttackSpeed:
                if (attackSpeedCoroutine != null) StopCoroutine(attackSpeedCoroutine);
                attackSpeedCoroutine = StartCoroutine(RunBuff(
                    type, duration,
                    () => stats.ApplyAttackSpeedMultiplier(multiplier),
                    () => stats.ResetAttackSpeedMultiplier()
                ));
                break;
        }
    }

    // Ejecuta el ciclo de vida del Buff: aplica, espera, remueve y notifica eventos.
    private IEnumerator RunBuff(BuffType type, float duration, Action apply, Action remove)
    {
        apply();
        OnBuffStarted?.Invoke(type, duration);

        yield return new WaitForSeconds(duration);

        remove();
        OnBuffEnded?.Invoke(type);
    }

    // Cancela y limpia todos los efectos activos. Útil al morir o cambiar de nivel.
    public void CancelAllBuffs()
    {
        if (speedCoroutine != null)      { StopCoroutine(speedCoroutine);       stats.ResetSpeedMultiplier();       OnBuffEnded?.Invoke(BuffType.Speed); }
        if (damageCoroutine != null)     { StopCoroutine(damageCoroutine);      stats.ResetDamageMultiplier();      OnBuffEnded?.Invoke(BuffType.Damage); }
        if (attackSpeedCoroutine != null){ StopCoroutine(attackSpeedCoroutine); stats.ResetAttackSpeedMultiplier(); OnBuffEnded?.Invoke(BuffType.AttackSpeed); }

        speedCoroutine = null;
        damageCoroutine = null;
        attackSpeedCoroutine = null;
    }
}
