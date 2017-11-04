using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public GameObject joystickMapPanel, interactableObject;

    private void Start()
    {
        Debug.Log(Application.platform.ToString());
        UniversalJoystickMapper.Instance.joystickConfigPanel = joystickMapPanel;
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            UniversalJoystickMapper.Instance.WriteToFile();
        }
    }
}
