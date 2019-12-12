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
    public float reactorInnerLoopPressure = 1.0f;
    public float reactorOuterLoopPressure = 1.0f;

    public float reactorCoreInnerHeatExchangerEfficiency = 0.0001f;
    public float reactorInnerOuterHeatExchangerEfficiency = 0.0002f;

    public float reactorCoreLoopPump1RPM = 1.0f;
    public float reactorCoreLoopPump2RPM = 1.0f;

    public float reactorInnerLoopPump1RPM = 1.0f;
    public float reactorInnerLoopPump2RPM = 1.0f;

    public float reactorTurbine1RPM = 1.0f;
    public float reactorTurbine2RPM = 1.0f;
    public float reactorTurbine3RPM = 1.0f;
    public float reactorTurbine4RPM = 1.0f;

    public float reactorTurbine1Output = 1.0f;
    public float reactorTurbine2Output = 1.0f;
    public float reactorTurbine3Output = 1.0f;
    public float reactorTurbine4Output = 1.0f;

    public float reactorPowerDrawKW = 1.0f;
    public float reactorPowerProduction = 1.0f;
    public float reactorFlux = 1.0f;
    public float reactorModeratorPosition = 1.0f;
    public float reactorControlRodPosition = 1.0f;

    public float reactorFuelLevel = 100.0f;
    public float reactorDecayProducts = 0.0f;
    public float reactorXenonBuildup = 0.0f;

    public float reactorCapacitorVoltage = 1.0f;

    public float radiator1Extension = 1.0f;
    public float radiator2Extension = 1.0f;
    public float radiator1Temperature = 1.0f;
    public float radiator2Temperature = 1.0f;


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

        reactorCoreInnerDelta  = reactorCoreTemperature - reactorInnerLoopTemp;
        reactorInnerOuterDelta = reactorInnerLoopTemp - reactorOuterLoopTemp;

        reactorCoreTemperature -= reactorCoreInnerDelta  * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM+ reactorCoreLoopPump2RPM);
        reactorInnerLoopTemp   += reactorCoreInnerDelta  * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM);
        reactorInnerLoopTemp   -= reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM);
        reactorOuterLoopTemp   += reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM);

        reactorCorePressure = (reactorCorePressure * 0.99f) + (reactorCoreLoopPump1RPM * reactorCoreTemperature) + (reactorCoreLoopPump2RPM * reactorCoreTemperature) + reactorFlux;
    }
    
}


//notes
//Coolant https://en.wikipedia.org/wiki/Sodium-potassium_alloy
//https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/19730004113.pdf // boils at 1057

//steam pressure 373.15 is boiling at 0kpa. 1000 = 12676.2 kpa, Pressure = energy.