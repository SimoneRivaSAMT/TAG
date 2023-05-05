using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class EnemyLaserSystem : LaserSystem
{
    // Reference Variables
    private Enemy enemy;
    private GameObject target;
    private GameObject agent;

    [Header("Enemy stats")]
    public float enemyWaitTime;

    private void Awake()
    {
        // References
        agent = transform.parent.gameObject;
        enemy = agent.GetComponent<Enemy>();
        NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();
        enemy.Agent = navMeshAgent;
        laserEffect = agent.transform.Find("LaserEffect").gameObject;
        laserPresets = new LaserPresets(gameObject);
    }
    private void Start()
    {
        target = enemy.GetClosestPlayer();
        direction = target.transform.position - transform.position;
        position = transform.position;

        // Default values
        lasersLeft = magazineSize;
        readyToShoot = true;
        laserEffect.SetActive(false);
        StartCoroutine(EnemyTag());

        ChangeWeapon();
    }

    private void Update()
    {
        enemy.Agent = agent.GetComponent<NavMeshAgent>();
        target = enemy.GetClosestPlayer();
        direction = target.transform.position - transform.position;
        position = transform.position;
        MyInput();

        // Look at Player
        transform.parent.LookAt(target.transform);
    }

    private void MyInput()
    {
        // Shoot 
        if (readyToShoot && shooting)
        {
            lasersShot = bulletsPerTap;
            Shoot();
        }
    }
    private IEnumerator EnemyTag()
    {
        shooting = true;
        yield return new WaitForSeconds(enemyWaitTime);
        shooting = false;
        yield return new WaitForSeconds(enemyWaitTime);
        StartCoroutine(EnemyTag());
    }

    public override void ChangeWeapon()
    {
        dualSenseEffectType = (DualSenseEffectType)Random.Range(0, 2);
        
        switch ((int)dualSenseEffectType)
        {
            case 0:
                laserPresets.SelectSMG();
                break;
            case 1:
                laserPresets.SelectShotgun();
                break;
        }
    }
}
