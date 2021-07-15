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
        bool timeBool=true;
        private void Start()
        {
            Debug.Log("first"+ unchecked(PhotonNetwork.ServerTimestamp));
            endTime += unchecked(PhotonNetwork.ServerTimestamp);
        }
        private void Update()
        {
            //Debug.Log(endTime - unchecked(PhotonNetwork.ServerTimestamp));

            if (timeBool&&endTime - unchecked(PhotonNetwork.ServerTimestamp) < 0)
            {
                Debug.Log("Timer Finish");
                //arrowManager.generateArrow = false;
                timeBool = false;
 
                    arrowManager.TimeOver();

                //gameObject.GetComponent<Timer>().enabled=false;
  
            }
        }

    }
}

