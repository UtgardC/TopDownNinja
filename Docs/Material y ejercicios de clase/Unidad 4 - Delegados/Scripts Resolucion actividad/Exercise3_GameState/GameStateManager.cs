using System;
using UnityEngine;
public class GameStateManager : MonoBehaviour
{
    public event Action OnGameStart;
    public event Action OnGamePause;

    public void StartGame()
    {
        Time.timeScale = 1f;
        OnGameStart?.Invoke();
        Debug.Log("Juego iniciado.");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePause?.Invoke();
        Debug.Log("Juego pausado.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0f)
                PauseGame();
            else
                StartGame();
        }
    }
}
