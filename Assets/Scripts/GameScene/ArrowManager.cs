using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Photon.Pun;
using Photonmanager;
using System;
using System.Threading;
using UnityEngine;

namespace GameScene
{
    public class ArrowManager : MonoBehaviour
    {
        private Vector3 _arrowPos=new Vector3(-5.88f,3.9f,-0.57f);
        private Vector3 _arrowRot= new Vector3(-7.5f, 6.4f, -116.9f);
        private Vector3 _arrowScale= new Vector3(2f, 2f, 2f);
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
        private Transform _parent;
        private Transform _parent2;
        private string _hitTrigger = "hit";
        //public bool generateArrow = true;
        [SerializeField] private PlayerInstance playerInstance;
        //[SerializeField] private ArrowGenerater arrowGenerater;
        [SerializeField] private TargetManager targetManager;
        [SerializeField] private Animator _hitanimator;

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
            _parent = myRotChangeObj.transform.parent;
            _parent2 = _parent.transform.parent;


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
                    else if (_cancelCause == CancelCause.CONTINUE)
                    {
                        Debug.Log("aaaaaContinue");
                        _cancellationTokenSource = new CancellationTokenSource();
                        _linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, this.GetCancellationTokenOnDestroy());
                        _hitanimator.SetTrigger(_hitTrigger);
                        await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: _linkedToken.Token);

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

            _arrowObj.transform.SetParent(_myRotChangeObj.transform);
            //await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
            _arrowObj.transform.localScale = _arrowScale;
            //_arrowObj.transform.rotation = Quaternion.Euler(_arrowRot);
             _arrowObj.transform.localRotation = Quaternion.Euler(_arrowRot);
            _arrowObj.transform.localPosition = _arrowPos;
     
            Debug.Log("PosChange");
           
 
            
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0),cancellationToken:token);
   
            //アニメーション
            await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: token);
            

            Debug.Log("click");
            if (_arrowObj!=null)
            {
                _arrowRb = _arrowObj.GetComponent<Rigidbody>();
            }
            else
            {
                return;
            }
            
            //_arrowRb.AddRelativeForce(new Vector3(0,-50f,0),ForceMode.VelocityChange);
            _arrowRb.AddRelativeForce(new Vector3(0,0,-50f),ForceMode.VelocityChange);
            //arrowRb.AddRelativeForce(_arrowObj.transform*-10f,ForceMode.VelocityChange);
            Debug.Log("arrowparent1: "+ _arrowObj);
            _arrowObj.transform.parent = null;
            Debug.Log("arrowparent2: "+ _arrowObj);
            _arrowObj.transform.parent = null;
            _arrowObj = null;
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
            if (_arrowObj!=null)
            {
                PhotonNetwork.Destroy(_arrowObj.GetComponent<PhotonView>());//GetPhotonView使ってみた
            }
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
