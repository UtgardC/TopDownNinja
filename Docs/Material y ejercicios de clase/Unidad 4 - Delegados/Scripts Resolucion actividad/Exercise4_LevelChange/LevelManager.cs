using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;

    public event Action<int> OnLevelChanged;

    private void Start()
    {
        OnLevelChanged?.Invoke(currentLevel);
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        OnLevelChanged?.Invoke(currentLevel);
        Debug.Log($"Cargando nivel {currentLevel}...");
    }

    public void ResetToLevel(int level)
    {
        currentLevel = level;
        OnLevelChanged?.Invoke(currentLevel);
    }
}
