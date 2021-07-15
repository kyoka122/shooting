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

        // �ő�l��
        [SerializeField] private int maxPlayers = 20;

        // ���J�E����J
        [SerializeField] private bool isVisible = true;

        // �����̉�
        [SerializeField] private bool isOpen = true;

        // ������
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

        // �j�b�N�l�[����t����
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

        // ���r�[�ɓ���
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

        /* 1. �������쐬���ē�������
        public void CreateAndJoinRoom()
        {
            // ���[���I�v�V�����̊�{�ݒ�
            RoomOptions roomOptions = new RoomOptions
            {
                // �����̍ő�l��
                MaxPlayers = (byte)maxPlayers,

                // ���J
                IsVisible = isVisible,

                // ������
                IsOpen = isOpen
            };

            // �������쐬���ē�������
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.CreateRoom(roomName, roomOptions);
            }
        }*/


        // 2. �����ɓ������� �i���݂��Ȃ���΍쐬���ē�������j
        public void JoinOrCreateRoom()
        {
            // ���[���I�v�V�����̊�{�ݒ�
            RoomOptions roomOptions = new RoomOptions
            {
                // �����̍ő�l��
                MaxPlayers = (byte)maxPlayers,

                // ���J
                IsVisible = isVisible,

                // ������
                IsOpen = isOpen
            };

            ExitGames.Client.Photon.Hashtable customproperties = new ExitGames.Client.Photon.Hashtable
            {
                {1,_propertiesList.roundKey },
                {0,_propertiesList.scoreKey }
            };
            roomOptions.CustomRoomProperties = customproperties;

            // ���� (���݂��Ȃ���Ε������쐬���ē�������)
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
            }
        }



        // 4. �����_���ȕ����ɓ�������
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

        // ��������ގ�����
        public void LeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                // �ގ�
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

        // Photon�ɐڑ�������
        public override void OnConnected()
        {
            Debug.Log("OnConnected");

            // �j�b�N�l�[����t����
            SetMyNickName();
        }


        // Photon����ؒf���ꂽ��
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected");
            Debug.Log(cause);
        }


        // �}�X�^�[�T�[�o�[�ɐڑ�������
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");

            // ���r�[�ɓ���
            JoinLobby();
        }


        // ���r�[�ɓ�������
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
            JoinOrCreateRoom();
        }


        // ���r�[����o����
        public override void OnLeftLobby()
        {
            Debug.Log("OnLeftLobby");
        }


        // �������쐬������
        public override void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");
        }


        // �����̍쐬�Ɏ��s������
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnCreateRoomFailed");
        }


        // �����ɓ���������
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");

            // �����̏���\��
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("RoomName: " + PhotonNetwork.CurrentRoom.Name);
                Debug.Log("HostName: " + PhotonNetwork.MasterClient.NickName);
                Debug.Log("Slots: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);

                _roomManager.InstanceAndSettings();
            }
        }


        // ����̕����ւ̓����Ɏ��s������
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed");
        }


        // �����_���ȕ����ւ̓����Ɏ��s������
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
        }


        // ��������ގ�������
        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
        }



        // ���r�[�ɍX�V����������
        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            Debug.Log("OnLobbyStatisticsUpdate");
        }


        // ���[�����X�g�ɍX�V����������
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
        }


        // ���[���v���p�e�B���X�V���ꂽ��
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