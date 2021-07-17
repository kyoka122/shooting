using Cysharp.Threading.Tasks;
//using GameScene;
using Photon.Pun;
using Photon.Realtime;
using Photonmanager;
//using System.Collections;
//using System.Collections.Generic;
//using Cysharp.Threading.Tasks;
//using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace GameScene
{
    public enum State
    {
        None,
        StartSetUp,
        ScoreSent,
        Playing,
        Playing2
    }

    public class GameManager : MonoBehaviourPunCallbacks
    {
        private int maxNum = 20;
        private string roomName = "Room";
        private string _gameScene = "GameScene";
        private MyRotManager _myRotManager;
        private GameObject _myRotObj;
        private int _round;
        private bool _rpcbool_game=true;

        private CancellationTokenSource _cancellationTokenSource_Fst;
        private CancellationTokenSource _linkedToken_Fst;

        private CancellationTokenSource _cancellationTokenSource_Ist;
        private CancellationTokenSource _linkedToken_Ist;

        private CancellationTokenSource _cancellationTokenSource_Shot;
        private CancellationTokenSource _linkedToken_Shot;

        private CancellationTokenSource _cancellationTokenSource_Dly;
        private CancellationTokenSource _linkedToken_Dly;
        private SetCustomPropertiesManager _propertiesManager=new SetCustomPropertiesManager();
        private CustomPropertiesList _customProperties=new CustomPropertiesList();
        private ResourceList _resourceList = new ResourceList();
        private State _nextState;

        //private List<Player> _players=new List<Player>();
        private NetworkManager _networkManager;

        private PlayerInstance _playerInstance;

        //private StartGameSettings _gameSettings;//あとでRoomシーンに移す

        private TargetManager _targetManager;
        [SerializeField]private ArrowManager _arrowManager;
        [SerializeField] private ResultManager _resultManager;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private Timer _timer;
        [SerializeField] private GameObject _camera;
        [SerializeField] State state=State.None;
        [SerializeField] private GameObject _text_start;
        [SerializeField] private GameObject _text_finish;
        [SerializeField] private GameObject _bgm;
        [SerializeField] private GameObject _whistle;
        //[SerializeField] private AudioSource _whistle;

        public State Readstate()
        {
            return state;
        }

        private async void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            _cancellationTokenSource_Fst = new CancellationTokenSource();
            _linkedToken_Fst = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource_Fst.Token, this.GetCancellationTokenOnDestroy());
            
            //PhotonNetwork.IsMessageQueueRunning = true;
            //_propertiesManager.PlayerCustomPropertiesSettings(false, _customProperties.startKey, PhotonNetwork.LocalPlayer);
            //_gameSettings.RoundSettings();
            //_scoreManager = FindObjectOfType<ScoreManager>();
            _targetManager = FindObjectOfType<TargetManager>();
            _networkManager = FindObjectOfType<NetworkManager>();
            _playerInstance = FindObjectOfType<PlayerInstance>();
            //_arrowManager = FindObjectOfType<ArrowManager>();
            //_gameSettings = FindObjectOfType<StartGameSettings>();
            _resultManager = FindObjectOfType<ResultManager>();
           
            _scoreManager.SetScore();
           
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < 25; i++)
                {
                    _targetManager.TargetInstance();
                }
            }

            _myRotObj = await _playerInstance.InstancePlayer(_linkedToken_Fst.Token);//引数入れる？
            Debug.Log("_myRotObj1: "+ _myRotObj);                                                                        //アニメーション           
            _myRotManager = _myRotObj.GetComponent<MyRotManager>();
     
            _myRotManager.enabled = true;
            state = State.StartSetUp;
            if (PhotonNetwork.CurrentRoom.CustomProperties[_customProperties.stateKey]is int)
            {
                _propertiesManager.PlayerCustomPropertiesSettings((int)State.Playing2, _customProperties.stateKey, PhotonNetwork.LocalPlayer);
                _nextState = State.Playing2;
                Debug.Log("Round: "+ State.Playing2);
            }
            else
            {
                _propertiesManager.PlayerCustomPropertiesSettings((int)State.Playing, _customProperties.stateKey, PhotonNetwork.LocalPlayer);
                _nextState = State.Playing;
                Debug.Log("Round: " + State.Playing);
            }
            return;
        }

        //Masterのみ通る
        public void CheckStart(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (_rpcbool_game)
            {
                /*if (state == State.Playing)
                {
                    if ((int)targetPlayer.CustomProperties[_customProperties.startKey] != (int)State.Playing)
                    {
                        return;
                    }
                }
                else if (state == State.Playing2)
                {
                    if ((int)targetPlayer.CustomProperties[_customProperties.startKey] != (int)State.Playing2)
                    {
                        return;
                    }
                }*/
                Player[] players = PhotonNetwork.PlayerList;

                for (int i=0; i< players.Length;i++)
                {
                    Debug.Log("playerscheck"+ players[i]);
                    Debug.Log("state"+ _nextState+" "+(int)_nextState);
                    if ((int)players[i].CustomProperties[_customProperties.stateKey] != (int)_nextState)
                    {
                        Debug.Log("playerscheckreturn" + players[i]);
                        return;
                    }
                }
                _rpcbool_game = false;
                GetComponent<PhotonView>().RPC(_resourceList.gameRPC, RpcTarget.AllViaServer);
                /*Debug.Log(targetPlayer);
                if (!_players.Contains(targetPlayer))
                {
                    _players.Add(targetPlayer);
                }

                Debug.Log("ready: " + PhotonNetwork.CurrentRoom.PlayerCount);
                Debug.Log("playercount: " + _players.Count);
                if (_players.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    _rpcbool_game = false;
                    GetComponent<PhotonView>().RPC(_resourceList.gameRPC, RpcTarget.AllViaServer);
                }*/
            }
        }

        //全員の準備ができたら
        [PunRPC]
        private async UniTaskVoid Game()
        {
            Debug.Log("GameStart");
            state = State.Playing;

            _cancellationTokenSource_Dly = new CancellationTokenSource();
            _linkedToken_Dly = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource_Dly.Token, this.GetCancellationTokenOnDestroy());
            Debug.Log("_linkedToken_Dly.Token: "+ _linkedToken_Dly.Token);
            Debug.Log("_myRotObj: " + _myRotObj);

            _cancellationTokenSource_Ist = new CancellationTokenSource();
            _linkedToken_Ist = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource_Ist.Token, this.GetCancellationTokenOnDestroy());

            _text_start.SetActive(true);
            _whistle.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(2.5f), cancellationToken: _linkedToken_Ist.Token);
            _text_start.SetActive(false);
            _bgm.SetActive(true);
            _timer.enabled = true;
            _whistle.SetActive(false);

            await _arrowManager.StartShooting(_linkedToken_Dly.Token, _myRotObj);
            Debug.Log("Task脱出");


            _whistle.SetActive(true);
            _text_start.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: _linkedToken_Ist.Token);
            _whistle.SetActive(false);
            _text_finish.SetActive(false);

            //アニメーション（ラウンド１結果発表）
            state = State.ScoreSent;
            _propertiesManager.PlayerCustomPropertiesSettings(_scoreManager.ReadScore(),_customProperties.scoreKey, PhotonNetwork.LocalPlayer);
            _propertiesManager.PlayerCustomPropertiesSettings(state, _customProperties.stateKey, PhotonNetwork.LocalPlayer);
        }

        //Result
        [PunRPC]
        public async void DispResultRPC()//できるかな？
        {

            

            Debug.Log("DispRPC");
            _resultManager.Result();
           
            if (PhotonNetwork.IsMasterClient)
            {
                _cancellationTokenSource_Shot = new CancellationTokenSource();
                _linkedToken_Shot = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource_Shot.Token, this.GetCancellationTokenOnDestroy());
                try
                {
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(10f), cancellationToken: _linkedToken_Shot.Token);
                }
                catch
                {
                    Debug.Log("TaskCancel");
                }
                _round = (int)PhotonNetwork.CurrentRoom.CustomProperties[_customProperties.roundKey];
                if (_round == 1)
                {
                    Debug.Log("GoToRound2");
                    _propertiesManager.RoomCustomPropertiesSettings(2, _customProperties.roundKey);
                    photonView.RPC(_resourceList.LoadGameSceneRPC, RpcTarget.AllViaServer);
                                   
                    //SceneManager.LoadScene(_gameScene);
                }
                else
                {

                    Debug.Log("GameFinish");
                    photonView.RPC(_resourceList.LoadDisConnectRPC, RpcTarget.AllViaServer);
                    
                }
            }
        }

        [PunRPC]
        public void LoadGameScene()
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneManager.LoadScene(_gameScene);
        }

        [PunRPC]
        public void LoadDisConnect()
        {
            _camera.SetActive(true);
            _networkManager.LeaveConnect();
        }
    }
}