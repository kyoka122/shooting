using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class Targets2 : MonoBehaviour
    {
        //TargetObject�ɂ���
        //�_��������
        TargetManager _targetManager;
        ScoreManager _scoreManager;
        [SerializeField] private int highPoint = 500;
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("hit2!!!");
                _targetManager = FindObjectOfType<TargetManager>();
                _scoreManager = FindObjectOfType<ScoreManager>();
                _targetManager.TargetInstance();
                _targetManager.TargetDestroy(gameObject);
                _scoreManager.UpdateScore(highPoint);
            }

            //����������\���i���[���h���W��Image�Ŗ��O�Ɠ��_�i���ꂩ�v���C���[���X�g�Ɂj�j

        }

    }
}
