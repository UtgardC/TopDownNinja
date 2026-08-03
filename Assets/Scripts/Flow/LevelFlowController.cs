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
    
    [Header("Transición")]
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime = 1f;

    // Carga el nivel principal (ej: desde el botón del tutorial).
    public void LoadMainLevel()
    {
        StartCoroutine(LoadLevelCoroutine(mainLevelSceneName));
    }

    // Carga el tutorial (ej: al hacer clic en menú desde derrota/victoria).
    public void LoadTutorial()
    {
        StartCoroutine(LoadLevelCoroutine(tutorialSceneName));
    }

    // Reinicia el nivel actual (ej: desde el botón del panel de derrota).
    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().name));
    }

    // Carga un nivel específico por nombre.
    public void LoadCustomLevel(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    // Corrutina que activa la animación y espera antes de cargar.
    private System.Collections.IEnumerator LoadLevelCoroutine(string sceneName)
    {
        // Si hay una transición asignada, activa el trigger "Start" y espera.
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Start");
            
            // Si el tiempo está pausado (ej: por derrota), WaitForSeconds no funciona.
            // Por eso usamos WaitForSecondsRealtime.
            yield return new WaitForSecondsRealtime(transitionTime);
        }

        // Se asegura de reanudar el tiempo antes de cargar la nueva escena.
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
