using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


namespace Com.Th1209.PunTutorial
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        const string playerNamePrefKey = "PlayerName";

        #endregion


        #region MonoBehaviour CallBacks

        void Start()
        {
            Debug.Assert(this.GetComponent<InputField>() != null);

            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();
            if (_inputField != null) {
                if (PlayerPrefs.HasKey(playerNamePrefKey)) {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion


        #region Public Methods

        public void SetPlayerName(string value)
        {
                if (string.IsNullOrEmpty(value)) {
                    Debug.Log("Player Name is null or empty.");
                    return;
                }

                Debug.LogFormat("Set Name {0}", value);
                PlayerPrefs.SetString(playerNamePrefKey, value);
                PhotonNetwork.NickName = value;
        }

        #endregion
    }
}
