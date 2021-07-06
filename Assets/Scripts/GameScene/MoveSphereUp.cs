using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class MoveSphereUp : MonoBehaviour
    {
        private float _rotx = 0;
        private void FixedUpdate()
        {
            _rotx = 0.07f;
            transform.Rotate(new Vector3(1, 0, 0), _rotx);
            //transform.localRotation =Quaternion.Euler(rotx, gameObject.transform.localRotation.y, 0f) ;
        }
    }
}
