using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class Targets2 : MonoBehaviour
    {
        //TargetObjectにつける
        //点数高い方
        TargetManager _targetManager;
        ScoreManager _scoreManager;
        [SerializeField] private int highPoint = 500;
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("hit2!!!");
                _targetManager = FindObjectOfType<TargetManager>();
                _scoreManager = FindObjectOfType<ScoreManager>();
                _targetManager.TargetInstance();
                _targetManager.TargetDestroy(gameObject);
                _scoreManager.UpdateScore(highPoint);
            }

            //当たったよ表示（ワールド座標でImageで名前と得点（それかプレイヤーリストに））

        }

    }
}
