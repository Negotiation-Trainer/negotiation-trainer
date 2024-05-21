using System;
using UnityEngine;

namespace Presenters
{
    public class HourglassPresenter : MonoBehaviour
    {
        public Action OnHourglassFinished;
        
        private float _lerpPercentage;

        #region Sand

        /* Top Sand */
        [SerializeField] private Transform sandTop;
        private const float SandTopStartY = 0;
        private const float SandTopTargetY = -745;

        /* Bottom Sand */
        [SerializeField] private Transform sandBottom;
        private const float SandBottomStartY = -1860;
        private const float SandBottomTargetY = -1050;

        /* Falling Sand */
        [SerializeField] private Transform fallingSand;
        private Vector3 _sandStartPos;
        private static readonly Vector3 SandInitialPos = new(0, -1000, 0);
        private const float FallingSandEndTarget = -2000;
        private const float FallingSandSpeed = 6f;
        private static readonly Vector3 FallingSandDirection = Vector3.down * FallingSandSpeed;

        #endregion

        /* Timer */
        public float duration = 3.0f; // Timer duration
        private float _elapsedTime;

        private void Start()
        {
            ResetHourglass();
        }

        private void ResetHourglass()
        {
            _elapsedTime = 0;
            _sandStartPos = SandInitialPos;

            fallingSand.gameObject.SetActive(true);
            fallingSand.localPosition = _sandStartPos;

            // Ensure the top and bottom sand positions are reset
            sandTop.localPosition = new Vector3(0, SandTopStartY, 0);
            sandBottom.localPosition = new Vector3(0, SandBottomStartY, 0);

            enabled = true; // Ensure the script is enabled to start the timer
        }

        private void OnEnable()
        {
            ResetHourglass();
        }

        private void OnDisable()
        {
            fallingSand.gameObject.SetActive(false);
            Debug.Log("Hourglass disabled");
        }

        private void Update()
        {
            // Increment elapsed time
            _elapsedTime += Time.deltaTime;

            LerpSand();
            MoveFallingSand();

            if (_elapsedTime >= duration)
            {
                TimerFinished();
            }
        }

        private void TimerFinished()
        {
            enabled = false;
            OnHourglassFinished?.Invoke();
        }

        private void LerpSand()
        {
            // Calculate the percentage of time elapsed
            _lerpPercentage = Mathf.Clamp01(_elapsedTime / duration);

            sandTop.localPosition = new Vector3(0, Mathf.Lerp(SandTopStartY, SandTopTargetY, _lerpPercentage), 0);
            sandBottom.localPosition = new Vector3(0, Mathf.Lerp(SandBottomStartY, SandBottomTargetY, _lerpPercentage), 0);
        }

        private void MoveFallingSand()
        {
            // Move the sand down over time
            fallingSand.localPosition += FallingSandDirection * Time.deltaTime;

            // Check if the falling sand has reached the bottom and reset it
            if (fallingSand.localPosition.y < FallingSandEndTarget)
            {
                fallingSand.localPosition = _sandStartPos;
            }
        }

        public void StartHourglass()
        {
            ResetHourglass(); // Reset and start the hourglass
        }
    }
}