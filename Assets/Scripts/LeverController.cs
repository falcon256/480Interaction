using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LeverController : MonoBehaviour
{
    private Vector3 leverStartPosition;
    private float leverAngleLimit = 80.0f;
    public GameObject leverGO;
    public float value;
    public float maxDelta = 10.0f;
    public float previousAngle = 0;
    public TextMeshPro outputTMP = null;
    public Light pointlight = null;
    // Start is called before the first frame update
    void Start()
    {
        if(!leverGO)
        {
            Debug.LogError("You didn't set up the lever correctly");
        }
        leverStartPosition = leverGO.GetComponent<Rigidbody>().transform.position;
        previousAngle = leverGO.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z;
    }


    private void Update()
    {
        leverGO.GetComponent<Rigidbody>().transform.position = leverStartPosition;
    }

    private void FixedUpdate()
    {
        leverGO.GetComponent<Rigidbody>().transform.position = leverStartPosition;
        float currentAngle = leverGO.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z;

        if (currentAngle > 180.0f)
            currentAngle -= 360.0f;
        float delta = currentAngle - previousAngle;
        if (delta > maxDelta)
            currentAngle = previousAngle + maxDelta;
        if (delta < -maxDelta)
            currentAngle = previousAngle - maxDelta;
        leverGO.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(currentAngle, -leverAngleLimit, leverAngleLimit));
        currentAngle = leverGO.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z;
        if (currentAngle > 180.0f)
            currentAngle -= 360.0f;
        previousAngle = currentAngle;
        float dif = leverAngleLimit * 2.0f;
        float position = currentAngle + leverAngleLimit;
        value = position / dif;

        outputTMP.text = string.Format("{0:00.0}", value * 100.0f);
        outputTMP.color = getColorValue(value * 100.0f);
        pointlight.color = outputTMP.color;
        //Debug.Log(value);
    }
    public Color getColorValue(float v)
    {
        //Debug.Log(v);
        //v = Mathf.Clamp(v, -0.5f, 0.5f);
        v *= 0.01f;
        if (v > 0.5f)
            return Color.Lerp(Color.green, Color.red, v - 0.5f);
        return Color.Lerp(Color.green, Color.blue, 0.5f - v);
    }
}
