using UnityEngine;
using UnityEngine.UI;

// Opcional — Muestra el ícono del pergamino equipado en el HUD

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir al GameObject "ScrollIcon" que es una Image dentro del Canvas/HUD.

Componentes necesarios:
- Image en el mismo GameObject (ya existe al crear un UI Image).

Referencias del Inspector:
- scrollLoadout: arrastrar el componente ScrollLoadout del jugador.
- iconReady: arrastrar el sprite BookFire.png (habilidad disponible).
- iconCooldown: arrastrar el sprite BookFireDisabled.png (en cooldown).
- icon: arrastrar el componente Image de este mismo GameObject.
*/
public class ScrollIconHUD : MonoBehaviour
{
    [SerializeField] private ScrollLoadout scrollLoadout;
    [SerializeField] private Sprite iconReady;
    [SerializeField] private Sprite iconCooldown;
    [SerializeField] private Image icon;

    private void Update()
    {
        if (scrollLoadout == null || icon == null) return;

        // Si no hay habilidad equipada, oculta el ícono.
        if (scrollLoadout.EquippedAbility == null)
        {
            icon.enabled = false;
            return;
        }

        icon.enabled = true;

        // Muestra el ícono activo si la habilidad está lista, o el desactivado si está en cooldown.
        bool canUse = scrollLoadout.EquippedAbility.CanUse();
        icon.sprite = canUse ? iconReady : iconCooldown;
    }
}
