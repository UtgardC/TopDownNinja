using UnityEngine;

public class Ejercicio5_SistemaDePuntuacion : MonoBehaviour
{
    void Start()
    {
        // Combinacion 1 - Rango S
        int puntaje1 = CalcularPuntaje(35, 45, 1.5f);
        string rango1 = ObtenerCalificacion(puntaje1);
        MostrarResumen("Player1", puntaje1, rango1);

        // Combinacion 2 - Rango B
        int puntaje2 = CalcularPuntaje(15, 20, 1.0f);
        string rango2 = ObtenerCalificacion(puntaje2);
        MostrarResumen("Player2", puntaje2, rango2);

        // Combinacion 3 - Rango D
        int puntaje3 = CalcularPuntaje(5, 8, 0.8f);
        string rango3 = ObtenerCalificacion(puntaje3);
        MostrarResumen("NewPlayer", puntaje3, rango3);
    }

    int CalcularPuntaje(int enemigos, int tiempoRestante, float bonusDificultad)
    {
        return Mathf.RoundToInt((enemigos * 100 + tiempoRestante * 10) * bonusDificultad);
    }

    string ObtenerCalificacion(int puntaje)
    {
        if      (puntaje >= 5000) return "S";
        else if (puntaje >= 3500) return "A";
        else if (puntaje >= 2000) return "B";
        else if (puntaje >= 1000) return "C";
        else                      return "D";
    }

    void MostrarResumen(string nombreJugador, int puntaje, string calificacion)
    {
        Debug.Log("Jugador : " + nombreJugador);
        Debug.Log("Puntaje : " + puntaje);
        Debug.Log("Rango   : " + calificacion);
    }
}
