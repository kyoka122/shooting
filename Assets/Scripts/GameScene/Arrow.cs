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
        private void Update()
        {
            _posx = transform.position.x;
            _posy = transform.position.y;
            _posz = transform.position.z;
            if (_posx < -70 || 70 < _posx || _posy < -70 || 70 < _posy || _posz < -70 || 70 < _posz)
            {
                _radsquere = _posx * _posx + _posy * _posy + _posz * _posz;
                if (_radsquere > 490)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}
