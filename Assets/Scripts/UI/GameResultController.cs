using System.Collections;
using UnityEngine;

// Hito 13 — Victoria, derrota y HUD

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject "GameResultController" en la escena.

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- playerHealth: arrastrar el componente Health del jugador.
- objectiveTracker: arrastrar el componente ObjectiveTracker de la escena.
- levelFlow: arrastrar el componente LevelFlowController de la escena.
- victoryPanel: arrastrar el panel de UI de victoria (puede estar desactivado al inicio).
- defeatPanel: arrastrar el panel de UI de derrota (puede estar desactivado al inicio).

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Se suscribe a Health.OnDied del jugador para detectar la derrota.
- Se suscribe a ObjectiveTracker.OnVictory para detectar la victoria.
- Los paneles de victoria/derrota deben tener botones que llamen a
  levelFlow.ReloadCurrentScene() (reiniciar) o GameResultController.OnClickQuit() (salir).
*/
public class GameResultController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private ObjectiveTracker objectiveTracker;
    [SerializeField] private LevelFlowController levelFlow;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private float resultDelay = 0.75f;

    private bool resultTriggered;

    private void Awake()
    {
        // Evita que una escena nueva quede congelada si la anterior terminó pausada.
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnDied += HandleDefeat;

        if (objectiveTracker != null)
            objectiveTracker.OnVictory += HandleVictory;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnDied -= HandleDefeat;

        if (objectiveTracker != null)
            objectiveTracker.OnVictory -= HandleVictory;
    }

    private void Start()
    {
        // Asegura que los paneles estén ocultos al iniciar.
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null)  defeatPanel.SetActive(false);
    }

    // Muestra la pantalla de victoria y pausa el juego.
    private void HandleVictory()
    {
        if (!resultTriggered) StartCoroutine(ShowResult(victoryPanel));
    }

    // Muestra la pantalla de derrota y pausa el juego.
    private void HandleDefeat()
    {
        if (!resultTriggered) StartCoroutine(ShowResult(defeatPanel));
    }

    private IEnumerator ShowResult(GameObject panel)
    {
        resultTriggered = true;

        if (resultDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(resultDelay);
        }

        if (panel != null) panel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Reanuda el tiempo del juego. Llamar antes de cambiar de escena.
    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    // Reinicia la escena actual. Conectar al botón "Reintentar" del panel de derrota.
    public void OnClickRestart()
    {
        ResumeTime();
        if (levelFlow != null) levelFlow.ReloadCurrentScene();
    }

    // Cierra la aplicación. Conectar al botón "Salir" del panel de victoria o derrota.
    // En el Editor de Unity esto detiene el Play Mode en lugar de cerrar la ventana.
    public void OnClickQuit()
    {
        ResumeTime();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
