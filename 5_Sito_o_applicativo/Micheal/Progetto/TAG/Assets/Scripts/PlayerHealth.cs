using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("Damage Overlay")]
    public Image overlay; // Our DamageOverlay Gameobject
    public float duration; // How long the image stays fully opaque
    public float fadeSpeed; // How quickly the image will fade

    [Header("Camera Shake")]
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;

    private float durationTimer; // Timer to check against the duration

    void Start()
    {
        health = maxHealth;
        if(overlay != null)
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0 , maxHealth);
        UpdateHealthUI();
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        if(overlay != null)
        {
            if (overlay.color.a > 0)
            {
                if (health < 30)
                    return;
                durationTimer += Time.deltaTime;
                if (durationTimer > duration)
                {
                    // Fade the image
                    float tempAlpha = overlay.color.a;
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
                }
            }
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if(fillBack > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);
        }

        if(fillFront < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
        healthText.text = health + "/" + maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        if(overlay != null)
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);

        // ShakeCamera
        if(camShake != null)
            StartCoroutine(camShake.Shake(camShakeMagnitude, camShakeDuration));
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
