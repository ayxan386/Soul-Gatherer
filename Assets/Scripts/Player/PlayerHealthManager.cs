using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    private Image healthImage;
    private TextMeshProUGUI healthDisplayText;

    private void OnEnable()
    {
        EventStore.Instance.OnPlayerAbilityAffected += OnPlayerAbilityAffected;
        EventStore.Instance.OnPlayerHealthChange += OnPlayerHealthChange;
    }

    private void OnDisable()
    {
        EventStore.Instance.OnPlayerAbilityAffected -= OnPlayerAbilityAffected;
        EventStore.Instance.OnPlayerHealthChange -= OnPlayerHealthChange;
    }

    private void OnPlayerHealthChange(float obj)
    {
        UpdateUI();
    }

    private void OnPlayerAbilityAffected(AbilityParam ability)
    {
        UpdateHealth(currentHealth - ability.damage * (ability.tickDamage ? Time.deltaTime : 1));
    }

    private void Start()
    {
        healthImage = GameObject.Find("Player-health-image-fill").GetComponent<Image>();
        healthDisplayText = GameObject.Find("Player-health-text").GetComponent<TextMeshProUGUI>();
        UpdateHealth(maxHealth);
    }

    private void UpdateHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        EventStore.Instance.PublishPlayerHealthChange(currentHealth);
    }

    private void UpdateUI()
    {
        healthImage.fillAmount = 1f * currentHealth / maxHealth;
        healthDisplayText.text = $"{currentHealth:0}/{maxHealth:0}";
    }
}