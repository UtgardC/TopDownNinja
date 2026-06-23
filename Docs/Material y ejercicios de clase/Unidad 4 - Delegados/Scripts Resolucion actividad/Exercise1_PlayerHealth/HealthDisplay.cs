using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(float health)
    {
        healthText.text = $"Salud: {health:F0}";
    }
}
