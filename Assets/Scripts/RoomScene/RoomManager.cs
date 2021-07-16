using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photonmanager;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;
using Photon.Pun;
using UnityEngine.SceneManagement;
using GameScene;
using System;

namespace RoomScene
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        private float value;
        [SerializeField] private const float intensity = 1.8f;//emissionÇÃintensityÇÃílÇàÍíËÇ…
        private float value2;

        private string _inputStr;
        private string _inputRoomName;
        private string _gameScene = "GameScene";
        private string _demoScene = "DemoScene";
        private InputField _inputField;
        private InputField _inputField_Room;
        private Color _sphereColor=new Color(1f,0.325f,0.133f);
        private PlayMode _playMode;
        private ResourceList _resourceList = new ResourceList();
        [SerializeField]private PhotonView pV_gmsettings;
        [SerializeField] private Renderer _sphereRendere;
        [SerializeField] private GameObject _roomUI;
        [SerializeField] private GameObject _title;
        [SerializeField] private GameObject _plane;       
        [SerializeField] private Text _nameText;
        [SerializeField] private Photonmanager.NetworkManager _networkManager;
        [SerializeField] private RoomScene.PlayerInstance _playerInstance;
        [SerializeField] private StartGameSettings _gameSettings;
        [SerializeField] private GameObject _inputFieldObj;
        [SerializeField] private GameObject _inputFieldRoomObj;
        [SerializeField] private GameObject _colorSetSphere;
        [SerializeField] private GameObject unitychan_OffLine;
        
        public enum PlayMode
        {
            Local,
            Online
        }

        /// <summary>
        /// DemoScene(Button)
        /// </summary>
        public void StartLocal()
        {
            _playMode = PlayMode.Local;
            _ =RoomSettings_Local();   
        }

        public async UniTaskVoid RoomSettings_Local()
        {
            await MoveInputField();            
            Instantiate(unitychan_OffLine);
            RoomSceneSettings();         
        }

        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// GameScene(Button)
        /// </summary>
        /// <returns></returns>
        public void StartMatching()
        {
            _playMode = PlayMode.Online;
            _ = RoomSettings_Online();
        }

        public async UniTask RoomSettings_Online()
        {
            await MoveInputField();
            _networkManager.ConectSettings(_inputStr,_inputRoomName);//PhotonÇ≈îÒìØä˙Ç≈Ç´ÇÈÇÊÇ§Ç…Ç»Ç¡ÇΩÇÁÅ´Ç©ÇÁPlayerInstanceåƒÇ‘ÅB
        }

        public void InstanceAndSettings()
        {
            RoomSceneSettings();
            _playerInstance.InstancePlayer();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //
        private async UniTask MoveInputField()
        {
            _title.SetActive(false);
            _inputFieldObj.SetActive(true);
            _inputField = _inputFieldObj.GetComponent<InputField>();
            _inputField_Room = _inputFieldRoomObj.GetComponent<InputField>();
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            _inputStr = await AwaitInputField(token);

            _inputFieldObj.SetActive(false);
            _inputFieldRoomObj.SetActive(true);

            CancellationToken tokena = this.GetCancellationTokenOnDestroy();
            _inputRoomName = await AwaitInputRoomNme(tokena);
        }


        private async UniTask<string> AwaitInputField(CancellationToken token)
        {
            IAsyncEndEditEventHandler<string> handler = _inputField.GetAsyncEndEditEventHandler(token);
            string input = await handler.OnEndEditAsync();
            Debug.Log("InputNickName : " + input);
            return input;
        }
        private async UniTask<string> AwaitInputRoomNme(CancellationToken token)
        {
            IAsyncEndEditEventHandler<string> handler = _inputField_Room.GetAsyncEndEditEventHandler(token);
            string input = await handler.OnEndEditAsync();
            Debug.Log("InputNickName : " + input);
            return input;
        }
        private void RoomSceneSettings()
        {
            value =191*(Mathf.Pow(Mathf.Exp(1),intensity * Mathf.Log(2f))/255);

            _inputFieldObj.SetActive(false);
            _roomUI.SetActive(true);
            _plane.SetActive(true);
            _colorSetSphere.SetActive(true);
            _nameText.text = _inputStr;
        }


        //Button
        public void GameSettings()
        {
            value2 = value / _sphereColor.maxColorComponent;

            if (_playMode==PlayMode.Local)
            {
                SceneManager.LoadScene(_demoScene);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("pV_gmsettings: " + pV_gmsettings);
                    _gameSettings.RoundSettings();
                    pV_gmsettings.RPC(_resourceList.setColorPropertiesRPC, RpcTarget.All, _sphereColor.r, _sphereColor.g, _sphereColor.b, value2);      
                }
                
            }            
           
        }
        public void GameSettingsWait()
        {
            
            //PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LoadLevel(_gameScene);
            //SceneManager.LoadScene(_gameScene);
        }










        public void ChangeColor_R(float r)
        {
            _sphereColor = new Color(r,_sphereColor.g,_sphereColor.b,_sphereColor.a);
            _sphereRendere.material.color = _sphereColor;
            SetRenderer();
        }
        public void ChangeColor_G(float g)
        {
            _sphereColor = new Color(_sphereColor.r, g, _sphereColor.b, _sphereColor.a);
            _sphereRendere.material.color = _sphereColor;
            SetRenderer();
        }
        public void ChangeColor_B(float b)
        {
            _sphereColor = new Color(_sphereColor.r, _sphereColor.g, b, _sphereColor.a);
            _sphereRendere.material.color = _sphereColor;
            SetRenderer();
        }

        private void SetRenderer()
        {
            value2=value / _sphereColor.maxColorComponent;

            _sphereRendere.material.EnableKeyword("_EMISSION");
            _sphereRendere.material.SetColor("_EmissionColor",new Color( _sphereColor.r* value2, _sphereColor.g* value2, _sphereColor.b* value2));
        }

        public void ChangeColor_A(float a)
        {
            _sphereColor = new Color(_sphereColor.r, _sphereColor.g, _sphereColor.b, a);
            _sphereRendere.material.color = _sphereColor;
        }

        
    }
}