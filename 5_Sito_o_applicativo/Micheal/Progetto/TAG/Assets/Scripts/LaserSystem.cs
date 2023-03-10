using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserSystem : MonoBehaviour
{
    // Laser stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    // Bools
    private bool shooting, readyToShoot, reloading;

    //References
    public Camera fpsCam;
    public GameObject player;
    //public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public GameObject laserEffect;

    // Graphics
    //public GameObject muzzleFlash, bulletHoleGraphic; //https://assetstore.unity.com/packages/vfx/particles/legacy-particle-pack-73777#publisher
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    // InputManager
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = player.GetComponent<InputManager>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        laserEffect.SetActive(false);
    }

    private void Update()
    {
        MyInput();

        // SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold)
            //shooting = Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.JoystickButton7);
            shooting = inputManager.onAction.Tag.IsPressed();
        else
            //shooting = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.JoystickButton7);
            shooting = inputManager.onAction.Tag.triggered;

        if (inputManager.onAction.Reload.triggered && bulletsLeft < magazineSize && !reloading) 
            Reload();

        // Shoot 
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Spread
        /*if (rigidbody.velocity.magnitude > 0)
            spread *= 1.5f
        else
            spread = default spread*/
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);
        
        // RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy") || rayHit.collider.CompareTag("Player"))
            {
                Debug.Log("You tagged someone");
                rayHit.collider.GetComponent<PlayerHealth>().TakeDamage(damage);
            }else if (rayHit.collider.CompareTag("Shield"))
            {
                StartCoroutine(rayHit.collider.GetComponent<ShieldMaterial>().changeMaterial());
            }
        }

        // ShakeCamera
        StartCoroutine(camShake.Shake(camShakeMagnitude, camShakeDuration));

        // Graphics - need particles
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        StartCoroutine(LaserEffect());

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
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
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator LaserEffect()
    {
        laserEffect.SetActive(true);
        yield return new WaitForSeconds(timeBetweenShooting / 2);
        laserEffect.SetActive(false);
        yield break;
    }
}
