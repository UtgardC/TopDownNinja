using UnityEngine;
using UnityEngine.SceneManagement;

// Script de zona de salida de nivel — se activa cuando el jugador la toca.

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject vacío "LevelExit" en el punto de salida del nivel.

Componentes necesarios:
- Collider2D configurado como Trigger (BoxCollider2D o CircleCollider2D).

Referencias del Inspector:
- nextSceneName: nombre exacto de la escena a cargar.
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

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("LevelExit: el campo 'nextSceneName' está vacío. Asignarlo en el Inspector.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

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
