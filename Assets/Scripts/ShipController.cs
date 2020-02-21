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

        reactorControlRodPosition = Mathf.Lerp(reactorControlRodPosition,reactorControlRodPositionInput.output*2,0.001f);
        reactorFlux = Mathf.Lerp(reactorFlux, (((reactorModeratorPosition * reactorControlRodPosition * reactorFuelLevel * 0.001f) + reactorDecayProducts + (reactorFlux*0.5f)) - reactorXenonBuildup),0.01f);
        reactorCoreTemperature += reactorFlux*0.01f;
        reactorCoreTemperature += Random.Range(-1.0f, 1.0f);
        //reactorCoreTemperature += Mathf.Sin(Time.realtimeSinceStartup*0.5f)*5.0f;
        reactorCoreTemperature *= 0.9999f;
        reactorCoreTemperature = Mathf.Clamp(reactorCoreTemperature, 0, 100000.0f);

        reactorCoreInnerDelta  = reactorCoreTemperature - reactorInnerLoopTemp;
        reactorInnerOuterDelta = reactorInnerLoopTemp - reactorOuterLoopTemp;

        reactorCoreTemperature -= reactorCoreInnerDelta  * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM+ reactorCoreLoopPump2RPM);
        reactorInnerLoopTemp   += reactorCoreInnerDelta  * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM);
        reactorInnerLoopTemp   -= reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM);
        reactorOuterLoopTemp   += reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM);

        reactorCorePressure = (reactorCorePressure * 0.99f) + (reactorCoreLoopPump1RPM * getNaKPressure(reactorCoreTemperature)) + (reactorCoreLoopPump2RPM * getNaKPressure(reactorCoreTemperature)) + reactorFlux;
        reactorInnerLoopPressure = (reactorInnerLoopPressure * 0.99f) + (reactorInnerLoopPump1RPM * getNaKPressure(reactorInnerLoopTemp)) + (reactorInnerLoopPump2RPM * getNaKPressure(reactorInnerLoopTemp));

    }
    public float getNaKPressure(float temp)
    {
        return Mathf.Pow(Mathf.Clamp(temp - 1057, 0, 100000.0f), 1.1f);
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