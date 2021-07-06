using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
//using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Photon.Pun;

namespace GameScene
{
    public class WaitScore 
    {
        public async UniTaskVoid SendScore(int playerNum)
        {
            Debug.Log("Num"+playerNum+" send");
        }
    }
}