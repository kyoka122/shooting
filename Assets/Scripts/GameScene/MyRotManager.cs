using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class MyRotManager : MonoBehaviour
    {
        private Vector3 _mousePos;
        private Vector3 _mousePosPrv = new Vector3(0, 0, 0);
        private Vector3 _firstMyRot;
        //public bool controlMyRot = true;
        public void Start()
        {
            _firstMyRot = transform.localEulerAngles;
            //cm = Camera.main;
        }
        public void Update()
        {
            //Debug.Log("aaa");
            _mousePos = Input.mousePosition;

            //プラス？
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + (_mousePos.y - _mousePosPrv.y) * 0.5f, transform.localEulerAngles.y - (_mousePosPrv.x - _mousePos.x) * 0.5f, transform.localEulerAngles.z);
            _mousePosPrv = _mousePos;
            //初期角度に戻す
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.localEulerAngles = _firstMyRot;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + 20, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y -20, transform.localEulerAngles.z);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - 20, transform.localEulerAngles.y, transform.localEulerAngles.z + 20);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 20, transform.localEulerAngles.z);
            }
            

        }
    }

}