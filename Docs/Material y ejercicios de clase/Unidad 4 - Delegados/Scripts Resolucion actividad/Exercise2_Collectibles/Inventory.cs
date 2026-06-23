using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Collectible collectible;
    [SerializeField] private TextMeshProUGUI inventoryText;

    private int itemCount = 0;

    private void OnEnable()
    {
        collectible.OnCollected += AddItem;
    }

    private void OnDisable()
    {
        collectible.OnCollected -= AddItem;
    }

    private void AddItem()
    {
        itemCount++;
        if (inventoryText != null)
            inventoryText.text = $"Ítems: {itemCount}";

        Debug.Log($"Ítem recolectado. Total: {itemCount}");
    }
}
