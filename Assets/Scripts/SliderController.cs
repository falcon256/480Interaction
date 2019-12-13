using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour
{

    public GameObject handle = null;
    public float output = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {

        Vector3 handlePos = handle.GetComponent<Rigidbody>().transform.localPosition;
        handlePos.x = 0;
        handlePos.y = 0;
        handlePos.z = Mathf.Clamp(handlePos.z, -25.0f, 25.0f);
        output = handlePos.z + 25.0f;
        handle.GetComponent<Rigidbody>().transform.localPosition = handlePos;
    }
}
