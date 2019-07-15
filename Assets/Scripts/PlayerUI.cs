using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Com.Th1209.PunTutorial
{
    public class PlayerUI: MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider playerHealthSlider;

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0.0f, 30.0f, 0.0f);

        #endregion


        #region Public Fields

        #endregion


        #region Private Fields

        PlayerManager target;

        float characterControllerHeight = 0.0f;

        Transform targetTransform;

        Vector3 targetPosition;

        #endregion


        #region MonoBehaviour CallBacks

        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void Start()
        {
            Debug.Assert(playerNameText != null);
            Debug.Assert(playerHealthSlider != null);
        }

        void Update()
        {
            if (playerHealthSlider != null) {
                playerHealthSlider.value = target.Health;
            }

            if (target == null) {
                // プレイヤーが破棄された時点で､このインスタンスも破棄.
                Destroy(this.gameObject);
                return;
            }
        }

        void LateUpdate()
        {
            if (targetTransform == null) {
                return;
            }

            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }

        #endregion


        #region Public Methods

        public void SetTarget(PlayerManager _target)
        {
            if (_target == null) {
                Debug.Assert(false);
            }

            target = _target;
            playerNameText.text = target.photonView.Owner.NickName;

            CharacterController _characterController = _target.GetComponent<CharacterController>();
            Debug.Assert(_characterController != null);
            targetTransform = _target.transform;
            characterControllerHeight = _characterController.height;
        }

        #endregion
    }
}
