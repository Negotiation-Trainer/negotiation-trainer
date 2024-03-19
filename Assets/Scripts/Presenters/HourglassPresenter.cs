using UnityEngine;

namespace Presenters
{
    public class HourglassPresenter : MonoBehaviour
    {
        private float _lerpPercentage;

        #region Sand

        /* Top Sand */
        [SerializeField] private Transform sandTop;
        private float _sandTopStartY;
        private const float SandTopTargetY = -745;

        /* Bottom Sand */
        [SerializeField] private Transform sandBottom;
        private float _sandBottomStartY;
        private const float SandBottomTargetY = -1050;


        /* Falling Sand */
        [SerializeField] private Transform fallingSand;
        private Vector3 _sandStartPos;
        private const float FallingSandEndTarget = -2000;
        private const float FallingSandSpeed = 6f;
        
        #endregion

        /* Timer */
        public float duration = 3.0f; // Timer duration
        private float _elapsedTime;

        private void Start()
        {
            _elapsedTime = 0;
            _sandTopStartY = sandTop.transform.localPosition.y;
            _sandBottomStartY = sandBottom.transform.localPosition.y;

            _sandStartPos = new Vector3(0, -1000, 0);
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
            fallingSand.gameObject.SetActive(false);
            enabled = false;
        }

        private void LerpSand()
        {
            // Calculate the percentage of time elapsed
            _lerpPercentage = Mathf.Clamp01(_elapsedTime / duration);

            sandTop.localPosition = new Vector3(0, Mathf.Lerp(_sandTopStartY, SandTopTargetY, _lerpPercentage), 0);
            sandBottom.localPosition =
                new Vector3(0, Mathf.Lerp(_sandBottomStartY, SandBottomTargetY, _lerpPercentage), 0);
        }

        private void MoveFallingSand()
        {
            //move the sand down over time
            fallingSand.localPosition -= new Vector3(0, FallingSandSpeed, 0);

            //check if the falling sand has reached the bottom and reset it
            if (fallingSand.localPosition.y < FallingSandEndTarget) fallingSand.localPosition = _sandStartPos;
        }
    }
}