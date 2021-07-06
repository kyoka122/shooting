using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photonmanager;

namespace GameScene
{
    public class TargetManager : MonoBehaviour
    {
        private string _target;
        List<GameObject> _targetList = new List<GameObject>();
        private ResourceList _resourceList=new ResourceList();

        [PunRPC]
        public void TargetInstance()
        {
            float parts = Random.value;
            if (parts<0.8)
            {
                _target = _resourceList.piece;
            }
            else
            {
                _target = _resourceList.blossom;
            }

            float[] random=new float[6];
            for (int i=0; i<3;i++)
            {
                random[i] = Random.Range(0f,50f);
            }
            for (int i = 3; i < 6; i++)
            {
                random[i] = Random.Range(0f, 360f);
            }
            _targetList.Add(PhotonNetwork.InstantiateRoomObject(_target, new Vector3(random[0], random[1], random[2]),Quaternion.Euler(random[3], random[4], random[5])));
        }

        [PunRPC]
        public void TargetDestroy(GameObject desObj)
        {
            _targetList.Remove(desObj);
            Debug.Log("desObj :"+ desObj);
            PhotonNetwork.Destroy(desObj);
        }

        public void TargetOff()
        {
            foreach (GameObject list in _targetList)
            {
                list.SetActive(false);
            }
        }
    }
}

