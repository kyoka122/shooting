using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Photonmanager
{
    public class SetCustomPropertiesManager
    {
        /// <summary>
        /// ���[���J�X�^���v���p�e�B
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        public void RoomCustomPropertiesSettings<T>(T properties, string name)
        {
            ExitGames.Client.Photon.Hashtable customRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            customRoomProperties[name] = properties;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            Debug.Log("SetRoomCustomProperties:"+ properties);
        }

        /// <summary>
        /// �v���C���[�J�X�^���v���p�e�B
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <param name="player"></param>
        public void PlayerCustomPropertiesSettings<T>(T properties, string name, Player player)
        {
            ExitGames.Client.Photon.Hashtable customPlayerProperties = player.CustomProperties;
            customPlayerProperties[name] = properties;
            player.SetCustomProperties(customPlayerProperties);
            Debug.Log("SetPlayerCustomProperties: "+ properties);
        }

    }
}