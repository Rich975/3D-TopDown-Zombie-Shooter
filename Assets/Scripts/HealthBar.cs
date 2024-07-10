using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Image damagePanel;
    public float flashDuration = 0.2f;

    [SerializeField] private Color originalColor;
    [SerializeField] private Color flashColor;

    private void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    public void UpdateHealthBar(float healthPercent)
    {
        healthSlider.value = healthPercent;
    }

    public void DamageFlash()
    {
        StartCoroutine(FlashDamagePanel());
    }

    private IEnumerator FlashDamagePanel()
    {
        float timer = 0f;
        float alpha = 0f;
        damagePanel.color = originalColor;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Sin((timer / flashDuration) * Mathf.PI);
            damagePanel.color = Color.Lerp(originalColor, flashColor, alpha);
            yield return null;
        }

        // Ensure the panel returns to the original (transparent) color
        //alpha = 0f;
        //damagePanel.color = originalColor;
    }
}