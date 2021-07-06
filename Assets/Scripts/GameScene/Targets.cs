using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Drawing;

namespace GameScene
{
    //TargetObjectにつける
    //点数低い方
    public class Targets : MonoBehaviour
    {
        TargetManager targetManager;
        ScoreManager scoreManager;
        [SerializeField] private int lowPoint=100;
        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("hit!!!");
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("hit!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                targetManager = FindObjectOfType<TargetManager>();
                targetManager.TargetDestroy(gameObject);
                scoreManager = FindObjectOfType<ScoreManager>();
                targetManager.TargetInstance();          
                scoreManager.UpdateScore(lowPoint);
                //当たったよ表示（ワールド座標でImageで名前と得点（それかプレイヤーリストに））
            }

        }
    }

}
