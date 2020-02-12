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

    // Start is called before the first frame update
    void Start()
    {
        if(switchLever==null)
        {
            Debug.LogError("You didn't set up a switch correctly.");
            return;
        }
        switchStartPosition = switchLever.GetComponent<Rigidbody>().transform.position;
        guardStartPosition = switchGuard.GetComponent<Rigidbody>().transform.position;
        switchLever.transform.localRotation = Quaternion.Euler(0,0, switchStartingAngle);
        

    }


    private void FixedUpdate()
    {
        switchLever.GetComponent<Rigidbody>().transform.position = switchStartPosition;
        switchGuard.GetComponent<Rigidbody>().transform.position = guardStartPosition;
        switchLever.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z+((switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z - 180.0f) * -0.001f));
        //this actually works great for some reason........
        if (switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z > 45.0f)
            switchGuard.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z + ((switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z - 100.0f) * -0.002f));
        else
        {
            if (switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z < 26.0f)
                switchGuard.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z + ((switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z + 10.0f) * -0.02f));
            else
                switchGuard.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z + ((switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z - 10.0f) * -0.002f));
        }
        if (switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z > 180 && switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z < 360.0f + switchOffAngle)
        {
            switchLever.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0,0,360.0f + switchOffAngle);
            on = false;
            return;
        }
        if(switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z < 180 && switchLever.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z > switchOnAngle)
        {
            switchLever.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchOnAngle);
            on = true;
            return;
        }      
        /*
        if(switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z > 45.0f && switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z<120.0f)
        {
            switchGuard.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z + 2.0f);
        }
        else if (switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z<=45.0f && switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z > 0.0f)
        {
            switchGuard.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, switchGuard.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z - 2.0f);
        }*/


    }
}
