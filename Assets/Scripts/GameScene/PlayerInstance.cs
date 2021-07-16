using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photonmanager;
using GameScene;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Threading;

namespace GameScene
{
    public class PlayerInstance : MonoBehaviour
    {
        public const int playerNum = 20;
        private int _round;
        private int _myPlayerNum;
        private string _myPlayerObjName;
        private GameObject _rotChangeObj;
        private TagList _tagList = new TagList();
        private ResourceList _resourceList=new ResourceList();
        private int[] _scBool_Up = { 1,2,4,5,7,8,10,11,13,14,16,17,19};
        

        //private GameObject _posChangeObj;
        CustomPropertiesList _customPropertiesList=new CustomPropertiesList();
        [System.NonSerialized] public GameObject _myRotChangeObj;
        [SerializeField] private GameObject _prevCamera;
        [SerializeField] GameObject center;
        //[SerializeField] private ArrowManager _arrowManager;

        public async UniTask<GameObject> InstancePlayer(CancellationToken token)
        {
            _round = (int)PhotonNetwork.CurrentRoom.CustomProperties[_customPropertiesList.roundKey];
            _myPlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;

            if (_round == 2)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber > 10)
                {
                    _myPlayerNum -= 10;
                }
                else
                {
                    _myPlayerNum += 10;
                }

            }
            _myPlayerNum--;

            _myPlayerObjName= _resourceList.PlayerObjArray(_myPlayerNum);

            _rotChangeObj = PhotonNetwork.Instantiate(_myPlayerObjName, transform.position, transform.rotation);
            _rotChangeObj.transform.SetParent(center.transform);
            _rotChangeObj.transform.localPosition = new Vector3(0, 0, 0);

            foreach (int n in _scBool_Up) {
                if (_myPlayerNum==n)
                {
                    _rotChangeObj.AddComponent<MoveSphereUp>();
                    break;
                }
                if (n==20)
                {
                    _rotChangeObj.AddComponent<MoveSphereDown>();
                }
                
            }
            foreach(Transform childTf in _rotChangeObj.transform)
            {
                Debug.Log("childObject"+ childTf);
                foreach (Transform grandchildTf in childTf.transform)
                {
                    if (grandchildTf.CompareTag(_tagList.myRotCTag))
                    {
                        _myRotChangeObj = grandchildTf.gameObject;
                        break;
                    }
                }

            }
            Debug.Log("_myRotChangeObj" + _myRotChangeObj);
           
            foreach (Transform childTf in _myRotChangeObj.transform)
            {
         
                if (childTf.CompareTag(_tagList.mainCmTag))
                {
                    childTf.gameObject.SetActive(true);
                }
            }
            //ŠÖ”‚É‚·‚ê‚Î‚æ‚©‚Á‚½c
            _prevCamera.SetActive(false);
           
            return await Task.Run(()=> _myRotChangeObj) ;
        }

    }

}
