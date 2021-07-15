using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameScene;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using RoomScene;

namespace DemoScene
{
    public class DemoManager : MonoBehaviour
    {
        private string roomScene = "RoomScene";
        private MyRotManager _myRotManager;
        private GameObject _myRotObj;
        private int _round;
        private CancellationTokenSource _cancellationTokenSource_Ist;
        private CancellationTokenSource _linkedToken_Ist;

        private CancellationTokenSource _cancellationTokenSource_Shot;
        private CancellationTokenSource _linkedToken_Shot;
        //[SerializeField] MyRotManager myRotManager;
        [SerializeField] GameScene.Timer _timer;
        [SerializeField] GameScene.PlayerInstance _playerInstance;
        [SerializeField]  ArrowManager _arrowManager;
        [SerializeField] StartGameSettings _gameSettings;//���Ƃ�Room�V�[���Ɉڂ�
        [SerializeField] ScoreManager _scoreManager;
        [SerializeField] Text result;
        private async void Awake()
        {
            _cancellationTokenSource_Ist = new CancellationTokenSource();
            _linkedToken_Ist = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource_Ist.Token, this.GetCancellationTokenOnDestroy());
            _myRotObj = await _playerInstance.InstancePlayer(_linkedToken_Ist.Token);//���������H
            //�A�j���[�V����
            _myRotManager = _myRotObj.GetComponent<MyRotManager>();
            _myRotManager.enabled = true;//forget����H���g��Ȃ�
            _timer.enabled = true;
            _cancellationTokenSource_Ist = new CancellationTokenSource();
            _linkedToken_Ist = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource_Ist.Token, this.GetCancellationTokenOnDestroy());
            await _arrowManager.StartShooting(_linkedToken_Shot.Token, _myRotObj);
            _myRotManager.enabled = false;
            _scoreManager = FindObjectOfType<ScoreManager>();
            int score=_scoreManager.ReadScore();
            result.gameObject.SetActive(true);
            result.text = score.ToString();
            //�A�j���[�V�����i���E���h�P���ʔ��\�j
            //_resultManager.Result();
            SceneManager.LoadScene(roomScene);
            
            return;
        }

        //private void 

    }
}
