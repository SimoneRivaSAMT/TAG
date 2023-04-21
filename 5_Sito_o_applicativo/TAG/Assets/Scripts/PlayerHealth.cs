using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private GameObject parentBar;
    private CameraShake camShake;
    private PlayerManager playerManager;
    private Image frontHealthBar;
    private Image backHealthBar;
    private TextMeshProUGUI healthText;
    private List<GameObject> players;
    //private List<GameObject> healthBars;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public GameObject healthBarPrefab;

    [Header("Damage Overlay")]
    public Image overlay; // DamageOverlay Gameobject
    public float duration; // How long the image stays fully opaque
    public float fadeSpeed; // How quickly the image will fade

    [Header("Camera Shake")]
    public float camShakeMagnitude;
    public float camShakeDuration;

    private float durationTimer; // Timer to check against the duration

    void Start()
    {
        // Array that contains health bars of all players
        playerManager = new PlayerManager();
        players = playerManager.GetPlayers();

        health = maxHealth;
        lerpTimer = 0;

        // References from Player
        parentBar = GameObject.FindGameObjectWithTag("Container");
        if (tag == "Player")
        {
            camShake = transform.Find("CameraHolder").Find("MainCamera").gameObject.GetComponent<CameraShake>();
        }

        if(overlay != null)
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);

        SpawnHealthBar(-359, 96);
    }

    private void SpawnHealthBar(float x, float y)
    {
        int num = 0;
        GameObject g = Instantiate(healthBarPrefab, new Vector3(x, y), Quaternion.identity);
        g.transform.parent = parentBar.transform;
        if (tag == "Player")
        {
            g.name = "HealthBarP";
            g.transform.localPosition = new Vector3(0, 0);
            g.transform.localScale = new Vector3(0.9f, 0.9f);
        }
        else
        {
            Debug.Log(parentBar.transform.Find("HealthBarE" + num));
            while(parentBar.transform.Find("HealthBarE" + num) != null)
            {
                num++;
            }
            g.name = "HealthBarE" + num;
            g.transform.localPosition = new Vector3(x, y - (65 * num-1));
            g.transform.localScale = new Vector3(0.5f, 0.5f);
        }
        frontHealthBar = g.transform.Find("FrontHealthBar")
                         .gameObject.GetComponent<Image>();
        backHealthBar = g.transform.Find("BackHealthBar")
                        .gameObject.GetComponent<Image>();
        healthText = g.transform.Find("HealthText")
                     .gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthUI();
            if (health <= 0)
            {
                if (gameObject.GetComponent<MeshRenderer>().enabled)
                {
                    playerManager.RespawnPlayer(gameObject);
                }
            }

            if (overlay != null)
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

    public float GetHealth()
    {
        return health;
    }
}
