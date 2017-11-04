using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class AxisMapper : Selectable, IPointerClickHandler
{
    public Image m_image;
    Axis axis;
    public Text axisNumber;

    public bool onSubmit = false;
    public bool OnSubmit
    {
        get { return onSubmit; }
        set
        {
            if (onSubmit == value) return;
            onSubmit = value;
            if (OnSubmitChange != null)
                OnSubmitChange(onSubmit);
        }
    }

    public bool isActive = false;
    public bool m_IsActive
    {
        get { return isActive; }
        set
        {
            if (isActive == value) return;
            isActive = value;
            if (OnActiveChange != null)
                OnActiveChange(isActive);
        }
    }

    public delegate void OnVariableChangeDelegate(bool newValue);
    public event OnVariableChangeDelegate OnActiveChange;
    public event OnVariableChangeDelegate OnSubmitChange;

    protected override void Awake()
    {
        OnActiveChange += OnActiveChangeHandler;
        OnSubmitChange += OnSubmitChangeHandler;
        axis = GetAxisValue(axisNumber.text);
    }

    private void Update()
    {
        if (m_IsActive && GetJoystickAxisPressed())
        {
            m_IsActive = false;
            onSubmit = true;
        }
    }

    protected override void OnEnable()
    {
        if (UniversalJoystickMapper.CurrentJoystickBeingMapped.state.AxesStates[(int)axis])
            OnSubmit = true;
    }

    protected override void OnDisable()
    {
        OnSubmit = false;
    }

    private void OnActiveChangeHandler(bool newValue)
    {
        if (newValue)
            m_image.color = Color.gray;
        else
            m_image.color = Color.green;
    }

    private void OnSubmitChangeHandler(bool newValue)
    {
        if (newValue)
            m_image.color = Color.green;
        else
            m_image.color = Color.white;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        m_IsActive = true;
    }

    private bool GetJoystickAxisPressed()
    {
        JoystickConfig currentJoystick = UniversalJoystickMapper.CurrentJoystickBeingMapped;
        int joyNum = currentJoystick.joystickNumber + 1;
        int axisN = 1; // start at axis 1

        for (int i = 0; i < 28; i++)
        {
            string sourceString = "JoystickAxis " + axisN;
            // Log any key press
            if (Input.GetAxis(sourceString) > 0)
            {
                UniversalJoystickMapper.CurrentJoystickBeingMapped.sourceAxisNames[(int)axis] = sourceString;
                Debug.Log(UniversalJoystickMapper.CurrentJoystickBeingMapped.name + " Joystick " + 
                    UniversalJoystickMapper.CurrentJoystickBeingMapped.joystickNumber + " axis " + (axisN) + " @ " + Time.time);
                return true;
            }
            axisN++;
        }
        return false;
    }

    Axis GetAxisValue(string buttonName)
    {
        if (buttonName == "RAnalogX")
            return Axis.RAnalogX;
        else if (buttonName == "RAnalogY")
            return Axis.RAnalogY;
        else if (buttonName == "LAnalogX")
            return Axis.LAnalogX;
        else if (buttonName == "LAnalogY")
            return Axis.LAnalogY;
        else if (buttonName == "DpadX")
            return Axis.DpadX;
        else if (buttonName == "DpadY")
            return Axis.DpadY;


        Debug.LogError("Invald axis set!");
        return Axis.None;
    }
}
