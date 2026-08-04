using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Hito 10 — Carga y uso de pergaminos de habilidad

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador ("Player").

Componentes necesarios:
- PlayerInput con "Send Messages".
- PlayerMovement en el mismo GameObject (para orientar el disparo).
- El pergamino específico (ej. FireAbility) como componente en el mismo GameObject.

Referencias del Inspector:
- equippedAbility: arrastrar la habilidad inicial (ej. FireAbility) del propio Player.
- movement: arrastrar el componente PlayerMovement del jugador.
*/
public class ScrollLoadout : MonoBehaviour
{
    [SerializeField] private ScrollAbility equippedAbility;
    [SerializeField] private PlayerMovement movement;

    // Notifica cambios al HUD cuando el jugador equipa un pergamino distinto.
    public event System.Action<ScrollAbility> OnScrollChanged;

    public ScrollAbility EquippedAbility => equippedAbility;
    private bool isCasting = false;

    // Mensaje automático enviado por PlayerInput al presionar la tecla del pergamino.
    private void OnUseScroll(InputValue value)
    {
        if (value.isPressed)
        {
            TryUseEquippedScroll();
        }
    }

    // Intenta ejecutar la habilidad equipada iniciando el proceso de casteo.
    public void TryUseEquippedScroll()
    {
        if (equippedAbility == null || isCasting) return;
        if (!equippedAbility.CheckCanUse()) return;

        StartCoroutine(CastRoutine());
    }

    private IEnumerator CastRoutine()
    {
        isCasting = true;
        if (movement != null) movement.IsCasting = true;

        // Dispara la animación de casteo si el Animator está presente
        PlayerAnimator anim = GetComponent<PlayerAnimator>();
        if (anim != null)
        {
            if (equippedAbility.AbilityType == ScrollType.Fire) 
                anim.TriggerCastFire();
            else if (equippedAbility.AbilityType == ScrollType.Rock) 
                anim.TriggerCastRock();
        }

        // Espera el tiempo de casteo configurado en el pergamino
        if (equippedAbility.CastTime > 0f)
        {
            yield return new WaitForSeconds(equippedAbility.CastTime);
        }

        // Si el jugador cambió de arma o se canceló por alguna razón a mitad del casteo
        if (equippedAbility != null && equippedAbility.CheckCanUse())
        {
            // Calcula la dirección en el instante exacto del disparo
            Vector2 direction = movement != null ? movement.GetFacingDirection() : Vector2.down;
            equippedAbility.ConsumeAndExecute(direction);
        }

        if (movement != null) movement.IsCasting = false;
        isCasting = false;
    }

    // Cambia el pergamino activo y lanza el evento correspondiente.
    public void EquipAbility(ScrollAbility newAbility)
    {
        equippedAbility = newAbility;
        OnScrollChanged?.Invoke(equippedAbility);
    }

    // Busca la habilidad entre los componentes del Player y la equipa.
    public bool EquipAbility(ScrollType type)
    {
        ScrollAbility[] abilities = GetComponents<ScrollAbility>();
        foreach (ScrollAbility ability in abilities)
        {
            if (ability.AbilityType == type)
            {
                ability.RefillCharges();
                EquipAbility(ability);
                return true;
            }
        }
        return false;
    }
}
