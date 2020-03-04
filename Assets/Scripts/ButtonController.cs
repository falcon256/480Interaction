using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonController : MonoBehaviour
{
    public TextMeshPro buttonText;
    public GameObject buttonObject = null;
    public Color textOffColor = Color.black;
    public Color textOnColor = Color.white;
    public Vector3 buttonStartPosition = Vector3.zero;
    public bool isOn = true;
    public float buttonMaxDelta = 0.2f;
    private float buttonLastPosition = 0;
    void Start()
    {
        buttonStartPosition = buttonObject.GetComponent<Rigidbody>().transform.localPosition;
        buttonLastPosition = buttonStartPosition.y;
    }


    void FixedUpdate()
    {
        Vector3 buttonPos = buttonObject.GetComponent<Rigidbody>().transform.localPosition;

        float buttonDelta = buttonPos.y - buttonLastPosition;
        if (buttonDelta > buttonMaxDelta)
            buttonPos.y = buttonLastPosition + buttonMaxDelta;
        if (buttonDelta < -buttonMaxDelta)
            buttonPos.y = buttonLastPosition - buttonMaxDelta;
        if (buttonDelta < -0.1f)
            isOn = true;
        else
            isOn = false;
        buttonPos.x = 0;
        buttonPos.z = 0;
        buttonPos.y = Mathf.Lerp(buttonPos.y, buttonStartPosition.y, 0.2f);
        buttonPos.y = Mathf.Clamp(buttonPos.y, buttonStartPosition.y - 0.2f, buttonStartPosition.y + 0.01f);
        buttonObject.GetComponent<Rigidbody>().transform.localPosition = buttonPos;
        buttonLastPosition = buttonPos.y;
        

    }
}
