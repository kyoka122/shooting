using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace GameScene
{
    //TargetObject‚É‚Â‚¯‚é
    //Ž©•ª—p
    public class Target3 : MonoBehaviour
    {
        ArrowManager _arrowManager;
        TagList _tagList = new TagList();
        [SerializeField] private int lowPoint = 100;
        private void Start()
        {
            _arrowManager = FindObjectOfType<ArrowManager>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer&& other.gameObject.CompareTag(_tagList.arrowTag))
            {
                _arrowManager.Pause();
                
            }
        }
    }
}