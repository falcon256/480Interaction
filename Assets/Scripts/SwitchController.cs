using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private float switchStartingAngle = -30.0f;
    private float switchOnAngle = 30.0f;
    private float switchOffAngle = -30.0f;
    private float angleDeadzone = 1.0f;
    private Vector3 switchStartPosition;
    private Vector3 guardStartPosition;
    public GameObject switchBase = null;
    public GameObject switchLever = null;
    public GameObject switchGuard = null;
    public bool on = false;
    public Light pointlight = null;

    // Start is called before the first frame update
    void Start()
    {
        if(switchLever==null)
        {
            Debug.LogError("You didn't set up a switch correctly.");
            return;
        }
        switchStartPosition = switchLever.GetComponent<Rigidbody>().transform.position;
        if(switchGuard)
            guardStartPosition = switchGuard.GetComponent<Rigidbody>().transform.position;
        switchLever.transform.localRotation = Quaternion.Euler(0,0, switchStartingAngle);
        

    }

    private void Update()
    {
        if (switchGuard)
        { 
        float currentGuardAngle = switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z;
        if (currentGuardAngle > 180.0f)
            currentGuardAngle -= 360.0f;

            currentGuardAngle = Mathf.Clamp(currentGuardAngle, 10, 90);

            switchGuard.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, currentGuardAngle);
            switchGuard.GetComponent<Rigidbody>().transform.position = guardStartPosition;
        }
        switchLever.GetComponent<Rigidbody>().transform.position = switchStartPosition;
           
    }

    private void FixedUpdate()
    {
        if (switchGuard)
        {
            float currentGuardAngle = switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z;
            if (currentGuardAngle > 180.0f)
                currentGuardAngle -= 360.0f;

            currentGuardAngle = Mathf.Clamp(currentGuardAngle, 0, 120);
            switchGuard.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, currentGuardAngle);
            switchGuard.GetComponent<Rigidbody>().transform.position = guardStartPosition;

            switchLever.GetComponent<Rigidbody>().transform.position = switchStartPosition;
        }
            
        switchLever.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z + ((switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z - 180.0f) * -0.001f));
        if (switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z > 180 && switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z < 360.0f + switchOffAngle)
        {
            switchLever.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, 360.0f + switchOffAngle);
            on = false;
            pointlight.color = Color.red;
            return;
        }
        if (switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z < 180 && switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z > switchOnAngle)
        {
            switchLever.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchOnAngle);
            on = true;
            pointlight.color = Color.green;
            return;
        }
        

    }
}
