using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace GameScene
{
    //TargetObject‚É‚Â‚¯‚é
    //PlayerObj—p
    public class Target3 : MonoBehaviour
    {
        PhotonView _photonView;
        ArrowManager _arrowManager;
        TagList _tagList = new TagList();
        MyDamageText _damageText;


        private void Awake()
        {
            _damageText = FindObjectOfType<MyDamageText>();
            _photonView = GetComponent<PhotonView>();           
            _arrowManager = FindObjectOfType<ArrowManager>();        
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_photonView.IsMine) {
                if(other.gameObject.transform.parent is Transform parentTf)
                {
                    Debug.Log("parentTf: " + parentTf);
                    if (parentTf.gameObject.GetComponent<PhotonView>().Owner is Player hitplayer)
                    {
                        if (hitplayer != PhotonNetwork.LocalPlayer && parentTf.CompareTag(_tagList.arrowChildTag))
                        {
                            _arrowManager.Pause();
                            _damageText.PopUpText(hitplayer);
                        }
                    }
                    
                }
     
                
            } 
        }

        


    }
}