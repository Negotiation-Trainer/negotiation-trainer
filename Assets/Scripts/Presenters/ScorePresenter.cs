using Models;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Presenters
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField] private Transform scoreCard;
        [SerializeField] private Transform scoreCardTarget;
        [SerializeField] private float speed;
        private Vector3 _originalPosition;
        private Vector3 _targetPosition;
        private bool _transitioning = false;
        
        
        // Start is called before the first frame update
        void Start()
        {
            _originalPosition = scoreCard.position;
            _targetPosition = _originalPosition;        
        }
        
        public void ToggleScoreCard()
        {
            if(_transitioning) return;
            if (_targetPosition.Compare(_originalPosition, 100))
            {
                _targetPosition = scoreCardTarget.position;
                _transitioning = true;
            }
            else
            {
                _targetPosition = _originalPosition;
                _transitioning = true;
            }
        }

        private void FixedUpdate()
        {
            if (_transitioning)
            {
                scoreCard.position = Vector3.MoveTowards(scoreCard.position, _targetPosition, speed);
                if (scoreCard.position.Compare(_targetPosition, 100)) _transitioning = false;
            }
        }
    }
}
