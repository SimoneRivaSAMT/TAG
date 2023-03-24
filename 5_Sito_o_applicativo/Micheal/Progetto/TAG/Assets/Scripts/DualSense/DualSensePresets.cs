using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualSensePresets
{
    private DualSenseManager dualSenseManager;
    public DualSensePresets(GameObject player)
    {
        dualSenseManager = player.GetComponent<DualSenseManager>();
    }

    public void LeftShield()
    {
        dualSenseManager.leftEffectType = DualSenseManager.TriggerEffectType.continuousResistance;
        dualSenseManager.leftContinuousStartPosition = 0;
        dualSenseManager.leftContinuousForce = 0.5f;
        dualSenseManager.leftSectionStartPosition = 0.15f;
        dualSenseManager.leftSectionEndPosition = 0.5f;
    }
    public void RightEmpty()
    {
        dualSenseManager.rightEffectType = DualSenseManager.TriggerEffectType.sectionResistance;
        dualSenseManager.rightSectionStartPosition = 0;
        dualSenseManager.rightSectionEndPosition = 0.3f;
        dualSenseManager.rightEffectStartPosition = 0;
        dualSenseManager.rightEffectBeginForce = 0;
        dualSenseManager.rightEffectMiddleForce = 0;
        dualSenseManager.rightEffectEndForce = 0;
        dualSenseManager.rightEffectFrequency = 0;
    }

    public void RightSMG()
    {
        dualSenseManager.rightEffectType = DualSenseManager.TriggerEffectType.effectResistance;
        dualSenseManager.rightSectionStartPosition = 0.3f;
        dualSenseManager.rightSectionEndPosition = 0.7f;
        dualSenseManager.rightEffectStartPosition = 0;
        dualSenseManager.rightEffectBeginForce = 0.5f;
        dualSenseManager.rightEffectMiddleForce = 0.75f;
        dualSenseManager.rightEffectEndForce = 1f;
        dualSenseManager.rightEffectFrequency = 0.075f;
    }

    public void TouchPadColor(float r, float g, float b)
    {
        dualSenseManager.red = r;
        dualSenseManager.green = g;
        dualSenseManager.blue = b;
    }
}
