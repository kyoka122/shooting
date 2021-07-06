using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class CameraManager : MonoBehaviour
    {
        //private Vector3 worldPointRot;
        private Vector3 _cmZoomPrv;

        public void CameraZoom()
        {
            _cmZoomPrv = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(_cmZoomPrv.x, _cmZoomPrv.y + 10f, _cmZoomPrv.z);
        }
        public void CameraOut()
        {
            transform.localEulerAngles = new Vector3(_cmZoomPrv.x, _cmZoomPrv.y, _cmZoomPrv.z);
        }
    }


}