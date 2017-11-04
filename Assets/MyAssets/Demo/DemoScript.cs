using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public GameObject joystickMapPanel, interactableObject;

    private void Start()
    {
        Debug.Log(Application.platform.ToString());
        UniversalJoystickMapper.Instance.joystickMapPanel = joystickMapPanel;
    }

    private void Update()
    {
        UniversalJoystickMapper.Instance.Update();
        InteractWithObject();
    }
    
    void InteractWithObject()
    {
        if (UniversalJoystickMapper.GetButtonDown(Buttons.Cross))
        {
            interactableObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.Circle))
        {
            interactableObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.Triangle))
        {
            interactableObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.Square))
        {
            interactableObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.RShoulder))
        {
            interactableObject.transform.Rotate(90, 0, 0);
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.LShoulder))
        {
            interactableObject.transform.Rotate(-90, 0, 0);
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.RStick))
        {
            interactableObject.transform.localScale *= 1.2f;
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.LStick))
        {
            interactableObject.transform.localScale /= 1.2f;
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.Select))
        {
            Debug.Log("Select");
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.Start))
        {
            Debug.Log("START");
        }
        if (UniversalJoystickMapper.GetButtonDown(Buttons.RTrigger))
        {
            Debug.Log("RTRIGGER");
            interactableObject.transform.Rotate(0, 90, 0);
        }
        if (UniversalJoystickMapper.GetButtonUp(Buttons.LTrigger))
        {
            Debug.Log("LTRIGGER");
            interactableObject.transform.Rotate(0, -90, 0);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            UniversalJoystickMapper.CurrentJoystickBeingMapped = null;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UniversalJoystickMapper.Instance.WriteToFile();
        }
    }
    //void Examples()
    //{
    //    GamePad.GetButtonDown (GamePad.Button.A, GamePad.Index.One);
    //    GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);
    //    GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One);

    //    GamepadState state = GamePad.GetState(GamePad.Index.One);

    //    print("A: " + state.A);
    //}

    //void DrawState(JoystickMap.Index controller)
    //{
    //    GUILayout.Space(45);

    //    GUILayout.BeginVertical();


    //    JoystickMap.GamepadState state = UniversalJoystickMapper.joysticksAvailable[0].GetState(controller);

    //    // buttons
    //    GUILayout.Label("Gamepad " + controller);
    //    GUILayout.Label("" + state.Cross);
    //    GUILayout.Label("" + state.Circle);
    //    GUILayout.Label("" + state.Triangle);
    //    GUILayout.Label("" + state.Square);
    //    GUILayout.Label("" + state.Start);
    //    GUILayout.Label("" + state.Back);
    //    GUILayout.Label("" + state.LeftShoulder);
    //    GUILayout.Label("" + state.RightShoulder);
    //    GUILayout.Label("" + state.Left);
    //    GUILayout.Label("" + state.Right);
    //    GUILayout.Label("" + state.Up);
    //    GUILayout.Label("" + state.Down);
    //    GUILayout.Label("" + state.LeftStick);
    //    GUILayout.Label("" + state.RightStick);

    //    GUILayout.Label("");

    //    // triggers
    //    GUILayout.Label("" + System.Math.Round(state.LeftTrigger, 2));
    //    GUILayout.Label("" + System.Math.Round(state.RightTrigger, 2));

    //    GUILayout.Label("");

    //    // Axes
    //    GUILayout.Label("" + state.LeftStickAxis);
    //    GUILayout.Label("" + state.RightStickAxis);
    //    GUILayout.Label("" + state.dPadAxis);


    //    //GUILayout.EndArea();
    //    GUILayout.EndVertical();

    //}

    //void DrawLabels()
    //{
    //    //GUILayout.BeginArea(new Rect(30, 0, width - 30, Screen.height));

    //    GUILayout.BeginVertical();
    //    // buttons
    //    GUILayout.Label(" ", GUILayout.Width(80));
    //    GUILayout.Label("A");
    //    GUILayout.Label("B");
    //    GUILayout.Label("X");
    //    GUILayout.Label("Y");
    //    GUILayout.Label("Start");
    //    GUILayout.Label("Back");
    //    GUILayout.Label("Left Shoulder");
    //    GUILayout.Label("Right Shoulder");
    //    GUILayout.Label("Left");
    //    GUILayout.Label("Right");
    //    GUILayout.Label("Up");
    //    GUILayout.Label("Down");
    //    GUILayout.Label("LeftStick");
    //    GUILayout.Label("RightStick");

    //    GUILayout.Label("");

    //    GUILayout.Label("LeftTrigger");
    //    GUILayout.Label("RightTrigger");

    //    GUILayout.Label("");

    //    GUILayout.Label("LeftStickAxis");
    //    GUILayout.Label("rightStickAxis");
    //    GUILayout.Label("dPadAxis");

    //    GUILayout.EndVertical();

    //}
}
