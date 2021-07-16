using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace DemoScene
{ 
    public class TargetManager : MonoBehaviour
    {
        private GameObject _target;
        [SerializeField] private GameObject piece;
        [SerializeField] private GameObject blossom;
        //List<GameObject> _targetList = new List<GameObject>();
       

        public void TargetInstance()
        {
            float parts = Random.value;
            if (parts < 0.8)
            {
                _target = piece;
            }
            else
            {
                _target = blossom;
            }

            float[] random = new float[6];
            for (int i = 0; i < 3; i++)
            {
                random[i] = Random.Range(0f, 50f);
            }
            for (int i = 3; i < 6; i++)
            {
                random[i] = Random.Range(0f, 360f);
            }
            Instantiate(_target, new Vector3(random[0], random[1], random[2]), Quaternion.Euler(random[3], random[4], random[5]));
            //_targetList.Add(PhotonNetwork.InstantiateRoomObject(_target, new Vector3(random[0], random[1], random[2]),Quaternion.Euler(random[3], random[4], random[5])));
        }
        public void TargetDestroy(GameObject desObj)
        {
            //_targetList.Remove(desObjView.gameObject);
            Debug.Log("desObj :" + desObj);
            PhotonNetwork.Destroy(desObj);
        }


        //‘S•”Master‚ÅŠÇ—‚·‚×‚«HH
        public void TargetOff()
        {
            /*foreach (GameObject list in _targetList)
            {
                list.SetActive(false);
            }*/
        }
    }
}

