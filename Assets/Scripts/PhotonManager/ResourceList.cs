using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photonmanager
{
    public class ResourceList
    {
        public string rotChangeObj { get => "RotChange"; }
        public string arrowObj { get => "Arrow"; }
        public string unitychan { get => "unitychan"; }

        public string blossom { get => "cherryblossom"; }
        public string piece { get => "cherryblossompiece"; }
        private string[] str = new string[2];
        private string rotChangeObjName;


        //Method©keyList‚É‚·‚ê‚Î‚æ‚©‚Á‚½c
        public string dispResultRPC { get => "DispResultRPC"; }
        public string gameRPC { get => "Game"; }

        public string SortMScoreRPC { get => "SortMemberScore"; }

        public string TargetDestroyRPC{get => "TargetDestroy";   }
        public string TargetInstanceRPC{get => "TargetInstance";   }
        public string PlayerObjArray(int num)
        {
            str[0] = rotChangeObj;
            str[1] = num.ToString();

            rotChangeObjName = string.Join("", str);

            return rotChangeObjName;
        }
    }
}