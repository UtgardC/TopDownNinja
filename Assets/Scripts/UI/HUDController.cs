using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Hito 13 — Victoria, derrota y HUD

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Crear un GameObject "HUDController" hijo del Canvas.

Componentes necesarios:
- Ninguno adicional.

Referencias del Inspector:
- playerHealth: arrastrar el componente Health del jugador.
- scoreTracker: arrastrar el componente ScoreTracker del jugador.
- powerUpController: arrastrar el componente TemporaryPowerUpController del jugador.
- healthText: arrastrar un TextMeshProUGUI que muestre la vida.
- scoreText: arrastrar un TextMeshProUGUI que muestre el puntaje.
- buffText: arrastrar un TextMeshProUGUI que muestre el buff activo.

Layers y Tags:
- Ninguno requerido por este script.

Notas:
- Se suscribe a los eventos en OnEnable y se desuscribe en OnDisable
  para evitar referencias huérfanas si el objeto es destruido.
- buffText se muestra solo cuando hay un buff activo.
*/
public class HUDController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private TemporaryPowerUpController powerUpController;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI buffText;

    private readonly Dictionary<BuffType, float> activeBuffs = new Dictionary<BuffType, float>();

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
        // Muestra los valores iniciales al cargar la escena.
        if (playerHealth != null)
            UpdateHealthDisplay(playerHealth.CurrentHealth, playerHealth.MaxHealth);

        if (scoreTracker != null)
            UpdateScoreDisplay(scoreTracker.GetScore());

        if (buffText != null)
            buffText.gameObject.SetActive(false);
    }

    // Actualiza el texto de vida. Recibe salud actual y máxima.
    private void UpdateHealthDisplay(int current, int max)
    {
        if (healthText != null)
            healthText.text = "HP: " + current + " / " + max;
    }

    // Actualiza el texto del puntaje.
    private void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }

    // Muestra el texto del buff activo con su duración.
    private void ShowBuffDisplay(BuffType type, float duration)
    {
        activeBuffs[type] = duration;
        RefreshBuffDisplay();
    }

    // Oculta el texto del buff cuando termina.
    private void HideBuffDisplay(BuffType type)
    {
        activeBuffs.Remove(type);
        RefreshBuffDisplay();
    }

    private void RefreshBuffDisplay()
    {
        if (buffText == null) return;

        if (activeBuffs.Count == 0)
        {
            buffText.gameObject.SetActive(false);
            return;
        }

        List<string> labels = new List<string>();
        foreach (KeyValuePair<BuffType, float> buff in activeBuffs)
        {
            labels.Add(GetBuffName(buff.Key) + " (" + buff.Value + "s)");
        }

        buffText.gameObject.SetActive(true);
        buffText.text = "BUFF: " + string.Join(" / ", labels);
    }

    // Devuelve el nombre en español del buff para mostrarlo en el HUD.
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
