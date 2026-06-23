using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    [Header("Paneles de UI")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;

    private void OnEnable()
    {
        gameStateManager.OnGameStart += ShowHUD;
        gameStateManager.OnGamePause += ShowPauseMenu;
    }

    private void OnDisable()
    {
        gameStateManager.OnGameStart -= ShowHUD;
        gameStateManager.OnGamePause -= ShowPauseMenu;
    }

    private void ShowHUD()
    {
        hudPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    private void ShowPauseMenu()
    {
        hudPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
