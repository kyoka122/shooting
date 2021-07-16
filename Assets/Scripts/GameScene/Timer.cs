using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace GameScene
{
    public class Timer : MonoBehaviour
    {
        private int _time;
        bool timeBool = true;
        [SerializeField] private int endTime = 1000*5;
        [SerializeField] private ArrowManager arrowManager;
        [SerializeField] private Text _timeTxt;
        
        private void Start()
        {
            Debug.Log("first"+ unchecked(PhotonNetwork.ServerTimestamp));
            endTime += unchecked(PhotonNetwork.ServerTimestamp);
        }
        private void Update()
        {

            if (timeBool) {
                if (endTime - unchecked(PhotonNetwork.ServerTimestamp) < 0)
                {
                    _timeTxt.text = "00:00.00";
                    Debug.Log("Timer Finish");
                    //arrowManager.generateArrow = false;
                    timeBool = false;

                    arrowManager.TimeOver();

                    //gameObject.GetComponent<Timer>().enabled=false;

                }
                else
                {
                    //Debug.Log();
                    _time = endTime - unchecked(PhotonNetwork.ServerTimestamp);
                    _timeTxt.text = _time / 600000 + (_time / 60000) % 10 + " : " + _time / 10000 + (_time / 1000) % 10 + " . " + _time/100  % 10 + _time/10 % 10;
                }
            }
        }

    }
}

