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
        //List<GameObject> _targetList = new List<GameObject>();
        private ResourceList _resourceList=new ResourceList();

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
            PhotonNetwork.InstantiateRoomObject(_target, new Vector3(random[0], random[1], random[2]), Quaternion.Euler(random[3], random[4], random[5]));
            //_targetList.Add(PhotonNetwork.InstantiateRoomObject(_target, new Vector3(random[0], random[1], random[2]),Quaternion.Euler(random[3], random[4], random[5])));
        }
        public void TargetDestroy(PhotonView desObjView)
        {
            //_targetList.Remove(desObjView.gameObject);
            Debug.Log("desObj :"+ desObjView);
            if (desObjView.gameObject!=null) {
                desObjView.TransferOwnership(PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(desObjView);
            }
        }


        //�S��Master�ŊǗ����ׂ��H�H
        public void TargetOff()
        {
            /*foreach (GameObject list in _targetList)
            {
                list.SetActive(false);
            }*/
        }
    }
}

