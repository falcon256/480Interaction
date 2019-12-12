using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{


    //Public Information for the ship data
    public float reactorCoreTemperature = 100.0f;
    public float reactorInnerLoopTemp = 100.0f;
    public float reactorOuterLoopTemp = 100.0f;
    public float reactorCoreInnerDelta = 0.0f;
    public float reactorInnerOuterDelta = 0.0f;
    public float reactorCorePressure = 1.0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        reactorCoreTemperature += Random.Range(-10.1f, 10.1f);
        reactorCoreTemperature += Mathf.Sin(Time.realtimeSinceStartup*0.5f)*5.0f;
        reactorCoreTemperature *= 0.9999f;
        reactorCoreTemperature = Mathf.Clamp(reactorCoreTemperature, 0, 100000.0f);
       
    }
    
}


//notes
//Coolant https://en.wikipedia.org/wiki/Sodium-potassium_alloy
//https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/19730004113.pdf // boils at 1057