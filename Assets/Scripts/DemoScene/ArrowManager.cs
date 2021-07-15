using Cysharp.Threading.Tasks;

using System;
using System.Threading;
using UnityEngine;

namespace DemoScene
{
    public class ArrowManager : MonoBehaviour
    {
        private Rigidbody _arrowRb;
        [SerializeField]private GameObject _arrowObj;
        private CancellationTokenSource _cancellationTokenSource;
        private UniTaskCompletionSource _uniTaskCompletionSource;
        private CancellationTokenSource _linkedToken;
        private CancelCause _cancelCause = CancelCause.CANCEL;
        //public bool gameFinish;

        //[SerializeField]ArrowManager arrowManager;
        //private CameraManager _cameraManager;

        private GameObject _myRotChangeObj;
        //public bool generateArrow = true;
        //[SerializeField] private PlayerInstance playerInstance;
        //[SerializeField] private ArrowGenerater arrowGenerater;
        [SerializeField] private TargetManager targetManager;
        //[SerializeField] GameManager gameManager;
        //[SerializeField] PlayerInstance playerInstance;

        enum CancelCause
        {
            TIMEOVR,
            CANCEL,
            CONTINUE
        }

        public async UniTask StartShooting(CancellationToken token, GameObject myRotObj)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, this.GetCancellationTokenOnDestroy());
            _myRotChangeObj = myRotObj;
            //_cameraManager = FindObjectOfType<CameraManager>();//()内にtrueを入れると非アクティブも検索する(おー！)
            //CancellationTokenSource cancellationTokenSource=new CancellationTokenSource();
            //var token=cancellationTokenSource.Token;

            while (true)
            {
                _cancelCause = CancelCause.CANCEL;
                //await WaitClick(_linkedToken.Token).Forget();エラー出る
                try
                {
                    await WaitClick(token);//値が変化するのを待つgenerateArrow=>false
                }
                catch
                {
                    if (_cancelCause == CancelCause.TIMEOVR || _cancelCause == CancelCause.CANCEL)
                    {
                        break;
                    }

                }

            }
            Debug.Log("Finish!");
            targetManager.TargetOff();
            //token.ThrowIfCancellationRequested();

        }
        private async UniTask WaitClick(CancellationToken token)
        {
            _arrowObj = Instantiate(_arrowObj, transform.position, transform.rotation);
            _arrowObj.transform.SetParent(_myRotChangeObj.transform);//一時的
            _arrowObj.transform.localRotation = Quaternion.Euler(85.95f, 0, 0);
            _arrowObj.transform.localPosition = new Vector3(-1f, 2.5f, 0f);//一時的
            _arrowRb = _arrowObj.GetComponent<Rigidbody>();
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: token);
            //_cameraManager.CameraZoom();
            //アニメーション
            await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: token);
            Debug.Log("click");

            Debug.Log("aaa");
            _arrowRb.AddRelativeForce(new Vector3(0, -50f, 0), ForceMode.VelocityChange);
            //arrowRb.AddRelativeForce(_arrowObj.transform*-10f,ForceMode.VelocityChange);
            _arrowObj.transform.parent = null;
            _arrowObj.transform.parent = null;
            //_cameraManager.CameraOut();
            //click = true;
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
            //token.ThrowIfCancellationRequested();
            return;
        }

        public void Pause()
        {
            _cancelCause = CancelCause.CONTINUE;
            _linkedToken.Cancel();
        }

        public void TimeOver()
        {
            Debug.Log("TimeOver");
            _cancelCause = CancelCause.TIMEOVR;
            _linkedToken.Cancel();
        }
    }
}
