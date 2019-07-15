using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


namespace Com.Th1209.PunTutorial
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        #endregion


        #region Private Fields
        #endregion


        #region MonoBehaviour CallBacks
        #endregion


        #region Photon Callbacks

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        #endregion


        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion


        #region Public Methods

        public void LoadArena()
        {
            if (! PhotonNetwork.IsMasterClient) {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

            // Photonの機能をしてシーンを読み込む.
            // (同じルームに接続している全てのクライアントが同じシーンを読むようにするため.)
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks
        #endregion
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
