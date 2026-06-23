using UnityEngine;

public class Ejercicio2_SistemaDeSalud : MonoBehaviour
{
    int vidaActual = 100;

    void Start()
    {
        ModificarSalud(50, true);   // Dano del jefe
        ModificarSalud(30, false);  // Curacion con pocion
        ModificarSalud(10, true);   // Dano de enemigo debil
    }

    void ModificarSalud(int cantidad, bool esDano)
    {
        int vidaAnterior = vidaActual;

        if (esDano)
        {
            vidaActual = Mathf.Max(0, vidaActual - cantidad);
            Debug.Log("HP antes: " + vidaAnterior + " | Dano recibido: " + cantidad + " | HP ahora: " + vidaActual);
        }
        else
        {
            vidaActual += cantidad;
            Debug.Log("HP antes: " + vidaAnterior + " | Curacion: " + cantidad + " | HP ahora: " + vidaActual);
        }
    }
}
