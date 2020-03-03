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
    }

    /* our other code makes this not needed.
    private void OnCollisionEnter(Collision collision)
    {
        if(handle.GetComponent<PhotonView>().Owner!=PhotonNetwork.LocalPlayer)
            handle.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
    }
    */
}
