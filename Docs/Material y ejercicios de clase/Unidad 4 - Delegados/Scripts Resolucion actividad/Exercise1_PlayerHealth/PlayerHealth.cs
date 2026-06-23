using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public event Action<float> OnHealthChanged;

    public float Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0f, 100f);
            OnHealthChanged?.Invoke(health);
        }
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(health);
    }

    public void TakeDamage(float amount) => Health -= amount;
    public void Heal(float amount) => Health += amount;
}
