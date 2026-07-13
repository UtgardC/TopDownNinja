using UnityEngine;
using UnityEngine.SceneManagement;

// Hito 12 — Controlador de flujo de nivel (cambio de escenas)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject "LevelFlowController" en la escena.

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- mainLevelSceneName: nombre exacto de la escena de juego principal (ej: "Level1").
- tutorialSceneName: nombre exacto de la escena de tutorial (ej: "Tutorial").

Notas:
- Las escenas deben estar agregadas en File -> Build Settings para poder cargarse.
*/
public class LevelFlowController : MonoBehaviour
{
    [SerializeField] private string mainLevelSceneName = "Level1";
    [SerializeField] private string tutorialSceneName = "Tutorial";

    // Carga el nivel principal (ej: desde el botón del tutorial).
    public void LoadMainLevel()
    {
        SceneManager.LoadScene(mainLevelSceneName);
    }

    // Carga el tutorial (ej: al hacer clic en menú desde derrota/victoria).
    public void LoadTutorial()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    // Reinicia el nivel actual (ej: desde el botón del panel de derrota).
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
