using UnityEngine;

// Hace que la cámara siga al jugador con suavizado opcional.

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir este script a la Main Camera de la escena.

Referencias del Inspector:
- target: arrastrar el Transform del jugador ("Player").
- smoothSpeed: qué tan suavemente sigue al jugador (0 = instantáneo, valores cercanos a 1 = muy lento).
- offset: desplazamiento fijo respecto al jugador (Z debe ser negativo, ej: -10).
*/
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
