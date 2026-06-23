using UnityEngine;

public class Ejercicio4_InventarioDelPersonaje : MonoBehaviour
{
    float pesoActual = 0f;
    float pesoMaximo = 20f;

    void Start()
    {
        RecogerObjeto("Espada",8.5f);
        RecogerObjeto("Escudo",6.5f);
        RecogerObjeto("Armadura",12f);
        RecogerObjeto("Pocion",2f);
    }

    bool HayEspacio(float pesoObjeto)
    {
        return (pesoActual + pesoObjeto) <= pesoMaximo;
    }

    void RecogerObjeto(string nombre, float peso)
    {
        if (HayEspacio(peso))
        {
            pesoActual += peso;
            float pesoRedondeado = Mathf.Round(pesoActual * 10f) / 10f;
            Debug.Log("Recogiste " + nombre + ". Peso actual: " + pesoRedondeado + "/" + pesoMaximo);
        }
        else
        {
            Debug.Log("No puedes cargar " + nombre + ". Inventario lleno.");
        }
    }
}
