using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    private Vector3 leverStartPosition;
    private float leverAngleLimit = 80.0f;
    public GameObject leverGO;
    public float value;


    // Start is called before the first frame update
    void Start()
    {
        if(!leverGO)
        {
            Debug.LogError("You didn't set up the lever correctly");
        }
        leverStartPosition = leverGO.GetComponent<Rigidbody>().transform.position;
    }


    private void FixedUpdate()
    {
        leverGO.GetComponent<Rigidbody>().transform.position = leverStartPosition;
        float currentAngle = leverGO.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z;
        if (currentAngle > 180.0f)
            currentAngle -= 360.0f;
        leverGO.GetComponent<Rigidbody>().transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(currentAngle, -leverAngleLimit, leverAngleLimit));
        currentAngle = leverGO.GetComponent<Rigidbody>().transform.localRotation.eulerAngles.z;
        if (currentAngle > 180.0f)
            currentAngle -= 360.0f;
        float dif = leverAngleLimit * 2.0f;
        float position = currentAngle + leverAngleLimit;
        value = position / dif;
        //Debug.Log(value);
    }
}
