using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Hito 10 — Pergaminos y habilidades

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject del jugador junto a FireAbility y PlayerMovement.

Componentes necesarios:
- FireAbility (u otra ScrollAbility) en el mismo GameObject.
- PlayerMovement en el mismo GameObject.

Referencias del Inspector:
- equippedAbility: arrastrar el componente FireAbility del mismo GameObject.
- movement: arrastrar el componente PlayerMovement del mismo GameObject.

Layers y Tags:
- Ninguno requerido por este script.

Animación e Input:
- El componente PlayerInput llama a OnUseScroll cuando el jugador
  presiona el botón configurado para habilidades (ej: botón derecho del ratón, E, etc.).
  Asegurarse de que existe una acción "UseScroll" en el InputSystem_Actions asset.

Notas:
- OnScrollChanged notifica al HUD cuando cambia el pergamino equipado.
- Si se quiere agregar más habilidades en el futuro, solo hay que cambiar
  la referencia en el Inspector sin modificar este script.
*/
public class ScrollLoadout : MonoBehaviour
{
    [SerializeField] private ScrollAbility equippedAbility;
    [SerializeField] private ScrollAbility[] availableAbilities;
    [SerializeField] private PlayerMovement movement;

    // Notifica cuando cambia el pergamino equipado.
    public event Action<ScrollAbility> OnScrollChanged;

    public ScrollAbility EquippedAbility => equippedAbility;
    public bool HasEquippedAbility => equippedAbility != null;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (availableAbilities == null || availableAbilities.Length == 0)
            availableAbilities = GetComponents<ScrollAbility>();
    }

    // Recibe el input del pergamino desde el sistema de input (PlayerInput → Send Messages).
    private void OnUseScroll(InputValue value)
    {
        if (value.isPressed)
        {
            TryUseEquippedScroll();
        }
    }

    // Intenta usar el pergamino equipado en la dirección actual del jugador.
    // Devuelve verdadero si la habilidad se activó correctamente.
    public bool TryUseEquippedScroll()
    {
        if (equippedAbility == null) return false;

        Vector2 direction = movement != null ? movement.GetFacingDirection() : Vector2.down;
        return equippedAbility.TryUse(direction);
    }

    // Equipa una nueva habilidad. Notifica el cambio.
    public void EquipAbility(ScrollAbility newAbility)
    {
        if (newAbility == null) return;

        equippedAbility = newAbility;
        OnScrollChanged?.Invoke(equippedAbility);
    }

    // Busca y equipa una habilidad del tipo indicado entre los componentes
    // configurados en availableAbilities. Devuelve false si falta el componente.
    public bool EquipAbility(ScrollType type)
    {
        if (availableAbilities == null) return false;

        foreach (ScrollAbility ability in availableAbilities)
        {
            if (ability != null && ability.AbilityType == type)
            {
                EquipAbility(ability);
                return true;
            }
        }

        Debug.LogWarning("No hay una habilidad configurada para el pergamino " + type + ".", this);
        return false;
    }
}
