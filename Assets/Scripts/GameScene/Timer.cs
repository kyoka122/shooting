using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace GameScene
{
    public class Timer : MonoBehaviour
    {
        //private int currentTime ;
        //1000=1s
        [SerializeField] private int endTime = 1000*5;
        [SerializeField] private ArrowManager arrowManager;

        private void Start()
        {
            Debug.Log("first"+ PhotonNetwork.ServerTimestamp);
            endTime +=PhotonNetwork.ServerTimestamp;
        }
        private void Update()
        {
            Debug.Log(endTime - unchecked(PhotonNetwork.ServerTimestamp));
            if (endTime - unchecked(PhotonNetwork.ServerTimestamp) < 0)
            {
                //arrowManager.generateArrow = false;
                arrowManager.TimeOver();
                gameObject.SetActive(false);
            }
        }

    }
}

