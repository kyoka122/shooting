using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photonmanager;


namespace GameScene
{
    public class ResultManager : MonoBehaviourPunCallbacks
    {
        private int _properChangedNum;
        private List<Player> _players=new List<Player>();
        private List<int> _playerscore=new List<int>();
        private bool rpcbool_dispresult=true;
        private ResourceList resourceList = new ResourceList();
        private CustomPropertiesList _propertiesList = new CustomPropertiesList();
        //private ScoreManager _scoreManager;
        [SerializeField] Text[] _scoreText=new Text[20];
        [SerializeField] GameObject[] _scoreTextObj=new GameObject[20];
        [SerializeField] private PhotonView _photonView_GM;
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private ScoreManager _scoreManager;
        //[SerializeField] private GameManager _gameManager;

        //NetworkManagerに入れる？？
        public void CheckScoreSet(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (rpcbool_dispresult) {
                Player[] players = PhotonNetwork.PlayerList;

                for (int i = 0; i < players.Length; i++)
                {
                    Debug.Log("playerscheck" + players[i]);

                    if ((int)players[i].CustomProperties[_propertiesList.stateKey] != (int)State.ScoreSent)
                    {
                        Debug.Log("playerscheckreturn" + players[i]);
                        return;
                    }
                }
                rpcbool_dispresult = false;
                _photonView_GM.RPC(resourceList.dispResultRPC, RpcTarget.AllViaServer);
            }
            //Masterのみ通る
            /*_players.Add(targetPlayer);
            //_playerscore.Add((int)changedProps[_propertiesList.scoreKey]);
            _playerscore.Add((int)targetPlayer.CustomProperties[_propertiesList.scoreKey]);
            if (_players.Count == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (rpcbool_dispresult)
                {
                    rpcbool_dispresult = false;
                    _photonView_GM.RPC(resourceList.dispResultRPC, RpcTarget.AllViaServer);
                    //通信切れたら？？←今回は無視
                }

            }*/

        }
 

        public void Result()
        {
            List<Player> plList = _scoreManager.ReadAllPlayer();
            List<int> plScoreList = _scoreManager.ReadAllScore();

            _resultPanel.SetActive(true);
            int playerCount;
            int prevMax=0;
            int prevRank = 1;
            int max=0;
            int maxPrListNm = 0;
            //list.Sort((a, b) => b - a);
            for (int i=0; (playerCount= plList.Count)!=0; i++)
            {
                _scoreTextObj[i].SetActive(true);
                for (int j=0; j < playerCount; j++)
                {
                    if (j == 0)
                    {
                        max = plScoreList[j];
                        maxPrListNm = 0;
                    }
                    else
                    {
                        if (max<= plScoreList[j])
                        {
                            max = plScoreList[j];
                            maxPrListNm = j;
                        }
                        
                    }
                }
                if (prevMax==max)
                {
                    Debug.Log("ResultTextEdit(same)");
                    _scoreText[i].text = (prevRank+"  "+ plList[maxPrListNm].NickName+"  "+prevMax+"\n");
                }
                else
                {
                    Debug.Log("ResultTextEdit");
                    _scoreText[i].text = (i+1 + "  " + plList[maxPrListNm].NickName + "  " + max + "\n");
                    prevMax = max;
                    prevRank = i+1;
                }
                plList.RemoveAt(maxPrListNm);
                plScoreList.RemoveAt(maxPrListNm);
            }
            

        }
    }
}