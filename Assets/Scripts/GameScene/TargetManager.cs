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
        private string _destroyTime = "DestroyTime";
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
                random[i] = Random.Range(0f,70f);
            }
            for (int i = 3; i < 6; i++)
            {
                random[i] = Random.Range(0f, 360f);
            }
            var rargetObj=PhotonNetwork.InstantiateRoomObject(_target, new Vector3(random[0], random[1], random[2]), Quaternion.Euler(random[3], random[4], random[5]));
            var components = rargetObj.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Debug.Log(" getcomponent: " + component);
                var type = component.GetType();
                if (type == typeof(Targets) || type == typeof(Targets2))
                {
                    var setMethod = GetType().GetMethod(_destroyTime);
                    if (setMethod != null)
                    {
                        setMethod.Invoke(this, null);
                    }

                }

            }
            //_targetList.Add(PhotonNetwork.InstantiateRoomObject(_target, new Vector3(random[0], random[1], random[2]),Quaternion.Euler(random[3], random[4], random[5])));
        }
        public void TargetDestroy(PhotonView desObjView)
        {
            //_targetList.Remove(desObjView.gameObject);
            Debug.Log("desObj :"+ desObjView);
            if (desObjView.gameObject!=null) {
                PhotonNetwork.Destroy(desObjView);
            }
        }


        //ëSïîMasterÇ≈ä«óùÇ∑Ç◊Ç´ÅHÅH
        public void TargetOff()
        {
            /*foreach (GameObject list in _targetList)
            {
                list.SetActive(false);
            }*/
        }
    }
}

