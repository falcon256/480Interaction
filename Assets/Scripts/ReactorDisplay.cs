﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using geniikw.DataRenderer2D;
using TMPro;

public class ReactorDisplay : MonoBehaviour
{
    public float graphScale = 0.1f;
    public float graphWidth = 64.0f;
    public int updateInterval = 2;
    public int currentUpdateTick = 0;
    public ShipController ship = null;
    //public List<float> reactorCoreTempPoints = null;

    public int maxDataPoints = 256;

    public GraphHandler reactorCoreTemperatureGraph = null;
    public GraphHandler reactorInnerLoopTemperatureGraph = null;
    public GraphHandler reactorOuterLoopTemperatureGraph = null;
    public GraphHandler reactorCoreInnerDeltaGraph = null;
    public GraphHandler reactorInnerOuterDeltaGraph = null;
    public GraphHandler reactorCorePressureGraph = null;

    public GraphHandler reactorInnerLoopPressureGraph = null;
    public GraphHandler reactorOuterLoopPressureGraph = null;
    public GraphHandler reactorTurbine1RPMGraph = null;
    public GraphHandler reactorTurbine2RPMGraph = null;
    public GraphHandler reactorTurbine3RPMGraph = null;
    public GraphHandler reactorTurbine4RPMGraph = null;

    public GraphHandler reactorPowerDrawKWGraph = null;
    public GraphHandler reactorPowerProductionGraph = null;
    public GraphHandler reactorFuelLevelGraph = null;
    public GraphHandler reactorDecayProductsGraph = null;
    public GraphHandler reactorXenonBuildupGraph = null;
    public GraphHandler reactorFluxGraph = null;

    // Start is called before the first frame update
    void Start()
    {
        //reactorCoreTempPoints = new List<float>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        //if (currentUpdateTick++ % updateInterval != 0)
        //    return;
        //currentUpdateTick = 1;
        if (ship == null)
        {
            Debug.LogError("Ship was null, you forgot to set it");
            return;
        }
        switch (currentUpdateTick++)
        {
            case 0:
                reactorFluxGraph.tickGraph(ship.reactorFlux, 0, 32.0f, 0.001f);
                break;
            case 1:
                reactorCoreTemperatureGraph.tickGraph(ship.reactorCoreTemperature, 0, 32.0f, 0.00094607379f / 3.0f);//I want 1057 to max it out, so (32/1057)/32 = 0.00094607379 nak77
                break;
            case 2:
        reactorInnerLoopTemperatureGraph.tickGraph(ship.reactorInnerLoopTemp, 0, 32.0f, 0.00094607379f);//I want 1057 to max it out, so (32/1057)/32 = 0.00094607379 nak77
                break;
            case 3:
        reactorOuterLoopTemperatureGraph.tickGraph(ship.reactorOuterLoopTemp, 0, 32.0f, 0.002f);//I want 500 to max it out, so (32/500)/32 = 0.002 h2o
                break;
            case 4:
        reactorCoreInnerDeltaGraph.tickGraph(ship.reactorCoreInnerDelta, 16.0f, 32.0f, 0.001f);//I want 1000 to max it out, so (32/1000)/32 = 0.001
                break;
            case 5:              
        reactorInnerOuterDeltaGraph.tickGraph(ship.reactorInnerOuterDelta, 16.0f, 32.0f, 0.001f);//I want 1000 to max it out, so (32/1000)/32 = 0.001
                break;
            case 6:
        reactorCorePressureGraph.tickGraph(ship.reactorCorePressure / 1000.0f, 0.0f, 32.0f, 0.0001f);//I want 10000 to max it out, so (32/10000)/32 = 0.0001
                break;
            case 7:
        reactorInnerLoopPressureGraph.tickGraph(ship.reactorInnerLoopPressure, 0, 32.0f, 0.001f);
                break;
            case 8:
        reactorOuterLoopPressureGraph.tickGraph(ship.reactorOuterLoopPressure, 0, 32.0f, 0.001f);
                break;
            case 9:
        reactorTurbine1RPMGraph.tickGraph(ship.reactorTurbine1RPM, 0, 32.0f, 0.001f);
                break;
            case 10:
        reactorTurbine2RPMGraph.tickGraph(ship.reactorTurbine2RPM, 0, 32.0f, 0.001f);
                break;
            case 11:
        reactorTurbine3RPMGraph.tickGraph(ship.reactorTurbine3RPM, 0, 32.0f, 0.001f);
                break;
            case 12:
        reactorTurbine4RPMGraph.tickGraph(ship.reactorTurbine4RPM, 0, 32.0f, 0.001f);
                break;
            case 13:
        reactorPowerDrawKWGraph.tickGraph(ship.reactorPowerDrawKW, 0, 32.0f, 0.002f);
                break;
            case 14:
        reactorPowerProductionGraph.tickGraph(ship.reactorPowerProduction, 0, 32.0f, 0.002f);
                break;
            case 15:
        reactorFuelLevelGraph.tickGraph(100.0f - ship.reactorFuelLevel, 0, 32.0f, 0.01f);
                break;
            case 16:
        reactorDecayProductsGraph.tickGraph(ship.reactorDecayProducts, 0, 32.0f, 0.005f);
                break;
            case 17:
        reactorXenonBuildupGraph.tickGraph(ship.reactorXenonBuildup, 0, 32.0f, 0.005f);
                break;
            case 18:
                currentUpdateTick=0;
                break;
        }
    }


}
