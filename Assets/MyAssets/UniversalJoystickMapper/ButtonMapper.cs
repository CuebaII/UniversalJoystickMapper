using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonMapper : Selectable, IPointerClickHandler
{
    public Image m_image;
    Buttons button;
    public Text btnNumber;

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
        button = GetButtonValue(btnNumber.text);
        if (UniversalJoystickMapper.CurrentJoystickBeingMapped.state.Buttons[(int)button] != false)
            OnSubmit = true;
    }

    private void Update()
    {
        if (m_IsActive && GetJoystickButtonPressed())
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
        {
            m_image.color = Color.green;
            UniversalJoystickMapper.CurrentJoystickBeingMapped.state.Buttons[(int)button] = true;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        m_IsActive = true;
    }

    private bool GetJoystickButtonPressed()
    {
        int joyNum = 1; // start at 1 because unity calls them joystick 1 - 4
        int buttonNum = 0;
        int keyCode = 350; // start at joy 1 keycode

        // log button presses on 3 joysticks (20 button inputs per joystick)
        // NOTE THAT joystick 4 is not supported via keycodes for some reason, so only polling 1-3
        for (int i = 0; i < 160; i++)
        {
            // Log any key press
            if (Input.GetKeyDown((KeyCode)keyCode + i))
            {
                UniversalJoystickMapper.CurrentJoystickBeingMapped.sourceKeyCodes[(int)button] = (KeyCode)keyCode + i;
                Debug.Log(UniversalJoystickMapper.CurrentJoystickBeingMapped.name + " Joystick " + joyNum + " Button " + (keyCode+i)  + " @ " + Time.time);
                return true;
            }

            buttonNum++; // Increment

            // Reset button count when we get to last joy button
            if (buttonNum == 20)
            {
                buttonNum = 0;
                joyNum++; // next joystick
            }
        }
        return false;
    }

    Buttons GetButtonValue(string buttonName)
    {
        if (buttonName == Convert.ToString(Buttons.Cross))
            return Buttons.Cross;
        else if (buttonName == Convert.ToString(Buttons.Circle))
            return Buttons.Circle;
        else if (buttonName == Convert.ToString(Buttons.Triangle))
            return Buttons.Triangle;
        else if (buttonName == Convert.ToString(Buttons.Square))
            return Buttons.Square;
        else if (buttonName == Convert.ToString(Buttons.RShoulder))
            return Buttons.RShoulder;
        else if (buttonName == Convert.ToString(Buttons.LShoulder))
            return Buttons.LShoulder;
        else if (buttonName == Convert.ToString(Buttons.RStick))
            return Buttons.RStick;
        else if (buttonName == Convert.ToString(Buttons.LStick))
            return Buttons.LStick;
        else if (buttonName == Convert.ToString(Buttons.Select))
            return Buttons.Select;
        else if (buttonName == Convert.ToString(Buttons.Start))
            return Buttons.Start;
        else if (buttonName == Convert.ToString(Buttons.RTrigger))
            return Buttons.RTrigger;
        else if (buttonName == Convert.ToString(Buttons.LTrigger))
            return Buttons.LTrigger;

        Debug.LogError("Invald button set!");
        return Buttons.None;
    }
}
