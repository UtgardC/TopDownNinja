using UnityEngine;

// Pergamino intercambiable. Al recogerlo, el objeto del mundo pasa a
// representar la habilidad que el jugador tenía equipada previamente.
public class ScrollCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private ScrollType scrollType = ScrollType.Fire;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite rockSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public ScrollType ScrollType => scrollType;

    public void Collect(PlayerCollector collector)
    {
        if (collector == null || collector.ScrollLoadout == null) return;

        ScrollAbility previousAbility = collector.ScrollLoadout.EquippedAbility;
        if (!collector.ScrollLoadout.EquipAbility(scrollType)) return;

        // Si no había pergamino anterior, el objeto fue consumido.
        if (previousAbility == null || previousAbility.AbilityType == scrollType)
        {
            gameObject.SetActive(false);
            return;
        }

        // Intercambio real: el pergamino anterior queda en el lugar del nuevo.
        scrollType = previousAbility.AbilityType;
        RefreshSprite();
    }

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        RefreshSprite();
    }

    private void RefreshSprite()
    {
        if (spriteRenderer == null) return;

        switch (scrollType)
        {
            case ScrollType.Fire:
                if (fireSprite != null) spriteRenderer.sprite = fireSprite;
                break;
            case ScrollType.Rock:
                if (rockSprite != null) spriteRenderer.sprite = rockSprite;
                break;
        }
    }
}
