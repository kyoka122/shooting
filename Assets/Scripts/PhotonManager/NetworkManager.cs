using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using RoomScene;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameScene;
/// <summary>
/// 
/// Unity 2019.1.11f1
/// 
/// Pun: 2.4
/// 
/// Photon lib: 4.1.2.4
/// 
/// </summary>

namespace Photonmanager
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
        private string _roomName ;

        private string _nickName;
        private CustomPropertiesList _propertiesList=new CustomPropertiesList();

        [SerializeField] RoomManager _roomManager;


        /////////////////////////////////////////////////////////////////////////////////////
        // Awake & Start ////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////


        // Start is called before the first frame update
        public void ConectSettings(string nickname,string inputRoomName)
        {
            _nickName = nickname;
            _roomName = inputRoomName;

            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings();
        }

       

        /////////////////////////////////////////////////////////////////////////////////////
        // Connect //////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

        // ニックネームを付ける
        private void SetMyNickName()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = _nickName;
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // Join Lobby ///////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

        // ロビーに入る
        private void JoinLobby()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinLobby();
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // Join Room ////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

        /* 1. 部屋を作成して入室する
        public void CreateAndJoinRoom()
        {
            // ルームオプションの基本設定
            RoomOptions roomOptions = new RoomOptions
            {
                // 部屋の最大人数
                MaxPlayers = (byte)maxPlayers,

                // 公開
                IsVisible = isVisible,

                // 入室可
                IsOpen = isOpen
            };

            // 部屋を作成して入室する
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.CreateRoom(roomName, roomOptions);
            }
        }*/


        // 2. 部屋に入室する （存在しなければ作成して入室する）
        public void JoinOrCreateRoom()
        {
            // ルームオプションの基本設定
            RoomOptions roomOptions = new RoomOptions
            {
                // 部屋の最大人数
                MaxPlayers = (byte)maxPlayers,

                // 公開
                IsVisible = isVisible,

                // 入室可
                IsOpen = isOpen
            };

            ExitGames.Client.Photon.Hashtable customproperties = new ExitGames.Client.Photon.Hashtable
            {
                {1,_propertiesList.roundKey },
                {0,_propertiesList.scoreKey }
            };
            roomOptions.CustomRoomProperties = customproperties;

            // 入室 (存在しなければ部屋を作成して入室する)
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
            }
        }



        // 4. ランダムな部屋に入室する
        public void JoinRandomRoom()
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinRandomRoom();
            }
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

        public void LeaveConnect()
        {
            PhotonNetwork.Disconnect();
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // Pun Callbacks ////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

        // Photonに接続した時
        public override void OnConnected()
        {
            Debug.Log("OnConnected");

            // ニックネームを付ける
            SetMyNickName();
        }


        // Photonから切断された時
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected");
            Debug.Log(cause);
        }


        // マスターサーバーに接続した時
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");

            // ロビーに入る
            JoinLobby();
        }


        // ロビーに入った時
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
            JoinOrCreateRoom();
        }


        // ロビーから出た時
        public override void OnLeftLobby()
        {
            Debug.Log("OnLeftLobby");
        }


        // 部屋を作成した時
        public override void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");
        }


        // 部屋の作成に失敗した時
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnCreateRoomFailed");
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

                _roomManager.InstanceAndSettings();
            }
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


        // ルームプロパティが更新された時
        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            Debug.Log("OnRoomPropertiesUpdate");
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            Debug.Log("OnPlayerPropertiesUpdate");
  
        }

    
    }
}