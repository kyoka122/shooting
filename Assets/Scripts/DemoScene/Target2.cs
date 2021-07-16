using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace DemoScene
{
    public class Target2 : MonoBehaviour
    {
        //TargetObjectにつける
        //点数高い方
        TargetManager _targetManager;
        //PhotonView _photonView_targetMn;
        ScoreManager _scoreManager;
        TagList _tagList = new TagList();
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _linkedToken;
        [SerializeField] private int highPoint = 500;

        public async void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, this.GetCancellationTokenOnDestroy());
            await UniTask.Delay(TimeSpan.FromSeconds(6f), cancellationToken: _linkedToken.Token);
            if (gameObject)
            {
                Debug.Log("desObj :" + gameObject);
                Destroy(gameObject);
            }

        }
        public void OnTriggerEnter(Collider other)
        {

            Debug.Log("hit2!!!");
            _targetManager = FindObjectOfType<TargetManager>();

            _scoreManager = FindObjectOfType<ScoreManager>();
            _targetManager.TargetInstance();

            _scoreManager.UpdateScore(highPoint);
        }

        //当たったよ表示（ワールド座標でImageで名前と得点（それかプレイヤーリストに））

    }

}
