using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private float switchStartingAngle = -30.0f;
    private float switchOnAngle = 30.0f;
    private float switchOffAngle = -30.0f;
    private float angleDeadzone = 1.0f;
    public GameObject switchBase = null;
    public GameObject switchLever = null;
    public GameObject switchGuard = null;


    // Start is called before the first frame update
    void Start()
    {
        if(switchLever==null)
        {
            Debug.LogError("You didn't set up a switch correctly.");
            return;
        }

        switchLever.transform.localRotation = Quaternion.Euler(0,0, switchStartingAngle);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(1, 100) > 98)
            Debug.Log(switchLever.transform.localRotation.eulerAngles.z);
    }
}
