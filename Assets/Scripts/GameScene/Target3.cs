using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace GameScene
{
    //TargetObject‚É‚Â‚¯‚é
    //PlayerObj—p
    public class Target3 : MonoBehaviour
    {
        ArrowManager _arrowManager;
        TagList _tagList = new TagList();
        [SerializeField] private Text hitText;
        private void Start()
        {
            _arrowManager = FindObjectOfType<ArrowManager>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer&& other.gameObject.CompareTag(_tagList.arrowChildTag))
            {   
                _arrowManager.Pause();
                
            }
        }
    }
}