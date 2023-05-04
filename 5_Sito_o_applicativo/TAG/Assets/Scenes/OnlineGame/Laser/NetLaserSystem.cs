using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class NetLaserSystem : MonoBehaviour
{
    #region Reference Variables
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Camera fpsCam;
    [HideInInspector]
    public GameObject laserEffect;
    [HideInInspector]
    public TextMeshProUGUI ammoText;
    [HideInInspector]
    public InputManager inputManager;
    [HideInInspector]
    public DualSensePresets dualSensePresets;
    [HideInInspector]
    public NetLaserPresets laserPresets;
    [HideInInspector]
    public RaycastHit rayHit;
    [HideInInspector]
    public Vector3 direction, position;
    #endregion

    public enum DualSenseEffectType
    {
        SMG,
        Shotgun
    };

    public DualSenseEffectType dualSenseEffectType;

    [Header("Laser Stats")]
    public int damage;
    public float timeEachBurst, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    [HideInInspector]
    public int lasersLeft, lasersShot;
    [HideInInspector]
    public bool shooting, readyToShoot, reloading;

    public void Shoot()
    {
        readyToShoot = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate Direction with Spread
        Vector3 raycastDirection = direction + new Vector3(x, y, 0);

        // Check what's in front using RayCast
        if (Physics.Raycast(position, raycastDirection, out rayHit, range))
        {
            // If it's another player/AI deal damage
            if (rayHit.collider.CompareTag("Enemy") || rayHit.collider.CompareTag("Player"))
            {
                //Tells server to decrement player's life
                FindObjectOfType<DamageManager>().PlayerHittedServerRpc(
                    rayHit.collider.GetComponent<NetworkPlayer>().NetworkObjectId,
                    transform.parent.parent.parent.GetComponent<NetworkPlayer>().NetworkObjectId);
                // Display bullet holes effect very briefly
                DisplayBulletHoleEffect(0.2f);
            }
            // If you hit a shield than it turns red
            else if (rayHit.collider.CompareTag("Shield"))
            {
                // Display bullet holes effect very briefly
                DisplayBulletHoleEffect(0.2f);
                StartCoroutine(rayHit.collider.GetComponent<ShieldMaterial>().changeMaterial());
            }
            else
            {
                // Display bullet holes effect very briefly
                DisplayBulletHoleEffect(5f);
            }
        }

        // Display laser effect briefly
        StartCoroutine(DisplayLaserEffect());
        lasersLeft--;
        lasersShot--;

        // If it's burst then allow more shots
        Invoke("ResetShot", timeEachBurst);

        // Keep on shooting after small period
        if (lasersShot > 0 && lasersLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    public void ResetShot()
    {
        readyToShoot = true;
    }

    public void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    public void ReloadFinished()
    {
        lasersLeft = magazineSize;
        reloading = false;
    }

    public IEnumerator DisplayLaserEffect()
    {
        laserEffect.SetActive(true);
        yield return new WaitForSeconds(timeEachBurst / 2);
        laserEffect.SetActive(false);
        yield break;
    }

    public void DisplayBulletHoleEffect(float seconds)
    {
        var go = Instantiate(laserEffect, rayHit.point, Quaternion.Euler(0, 180, 0));
        go.SetActive(true);
        Destroy(go.GetComponent<SphereCollider>());
        Destroy(go, seconds);
    }

    public virtual void ChangeWeapon()
    {
        if (inputManager.onAction.ChangeLaser.triggered)
        {
            dualSenseEffectType++;
            lasersLeft = 0;
        }

        if ((int)dualSenseEffectType > 1)
            dualSenseEffectType = 0;

        switch ((int)dualSenseEffectType)
        {
            case 0:
                dualSensePresets.SelectRightSMG();
                laserPresets.SelectSMG();
                break;
            case 1:
                dualSensePresets.SelectRightShotgun();
                laserPresets.SelectShotgun();
                break;
        }

        if (lasersLeft == 0 || reloading)
            dualSensePresets.SelectRightEmpty();
    }
}
