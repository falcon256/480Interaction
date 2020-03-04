using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class SliderHandleController : MonoBehaviourPun
{
    public SliderController parentSlider = null;
    void FixedUpdate()
    {
        
    }

    public void updatePositionOnPeers()
    {
        PhotonView pv = this.photonView;
        pv.RPC("handlePositionUpdate", RpcTarget.Others, (object)this.GetComponent<Rigidbody>().transform.localPosition, (object)this.GetComponent<Rigidbody>().transform.localRotation);

    }

    private void OnCollisionEnter(Collision collision)
    {
        updatePositionOnPeers();
    }

    private void OnCollisionStay(Collision collision)
    {
        updatePositionOnPeers();
    }

    private void OnCollisionExit(Collision collision)
    {
        updatePositionOnPeers();
    }

    [PunRPC]
    public void handlePositionUpdate(Vector3 pos, Quaternion rot)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.transform.localPosition = pos;
        rb.transform.localRotation = rot;
    }

}
