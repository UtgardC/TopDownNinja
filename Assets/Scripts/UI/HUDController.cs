using UnityEngine;
using TMPro;

// Hito 13 — Controlador del HUD (UI en pantalla)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir este script a la UI de HUD del Canvas.

Referencias del Inspector:
- playerHealth: arrastrar el componente Health del jugador.
- scoreTracker: arrastrar el componente ScoreTracker del jugador.
- powerUpController: arrastrar el componente TemporaryPowerUpController del jugador.
- healthText: arrastrar el texto de vida (TextMeshProUGUI).
- scoreText: arrastrar el texto de puntos (TextMeshProUGUI).
- buffText: arrastrar el texto indicador de buff activo (TextMeshProUGUI).
*/
public class HUDController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private TemporaryPowerUpController powerUpController;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI buffText;

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += UpdateHealthDisplay;

        if (scoreTracker != null)
            scoreTracker.OnScoreChanged += UpdateScoreDisplay;

        if (powerUpController != null)
        {
            powerUpController.OnBuffStarted += ShowBuffDisplay;
            powerUpController.OnBuffEnded += HideBuffDisplay;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;

        if (scoreTracker != null)
            scoreTracker.OnScoreChanged -= UpdateScoreDisplay;

        if (powerUpController != null)
        {
            powerUpController.OnBuffStarted -= ShowBuffDisplay;
            powerUpController.OnBuffEnded -= HideBuffDisplay;
        }
    }

    private void Start()
    {
        // Inicializa los textos de la interfaz con los valores actuales.
        if (playerHealth != null)
            UpdateHealthDisplay(playerHealth.CurrentHealth, playerHealth.MaxHealth);

        if (scoreTracker != null)
            UpdateScoreDisplay(scoreTracker.GetScore());

        if (buffText != null)
            buffText.gameObject.SetActive(false); // Oculto al inicio.
    }

    // Actualiza el marcador de vida del ninja.
    private void UpdateHealthDisplay(int current, int max)
    {
        if (healthText != null)
            healthText.text = "HP: " + current + " / " + max;
    }

    // Actualiza el marcador de puntuación.
    private void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }

    // Muestra el tipo y la duración restante del Buff recogido.
    private void ShowBuffDisplay(BuffType type, float duration)
    {
        if (buffText == null) return;

        buffText.gameObject.SetActive(true);
        buffText.text = "BUFF: " + GetBuffName(type) + " (" + duration + "s)";
    }

    // Oculta el texto del buff al finalizar el efecto.
    private void HideBuffDisplay(BuffType type)
    {
        if (buffText != null)
            buffText.gameObject.SetActive(false);
    }

    // Traduce el Enum a un string legible para mostrar en pantalla.
    private string GetBuffName(BuffType type)
    {
        switch (type)
        {
            case BuffType.Speed:       return "Velocidad";
            case BuffType.Damage:      return "Daño";
            case BuffType.AttackSpeed: return "Ataque rápido";
            default:                   return "Buff";
        }
    }
}
