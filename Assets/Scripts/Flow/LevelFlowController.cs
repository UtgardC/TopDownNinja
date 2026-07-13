using UnityEngine;
using UnityEngine.SceneManagement;

// Hito 12 — Tutorial, progresión y objetivo

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject vacío "LevelFlowController" en cada escena.

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- mainLevelSceneName: nombre exacto de la escena del nivel principal
  (debe estar registrada en Build Settings → Scenes In Build).
- tutorialSceneName: nombre exacto de la escena del tutorial.

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- LoadMainLevel() se llama al presionar el botón "Jugar" del tutorial.
- ReloadCurrentScene() se llama desde GameResultController al perder.
- Asegurarse de agregar ambas escenas al Build Settings de Unity
  (File → Build Settings → Add Open Scenes).
*/
public class LevelFlowController : MonoBehaviour
{
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private string mainLevelSceneName = "Level1";
    [SerializeField] private string secondaryLevelSceneName = "Level2";

    // Carga el nivel principal del juego.
    public void LoadMainLevel()
    {
        LoadScene(mainLevelSceneName);
    }

    // Carga la escena del tutorial.
    public void LoadTutorial()
    {
        LoadScene(tutorialSceneName);
    }

    public void LoadSecondLevel()
    {
        LoadScene(secondaryLevelSceneName);
    }

    // Recarga la escena actual. Útil para reiniciar tras una derrota.
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("El nombre de la escena no está configurado.", this);
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("La escena '" + sceneName + "' no está registrada en Build Settings.", this);
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
