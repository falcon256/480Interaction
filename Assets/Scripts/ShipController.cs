using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
public class ShipController : MonoBehaviourPunCallbacks, IPunObservable
{

    //inputs
    public ButtonController reactorEmergencySCRAM = null;//Safety Control Rods Activation Mechanism
    public SliderController reactorControlRodPositionInput = null;
    public SliderController reactorModeratorRodPositionInput = null;
    public LeverController reactorCoreLoopPump1PositionInput = null;
    public LeverController reactorCoreLoopPump2PositionInput = null;
    public LeverController reactorInnerLoopPump1PositionInput = null;
    public LeverController reactorInnerLoopPump2PositionInput = null;
    public SwitchController reactorCoreLoopPump1OnOff = null;
    public SwitchController reactorCoreLoopPump2OnOff = null;
    public SwitchController reactorInnerLoopPump1OnOff = null;
    public SwitchController reactorInnerLoopPump2OnOff = null;
    public SwitchController reactorTurbine1OnOff = null;
    public SwitchController reactorTurbine2OnOff = null;
    public SwitchController reactorTurbine3OnOff = null;
    public SwitchController reactorTurbine4OnOff = null;
    public SliderController radiator1TargetExtensionPositionInput = null;
    public SliderController radiator2TargetExtensionPositionInput = null;



    //Public Information for the ship data
    [SerializeField] public float reactorCoreTemperature = 100.0f;
    [SerializeField] public float reactorInnerLoopTemp = 100.0f;
    [SerializeField] public float reactorOuterLoopTemp = 100.0f;
    [SerializeField] public float reactorCoreInnerDelta = 0.0f;
    [SerializeField] public float reactorInnerOuterDelta = 0.0f;

    //health values;
    public float reactorCoreHealth = 1.0f;
    public float reactorCoreLoopHealth = 1.0f;
    public float reactorCoreInnerHeatExchangerHealth = 1.0f;
    public float reactorInnerOuterHeatExchangerHealth = 1.0f;



    public float reactorCorePressure = 1.0f;
    public float reactorInnerLoopPressure = 1.0f;
    public float reactorOuterLoopPressure = 1.0f;

    public float reactorCoreInnerHeatExchangerEfficiency = 0.0001f;
    public float reactorInnerOuterHeatExchangerEfficiency = 0.0002f;

    public float reactorCoreLoopPump1RPM = 1.0f;
    public float reactorCoreLoopPump2RPM = 1.0f;

    public float reactorInnerLoopPump1RPM = 1.0f;
    public float reactorInnerLoopPump2RPM = 1.0f;

    public float reactorCoreLoopFlowLPS = 0.0f;
    public float reactorInnerLoopFlowLPS = 0.0f;
    public float reactorOuterLoopFlowLPS = 0.0f;


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

    public bool autoSCRAM = true;


    // Start is called before the first frame update
    void Start()
    {
        autoSCRAM = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        doReactorUpdate();
    
    }
    
