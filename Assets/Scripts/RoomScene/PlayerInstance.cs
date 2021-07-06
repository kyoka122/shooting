using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photonmanager;

namespace RoomScene
{
    public class PlayerInstance : MonoBehaviour
    {
        private ResourceList resourceList;
        public void InstancePlayer()
        {
            resourceList = new ResourceList();
            PhotonNetwork.Instantiate(resourceList.unitychan, transform.position, transform.rotation);
            //–¼‘Oƒ^ƒO

        }
    }
}