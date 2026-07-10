// Hito 1 — Contratos y bases

// Contrato para objetos que el jugador puede recoger.
// Implementado por: CoinCollectible, FoodCollectible, BuffCollectible.
// Permite que PlayerCollector recolecte objetos distintos con el mismo flujo.
public interface ICollectible
{
    // Ejecuta el efecto del coleccionable. collector es el jugador que lo recoge.
    void Collect(PlayerCollector collector);
}
