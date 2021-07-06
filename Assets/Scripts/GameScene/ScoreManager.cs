using Cysharp.Threading.Tasks.Triggers;
using Photon.Pun;
using Photon.Realtime;
using Photonmanager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int _myScore=0;
    private CustomPropertiesList _propertiesList = new CustomPropertiesList();
    private SetCustomPropertiesManager _propertiesManager = new SetCustomPropertiesManager();
    private List<Player> _player=new List<Player>();
    private List<int> _playerScore=new List<int>();
    [SerializeField] private Text _myScoreText;
    [SerializeField] private GameObject[] scoreTextObj = new GameObject[20];
    [SerializeField] private Text[] _scoreText = new Text[20];


    public void SetScore()
    {
        _player = PhotonNetwork.PlayerList.ToList();
        int playerCount = _player.Count;
        int firstScore;
        //スコア初期化
        for (int i=0; i< playerCount; i++)
        {
            if (_player[i].CustomProperties[_propertiesList.scoreKey] is int score)
            {
                firstScore = score;
               
            }
            else
            {
                _propertiesManager.PlayerCustomPropertiesSettings(0, _propertiesList.scoreKey, PhotonNetwork.LocalPlayer);
                firstScore = 0;
            }
            _playerScore.Add(firstScore);

            if (_player[i]==PhotonNetwork.LocalPlayer)
            {
                _myScore = firstScore;
                _myScoreText.text = firstScore.ToString();
            }
        }

        //Sort
        int tmp;
        Player pltmp;
        for (int i=0; i< playerCount;i++)
        {
            for (int j = i + 1; j < playerCount;j++)
            {
                if(_playerScore[i] < _playerScore[j])
                {
                    tmp = _playerScore[i];
                    _playerScore[i] = _playerScore[j];
                    _playerScore[j] = tmp;
                    pltmp = _player[i];
                    _player[i] = _player[j];
                    _player[j]=pltmp;
                }

            }
        }

        //Text初期化
        for (int i=0;i< playerCount;i++)
        {
            scoreTextObj[i].SetActive(true);
            _scoreText[i].text = (i + 1 + ". " + _player[i].NickName + " : " + _playerScore[i]);
        }
        
        
    }

    public void UpdateScore(int getScore)
    {
        _myScore += getScore;
        _myScoreText.text = _myScore.ToString();
        _propertiesManager.PlayerCustomPropertiesSettings(_myScore,_propertiesList.scoreKey,PhotonNetwork.LocalPlayer);
    }

    public int ReadScore()
    {
        return _myScore;
    }


    [PunRPC]
    public void SortMemberScore(Player player,int score)
    {
        int playerIndex = _player.IndexOf(player);

        //点数は必ず上がる前提
        int newIndex=playerIndex;
        newIndex--;
        for (;0<= newIndex; newIndex--)
        {
            if (_playerScore[newIndex] > score)
            {
                _player.Insert(newIndex + 1, player);
                _playerScore.Insert(newIndex + 1, score);

                _playerScore.RemoveAt(playerIndex);
                _player.RemoveAt(playerIndex);
                break;
            }
            
        }
        //変動ない場合はスコアの入れ替えだけ
        if (newIndex==-1)
        {
            Debug.Log("NotChange");
            _playerScore[playerIndex] = score;
            newIndex = playerIndex;
        }
        for (; newIndex <= playerIndex; newIndex++)
        {
            _scoreText[newIndex].text = (newIndex + 1 + ". " + _player[newIndex].NickName + " : " + _playerScore[newIndex]);
        }

    }
}
