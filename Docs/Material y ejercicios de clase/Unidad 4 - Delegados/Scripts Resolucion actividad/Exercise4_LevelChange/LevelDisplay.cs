using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        levelManager.OnLevelChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        levelManager.OnLevelChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int level)
    {
        levelText.text = $"Nivel {level}";
    }
}
