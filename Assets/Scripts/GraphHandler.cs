using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using geniikw.DataRenderer2D;
using TMPro;

public class GraphHandler : MonoBehaviour
{
    public float graphScale = 0.1f;
    public float graphWidth = 64.0f;
    public int maxDataPoints = 256;
    public UILine outputGraph = null; 
    public TMP_Text textValue = null;
    public TMP_Text sideLabelText = null;
    public TMP_Text largeTextValue = null;
    public string componentString = "";
    public string unitString = "";


    private List<float> controlPoints = null;


    void Start()
    {
        controlPoints = new List<float>();
    }

    public void tickGraph(float value, float verticalOffset, float verticalScale, float multiplier)
    {
        float ratio = graphWidth / maxDataPoints;
        float offset = graphWidth / 2.0f;
        float lastPoint = 0;
        outputGraph.line.Clear();
        Gradient g = new Gradient();
        List<GradientColorKey> colors = new List<GradientColorKey>();
        controlPoints.Add(value);
        while (controlPoints.Count > maxDataPoints)
        {
            controlPoints.RemoveAt(0);
        }
        for (int i = 0; i < controlPoints.Count; i++)
        {
            outputGraph.line.Push(new Point(new Vector3((i * ratio) - offset, verticalOffset + Mathf.Clamp((controlPoints[i] * verticalScale * multiplier),0.5f,31.5f)), Vector3.zero, Vector3.zero, 1.0f * graphScale));
            if (i % (maxDataPoints / 8) == 0)
                colors.Add(new GradientColorKey(getColorValue((controlPoints[i] * verticalScale * multiplier) / 16.0f), i / (float)maxDataPoints));
            lastPoint = controlPoints[i];
        }
        g.colorKeys = colors.ToArray();
        outputGraph.line.option.color = g;
        textValue.text = "-  " + string.Format("{0:0.00}", lastPoint);
        textValue.transform.position = outputGraph.transform.position + new Vector3(controlPoints.Count * ratio, Mathf.Clamp((controlPoints[controlPoints.Count - 1] * verticalScale * multiplier), 0.5f, 31.5f), 0);
        sideLabelText.color = largeTextValue.color = textValue.color = getColorValue((controlPoints[controlPoints.Count - 1] * verticalScale * multiplier) / 16.0f);
        largeTextValue.text = componentString + ": " + string.Format("{0:0.00}", lastPoint) + " " + unitString;
    }
    public Color getColorValue(float v)
    {
        if (v > 0.5f)
            return Color.Lerp(Color.green, Color.red, v - 0.5f);
        return Color.Lerp(Color.green, Color.blue, 0.5f - v);
    }
}
