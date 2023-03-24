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
        EventStore.Instance.OnPlayerHealingApplied += OnPlayerHealingApplied;
        EventStore.Instance.OnPlayerMaxHealthChange += OnPlayerMaxHealthChange;
        EventStore.Instance.OnPlayerDataSave += OnPlayerDataSave;
    }


    private void OnDisable()
    {
        EventStore.Instance.OnPlayerAbilityAffected -= OnPlayerAbilityAffected;
        EventStore.Instance.OnPlayerHealingApplied -= OnPlayerHealingApplied;
        EventStore.Instance.OnPlayerMaxHealthChange -= OnPlayerMaxHealthChange;
        EventStore.Instance.OnPlayerDataSave -= OnPlayerDataSave;
    }


    private void OnPlayerDataSave(PlayerWorldData obj)
    {
        obj.currentHealth = currentHealth;
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
        if (currentHealth <= 0)
        {
            GameEndController.LoadEndScene(false);
        }

        UpdateUI();
        EventStore.Instance.PublishPlayerHealthChange(currentHealth);
    }

    private void UpdateUI()
    {
        healthImage.fillAmount = 1f * currentHealth / maxHealth;
        healthDisplayText.text = $"{currentHealth:0}/{maxHealth:0}";
    }


    private void OnPlayerHealingApplied(float amount, bool isFraction)
    {
        UpdateHealth(currentHealth + (isFraction ? maxHealth * amount : amount));
    }


    private void OnPlayerMaxHealthChange(float amount, bool isFraction)
    {
        var diff = (isFraction ? maxHealth * amount : amount);
        maxHealth += diff;
        print("Added " + diff);
        if (diff > 0) EventStore.Instance.PublishPlayerHealingApplied(diff, false);
        else UpdateUI();
    }
}