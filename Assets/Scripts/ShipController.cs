using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
public class ShipController : MonoBehaviourPun
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

    public float reactorCoreInnerHeatExchangerEfficiency = 0.0005f;
    public float reactorInnerOuterHeatExchangerEfficiency = 0.0005f;

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

    float lastNetworkUpdate = 0;
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

        if(Time.realtimeSinceStartup > lastNetworkUpdate + 1)
        {
            lastNetworkUpdate = Time.realtimeSinceStartup;
            if(PhotonNetwork.IsMasterClient)
                updateDataOnPeers();
        }
    }
    
    private void doReactorUpdate()
    {
        reactorControlRodPosition = Mathf.Lerp(reactorControlRodPosition, reactorControlRodPositionInput.output * 2, 0.001f);
        reactorModeratorPosition = Mathf.Lerp(reactorModeratorPosition, reactorModeratorRodPositionInput.output * 2, 0.001f);
        reactorFlux = Mathf.Lerp(reactorFlux, (((reactorModeratorPosition * reactorControlRodPosition * reactorFuelLevel * 0.0001f * reactorCoreHealth) + reactorDecayProducts + (reactorFlux * 0.5f)) - reactorXenonBuildup), 0.01f);
        reactorCoreTemperature += reactorFlux * 0.01f;
        reactorCoreTemperature += Random.Range(-1.0f, 1.0f);
        reactorCoreTemperature *= 0.9999f;
        reactorCoreTemperature = Mathf.Clamp(reactorCoreTemperature, 0, 100000.0f);

        reactorCoreLoopPump1RPM = Mathf.Lerp(reactorCoreLoopPump1RPM, reactorCoreLoopPump1PositionInput.value, 0.01f);
        reactorCoreLoopPump2RPM = Mathf.Lerp(reactorCoreLoopPump2RPM, reactorCoreLoopPump2PositionInput.value, 0.01f);
        reactorInnerLoopPump1RPM = Mathf.Lerp(reactorInnerLoopPump1RPM, reactorInnerLoopPump1PositionInput.value, 0.01f);
        reactorInnerLoopPump2RPM = Mathf.Lerp(reactorInnerLoopPump2RPM, reactorInnerLoopPump2PositionInput.value, 0.01f);

        reactorCoreInnerDelta = reactorCoreTemperature - reactorInnerLoopTemp;
        reactorInnerOuterDelta = reactorInnerLoopTemp - reactorOuterLoopTemp;
        reactorCoreLoopFlowLPS += (((reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM) * 0.1f * reactorCoreLoopHealth));
        reactorInnerLoopFlowLPS += (((reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM) * 0.1f));
        reactorOuterLoopFlowLPS += (((reactorTurbine1RPM + reactorTurbine2RPM + reactorTurbine3RPM + reactorTurbine4RPM) * 0.01f));
        reactorCoreLoopFlowLPS *= 0.90f;//keep these highly limited or they go nuts.
        reactorInnerLoopFlowLPS *= 0.90f;
        reactorOuterLoopFlowLPS *= 0.90f;
        reactorCoreTemperature -= (reactorCoreInnerDelta * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM) * (reactorCoreLoopFlowLPS + reactorInnerLoopFlowLPS)) * reactorCoreInnerHeatExchangerHealth * 0.05f;
        reactorInnerLoopTemp +=   (reactorCoreInnerDelta * reactorCoreInnerHeatExchangerEfficiency * (reactorCoreLoopPump1RPM + reactorCoreLoopPump2RPM) * (reactorCoreLoopFlowLPS + reactorInnerLoopFlowLPS)) * reactorCoreInnerHeatExchangerHealth * 0.05f;
        reactorInnerLoopTemp -= (reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM) * (reactorInnerLoopFlowLPS + reactorOuterLoopFlowLPS)) * reactorInnerOuterHeatExchangerHealth * 0.05f;
        reactorOuterLoopTemp += (reactorInnerOuterDelta * reactorInnerOuterHeatExchangerEfficiency * (reactorInnerLoopPump1RPM + reactorInnerLoopPump2RPM) * (reactorInnerLoopFlowLPS + reactorOuterLoopFlowLPS)) * reactorInnerOuterHeatExchangerHealth * 0.05f;


        //power production drops quickly over time
        reactorPowerProduction *= 0.99f;
        //figure out our turnbine speeds.
        reactorPowerProduction += Mathf.Pow(reactorTurbine1RPM, 1.1f);
        reactorPowerProduction += Mathf.Pow(reactorTurbine2RPM, 1.1f);
        reactorPowerProduction += Mathf.Pow(reactorTurbine3RPM, 1.1f);
        reactorPowerProduction += Mathf.Pow(reactorTurbine4RPM, 1.1f);
        reactorTurbine1RPM *= 0.99f;
        reactorTurbine2RPM *= 0.99f;
        reactorTurbine3RPM *= 0.99f;
        reactorTurbine4RPM *= 0.99f;

        float radiatorTempDelta1 = reactorOuterLoopTemp - radiator1Temperature;
        float radiatorTempDelta2 = reactorOuterLoopTemp - radiator2Temperature;

        //handle turbine power production.
        if (reactorTurbine1OnOff && reactorTurbine1OnOff.on)
        {
            reactorTurbine1RPM = Mathf.Lerp(reactorTurbine1RPM, reactorOuterLoopPressure, 0.001f);
            //reactorTurbine1RPM += reactorOuterLoopPressure * 0.0001f;
            //reactorTurbine1RPM *= 0.90f;
            reactorOuterLoopTemp *= 0.999f;
            reactorOuterLoopPressure *= 0.999f;
            //reactorOuterLoopTemp -= Mathf.Min(reactorOuterLoopTemp, reactorTurbine1RPM * 0.001f);
        }
        if (reactorTurbine2OnOff && reactorTurbine2OnOff.on)
        {
            reactorTurbine2RPM = Mathf.Lerp(reactorTurbine2RPM, reactorOuterLoopPressure, 0.001f);
            //reactorTurbine2RPM += reactorOuterLoopPressure * 0.0001f;
            //reactorTurbine2RPM *= 0.90f;
            reactorOuterLoopTemp *= 0.999f;
            reactorOuterLoopPressure *= 0.999f;
            //reactorOuterLoopTemp -= Mathf.Min(reactorOuterLoopTemp, reactorTurbine2RPM * 0.001f);
        }
        if (reactorTurbine3OnOff && reactorTurbine3OnOff.on)
        {
            reactorTurbine3RPM = Mathf.Lerp(reactorTurbine3RPM, reactorOuterLoopPressure, 0.001f);
            //reactorTurbine3RPM += reactorOuterLoopPressure * 0.0001f;
            //reactorTurbine3RPM *= 0.90f;
            reactorOuterLoopTemp *= 0.999f;
            reactorOuterLoopPressure *= 0.999f;
            //reactorOuterLoopTemp -= Mathf.Min(reactorOuterLoopTemp, reactorTurbine3RPM * 0.001f);
        }
        if (reactorTurbine4OnOff && reactorTurbine4OnOff.on)
        {
            reactorTurbine4RPM = Mathf.Lerp(reactorTurbine4RPM, reactorOuterLoopPressure, 0.001f);
            //reactorTurbine4RPM += reactorOuterLoopPressure * 0.0001f;
            //reactorTurbine4RPM *= 0.90f;
            reactorOuterLoopTemp *= 0.999f;
            reactorOuterLoopPressure *= 0.999f;
            //reactorOuterLoopTemp -= Mathf.Min(reactorOuterLoopTemp,reactorTurbine4RPM * 0.001f);
        }

        if (float.IsNaN(reactorInnerOuterDelta))
            Debug.LogError("reactorInnerOuterDelta nan");
        if (float.IsNaN(reactorInnerLoopPump1RPM))
            Debug.LogError("reactorInnerLoopPump1RPM nan");
        if (float.IsNaN(reactorInnerLoopPump2RPM))
            Debug.LogError("reactorInnerLoopPump2RPM nan");
        if (float.IsNaN(reactorInnerLoopFlowLPS))
            Debug.LogError("reactorInnerLoopFlowLPS nan");
        if (float.IsNaN(reactorOuterLoopFlowLPS))
            Debug.LogError("reactorOuterLoopFlowLPS nan");

        if (float.IsNaN(reactorCoreTemperature))
            Debug.LogError("reactorCoreTemperature nan");
        if (float.IsNaN(reactorInnerLoopTemp))
            Debug.LogError("reactorInnerLoopTemp nan");
        if (float.IsNaN(reactorOuterLoopTemp))
            Debug.LogError("reactorOuterLoopTemp nan");

        //calculate pressures by what is inside the loops
        //reactorCorePressure = (reactorCorePressure * 0.99f) + (reactorCoreLoopPump1RPM * getNaKPressure(reactorCoreTemperature)) + (reactorCoreLoopPump2RPM * getNaKPressure(reactorCoreTemperature)) + reactorFlux + (reactorOuterLoopTemp + reactorInnerLoopTemp) * 0.001f;
        //reactorInnerLoopPressure = (reactorInnerLoopPressure * 0.99f) + (reactorInnerLoopPump1RPM * getNaKPressure(reactorInnerLoopTemp)) + (reactorInnerLoopPump2RPM * getNaKPressure(reactorInnerLoopTemp))+ (reactorCoreTemperature+reactorOuterLoopTemp)*0.001f;
        //reactorOuterLoopPressure = (reactorOuterLoopPressure * 0.9f) + ((getH2OPressure(reactorOuterLoopTemp)) * 0.01f) + (reactorCoreTemperature + reactorInnerLoopTemp) * 0.001f;//this throws nans. NANS BAD!
        reactorCorePressure = Mathf.Lerp(reactorCorePressure, getNaKPressure(reactorCoreTemperature) + (reactorInnerLoopTemp + reactorOuterLoopTemp) * 0.001f, 0.1f);
        reactorInnerLoopPressure = Mathf.Lerp(reactorInnerLoopPressure, getNaKPressure(reactorInnerLoopTemp) + (reactorCoreTemperature + reactorOuterLoopTemp) * 0.001f, 0.01f);
        reactorOuterLoopPressure = Mathf.Lerp(reactorOuterLoopPressure, getH2OPressure(reactorOuterLoopTemp) + (reactorCoreTemperature + reactorInnerLoopTemp) * 0.001f, 0.01f);
        
        if (float.IsNaN(reactorCorePressure))
            Debug.LogError("reactorCorePressure nan");
        if (float.IsNaN(reactorInnerLoopPressure))
            Debug.LogError("reactorInnerLoopPressure nan");
        if (float.IsNaN(reactorOuterLoopPressure))
        {
            Debug.LogError("reactorOuterLoopPressure nan");
            Debug.LogError(this.ToString());
            reactorOuterLoopPressure = 0;
        }
        if (reactorEmergencySCRAM.isOn||autoSCRAM)
        {
            reactorControlRodPositionInput.doReset = true;
            reactorModeratorRodPositionInput.doReset = true;
            if (autoSCRAM)
                autoSCRAM = false;
        }


        //flow restriction from radiator extension
        reactorOuterLoopFlowLPS -= reactorOuterLoopFlowLPS * radiator1Extension * 0.01f;
        reactorOuterLoopFlowLPS -= reactorOuterLoopFlowLPS * radiator2Extension * 0.01f;

        //radiators act as a large mass heat sink, so these units are different.
        radiator1Temperature += radiatorTempDelta1 * 0.001f;
        reactorOuterLoopTemp -= radiatorTempDelta1 * 0.001f;

        radiator2Temperature += radiatorTempDelta2 * 0.001f;
        reactorOuterLoopTemp -= radiatorTempDelta2 * 0.001f;


        //autoscram triggers
        if (reactorCoreTemperature > 3300)
            autoSCRAM = true;
        if (reactorCoreInnerDelta > 2500)
            autoSCRAM = true;
        if (reactorInnerOuterDelta > 500)
            autoSCRAM = true;
        if (reactorInnerLoopTemp > 1057)
            autoSCRAM = true;
        if (reactorOuterLoopTemp > 700)
            autoSCRAM = true;
        if (reactorInnerLoopPressure > 1000)
            autoSCRAM = true;
        if (reactorOuterLoopPressure > 1000)
            autoSCRAM = true;

        //damage stuff.
        //turning off damage stuff for test
        /*
        if (reactorCoreTemperature>3422.0f)//tungsten
        {
            reactorCoreHealth -= Mathf.Max(0, ((reactorCoreTemperature - 3422.0f) * 0.0001f));
            if (reactorCoreHealth < 0)
                reactorCoreHealth = 0;
        }
        */
    }


    public void updateDataOnPeers()
    {
        PhotonView pv = this.photonView;
        pv.RPC("UpdateImportantStats", RpcTarget.Others, reactorCoreTemperature, reactorInnerLoopTemp, reactorOuterLoopTemp, reactorFlux);

    }

    [PunRPC]
    public void UpdateImportantStats(float a, float b, float c, float d)
    {
        reactorCoreTemperature = a;
        reactorInnerLoopTemp = b;
        reactorOuterLoopTemp = c;
        reactorFlux = d;
    }

    public float getNaKPressure(float temp)
    {
        return Mathf.Pow(Mathf.Clamp(temp - 1057, 0, 100000.0f), 1.1f);
    }
    public float getH2OPressure(float temp)
    {
        if (temp < 273.15f)
            return 0;
        float ftemp = ((temp-273.15f) * 9.0f) / 5.0f + 32.0f;//convert to f

        //steam pressure from F to PSIA
        float psia = (7.0f * Mathf.Pow(10.0f,-10.0f)) * Mathf.Pow(ftemp, 4.4374f);
        return psia * 6.89476f;//in kpa
    }
    public string ToString()
    {
        string s = "";
        s += reactorCoreHealth + "\n";
        s += reactorCoreLoopHealth + "\n";
        s += reactorCoreInnerHeatExchangerHealth + "\n";
        s += reactorInnerOuterHeatExchangerHealth + "\n";
        s += reactorCorePressure + "\n";
        s += reactorInnerLoopPressure + "\n";
        s += reactorOuterLoopPressure + "\n";
        s += reactorCoreInnerHeatExchangerEfficiency + "\n";
        s += reactorInnerOuterHeatExchangerEfficiency + "\n";
        s += reactorCoreLoopPump1RPM + "\n";
        s += reactorCoreLoopPump2RPM + "\n";
        s += reactorInnerLoopPump1RPM + "\n";
        s += reactorInnerLoopPump2RPM + "\n";
        s += reactorCoreLoopFlowLPS + "\n";
        s += reactorInnerLoopFlowLPS + "\n";
        s += reactorOuterLoopFlowLPS + "\n";
        s += reactorTurbine1RPM + "\n";
        s += reactorTurbine2RPM + "\n";
        s += reactorTurbine3RPM + "\n";
        s += reactorTurbine4RPM + "\n";
        s += reactorTurbine1Output + "\n";
        s += reactorTurbine2Output + "\n";
        s += reactorTurbine3Output + "\n";
        s += reactorTurbine4Output + "\n";
        s += reactorPowerDrawKW + "\n";
        s += reactorPowerProduction + "\n";
        s += reactorFlux + "\n";
        s += reactorModeratorPosition + "\n";
        s += reactorControlRodPosition + "\n";
        s += reactorFuelLevel + "\n";
        s += reactorDecayProducts + "\n";
        s += reactorXenonBuildup + "\n";
        s += reactorCapacitorVoltage + "reactorCapacitorVoltage\n";
        s += radiator1Extension + "radiator1Extension\n";
        s += radiator2Extension + "radiator2Extension\n";
        s += radiator1Temperature + "radiator1Temperature\n";
        s += radiator2Temperature + "radiator2Temperature\n";

        return s;
    }


}


//notes
//Coolant https://en.wikipedia.org/wiki/Sodium-potassium_alloy
//https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/19730004113.pdf // boils at 1057

//steam pressure 373.15 is boiling at 0kpa. 1000 = 12676.2 kpa, Pressure = energy.