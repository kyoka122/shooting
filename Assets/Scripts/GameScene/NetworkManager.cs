using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using RoomScene;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace GameScene
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {

        /////////////////////////////////////////////////////////////////////////////////////
        // Field ////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

        [Header("DefaultRoomSettings")]

        // 最大人数
        [SerializeField] private int maxPlayers = 20;

        // 公開・非公開
        [SerializeField] private bool isVisible = true;

        // 入室の可否
        [SerializeField] private bool isOpen = true;

        // 部屋名
        [SerializeField] private string roomName = "Room";

        private string _nickName;
        private Photonmanager.CustomPropertiesList _propertiesList = new Photonmanager.CustomPropertiesList();

        [SerializeField] private GameObject gameManagerObj;
 
        ResultManager _resultmanager;
        [SerializeField] GameManager _gameManager;
        [SerializeField] ScoreManager _scoreManager;
        /////////////////////////////////////////////////////////////////////////////////////
        // Awake & Start ////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////
        private void Awake()
        {
            Debug.Log("AddTarget");
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        public void LeaveConnect()
        {
            PhotonNetwork.Disconnect();
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // Leave Room ///////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

        // 部屋から退室する
        public void LeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                // 退室
                PhotonNetwork.LeaveRoom();
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // Pun Callbacks ////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

 

        // Photonから切断された時
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected");
            Debug.Log(cause);
        }

        // 部屋に入室した時
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");

            // 部屋の情報を表示
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("RoomName: " + PhotonNetwork.CurrentRoom.Name);
                Debug.Log("HostName: " + PhotonNetwork.MasterClient.NickName);
                Debug.Log("Slots: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
            }
            gameManagerObj.SetActive(true);
        }


        // 特定の部屋への入室に失敗した時
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed");
        }


        // ランダムな部屋への入室に失敗した時
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
        }


        // 部屋から退室した時
        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
        }



        // ロビーに更新があった時
        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            Debug.Log("OnLobbyStatisticsUpdate");
        }


        // ルームリストに更新があった時
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
        }

        //public virtual void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        // ルームプロパティが更新された時
        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            Debug.Log("OnRoomPropertiesUpdate GameScene");
        }

        //public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            Debug.Log("OnPlayerPropertiesUpdate GameScene");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("targetPlayer"+targetPlayer);
                if (_gameManager.Readstate()==State.StartSetUp)
                {
                    Debug.Log("startaKey");
                    _gameManager.CheckStart(targetPlayer, changedProps);
                }
                else if (_gameManager.Readstate() == State.Playing)
                {
                    Debug.Log("_scoreManager" + _scoreManager);
                    Debug.Log("targetPlayer" + targetPlayer);
                    Debug.Log("changedProps[_propertiesList.scoreKey]: "+changedProps[_propertiesList.scoreKey]);
                    int num = (int)changedProps[_propertiesList.scoreKey];
                    _scoreManager.SortMemberScore(targetPlayer, num);
                }
                else if(_gameManager.Readstate() == State.ScoreSent)
                {
                    Debug.Log("scoreKey");
                    _resultmanager = FindObjectOfType<ResultManager>();
                    _resultmanager.CheckScoreSet(targetPlayer, changedProps);
                }
            }
        }


    }
}


