using System;
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
    public event Action<ScrollAbility> OnScrollChanged;

    public ScrollAbility EquippedAbility => equippedAbility;

    // Mensaje automático enviado por PlayerInput al presionar la tecla del pergamino.
    private void OnUseScroll(InputValue value)
    {
        if (value.isPressed)
        {
            TryUseEquippedScroll();
        }
    }

    // Intenta ejecutar la habilidad equipada en la dirección actual de mirada del jugador.
    public bool TryUseEquippedScroll()
    {
        if (equippedAbility == null) return false;

        Vector2 direction = movement != null ? movement.GetFacingDirection() : Vector2.down;
        return equippedAbility.TryUse(direction);
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
