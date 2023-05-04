using DualSenseSample.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualSenseManager : MonoBehaviour
{
    private DualSenseTrigger trigger;
    private DualSenseTouchpadColor touchpadColor;
    private DualSenseRumble rumble;

    public enum TriggerEffectType
    {
        continuousResistance,
        sectionResistance,
        effectResistance
    };

    
    [Header("Rumble")]
    [Range(0.0f, 1.0f)]
    public float leftRumble = 0.1f;
    [Range(0.0f, 1.0f)]
    public float rightRumble = 0.1f;
    

    [Header("Touchpad Color")]
    [Range(0.0f, 1.0f)]
    public float red = 0;
    [Range(0.0f, 1.0f)]
    public float green = 0;
    [Range(0.0f, 1.0f)]
    public float blue = 0;

    [Header("Triggers")]
    #region Left Trigger Properties
    [Header("Left")]
    public TriggerEffectType leftEffectType;
    [Header("Continuous Resistance")]
    [Range(0.0f, 1.0f)]
    public float leftContinuousStartPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float leftContinuousForce = 0f;
    [Header("Section Resistance")]
    [Range(0.0f, 1.0f)]
    public float leftSectionStartPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float leftSectionEndPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float leftSectionForce = 0f;
    [Header("Effect Resistance")]
    [Range(0.0f, 1.0f)]
    public float leftEffectStartPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float leftEffectBeginForce = 0f;
    [Range(0.0f, 1.0f)]
    public float leftEffectMiddleForce = 0f;
    [Range(0.0f, 1.0f)]
    public float leftEffectEndForce = 0f;
    [Range(0.0f, 1.0f)]
    public float leftEffectFrequency = 0f;
    #endregion

    #region Right Trigger Properties
    [Header("Right")]
    public TriggerEffectType rightEffectType;
    [Header("Continuous Resistance")]
    [Range(0.0f, 1.0f)]
    public float rightContinuousStartPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float rightContinuousForce = 0f;
    [Header("Section Resistance")]
    [Range(0.0f, 1.0f)]
    public float rightSectionStartPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float rightSectionEndPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float rightSectionForce = 0f;
    [Header("Effect Resistance")]
    [Range(0.0f, 1.0f)]
    public float rightEffectStartPosition = 0f;
    [Range(0.0f, 1.0f)]
    public float rightEffectBeginForce = 0f;
    [Range(0.0f, 1.0f)]
    public float rightEffectMiddleForce = 0f;
    [Range(0.0f, 1.0f)]
    public float rightEffectEndForce = 0f;
    [Range(0.0f, 1.0f)]
    public float rightEffectFrequency = 0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<DualSenseTrigger>();
        touchpadColor = GetComponent<DualSenseTouchpadColor>();
        rumble = GetComponent<DualSenseRumble>();

        StartCoroutine(UpdateController());
    }

    // Update is called once per frame
    void Update()
    {
        // Touchpad Color
        touchpadColor.UpdateRedColor(red);
        touchpadColor.UpdateGreenColor(green);
        touchpadColor.UpdateBlueColor(blue);
    }

    
    public void StartHaptics()
    {
        rumble.DualSense.SetMotorSpeeds(leftRumble, rightRumble);
        rumble.DualSense.ResumeHaptics();
    }

    public void StopHaptics()
    {
        rumble.DualSense.PauseHaptics();
    }
    

    private IEnumerator UpdateController()
    {
        yield return new WaitForSeconds(0.1f);
        #region LeftTrigger 
        // Effect Type
        trigger.LeftTriggerEffectType = (int)leftEffectType;

        // Continuous Resistance
        trigger.LeftContinuousStartPosition = leftContinuousStartPosition;
        trigger.LeftContinuousForce = leftContinuousForce;

        // Section Resistance
        trigger.LeftSectionStartPosition = leftSectionStartPosition;
        trigger.LeftSectionEndPosition = leftSectionEndPosition;
        trigger.LeftSectionForce = leftSectionForce;

        // Effect Resistance
        trigger.LeftEffectStartPosition = leftEffectStartPosition;
        trigger.LeftEffectBeginForce = leftEffectBeginForce;
        trigger.LeftEffectMiddleForce = leftEffectMiddleForce;
        trigger.LeftEffectEndForce = leftEffectEndForce;
        trigger.LeftEffectFrequency = leftEffectFrequency;
        #endregion

        #region RightTrigger 
        // Effect Type
        trigger.RightTriggerEffectType = (int)rightEffectType;

        // Continuous Resistance
        trigger.RightContinuousStartPosition = rightContinuousStartPosition;
        trigger.RightContinuousForce = rightContinuousForce;

        // Section Resistance
        trigger.RightSectionStartPosition = rightSectionStartPosition;
        trigger.RightSectionEndPosition = rightSectionEndPosition;
        trigger.RightSectionForce = rightSectionForce;

        // Effect Resistance
        trigger.RightEffectStartPosition = rightEffectStartPosition;
        trigger.RightEffectBeginForce = rightEffectBeginForce;
        trigger.RightEffectMiddleForce = rightEffectMiddleForce;
        trigger.RightEffectEndForce = rightEffectEndForce;
        trigger.RightEffectFrequency = rightEffectFrequency;
        #endregion

        // Rumble
        rumble.LeftRumble = leftRumble;
        rumble.RightRumble = rightRumble;

        StartCoroutine(UpdateController());
        yield return null;
    }
}
