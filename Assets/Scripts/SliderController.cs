using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SliderController : MonoBehaviour
{

    public GameObject handle = null;
    public float output = 0;
    public TextMeshPro outputTMP = null;
    public bool doReset = false;
    public bool doDebugSend = false;
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
        output = (handlePos.z + 25.0f)*2.0f;
        handle.GetComponent<Rigidbody>().transform.localPosition = handlePos;
        outputTMP.text = string.Format("{0:00.0}", output);
        outputTMP.color = getColorValue(output);
        if(doReset)
        {
            if(handle.GetComponent<Rigidbody>().transform.localPosition.z<=-25.0f)
            {
                doReset = false;
            }
            else
            {
                handle.GetComponent<Rigidbody>().transform.localPosition = handle.GetComponent<Rigidbody>().transform.localPosition + new Vector3(0, 0, -0.1f);
                handle.GetComponent<SliderHandleController>().updatePositionOnPeers();
            }
        }
        if(doDebugSend)
        {
            doDebugSend = false;
            handle.GetComponent<SliderHandleController>().updatePositionOnPeers();
        }
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

    /* our other code makes this not needed.
    private void OnCollisionEnter(Collision collision)
    {
        if(handle.GetComponent<PhotonView>().Owner!=PhotonNetwork.LocalPlayer)
            handle.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
    }
    */
}
