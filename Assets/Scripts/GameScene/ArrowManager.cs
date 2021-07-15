using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photonmanager;
using System;
using System.Threading;
using UnityEngine;

namespace GameScene
{
    public class ArrowManager : MonoBehaviour
    {
        private Rigidbody _arrowRb;
        private GameObject _arrowObj;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _linkedToken;
        private CancelCause _cancelCause = CancelCause.CANCEL;
        private bool whileBool=true;
        //public bool gameFinish;

        //[SerializeField]ArrowManager arrowManager;
        private CameraManager _cameraManager;
        private ResourceList resource = new ResourceList();
        private GameObject _myRotChangeObj;
        private string _destroyTime = "DestroyTime";
        //public bool generateArrow = true;
        [SerializeField] private PlayerInstance playerInstance;
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

        public async UniTask StartShooting(CancellationToken token,GameObject myRotChangeObj)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, this.GetCancellationTokenOnDestroy());
            //_myRotChangeObj = myRotObj;
            _cameraManager = FindObjectOfType<CameraManager>();//()内にtrueを入れると非アクティブも検索する(おー！)
            //CancellationTokenSource cancellationTokenSource=new CancellationTokenSource();
            //var token=cancellationTokenSource.Token;
            Debug.Log("ArrowStart");
            Debug.Log("myRotChangeObj: " + myRotChangeObj);
            _myRotChangeObj = myRotChangeObj;

            while (whileBool)
            {
                _cancelCause = CancelCause.CANCEL;

                //await WaitClick(_linkedToken.Token).Forget();エラー出る
                try
                {
                    Debug.Log("LoopIn");
                    await WaitClick(_linkedToken.Token);
                    Debug.Log("LoopOut");
                }
                catch (OperationCanceledException e)
                {
                    Debug.Log("error_Arrow: "+ e.Message);
                    if(_cancelCause == CancelCause.TIMEOVR|| _cancelCause == CancelCause.CANCEL)
                    {
                        break;
                    }
  
                }
                Debug.Log("tryroop");
            }
            Debug.Log("Finish!");
            targetManager.TargetOff();
            //token.ThrowIfCancellationRequested();

        }
        private async UniTask WaitClick(CancellationToken token)
        {
            _arrowObj = PhotonNetwork.Instantiate(resource.arrowObj, transform.position, transform.rotation);
            Debug.Log("_arrowObj+myRot: "+ _arrowObj+"+"+_myRotChangeObj);

            _arrowObj.transform.SetParent(_myRotChangeObj.transform);//一時的
            _arrowObj.transform.localRotation = Quaternion.Euler(85.95f, 0, 0);
            _arrowObj.transform.localPosition = new Vector3(-1f, 2.5f,0f);//一時的
            _arrowRb = _arrowObj.GetComponent<Rigidbody>();
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0),cancellationToken:token);
            //_cameraManager.CameraZoom();
            //アニメーション
            await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: token);
            var components= _arrowObj.GetComponents<MonoBehaviour>();
            foreach(var component in components)
            {
                Debug.Log(" getcomponent: " + component);
                var type = component.GetType();
                if (type == typeof(Targets) || type == typeof(Targets2))
                {
                    var setMethod = GetType().GetMethod(_destroyTime);
                    if (setMethod != null)
                    {
                        setMethod.Invoke(this, null);
                    }

                }
   
            }

            Debug.Log("click");

            _arrowRb.AddRelativeForce(new Vector3(0,-50f,0),ForceMode.VelocityChange);
            //arrowRb.AddRelativeForce(_arrowObj.transform*-10f,ForceMode.VelocityChange);
            Debug.Log("arrowparent1: "+ _arrowObj);
            _arrowObj.transform.parent = null;
            Debug.Log("arrowparent2: "+ _arrowObj);
            _arrowObj.transform.parent = null;
            //_cameraManager.CameraOut();
            //click = true;
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
            //token.ThrowIfCancellationRequested();
            return;
        }

        public void Pause()
        {
            Debug.Log("Pause");
            _cancelCause = CancelCause.CONTINUE;
            _linkedToken.Cancel();
        }

        public void TimeOver()
        {
            Debug.Log("TimeOver");
            whileBool = false;
            _cancelCause = CancelCause.TIMEOVR;
            _linkedToken.Cancel();
        }
    }
}
