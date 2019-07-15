using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Th1209.PunTutorial
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Serializable Fields

        [SerializeField]
        /// <summary>
        /// 方向転換を完了するまでの時間.
        /// ※ 値を大きくするほど1回りするまでに時間がかかるため､回転の半径が大きくなる.
        /// </summary>
        private float directionDampTime = 0.25f;

        #endregion


        #region Private Fields

        private Animator animator;

        #endregion


        #region MonoBehaviour CallBacks

        void Start()
        {
            animator = GetComponent<Animator>();
            Debug.Assert(animator != null);
        }

        void Update()
        {
            if (! animator) {
                return;
            }

            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
                return;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Base Layer.Run")) {
                // 走っている場合だけ､ジャンプ可能にする.
                if (Input.GetButtonDown("Fire2")) {
                    animator.SetTrigger("Jump");
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0.0f) {
                v = 0.0f;
            }
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        #endregion
    }
}
