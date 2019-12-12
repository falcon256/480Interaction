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
    public List<float> reactorCoreTempPoints = null;

    public int maxDataPoints = 256;

    public UILine reactorCoreTempGraph = null;
    public TMP_Text reactorCoreTempValue = null;

    // Start is called before the first frame update
    void Start()
    {
        reactorCoreTempPoints = new List<float>();
    }

    public void renderGraph(UILine line, TMP_Text text, List<float> points, float verticalOffset, float verticalScale)
    {
        float ratio = graphWidth / maxDataPoints;
        float offset = graphWidth / 2.0f;
        float lastPoint = 0;
        line.line.Clear();
        Gradient g = new Gradient();
        List<GradientColorKey> colors = new List<GradientColorKey>();
        for (int i = 0; i < points.Count; i++)
        {
            line.line.Push(new Point(new Vector3((i * ratio) - offset,verticalOffset + (points[i] * verticalScale * 16.0f )), Vector3.zero, Vector3.zero, 2.0f * graphScale));
            if (i % (maxDataPoints / 8) == 0)
                colors.Add(new GradientColorKey(getColorValue(points[i] * verticalScale), i / (float)maxDataPoints));
            lastPoint = points[i];
        }
        g.colorKeys = colors.ToArray();
        line.line.option.color = g;
        text.text = "-  " + lastPoint;
        text.transform.position = line.transform.position + new Vector3(points.Count * ratio, lastPoint * verticalScale * 16.0f, 0);
        text.color = getColorValue(points[points.Count - 1] * verticalScale);
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
        //data insertion
        reactorCoreTempPoints.Add(ship.reactorCoreTemperature);





        //data trimming
        while(reactorCoreTempPoints.Count>maxDataPoints)
        {
            reactorCoreTempPoints.RemoveAt(0);
        }

        //debug output


        
        //data display;
        if (reactorCoreTempGraph!=null)
        {
            renderGraph(reactorCoreTempGraph, reactorCoreTempValue, reactorCoreTempPoints, 0, 0.001f);
        }
        else
        {
            Debug.LogError("A Graph was null");
            return;
        }

        reactorCoreTempGraph.SetAllDirty();
    }


}
