using GameScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLimit : MonoBehaviour
{
    private Vector3 _pos;
    [SerializeField] ArrowManager arrowManager;
    void FixedUpdate()
    {
        _pos = gameObject.transform.position;

        if (_pos.x<-10||10<_pos.x||_pos.z < -10 || 10 < _pos.z)
        {
            gameObject.transform.position=new Vector3(0,_pos.y,0);
        }   
    }

}
