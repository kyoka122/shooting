using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Photon.Pun;
using Photon.Realtime;
using Photonmanager;
using System.Linq;
using System.Threading.Tasks;

namespace GameScene
{
    public class ResultManager : MonoBehaviourPunCallbacks
    {
        private int _properChangedNum;
        private List<Player> _players=new List<Player>();
        private List<int> _playerscore=new List<int>();
        private ResourceList resourceList = new ResourceList();
        private CustomPropertiesList _propertiesList = new CustomPropertiesList();
        //private ScoreManager _scoreManager;
        [SerializeField] Text[] _scoreText=new Text[20];
        [SerializeField] GameObject[] _scoreTextObj=new GameObject[20];
        [SerializeField] private PhotonView _photonView_GM;
        [SerializeField] private GameObject _resultPanel;

        //NetworkManagerに入れる？？
        public void CheckScoreSet(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            //Masterのみ通る
            _players.Add(targetPlayer);
            //_playerscore.Add((int)changedProps[_propertiesList.scoreKey]);
            _playerscore.Add((int)targetPlayer.CustomProperties[_propertiesList.scoreKey]);
            if (_players.Count == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                _photonView_GM.RPC(resourceList.dispResultRPC, RpcTarget.AllViaServer);
                //通信切れたら？？←今回は無視

            }

        }
 

        public void Result()
        {
            _resultPanel.SetActive(true);
            int playerCount;
            int prevMax=0;
            int prevRank = 1;
            int max=0;
            int maxPrListNm = 0;
            //list.Sort((a, b) => b - a);
            for (int i=0; (playerCount=_players.Count)!=0; i++)
            {
                _scoreTextObj[i].SetActive(true);
                for (int j=0; j < playerCount; j++)
                {
                    if (j == 0)
                    {
                        max = _playerscore[j];
                        maxPrListNm = 0;
                    }
                    else
                    {
                        if (max<=_playerscore[j])
                        {
                            max = _playerscore[j];
                            maxPrListNm = j;
                        }
                        
                    }
                }
                if (prevMax==max)
                {
                    Debug.Log("ResultTextEdit(same)");
                    _scoreText[i].text = (prevRank+"  "+ _players[maxPrListNm].NickName+"  "+prevMax+"\n");
                }
                else
                {
                    Debug.Log("ResultTextEdit");
                    _scoreText[i].text = (i+1 + "  " + _players[maxPrListNm].NickName + "  " + max + "\n");
                    prevMax = max;
                    prevRank = i+1;
                }
                _players.RemoveAt(maxPrListNm);
                _playerscore.RemoveAt(maxPrListNm);
            }
            

        }
    }
}