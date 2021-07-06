using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Drawing;

namespace GameScene
{
    //TargetObject�ɂ���
    //�_���Ⴂ��
    public class Targets : MonoBehaviour
    {
        TargetManager targetManager;
        ScoreManager scoreManager;
        [SerializeField] private int lowPoint=100;
        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("hit!!!");
            if (other.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                targetManager = FindObjectOfType<TargetManager>();
                scoreManager = FindObjectOfType<ScoreManager>();
                targetManager.TargetInstance();
                targetManager.TargetDestroy(gameObject);
                scoreManager.UpdateScore(lowPoint);
                //����������\���i���[���h���W��Image�Ŗ��O�Ɠ��_�i���ꂩ�v���C���[���X�g�Ɂj�j
            }

        }
    }

}
