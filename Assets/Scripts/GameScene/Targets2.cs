using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photonmanager;
namespace GameScene
{
    public class Targets2 : MonoBehaviour
    {
        //TargetObject�ɂ���
        //�_��������
        TargetManager _targetManager;
        PhotonView _photonView_target;
        ScoreManager _scoreManager;
        [SerializeField] private int highPoint = 500;
        private ResourceList _resourceList = new ResourceList();
        public void OnTriggerEnter(Collider other)
        {
            
            _targetManager = FindObjectOfType<TargetManager>();
            PhotonView photonView = _targetManager.gameObject.GetComponent<PhotonView>();
            photonView.RPC(_resourceList.TargetDestroyRPC, PhotonNetwork.MasterClient, photonView.ViewID);
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("hit2!!!");
                _scoreManager = FindObjectOfType<ScoreManager>();
                _targetManager.TargetInstance();
                _targetManager.TargetDestroy(gameObject);
                _scoreManager.UpdateScore(highPoint);
            }

            //����������\���i���[���h���W��Image�Ŗ��O�Ɠ��_�i���ꂩ�v���C���[���X�g�Ɂj�j

        }

    }
}
