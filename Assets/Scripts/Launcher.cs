using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Th1209.PunTutorial
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        #endregion


        #region Private Fields

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel = null;
        
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel = null;

        /// <summary>
        /// ゲームバージョン毎に､接続は切り分けられる.
        /// </summary>
        string gameVersion = "1";

        /// <summary>
        /// 明示的にゲームサーバへの接続を試みていることを示すフラグ.
        /// </summary>
        bool isConnecting = false;

        #endregion


        #region MonoBehaviour CallBacks

        void Awake()
        {
            // MasterClientがPhotonNetwork.LoadLevel()を呼び出すと､
            // 接続済みのプレイヤーは､全員自動的に同じレベルを読み込んでくれる.
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            Debug.Assert(controlPanel  != null);
            Debug.Assert(progressLabel != null);

            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion


        #region Public Methods

        public void Connect()
        {
            isConnecting = true;

            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected) {
                // ランダムにルームへの参加を試みる.
                // 失敗した場合は､OnJoinRandomFailed()が呼び出される.
                PhotonNetwork.JoinRandomRoom();
            } else {
                // 設定値を用いて､PhotonCloudへ接続する.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

            if (isConnecting) {
                // isConnectingフラグをチェックして､ゲームサーバに接続する.
                // ※ ゲームサーバからの接続が解除された時､OnConnectToMaster()メソッドがコールされてしまう.
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            // ルーム名と設定値を指定して､ルームを作成する.
            PhotonNetwork.CreateRoom(null, new RoomOptions{
                MaxPlayers = maxPlayersPerRoom
            });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
                Debug.Log("We load the 'Room for 1' ");

                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        #endregion
    }
}
