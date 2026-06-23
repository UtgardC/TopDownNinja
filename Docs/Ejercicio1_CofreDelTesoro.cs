using UnityEngine;

public class Ejercicio1_CofreDelTesoro : MonoBehaviour
{
    string tipoCofre = "Cofre de Madera";
    string recompensa = "50 monedas de oro";

    void Start()
    {
        AbrirCofre("Gonza");
        AbrirCofre("Pepe");
    }

    void AbrirCofre(string nombreHeroe)
    {
        Debug.Log("El heroe " + nombreHeroe + " abre un " + tipoCofre + " y encuentra: " + recompensa);
    }
}
