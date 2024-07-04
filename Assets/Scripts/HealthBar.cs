using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthFillImage;

    public void UpdateHealthBar(float healthPercent)
    {
        healthFillImage.fillAmount = healthPercent;
    }
}

