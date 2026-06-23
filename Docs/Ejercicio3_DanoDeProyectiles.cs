using UnityEngine;

public class Ejercicio3_DanoDeProyectiles : MonoBehaviour
{
    void Start()
    {
        LanzarProyectil(30, 2.0f, 5,  "Orco Jefe");   // Critico
        LanzarProyectil(20, 1.0f, 8,  "Goblin");      // Normal
        LanzarProyectil(50, 1.8f, 10, "Dragon");      // Critico
    }

    // Metodo auxiliar que coordina el calculo y la aplicacion
    void LanzarProyectil(int danoBase, float multiplicador, int resistencia, string nombreEnemigo)
    {
        int danoFinal = CalcularDano(danoBase, multiplicador, resistencia);
        AplicarDano(danoFinal, nombreEnemigo, multiplicador);
    }

    int CalcularDano(int danoBase, float multiplicador, int resistencia)
    {
        int danoFinal = Mathf.Max(1, Mathf.RoundToInt(danoBase * multiplicador) - resistencia);
        return danoFinal;
    }

    void AplicarDano(int danoFinal, string nombreEnemigo, float multiplicador)
    {
        bool esCritico = multiplicador > 1.5f;

        if (esCritico)
            Debug.Log("[CRITICO] " + nombreEnemigo + " recibe " + danoFinal + " puntos de dano.");
        else
            Debug.Log(nombreEnemigo + " recibe " + danoFinal + " puntos de dano.");
    }
}
