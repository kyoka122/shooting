using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene {
    public class Timer : MonoBehaviour
    {
        [SerializeField] private int endTime = 1000 * 5;
        [SerializeField] private ArrowManager arrowManager;
        private void Start()
        {

        }
        private void Update()
        {
            endTime --;
            //Debug.Log(endTime - unchecked(PhotonNetwork.ServerTimestamp));
            if (endTime < 0)
            {
                //arrowManager.generateArrow = false;
                arrowManager.TimeOver();
                gameObject.SetActive(false);
            }
        }
    }
}
