using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        //reactorCoreTempPoints = new List<float>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentUpdateTick++ % updateInterval != 0)
            return;
        currentUpdateTick = 1;
        if(ship==null)
        {
            Debug.LogError("Ship was null, you forgot to set it");
            return;
        }
        reactorCoreTemperatureGraph.tickGraph(ship.reactorCoreTemperature,0,32.0f, 0.00094607379f);//I want 1057 to max it out, so (32/1057)/32 = 0.00094607379 nak77
        reactorInnerLoopTemperatureGraph.tickGraph(ship.reactorInnerLoopTemp, 0, 32.0f, 0.00094607379f);//I want 1057 to max it out, so (32/1057)/32 = 0.00094607379 nak77
        reactorOuterLoopTemperatureGraph.tickGraph(ship.reactorOuterLoopTemp, 0, 32.0f, 0.002f);//I want 500 to max it out, so (32/500)/32 = 0.002 h2o
        reactorCoreInnerDeltaGraph.tickGraph(ship.reactorCoreInnerDelta, 16.0f, 32.0f, 0.001f);//I want 1000 to max it out, so (32/1000)/32 = 0.001
        reactorInnerOuterDeltaGraph.tickGraph(ship.reactorInnerOuterDelta, 16.0f, 32.0f, 0.001f);//I want 1000 to max it out, so (32/1000)/32 = 0.001
        reactorCorePressureGraph.tickGraph(ship.reactorCorePressure/1000.0f, 0.0f, 32.0f, 0.0001f);//I want 10000 to max it out, so (32/10000)/32 = 0.0001
    }


}
