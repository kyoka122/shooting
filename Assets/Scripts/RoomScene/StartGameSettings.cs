using Photonmanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace GameScene
{
    public class StartGameSettings : MonoBehaviour
    {
        SetCustomPropertiesManager _propertiesManager = new SetCustomPropertiesManager();
        CustomPropertiesList _propertiesList = new CustomPropertiesList();
        public void RoundSettings()
        {
            _propertiesManager.RoomCustomPropertiesSettings(1,_propertiesList.roundKey);
        }


        public void SetColorProperties(float r,float g,float b,float value)
        {
            float[] colorArray = new float[4];
            colorArray[0] = r;
            colorArray[1] = g;
            colorArray[2] = b;
            colorArray[3] = value;

            _propertiesManager.PlayerCustomPropertiesSettings(colorArray, _propertiesList.colorKey, PhotonNetwork.LocalPlayer);
        }
    }
}