    private void doReactorUpdate()
    {
        reactorControlRodPosition = Mathf.Lerp(reactorControlRodPosition, reactorControlRodPositionInput.output * 2, 0.001f);
        reactorModeratorPosition = Mathf.Lerp(reactorModeratorPosition, reactorModeratorRodPositionInput.output * 2, 0.001f);
        reactorFlux = Mathf.Lerp(reactorFlux, (((reactorModeratorPosition * reactorControlRodPosition * reactorFuelLevel * 0.001f * reactorCoreHealth) + reactorDecayProducts + (reactorFlux * 0.5f)) - reactorXenonBuildup), 0.01f);
        reactorCoreTemperature += reactorFlux * 0.01f;
        reactorCoreTemperature += Random.Range(-1.0f, 1.0f);
        reactorCoreTemperature *= 0.9999f;
        reactorCoreTemperature = Mathf.Clamp(reactorCoreTemperature, 0, 100000.0f);

        reactorCoreInnerDelta = reactorCoreTemperature - reactorInnerLoopTemp;
        reactorInnerOuterDelta = reactorInnerLoopTemp - reactorOuterLoopTemp;
        reactorCoreLoopFlowLPS += (((reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM) * 0.1f * reactorCoreLoopHealth) - reactorCoreLoopFlowLPS) * 0.01f;
        reactorInnerLoopFlowLPS += (((reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM) * 0.1f) - reactorInnerLoopFlowLPS) * 0.01f;
        reactorOuterLoopFlowLPS += (((reactorTurbine1RPM + reactorTurbine2RPM + reactorTurbine3RPM + reactorTurbine4RPM) * 0.1f)- reactorOuterLoopFlowLPS) * 0.01f;
        reactorCoreLoopFlowLPS *= 0.99f;
        reactorInnerLoopFlowLPS *= 0.99f;
        reactorOuterLoopFlowLPS *= 0.999f;
        reactorCoreTemperature -= (reactorCoreInnerDelta * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM) * reactorCoreLoopFlowLPS) * reactorCoreInnerHeatExchangerHealth;
        reactorInnerLoopTemp += (reactorCoreInnerDelta * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM) * reactorCoreLoopFlowLPS * reactorInnerLoopFlowLPS) * reactorCoreInnerHeatExchangerHealth;
        reactorInnerLoopTemp -= (reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM) * reactorCoreLoopFlowLPS * reactorInnerLoopFlowLPS) * reactorInnerOuterHeatExchangerHealth;
        reactorOuterLoopTemp += (reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM) * reactorInnerLoopFlowLPS * reactorOuterLoopFlowLPS) * reactorInnerOuterHeatExchangerHealth;

        //figure out our turnbine speeds. A couple of temp values for what is left in the pipe after the turbine is needed.

        reactorPowerProduction += reactorTurbine1RPM;
        reactorPowerProduction += reactorTurbine2RPM;
        reactorPowerProduction += reactorTurbine3RPM;
        reactorPowerProduction += reactorTurbine4RPM;
        reactorTurbine1RPM *= 0.99f;
        reactorTurbine2RPM *= 0.99f;
        reactorTurbine3RPM *= 0.99f;
        reactorTurbine4RPM *= 0.99f;

        if (reactorTurbine1OnOff)
        {
            reactorTurbine1RPM += reactorOuterLoopPressure * reactorOuterLoopFlowLPS;
            reactorTurbine1RPM *= 0.98f;
        }
        if (reactorTurbine2OnOff)
        {
            reactorTurbine2RPM += reactorOuterLoopPressure * reactorOuterLoopFlowLPS;
            reactorTurbine2RPM *= 0.98f;
        }
        if (reactorTurbine3OnOff)
        {
            reactorTurbine3RPM += reactorOuterLoopPressure * reactorOuterLoopFlowLPS;
            reactorTurbine3RPM *= 0.98f;
        }
        if (reactorTurbine4OnOff)
        {
            reactorTurbine4RPM += reactorOuterLoopPressure * reactorOuterLoopFlowLPS;
            reactorTurbine4RPM *= 0.98f;
        }





        reactorCorePressure = (reactorCorePressure * 0.99f) + (reactorCoreLoopPump1RPM * getNaKPressure(reactorCoreTemperature)) + (reactorCoreLoopPump2RPM * getNaKPressure(reactorCoreTemperature)) + reactorFlux;
        reactorInnerLoopPressure = (reactorInnerLoopPressure * 0.99f) + (reactorInnerLoopPump1RPM * getNaKPressure(reactorInnerLoopTemp)) + (reactorInnerLoopPump2RPM * getNaKPressure(reactorInnerLoopTemp));
        reactorOuterLoopPressure = (reactorOuterLoopPressure*0.9f)+((getH2OPressure(reactorOuterLoopTemp) - reactorOuterLoopFlowLPS)*0.01f);

        if (reactorEmergencySCRAM.isOn||autoSCRAM)
        {
            reactorControlRodPositionInput.doReset = true;
            reactorModeratorRodPositionInput.doReset = true;
            if (autoSCRAM)
                autoSCRAM = false;
        }





        //damage stuff.
        if(reactorCoreTemperature>3422.0f)//tungsten
        {
            reactorCoreHealth = Mathf.Max(0, reactorCoreHealth - ((reactorCoreTemperature - 3422.0f) * 0.01f));
        }
    }


    public float getNaKPressure(float temp)
    {
        return Mathf.Pow(Mathf.Clamp(temp - 1057, 0, 100000.0f), 1.1f);
    }
    public float getH2OPressure(float temp)
    {
        float ftemp = (temp * 9.0f) / 5.0f + 32.0f;//convert to f
        //steam pressure from F to PSIA
        float psia = (7.0f * Mathf.Pow(10.0f,-10.0f)) * Mathf.Pow(temp, 4.4374f);
        return psia * 6.89476f;//in kpa
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView");
        if (stream.IsWriting)
        {
            stream.SendNext(reactorCoreTemperature);
        }
        else
        {
            reactorCoreTemperature = (float)stream.ReceiveNext();
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView");
        if (stream.IsWriting)
        {
            stream.SendNext(reactorCoreTemperature);
        }
        else
        {
            reactorCoreTemperature = (float)stream.ReceiveNext();
        }
    }
}


//notes
//Coolant https://en.wikipedia.org/wiki/Sodium-potassium_alloy
//https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/19730004113.pdf // boils at 1057

//steam pressure 373.15 is boiling at 0kpa. 1000 = 12676.2 kpa, Pressure = energy.