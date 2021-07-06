using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace GameScene
{
    //TargetObjectにつける
    //自分用
    public class Target3 : MonoBehaviour
    {
        ArrowManager _arrowManager;
        TagList _tagList = new TagList();
        [SerializeField] private int lowPoint = 100;
        private void Start()
        {
            _arrowManager = FindObjectOfType<ArrowManager>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(_tagList.arrowTag))
            {
                _arrowManager.Pause();
                
            }
        }
    }
}