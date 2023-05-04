using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetPlayerLaserSystem : NetLaserSystem
{
    [HideInInspector]
    public CameraShake camShake;

    [Header("Cam Shake Stats")]
    public float camShakeMagnitude;
    public float camShakeDuration;

    private void Awake()
    {
        // References from Player/CameraHolder/MainCamera/Laser
        fpsCam = transform.parent.gameObject.GetComponent<Camera>();
        player = fpsCam.transform.parent.transform.parent.gameObject;
        camShake = fpsCam.GetComponent<CameraShake>();
        laserEffect = fpsCam.transform.Find("LaserEffect").gameObject;
        ammoText = player.transform.Find("PlayerUI")
                   .Find("Ammo").gameObject.GetComponent<TextMeshProUGUI>();
        inputManager = player.GetComponent<InputManager>();
        dualSensePresets = new DualSensePresets(player);
        laserPresets = new NetLaserPresets(gameObject);

        // Default values
        lasersLeft = magazineSize;
        readyToShoot = true;
        laserEffect.SetActive(false);
        dualSensePresets.SetTouchPadColor(1, 0, 1);
        dualSensePresets.SelectLeftShield();
        camShakeMagnitude = 0.05f;
        camShakeDuration = 0.05f;
    }

    private void Update()
    {
        direction = fpsCam.transform.forward;
        position = fpsCam.transform.position;

        MyInput();

        // Set Ammo Text
        ammoText.SetText(lasersLeft + " / " + magazineSize);
    }

    
    public void MyInput()
    {
        ChangeWeapon();

        if (allowButtonHold)
            shooting = inputManager.onAction.Tag.IsPressed();
        else
            shooting = inputManager.onAction.Tag.triggered;

        // Reloading
        if (inputManager.onAction.Reload.triggered && lasersLeft < magazineSize && !reloading) 
            Reload();

        // Shoot 
        if (readyToShoot && shooting && !reloading && lasersLeft > 0)
        {
            lasersShot = bulletsPerTap;
            Shoot();

            // ShakeCamera
            StartCoroutine(camShake.Shake(camShakeMagnitude, camShakeDuration));
        }
    }
}
