using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualSensePresets
{
    private DualSenseManager dsm;
    public DualSensePresets(GameObject player)
    {
        dsm = player.GetComponent<DualSenseManager>();
    }

    public void SelectLeftShield()
    {
        ResetLeftTriggers();
        dsm.leftEffectType = DualSenseManager.TriggerEffectType.continuousResistance;

        dsm.leftContinuousForce = 0.5f;

        dsm.leftSectionStartPosition = 0.15f;
        dsm.leftSectionEndPosition = 0.5f;
    }
    public void SelectRightEmpty()
    {
        ResetRightTriggers();
        dsm.rightEffectType = DualSenseManager.TriggerEffectType.sectionResistance;

        dsm.rightSectionEndPosition = 0.3f;
    }

    public void SelectRightSMG()
    {
        ResetRightTriggers();
        dsm.rightEffectType = DualSenseManager.TriggerEffectType.effectResistance;

        dsm.rightSectionStartPosition = 0.3f;
        dsm.rightSectionEndPosition = 0.7f;

        dsm.rightEffectBeginForce = 0.5f;
        dsm.rightEffectMiddleForce = 0.75f;
        dsm.rightEffectEndForce = 1f;
        dsm.rightEffectFrequency = 0.05f;
    }

    public void SelectRightShotgun()
    {
        ResetRightTriggers();
        dsm.rightEffectType = DualSenseManager.TriggerEffectType.sectionResistance;

        dsm.rightSectionStartPosition = 0.2f;
        dsm.rightSectionEndPosition = 0.5f;
        dsm.rightSectionForce = 0.3f;
    }

    public void SetTouchPadColor(float r, float g, float b)
    {
        dsm.red = r;
        dsm.green = g;
        dsm.blue = b;
    }

    public void ResetRightTriggers()
    {
        dsm.rightContinuousForce = 0;
        dsm.rightContinuousStartPosition = 0;

        dsm.rightSectionStartPosition = 0;
        dsm.rightSectionEndPosition = 0;
        dsm.rightSectionForce = 0;

        dsm.rightEffectStartPosition = 0;
        dsm.rightEffectBeginForce = 0;
        dsm.rightEffectMiddleForce = 0;
        dsm.rightEffectEndForce = 0;
        dsm.rightEffectFrequency = 0;
    }

    public void ResetLeftTriggers()
    {
        dsm.leftContinuousForce = 0;
        dsm.leftContinuousStartPosition = 0;

        dsm.leftSectionStartPosition = 0;
        dsm.leftSectionEndPosition = 0;
        dsm.leftSectionForce = 0;

        dsm.leftEffectStartPosition = 0;
        dsm.leftEffectBeginForce = 0;
        dsm.leftEffectMiddleForce = 0;
        dsm.leftEffectEndForce = 0;
        dsm.leftEffectFrequency = 0;
    }
}
