using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace GameScene
{
    //TargetObjectにつける
    //点数低い方
    public class Targets : MonoBehaviour
    {
        TargetManager _targetManager;
        //PhotonView _photonView_targetMn;
        PhotonView _photonView;
        ScoreManager _scoreManager;
        [SerializeField] private int lowPoint=100;
        TagList _tagList = new TagList();
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _linkedToken;


        public void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, this.GetCancellationTokenOnDestroy());
            _photonView = GetComponent<PhotonView>();
            _scoreManager = FindObjectOfType<ScoreManager>();
            _targetManager = FindObjectOfType<TargetManager>();
       
        }


        public async void DestroyTime()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(6f), cancellationToken: _linkedToken.Token);
            }
            catch (OperationCanceledException e)
            {

                Debug.Log("erroe_Target: "+e.Message);
            }
            if (gameObject != null)
            {
                _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer && other.CompareTag(_tagList.arrowTag))
            {
                Debug.Log("hit!!!");
                //_photonView_targetMn = _targetManager.gameObject.GetComponent<PhotonView>();
                _targetManager.TargetDestroy(_photonView);
                _targetManager.TargetInstance();          
                _scoreManager.UpdateScore(lowPoint);
                //当たったよ表示（ワールド座標でImageで名前と得点（それかプレイヤーリストに））
            }

        }
    }

}
