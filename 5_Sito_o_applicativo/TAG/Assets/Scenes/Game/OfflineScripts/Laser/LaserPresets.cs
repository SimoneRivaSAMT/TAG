using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPresets
{
    private LaserSystem ls;

    public LaserPresets(GameObject laser)
    {
        ls = laser.GetComponent<LaserSystem>();
    }
    
    public void SelectSMG()
    {
        ls.damage = 2;
        ls.timeEachBurst = 0.05f;
        ls.spread = 0.02f;
        ls.range = 100;
        ls.reloadTime = 1;
        ls.timeBetweenShots = 0;
        ls.magazineSize = 30;
        ls.bulletsPerTap = 1;
        ls.allowButtonHold = true;
    }

    public void SelectShotgun()
    {
        ls.damage = 3;
        ls.timeEachBurst = 0;
        ls.spread = 0.06f;
        ls.range = 10;
        ls.reloadTime = 2;
        ls.timeBetweenShots = 0;
        ls.magazineSize = 24;
        ls.bulletsPerTap = 6;
        ls.allowButtonHold = false;
    }

    public void SelectBurstgun()
    {
        ls.damage = 7;
        ls.timeEachBurst = 0;
        ls.spread = 0.02f;
        ls.range = 100;
        ls.reloadTime = 1;
        ls.timeBetweenShots = 0.08f;
        ls.magazineSize = 30;
        ls.bulletsPerTap = 3;
        ls.allowButtonHold = true;
    }
}
