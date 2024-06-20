using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthFillImage;

    public void SetMaxHealth(int health)
    {
        // Optionally, set the initial fill amount
        healthFillImage.fillAmount = 1f;
    }

    public void SetHealth(int health, int maxHealth)
    {
        healthFillImage.fillAmount = (float)health / maxHealth;
    }
}
