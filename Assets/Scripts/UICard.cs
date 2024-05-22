using UnityEngine;

public class UICard : MonoBehaviour
{
    [SerializeField] private Vector2 distanceToMove;
    [SerializeField] private float duration;
    private Vector2 _topPosition;
    private Vector2 _bottomPosition;
    private bool _movingDown = false;

    private void Awake()
    {
        _topPosition = transform.localPosition;
        _bottomPosition = _topPosition - distanceToMove;
    }

    private void Start()
    {
        _topPosition = transform.localPosition;
        _bottomPosition = _topPosition - distanceToMove;
    }

    public void Move()
    {
        if (!_movingDown)
        {
            transform.LeanMoveLocal(_bottomPosition,duration).setEaseInOutQuart();
            _movingDown = true;
        }
        else
        {
            transform.LeanMoveLocal(_topPosition,duration).setEaseInOutQuart();
            _movingDown = false;
        }
        
    }
}
