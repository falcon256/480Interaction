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
        pv.RPC("handlePositionUpdate", RpcTarget.Others, (object)this.GetComponent<Rigidbody>().transform);
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
    public void handlePositionUpdate(Transform trans)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.transform.localPosition = trans.localPosition;
        rb.transform.localRotation = trans.localRotation;
        rb.transform.position = trans.position;
        rb.transform.rotation = trans.rotation;
    }

}
