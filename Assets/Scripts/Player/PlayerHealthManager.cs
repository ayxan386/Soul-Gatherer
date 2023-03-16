using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private Image healthImage;
    private TextMeshProUGUI healthDisplayText;

    void Start()
    {
        currentHealth = maxHealth;

        healthImage = GameObject.Find("Player-health-image-fill").GetComponent<Image>();
        healthDisplayText = GameObject.Find("Player-health-text").GetComponent<TextMeshProUGUI>();

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthImage.fillAmount = 1f * currentHealth / maxHealth;
        healthDisplayText.text = $"HP: {currentHealth}/{maxHealth}";
    }
}