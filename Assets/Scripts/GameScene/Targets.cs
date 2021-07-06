using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Drawing;
using Photonmanager;

namespace GameScene
{
    //TargetObjectにつける
    //点数低い方
    public class Targets : MonoBehaviour
    {
        TargetManager targetManager;
        PhotonView _photonView_target;
        ScoreManager scoreManager;
        [SerializeField] private int lowPoint=100;
        private ResourceList _resourceList = new ResourceList();

        public void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("hit!!!");
                targetManager = FindObjectOfType<TargetManager>();
                PhotonView photonView = targetManager.gameObject.GetComponent<PhotonView>();
                photonView.RPC(_resourceList.TargetDestroyRPC, PhotonNetwork.MasterClient, photonView.ViewID);
                scoreManager = FindObjectOfType<ScoreManager>();
                targetManager.TargetInstance();          
                scoreManager.UpdateScore(lowPoint);
                //当たったよ表示（ワールド座標でImageで名前と得点（それかプレイヤーリストに））
            }

        }
    }

}
