using Cysharp.Threading.Tasks.Triggers;
using Photon.Pun;
using Photon.Realtime;
using Photonmanager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemoScene
{
    public class ScoreManager : MonoBehaviour
    {
        private int _myScore = 0;
        private CustomPropertiesList _propertiesList = new CustomPropertiesList();
        private SetCustomPropertiesManager _propertiesManager = new SetCustomPropertiesManager();
        private List<Player> _player = new List<Player>();
        private List<int> _playerScore = new List<int>();
        [SerializeField] private Text _myScoreText;
        [SerializeField] private GameObject[] scoreTextObj = new GameObject[20];
        [SerializeField] private Text[] _scoreText = new Text[20];


        public void UpdateScore(int getScore)
        {
            _myScore += getScore;
            _myScoreText.text = _myScore.ToString();
            
        }

        public int ReadScore()
        {
            return _myScore;
        }

    }
}