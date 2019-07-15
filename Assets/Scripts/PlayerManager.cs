using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Th1209.PunTutorial
{
    // サンプルのCameraWorkクラスを利用する.
    using Photon.Pun.Demo.PunBasics;

    [RequireComponent(typeof(Animator))]
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Serializable Fields

        /// <summary>
        /// ビーム用オブジェクト.
        /// </summary>
        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        private GameObject playerUiPrefab;

        #endregion


        #region Public Fields

        /// <summary>
        /// HP.
        /// </summary>
        [Tooltip("The current Health of our player")]
        public float Health = 1.0f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion


        #region Private Fields

        /// <summary>
        /// 攻撃中フラグ.
        /// </summary>
        private bool isFiring = false;

        #endregion


        #region MonoBehaviour CallBacks

        void Awake()
        {
            if(photonView.IsMine) {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }

            // 人数変更によるシーン遷移時に破棄されないようにする.
            DontDestroyOnLoad(this.gameObject);

            Debug.Assert(beams != null);
            beams.SetActive(false);
        }

        void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
            Debug.Assert(_cameraWork != null);
            if (photonView.IsMine) {
                _cameraWork.OnStartFollowing();
            }

            Debug.Assert(playerUiPrefab != null);
            GameObject _uiGo = Instantiate(playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) => {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            };
        }

        void Update()
        {
            if (photonView.IsMine) {
                ProcessInputs();
            }

            if (isFiring != beams.activeSelf) {
                // ビームのアクティブを切り替える.
                beams.SetActive(isFiring);
            }

            if (Health <= 0.0f) {
                GameManager.Instance.LeaveRoom();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (! photonView.IsMine) {
                return;
            }

            if (! other.name.Contains("Beam")) {
                return;
            }
            Health -= 0.1f;
        }

        void OnTriggerStay(Collider other)
        {
            if (! photonView.IsMine) {
                return;
            }

            if (! other.name.Contains("Beam")) {
                return;
            }
            Health -= 0.1f * Time.deltaTime;
        }

        void CalledOnLevelWasLoaded(int level)
        {
            // UIの再生成.
            GameObject _uiGo = Instantiate(playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

            if (! Physics.Raycast(transform.position, -Vector3.up, 5.0f)) {
                // 人数が減少し､より小さいステージになった場合において､
                // ステージの範囲外にいれば､中央に位置を変更する.
                transform.position = new Vector3(0.0f, 5.0f, 0.0f);
            }
        }

        #endregion


        #region IPunObservable implementation

        /// <summary>
        /// Photonのデータ同期時に呼ばれる.
        /// このメソッドを実装するだけでは不十分で､このコンポーネントをPhotonViewの監視対象に追加する必要がある点にも注意.
        /// </summary>
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting) {
                // 自分のデータを､他のクライアントに渡す.
                // 自クライアントのインスタンスの場合にのみ､書き出しが行われる仕様である点に注目.
                // そのため､他クライアントのインスタンスの値も､書き出してしまう心配はしなくていい.
                stream.SendNext(isFiring);
                stream.SendNext(Health);
            } else {
                // 他クライアントのデータを受け取る.
                // object型が返るので､明示的なキャストが必要な点に注意.
                this.isFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }

        #endregion


        #region Custom

        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1")){
                if (! isFiring) {
                    isFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire1")){
                isFiring = false;
            }
        }

        #endregion
    }
}
