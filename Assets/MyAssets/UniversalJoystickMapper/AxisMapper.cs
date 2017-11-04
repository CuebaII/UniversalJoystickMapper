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

    protected override void Start()
    {
        OnActiveChange += OnActiveChangeHandler;
        OnSubmitChange += OnSubmitChangeHandler;
        axis = GetButtonValue(axisNumber.text);
        if (UniversalJoystickMapper.CurrentJoystickBeingMapped.sourceKeyCodes[(int)axis] != 0)
            OnSubmit = true;
    }

    private void Update()
    {
        if (m_IsActive && GetJoystickAxisPressed())
        {
            m_IsActive = false;
            onSubmit = true;
        }


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
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        m_IsActive = true;
    }

    private bool GetJoystickAxisPressed()
    {
        int axisN = 1; // start at joy 1 keycode

        // log button presses on 3 joysticks (20 button inputs per joystick)
        // NOTE THAT joystick 4 is not supported via keycodes for some reason, so only polling 1-3
        for (int i = 0; i < 26; i++)
        {
            string sourceString = "JoystickAxis " + axisN;
            // Log any key press
            if (Input.GetAxis(sourceString) > 0)
            {
                UniversalJoystickMapper.CurrentJoystickBeingMapped.sourceAxisName[(int)axis] = sourceString;
                Debug.Log(UniversalJoystickMapper.CurrentJoystickBeingMapped.name + " Joystick " + 
                    UniversalJoystickMapper.CurrentJoystickBeingMapped.joystickNumber + " axis " + (axisN + i) + " @ " + Time.time);
                return true;
            }
        }
        return false;
    }

    Axis GetButtonValue(string buttonName)
    {
        if (buttonName == Convert.ToString(Axis.LeftStick))
            return Axis.LeftStick;
        else if (buttonName == Convert.ToString(Axis.RightStick))
            return Axis.RightStick;
        else if (buttonName == Convert.ToString(Axis.Dpad))
            return Axis.Dpad;
        

        Debug.LogError("Invald axis set!");
        return Axis.None;
    }
}
