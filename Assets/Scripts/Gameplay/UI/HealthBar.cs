using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Entity target;

    [Header("Color Settings")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private float midHealthPercentage = 0.66f;
    [SerializeField] private Color midHealthColor = new Color(1f, .64f, 0f, 1f);

    [SerializeField] private float lowHealthPercentage = 0.25f;
    [SerializeField] private Color lowHealthColor = Color.red;

    [Header("Animation Settings")]

    [SerializeField] private float animationDurationMultiplier = 0.75f;

    private float maxHealth;
    private bool visible = false;

    private Vector3 oldScale;

    // Start is called before the first frame update
    void Start()
    {
        oldScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        maxHealth = target.health;
        target.addOnHealthChangeListener(onHealthChange);
    }

    void onHealthChange(float oldHealth, float newHealth)
    {
        if (!visible)
        {
            transform.DOScale(oldScale, 0.15f);
        }
        float duration = animationDurationMultiplier * (newHealth / maxHealth);

        healthBarImage.DOFillAmount(newHealth / maxHealth, duration);

        Color newColor = fullHealthColor;
        if (newHealth < maxHealth * lowHealthPercentage)
        {
            newColor = lowHealthColor;
        }
        else if (newHealth < maxHealth * midHealthPercentage)
        {
            newColor = midHealthColor;
        }

        healthBarImage.DOColor(newColor, duration);
    }
}
