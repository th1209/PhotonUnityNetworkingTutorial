using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Th1209.PunTutorial
{
    [RequireComponent(typeof(Animator))]
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// ビーム用オブジェクト.
        /// </summary>
        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;

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
            Debug.Assert(beams != null);
            beams.SetActive(false);
        }

        void Update()
        {
            ProcessInputs();

            if (isFiring != beams.activeSelf) {
                // ビームのアクティブを切り替える.
                beams.SetActive(isFiring);
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
                // if (isFiring) {
                //     isFiring = false;
                // }
            }
        }

        #endregion
    }
}
