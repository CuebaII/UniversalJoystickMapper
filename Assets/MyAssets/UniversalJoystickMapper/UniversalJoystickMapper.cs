using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class UniversalJoystickMapper {

    #region Defaults
    public const int MAX_NUM_JOYSTICK = 8;
    public const int MAX_NUM_BUTTONS = 12;
    public const int MAX_NUM_AXIS = 6;
    #endregion

    /// <summary>
    /// The directory where are stored all the configured joysticks.
    /// </summary>
    private static string JOYSTICK_CONFIG_DIRECTORY
    {
        get
        {
            return Application.streamingAssetsPath + "/JsonAssets/joystick_config.json";
        }
    }

    static UniversalJoystickMapper instance;
    public static UniversalJoystickMapper Instance
    {
        get
        {
            if (instance != null)
                return instance;
            else
                return instance = new UniversalJoystickMapper();
        }
    }

    public static JoystickConfig[] availableJoysticks;
    public static List<JoystickConfig> newUnknownJoysticks;
    public static JoystickConfig[] joysticksAttached;

    static List<JoystickConfig> queueOfJoysticksToMap;
    static JoystickConfig currentJoystickBeingMapped;
    public static JoystickConfig CurrentJoystickBeingMapped
    {
        get { return currentJoystickBeingMapped; }
        set
        {
            if (currentJoystickBeingMapped == value) return;
            currentJoystickBeingMapped = value;
            if (OnJoystickToMapChanged != null)
                OnJoystickToMapChanged();
        }
    }

    public delegate void JoystickToMapChangedDelegate();
    public static event JoystickToMapChangedDelegate OnJoystickToMapChanged;

    public GameObject joystickConfigPanel;

    static bool DEBUG_ON = true;
    private static JsonData configData;

    private UniversalJoystickMapper()
    {
        //Change this to initialize it through a json file
        newUnknownJoysticks = new List<JoystickConfig>();
        joysticksAttached = new JoystickConfig[MAX_NUM_JOYSTICK];
        queueOfJoysticksToMap = new List<JoystickConfig>();
        ReadFromFile();
        OnJoystickToMapChanged += OnJoystickToMapChangedHandler;
    }

    public void Update()
    {
        OnJoystickToggle();
    }

    /// <summary>
    /// Detects joystick being attached and detached.
    /// </summary>
    void OnJoystickToggle()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            //A just is attached
            if (Input.GetJoystickNames()[i] != null && Input.GetJoystickNames()[i].Length > 0)
            {
                //A joystick has just been attached
                if (joysticksAttached[i] == null)
                {
                    OnJoystickAttached(i, Input.GetJoystickNames()[i]);
                }
            }
            //A joystick has been detached
            else if (joysticksAttached[i] != null)
            {
                Debug.Log(Input.GetJoystickNames()[i] + " has been detached!");
                joysticksAttached[i] = null;
                if (CurrentJoystickBeingMapped.joystickNumber == i)
                    CurrentJoystickBeingMapped = null;
            }
        }
    }

    public void OnJoystickAttached(int joyNum, string joyName)
    {
        //Check if a profile for the joystick already exists
        if (availableJoysticks != null)
            for (int i = 0; i < availableJoysticks.Length; i++)
            {
                JoystickConfig j = availableJoysticks[i];
                if (j != null && j.name == joyName)
                    if (j.platform == Application.platform.ToString() && j.joystickNumber == joyNum)
                    {
                        Debug.Log(availableJoysticks[i].name + " has been plugged in has player " + (joyNum + 1) + "(LOADED)");
                        joysticksAttached[joyNum] = j;
                        if (CurrentJoystickBeingMapped == null)
                            CurrentJoystickBeingMapped = joysticksAttached[joyNum];
                        else
                            queueOfJoysticksToMap.Add(joysticksAttached[joyNum]);
                        return;
                    }
            }

        //Otherwise creates a new profile
        JoystickConfig newJoystick = new JoystickConfig(joyName, Application.platform.ToString(), joyNum);
        newUnknownJoysticks.Add(newJoystick);
        joysticksAttached[joyNum] = newJoystick;

        if (CurrentJoystickBeingMapped == null)
            CurrentJoystickBeingMapped = newJoystick;
        else
            queueOfJoysticksToMap.Add(newJoystick);
        Debug.Log(Input.GetJoystickNames()[joyNum] + " has been plugged in has player " + (joyNum + 1) + "(NEW)");
    }

    void OnJoystickToMapChangedHandler()
    {
        //If there is a Joystick to map
        if (CurrentJoystickBeingMapped != null)
        {
            joystickConfigPanel.SetActive(true);
            Debug.Log("Current joystick being mapped is: " + currentJoystickBeingMapped.name);
        }
        //If there is not, check if there is a Joystick in the queue waiting to be mapped.
        //TODO: Set the current joystick being mapped to null somehow after completely mapped.
        else
        {
            if (queueOfJoysticksToMap.Count > 0)
            {
                JoystickConfig newJoystickToMap = queueOfJoysticksToMap[0];
                queueOfJoysticksToMap.RemoveAt(0);
                joystickConfigPanel.SetActive(false);
                CurrentJoystickBeingMapped = newJoystickToMap;
            }
            else
            {
                joystickConfigPanel.SetActive(false);
            }
        }
    }

    public void CloseConfigPanel()
    {
        CurrentJoystickBeingMapped = null;
    }

    public static bool GetButtonDown(Buttons button)
    {
        for (int i = 0; i < joysticksAttached.Length; i++)
        {
            if (joysticksAttached[i] != null && joysticksAttached[i].GetButtonDown(button))
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetButtonUp(Buttons button)
    {
        for (int i = 0; i < joysticksAttached.Length; i++)
        {
            if (joysticksAttached[i] != null && joysticksAttached[i].GetButtonUp(button))
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetButton(Buttons button)
    {
        for (int i = 0; i < joysticksAttached.Length; i++)
        {
            if (joysticksAttached[i] != null && joysticksAttached[i].GetButton(button))
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetAxis(Axis axis)
    {
        for (int i = 0; i < joysticksAttached.Length; i++)
        {
            if (joysticksAttached[i] != null && joysticksAttached[i].GetAxis(axis))
            {
                return true;
            }
        }
        return false;
    }

    #region Json
    public static void ReadFromFile()
    {
        if (File.Exists(JOYSTICK_CONFIG_DIRECTORY))
        {
            string contents = File.ReadAllText(JOYSTICK_CONFIG_DIRECTORY);

            // If debug is on then prints the file being loaded and its contents.
            if (DEBUG_ON)
                Debug.LogFormat("ReadFromFile({0})\ncontents:\n{1}", JOYSTICK_CONFIG_DIRECTORY, contents);

            // If it happens that the file is somehow empty then notify us.
            if (string.IsNullOrEmpty(contents) || contents.Length < 20)
            {
                Debug.LogErrorFormat("File is empty. Saved configuration not found!");
            }
            else
            {
                availableJoysticks = JsonHelper.getJsonArray<JoystickConfig>(contents);
                for (int i = 0; i < availableJoysticks.Length; i++)
                    availableJoysticks[i].InitiateGamepadState();
            }
        }
        else
        {
            Debug.LogWarning("File does not exist. Saved configuration not found!");
        }

    }

    public void WriteToFile()
    {
        if (availableJoysticks != null)
            for (int i = 0; i < availableJoysticks.Length; i++)
            {
                if (availableJoysticks[i] == null)
                    Debug.LogError("Joystick configuration data lost.");
                else
                    newUnknownJoysticks.Add(availableJoysticks[i]);
            }


        string json = JsonHelper.arrayToJson<JoystickConfig>(newUnknownJoysticks.ToArray());

        
        // Write that JSON string to the specified file.
        File.WriteAllText(JOYSTICK_CONFIG_DIRECTORY, json);

        //ResourceManager.Instance.SetInventory();

        // Tell us what we just wrote if DEBUG_ON is on.
        if (DEBUG_ON)
            Debug.LogFormat("WriteToFile({0}) -- data:\n{1}", JOYSTICK_CONFIG_DIRECTORY, json);
    }
    #endregion
}
