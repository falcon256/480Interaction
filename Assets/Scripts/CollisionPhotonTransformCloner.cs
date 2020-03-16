using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class CollisionPhotonTransformCloner : MonoBehaviourPun
{
    public int ticksUntilUpdate = 100;
    void FixedUpdate()
    {
        ticksUntilUpdate--;
        if (ticksUntilUpdate <= 0)
        {
            ticksUntilUpdate = Physics.defaultSolverIterations * 10;
            if (PhotonNetwork.IsMasterClient)
                updatePositionOnPeers();
        }
    }

    public void updatePositionOnPeers()
    {
        PhotonView pv = this.photonView;
        pv.RPC("transformPositionUpdate", RpcTarget.Others, (object)this.GetComponent<Rigidbody>().transform.localPosition, (object)this.GetComponent<Rigidbody>().transform.localRotation);

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
    public void transformPositionUpdate(Vector3 pos, Quaternion rot)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.transform.localPosition = pos;
        rb.transform.localRotation = rot;
    }

}
