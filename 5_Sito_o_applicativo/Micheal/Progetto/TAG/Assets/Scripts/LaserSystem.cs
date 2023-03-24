using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserSystem : MonoBehaviour
{
    // Private Variables
    private InputManager inputManager;
    private DualSensePresets dualSensePresets;
    private RaycastHit rayHit;

    [Header("Laser Stats")]
    public int damage;
    public float timeEachBurst, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int lasersLeft, lasersShot;
    private bool shooting, readyToShoot, reloading;

    [Header("References")]
    public GameObject player;
    public Camera fpsCam;
    public GameObject laserEffect;
    public TextMeshProUGUI ammoText;
    public CameraShake camShake;
    [Header("Cam Shake Stats")]
    public float camShakeMagnitude, camShakeDuration;
    // Graphics
    //public GameObject muzzleFlash, bulletHoleGraphic; //https://assetstore.unity.com/packages/vfx/particles/legacy-particle-pack-73777#publisher


    private void Awake()
    {
        inputManager = player.GetComponent<InputManager>();
        dualSensePresets = new DualSensePresets(player);

        lasersLeft = magazineSize;
        readyToShoot = true;
        laserEffect.SetActive(false);
        dualSensePresets.TouchPadColor(1, 0, 1);
        dualSensePresets.LeftShield();
    }

    private void Update()
    {

        if (lasersLeft == 0 || reloading)
            dualSensePresets.RightEmpty();
        else
            dualSensePresets.RightSMG();

        MyInput();

        // Set Ammo Text
        ammoText.SetText(lasersLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
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
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);
        
        // Check what's in front using RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            // If it's another player/AI deal damage
            if (rayHit.collider.CompareTag("Enemy") || rayHit.collider.CompareTag("Player"))
            {
                rayHit.collider.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            // If you hit a shield than it turns red
            else if (rayHit.collider.CompareTag("Shield"))
            {
                StartCoroutine(rayHit.collider.GetComponent<ShieldMaterial>().changeMaterial());
            }
        }

        // ShakeCamera
        StartCoroutine(camShake.Shake(camShakeMagnitude, camShakeDuration));

        // Graphics - need particles
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        // Display laser effect briefly
        StartCoroutine(LaserEffect());

        lasersLeft--;
        lasersShot--;

        // If it's burst then allow more shots
        Invoke("ResetShot", timeEachBurst);

        // Keep on shooting after small period
        if (lasersShot > 0 && lasersLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        lasersLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator LaserEffect()
    {
        laserEffect.SetActive(true);
        yield return new WaitForSeconds(timeEachBurst / 2);
        laserEffect.SetActive(false);
        yield break;
    }
}
