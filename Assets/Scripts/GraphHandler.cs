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
    public List<float> controlPoints = null;

    void Start()
    {
        controlPoints = new List<float>();
    }
    public void tickGraph(float value, UILine line, TMP_Text text, List<float> points, float verticalOffset, float verticalScale, float multiplier)
    {
        float ratio = graphWidth / maxDataPoints;
        float offset = graphWidth / 2.0f;
        float lastPoint = 0;
        line.line.Clear();
        Gradient g = new Gradient();
        List<GradientColorKey> colors = new List<GradientColorKey>();
        controlPoints.Add(value);
        while (controlPoints.Count > maxDataPoints)
        {
            controlPoints.RemoveAt(0);
        }
        for (int i = 0; i < points.Count; i++)
        {
            line.line.Push(new Point(new Vector3((i * ratio) - offset, verticalOffset + (points[i] * verticalScale * multiplier)), Vector3.zero, Vector3.zero, 2.0f * graphScale));
            if (i % (maxDataPoints / 8) == 0)
                colors.Add(new GradientColorKey(getColorValue(points[i] * verticalScale), i / (float)maxDataPoints));
            lastPoint = points[i];
        }
        g.colorKeys = colors.ToArray();
        line.line.option.color = g;
        text.text = "-  " + lastPoint;
        text.transform.position = line.transform.position + new Vector3(points.Count * ratio, lastPoint * verticalScale * multiplier, 0);
        text.color = getColorValue(points[points.Count - 1] * verticalScale);
    }
    public Color getColorValue(float v)
    {
        if (v > 0.5f)
            return Color.Lerp(Color.green, Color.red, v - 0.5f);
        return Color.Lerp(Color.green, Color.blue, 0.5f - v);
    }
}
