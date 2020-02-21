using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SliderController : MonoBehaviour
{

    public GameObject handle = null;
    public float output = 0;
    public bool takeControlNextFrame = false;

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
        if (takeControlNextFrame)
            if (handle.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
            {
                handle.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                takeControlNextFrame = false;
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(handle.GetComponent<PhotonView>().Owner!=PhotonNetwork.LocalPlayer)
            handle.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
    }

}
