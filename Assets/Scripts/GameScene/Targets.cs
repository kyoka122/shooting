using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Drawing;
using Photonmanager;

namespace GameScene
{
    //TargetObject�ɂ���
    //�_���Ⴂ��
    public class Targets : MonoBehaviourPunCallbacks
    {
        TargetManager targetManager;
        PhotonView _photonView_target;
        ScoreManager scoreManager;
        [SerializeField] private int lowPoint=100;
        private ResourceList _resourceList = new ResourceList();

        public void OnTriggerEnter(Collider other)
        {
            
            targetManager = FindObjectOfType<TargetManager>();
            targetManager.gameObject.GetComponent<PhotonView>().RPC(_resourceList.TargetDestroyRPC, PhotonNetwork.MasterClient, photonView.ViewID);

            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("hit!!!");
                scoreManager = FindObjectOfType<ScoreManager>();
                targetManager.TargetInstance();          
                scoreManager.UpdateScore(lowPoint);
                //����������\���i���[���h���W��Image�Ŗ��O�Ɠ��_�i���ꂩ�v���C���[���X�g�Ɂj�j
            }

        }
    }

}
