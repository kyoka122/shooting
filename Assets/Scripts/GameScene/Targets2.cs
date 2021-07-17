using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace GameScene
{
    public class Targets2 : MonoBehaviour
    {
        //TargetObjectにつける
        //点数高い方
        TargetManager _targetManager;
        //PhotonView _photonView_targetMn;
        PhotonView _photonView;
        ScoreManager _scoreManager;
        [SerializeField] private int highPoint = 500;
        TagList _tagList = new TagList();
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _linkedToken;


        public void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, this.GetCancellationTokenOnDestroy());
            _targetManager = FindObjectOfType<TargetManager>();
            _photonView = GetComponent<PhotonView>();
            _scoreManager = FindObjectOfType<ScoreManager>();
        

        }
        public async void DestroyTime()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(6f), cancellationToken: _linkedToken.Token);
            }
            catch (OperationCanceledException e)
            {

                Debug.Log("erroe_Target2: " + e.Message);
            }
            if (gameObject!=null)
            {
                Debug.Log("desObj :" + _photonView);
                PhotonNetwork.Destroy(_photonView);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tagList.arrowChildTag))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("hit!!!");
                    //_photonView_targetMn = _targetManager.gameObject.GetComponent<PhotonView>();
                    _targetManager.TargetDestroy(_photonView);
                    _targetManager.TargetInstance();
                    //当たったよ表示（ワールド座標でImageで名前と得点（それかプレイヤーリストに））
                }
                Debug.Log("other.transform.parent.gameObject: " + other.transform.parent.gameObject);
                if (other.transform.parent.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
                {
                    _scoreManager.UpdateScore(highPoint);

                }


            }

        }
    }
}
