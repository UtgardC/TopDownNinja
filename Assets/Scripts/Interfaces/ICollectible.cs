// Hito 1 — Interfaces del sistema de combate y recelección

/*
CONFIGURACIÓN EN UNITY
- No se añaden a ningún GameObject directamente (son interfaces).
*/

// Interfaz para objetos del escenario que el jugador puede recoger al tocarlos.
public interface ICollectible
{
    // Se ejecuta cuando el jugador (a través de su PlayerCollector) entra en contacto con el objeto.
    void Collect(PlayerCollector collector);
}
