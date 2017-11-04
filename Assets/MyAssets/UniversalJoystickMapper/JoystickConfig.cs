using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JoystickConfig {

    public string name, platform;
    public int joystickNumber;
    public KeyCode[] sourceKeyCodes = new KeyCode[UniversalJoystickMapper.MAX_NUM_BUTTONS];
    public string[] sourceAxisNames = new string[UniversalJoystickMapper.MAX_NUM_AXIS];

    [System.NonSerialized]
    public GamepadState state;

    public JoystickConfig(string joyName, string currentPlatform, int joyNum)
    {
        state = new GamepadState();
        joystickNumber = joyNum;
        name = joyName;
        platform = currentPlatform;
    }

    public void InitiateGamepadState()
    {
        state = new GamepadState();
        //Initiate buttons state
        for (int i = 0; i < sourceKeyCodes.Length; i++)
        {
            if (sourceKeyCodes[i] != KeyCode.None)
                state.ButtonsStates[i] = true;
        }
        //Initiate axes state
        for (int i = 0; i < sourceAxisNames.Length; i++)
            if (sourceAxisNames[i] != null && sourceAxisNames[i] != "")
                state.AxesStates[i] = true;
    }

    public bool GetButtonDown(Buttons button)
    {
        KeyCode code = GetSourceKeyCode(button);
        return Input.GetKeyDown(code);
    }

    public bool GetButtonUp(Buttons button)
    {
        KeyCode code = GetSourceKeyCode(button);
        return Input.GetKeyUp(code);
    }

    public bool GetButton(Buttons button)
    {
        KeyCode code = GetSourceKeyCode(button);
        return Input.GetKey(code);
    }

    public bool GetAxis(Axis axis)
    {
        string axisTarget = GetSourceAxisName(axis);
        return Input.GetAxis(axisTarget) != 0;
    }

    /// <summary>
    /// returns a specified axis
    /// </summary>
    /// <param name="axis">One of the analogue sticks, or the dpad</param>
    /// <param name="controlIndex">The controller number</param>
    /// <param name="raw">if raw is false then the controlIndex will be returned with a deadspot</param>
    /// <returns></returns>
    //public static Vector2 GetAxis(Axis axis, Index controlIndex, bool raw = false)
    //{
    //    string xName = "", yName = "";
    //    switch (axis)
    //    {
    //        case Axis.Dpad:
    //            xName = "DPad_XAxis_" + (int)controlIndex;
    //            yName = "DPad_YAxis_" + (int)controlIndex;
    //            break;
    //        case Axis.LAnalog:
    //            xName = "L_XAxis_" + (int)controlIndex;
    //            yName = "L_YAxis_" + (int)controlIndex;
    //            break;
    //        case Axis.RAnalog:
    //            xName = "R_XAxis_" + (int)controlIndex;
    //            yName = "R_YAxis_" + (int)controlIndex;
    //            break;
    //    }

    //    Vector2 axisXY = Vector3.zero;

    //    try
    //    {
    //        if (raw == false)
    //        {
    //            axisXY.x = Input.GetAxis(xName);
    //            axisXY.y = -Input.GetAxis(yName);
    //        }
    //        else
    //        {
    //            axisXY.x = Input.GetAxisRaw(xName);
    //            axisXY.y = -Input.GetAxisRaw(yName);
    //        }
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError(e);
    //        Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
    //    }
    //    return axisXY;
    //}

    //public static float GetTrigger(Triggers trigger, Index controlIndex, bool raw = false)
    //{
    //    //
    //    string name = "";
    //    if (trigger == Triggers.LTrigger)
    //        name = "TriggersL_" + (int)controlIndex;
    //    else if (trigger == Triggers.RTrigger)
    //        name = "TriggersR_" + (int)controlIndex;

    //    //
    //    float axis = 0;
    //    try
    //    {
    //        if (raw == false)
    //            axis = Input.GetAxis(name);
    //        else
    //            axis = Input.GetAxisRaw(name);
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError(e);
    //        Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
    //    }
    //    return axis;
    //}


    public KeyCode GetSourceKeyCode(Buttons button)
    {
        switch (button)
        {
            case Buttons.Cross: return sourceKeyCodes[0];
            case Buttons.Circle: return sourceKeyCodes[1];
            case Buttons.Triangle: return sourceKeyCodes[2];
            case Buttons.Square: return sourceKeyCodes[3];
            case Buttons.RShoulder: return sourceKeyCodes[4];
            case Buttons.LShoulder: return sourceKeyCodes[5];
            case Buttons.RStick: return sourceKeyCodes[6];
            case Buttons.LStick: return sourceKeyCodes[7];
            case Buttons.Select: return sourceKeyCodes[8];
            case Buttons.Start: return sourceKeyCodes[9];
            case Buttons.RTrigger: return sourceKeyCodes[10];
            case Buttons.LTrigger: return sourceKeyCodes[11];
            default: return KeyCode.None;
        }
    }

    public string GetSourceAxisName(Axis axis)
    {
        switch (axis)
        {
            case Axis.RAnalogX: return sourceAxisNames[0];
            case Axis.RAnalogY: return sourceAxisNames[1];
            case Axis.LAnalogX: return sourceAxisNames[2];
            case Axis.LAnalogY: return sourceAxisNames[3];
            case Axis.DpadX: return sourceAxisNames[4];
            case Axis.DpadY: return sourceAxisNames[5];
            default: return "";
        }
    }

    public class GamepadState
    {
        public bool[] ButtonsStates = new bool[UniversalJoystickMapper.MAX_NUM_BUTTONS];
        public bool[] AxesStates = new bool[UniversalJoystickMapper.MAX_NUM_AXIS];

        public GamepadState()
        {
            for (int i = 0; i < ButtonsStates.Length; i++)
                ButtonsStates[i] = false;
            for (int i = 0; i < AxesStates.Length; i++)
                AxesStates[i] = false;
        }
    }
}
