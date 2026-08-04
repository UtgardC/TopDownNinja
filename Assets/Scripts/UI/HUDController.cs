using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Hito 13 — Controlador del HUD (UI en pantalla)

/*
CONFIGURACIÓN EN UNITY

GameObject:
- Añadir este script a la UI de HUD del Canvas.

Referencias del Inspector:
- playerHealth: arrastrar el componente Health del jugador.
- scoreTracker: arrastrar el componente ScoreTracker del jugador.
- powerUpController: arrastrar el componente TemporaryPowerUpController del jugador.

UI de Corazones:
- heartsContainer: un panel vacío con HorizontalLayoutGroup para alinear los corazones.
- heartPrefab: un prefab que solo tiene un componente Image.
- fullHeartSprite, halfHeartSprite, emptyHeartSprite: tus 3 assets pixel art.
- hpPerHeart: 10 (según lo acordado).

UI de Textos:
- scoreText: arrastrar el texto de puntos (TextMeshProUGUI).
- buffText: arrastrar el texto indicador de buff activo (TextMeshProUGUI).
*/
public class HUDController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private TemporaryPowerUpController powerUpController;

    [Header("Sistema de Salud (Corazones)")]
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private int hpPerHeart = 10;

    [Header("Sistema de Habilidades")]
    [SerializeField] private ScrollLoadout playerScrolls;
    [SerializeField] private Image cooldownFillImage;
    [SerializeField] private Image scrollIconBackground; // El fondo del icono (opcional)
    [SerializeField] private Sprite fireScrollSpriteUI;
    [SerializeField] private Sprite rockScrollSpriteUI;
    [SerializeField] private TextMeshProUGUI chargesText;

    [Header("Otros Elementos")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [Header("Sistema de Buffs")]
    [SerializeField] private Transform buffsContainer;
    [SerializeField] private GameObject buffPrefab;
    [SerializeField] private Sprite speedBuffSprite;
    [SerializeField] private Sprite damageBuffSprite;
    [SerializeField] private Sprite attackSpeedBuffSprite;

    [System.Serializable]
    private class ActiveBuffUI
    {
        public BuffType type;
        public float remainingTime;
        public GameObject uiObject;
        public Image icon;
        public TextMeshProUGUI timerText;
    }

    private List<ActiveBuffUI> activeBuffs = new List<ActiveBuffUI>();
    private List<Image> heartImages = new List<Image>();

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += UpdateHealthDisplay;

        if (scoreTracker != null)
            scoreTracker.OnScoreChanged += UpdateScoreDisplay;

        if (powerUpController != null)
        {
            powerUpController.OnBuffStarted += ShowBuffDisplay;
            powerUpController.OnBuffEnded += HideBuffDisplay;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;

        if (scoreTracker != null)
            scoreTracker.OnScoreChanged -= UpdateScoreDisplay;

        if (powerUpController != null)
        {
            powerUpController.OnBuffStarted -= ShowBuffDisplay;
            powerUpController.OnBuffEnded -= HideBuffDisplay;
        }
    }

    private void Start()
    {
        // Inicializa los textos de la interfaz con los valores actuales.
        if (playerHealth != null)
            UpdateHealthDisplay(playerHealth.CurrentHealth, playerHealth.MaxHealth);

        if (scoreTracker != null)
            UpdateScoreDisplay(scoreTracker.GetScore());

        if (chargesText != null)
            chargesText.gameObject.SetActive(false); // Oculto al inicio.
    }

    // Actualiza el marcador de corazones del ninja.
    private void UpdateHealthDisplay(int current, int max)
    {
        if (heartsContainer == null || heartPrefab == null) return;

        int totalHeartsNeeded = Mathf.CeilToInt((float)max / hpPerHeart);

        // 1. Asegurar que haya la cantidad correcta de contenedores de corazones
        while (heartImages.Count < totalHeartsNeeded)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
            Image img = newHeart.GetComponent<Image>();
            if (img != null) heartImages.Add(img);
        }
        while (heartImages.Count > totalHeartsNeeded)
        {
            Image imgToRemove = heartImages[heartImages.Count - 1];
            heartImages.RemoveAt(heartImages.Count - 1);
            Destroy(imgToRemove.gameObject);
        }

        // 2. Actualizar los sprites de cada corazón
        for (int i = 0; i < heartImages.Count; i++)
        {
            int currentHeartStartHP = i * hpPerHeart;
            int currentHeartEndHP = currentHeartStartHP + hpPerHeart;
            int halfHeartThreshold = currentHeartStartHP + (hpPerHeart / 2);

            if (current >= currentHeartEndHP)
            {
                // El jugador tiene más vida que el valor total de este corazón
                heartImages[i].sprite = fullHeartSprite;
            }
            else if (current >= halfHeartThreshold)
            {
                // El jugador tiene suficiente vida para llenar medio corazón, pero no entero
                heartImages[i].sprite = halfHeartSprite;
            }
            else
            {
                // El jugador no tiene vida suficiente para llenar ni la mitad
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    // Actualiza el marcador de puntuación.
    private void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    // Muestra el tipo y la duración restante del Buff recogido.
    private void ShowBuffDisplay(BuffType type, float duration)
    {
        if (buffsContainer == null || buffPrefab == null) return;

        // Si ya existe este buff, actualizamos su tiempo en lugar de crear uno nuevo.
        ActiveBuffUI existingBuff = activeBuffs.Find(b => b.type == type);
        if (existingBuff != null)
        {
            existingBuff.remainingTime = duration;
            return;
        }

        // Si no existe, creamos un nuevo elemento en el contenedor.
        GameObject newBuffObj = Instantiate(buffPrefab, buffsContainer);
        ActiveBuffUI newBuff = new ActiveBuffUI
        {
            type = type,
            remainingTime = duration,
            uiObject = newBuffObj,
            icon = newBuffObj.transform.Find("Icon")?.GetComponent<Image>(),
            timerText = newBuffObj.GetComponentInChildren<TextMeshProUGUI>()
        };

        // Asignamos el sprite correspondiente según el tipo de buff.
        if (newBuff.icon != null)
        {
            switch (type)
            {
                case BuffType.Speed:       newBuff.icon.sprite = speedBuffSprite; break;
                case BuffType.Damage:      newBuff.icon.sprite = damageBuffSprite; break;
                case BuffType.AttackSpeed: newBuff.icon.sprite = attackSpeedBuffSprite; break;
            }
        }

        activeBuffs.Add(newBuff);
    }

    // Oculta el texto del buff al finalizar el efecto.
    private void HideBuffDisplay(BuffType type)
    {
        ActiveBuffUI existingBuff = activeBuffs.Find(b => b.type == type);
        if (existingBuff != null)
        {
            activeBuffs.Remove(existingBuff);
            if (existingBuff.uiObject != null)
            {
                Destroy(existingBuff.uiObject);
            }
        }
    }

    private void Update()
    {
        // 1. Lógica del sistema de cargas (habilidades)
        if (playerScrolls != null)
        {
            ScrollAbility ability = playerScrolls.EquippedAbility;
            
            // Lógica si tenemos una habilidad válida y con cargas
            if (ability != null && ability.GetCurrentCharges() > 0)
            {
                if (cooldownFillImage != null && ability.GetMaxCooldown() > 0)
                {
                    float remaining = ability.GetCooldownRemaining();
                    float max = ability.GetMaxCooldown();
                    cooldownFillImage.fillAmount = 1f - (remaining / max);

                    // Cambiamos el sprite según el tipo de pergamino equipado
                    Sprite currentSprite = ability.AbilityType == ScrollType.Fire ? fireScrollSpriteUI : rockScrollSpriteUI;
                    cooldownFillImage.sprite = currentSprite;
                    if (scrollIconBackground != null) scrollIconBackground.sprite = currentSprite;
                }
                
                if (chargesText != null)
                {
                    chargesText.gameObject.SetActive(true);
                    chargesText.text = ability.GetCurrentCharges().ToString();
                }
            }
            else
            {
                // Si la habilidad se gastó o no tenemos habilidad
                if (cooldownFillImage != null)
                {
                    cooldownFillImage.fillAmount = 0f;
                }
                
                if (chargesText != null)
                {
                    chargesText.gameObject.SetActive(false);
                }
                
                // Si las cargas llegaron a 0, desequipamos la habilidad
                if (ability != null && ability.GetCurrentCharges() <= 0)
                {
                    playerScrolls.EquipAbility(null);
                }
            }
        }

        // 2. Lógica del sistema de buffs (temporizadores)
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            ActiveBuffUI buff = activeBuffs[i];
            buff.remainingTime -= Time.deltaTime;

            if (buff.timerText != null)
            {
                // Muestra siempre el tiempo como número entero (redondeado hacia arriba)
                buff.timerText.text = Mathf.CeilToInt(buff.remainingTime).ToString() + "s";
            }

            // Si el tiempo llegó a cero por nuestra cuenta, lo destruimos (el evento de OnBuffEnded también hace esto por seguridad)
            if (buff.remainingTime <= 0f)
            {
                if (buff.uiObject != null) Destroy(buff.uiObject);
                activeBuffs.RemoveAt(i);
            }
        }
    }
}
