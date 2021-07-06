using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photonmanager;
namespace GameScene
{
    public class Targets2 : MonoBehaviour
    {
        //TargetObjectにつける
        //点数高い方
        TargetManager _targetManager;
        PhotonView _photonView_target;
        ScoreManager _scoreManager;
        [SerializeField] private int highPoint = 500;
        private ResourceList _resourceList = new ResourceList();
        public void OnTriggerEnter(Collider other)
        {
            
            _targetManager = FindObjectOfType<TargetManager>();
            PhotonView photonView = _targetManager.gameObject.GetComponent<PhotonView>();
            photonView.RPC(_resourceList.TargetDestroyRPC, PhotonNetwork.MasterClient, photonView.ViewID);
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("hit2!!!");
                _scoreManager = FindObjectOfType<ScoreManager>();
                _targetManager.TargetInstance();
                _targetManager.TargetDestroy(gameObject);
                _scoreManager.UpdateScore(highPoint);
            }

            //当たったよ表示（ワールド座標でImageで名前と得点（それかプレイヤーリストに））

        }

    }
}
