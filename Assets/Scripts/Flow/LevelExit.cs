using UnityEngine;
using UnityEngine.SceneManagement;

// Script de zona de salida de nivel — se activa cuando el jugador la toca.

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject vacío "LevelExit" en el punto de salida del nivel.
- Puede ser invisible o tener un SpriteRenderer con un sprite de puerta/portal.

Componentes necesarios:
- Collider2D configurado como Trigger (BoxCollider2D o CircleCollider2D).
  Ajustar el tamaño para que cubra el área de salida.

Referencias del Inspector:
- nextSceneName: nombre exacto de la escena a cargar.
  Debe coincidir con el nombre en Build Settings (File → Build Settings → Scenes In Build).
- playerTag: tag del jugador. Por defecto "Player".

Layers y Tags:
- El jugador debe tener el Tag "Player".

Notas:
- Para usarlo entre cualquier par de escenas, simplemente cambiar nextSceneName
  en el Inspector. No hace falta modificar el script.
- Si querés mostrar una transición (fundido), podés agregar esa lógica en OnPlayerEnter.
- Asegurarse de que la escena destino esté en Build Settings.
*/
public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            LoadNextScene();
        }
    }

    // Carga la escena configurada en el Inspector.
    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("LevelExit: el campo 'nextSceneName' está vacío. Asignarlo en el Inspector.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    // Dibuja la zona de salida en el Editor para facilitar su posicionamiento.
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Collider2D col = GetComponent<Collider2D>();

        if (col is BoxCollider2D box)
        {
            Gizmos.DrawCube(transform.position + (Vector3)box.offset, box.size);
        }
        else if (col is CircleCollider2D circle)
        {
            Gizmos.DrawSphere(transform.position + (Vector3)circle.offset, circle.radius);
        }
    }
}
