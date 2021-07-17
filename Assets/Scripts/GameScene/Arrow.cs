using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace GameScene
{
    /// <summary>
    /// Arrowオブジェクトにアタッチ
    /// </summary>
    public class Arrow : MonoBehaviour
    {
        private float _posx;
        private float _posy;
        private float _posz;
        private float _radsquere;
        private PhotonView _photonView;
        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }
        private void Update()
        {
            if (_photonView.Owner == PhotonNetwork.LocalPlayer)
            {
                _posx = transform.position.x;
                _posy = transform.position.y;
                _posz = transform.position.z;
                if (_posx < -60 || 60 < _posx || _posy < -60 || 60 < _posy || _posz < -60 || 60 < _posz)
                {
                    _radsquere = _posx * _posx + _posy * _posy + _posz * _posz;
                    if (_radsquere > 3600)
                    {
                        Debug.Log("desObj :" + _photonView);
                        PhotonNetwork.Destroy(_photonView);
                    }
                }
            }
        }
    }
}
