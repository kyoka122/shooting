using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class MyDamageText : MonoBehaviour
{
    [SerializeField] private GameObject _hitTextObj;
    private Text _hitText;
    private CancellationTokenSource _cancellationTokenSource;
    private CancellationTokenSource _linkedToken;

    public void Awake()
    {
        _hitText = GetComponent<Text>();
        _cancellationTokenSource = new CancellationTokenSource();
        _linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, this.GetCancellationTokenOnDestroy());
    }

    public async void PopUpText(Player hitplayer)
    {
        _hitTextObj.SetActive(true);
        _hitText.text = hitplayer + "\nÅ´\n" + PhotonNetwork.LocalPlayer;
        await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: _linkedToken.Token);
        _hitTextObj.SetActive(false);

    }
}
