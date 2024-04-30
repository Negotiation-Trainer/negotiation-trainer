using ModelLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text woodA;
        [SerializeField] private TMP_Text woodB;
        [SerializeField] private TMP_Text woodC;
        
        [SerializeField] private TMP_Text insulationA;
        [SerializeField] private TMP_Text insulationB;
        [SerializeField] private TMP_Text insulationC;
        
        [SerializeField] private TMP_Text stoneA;
        [SerializeField] private TMP_Text stoneB;
        [SerializeField] private TMP_Text stoneC;
        
        [SerializeField] private TMP_Text fertilizerA;
        [SerializeField] private TMP_Text fertilizerB;
        [SerializeField] private TMP_Text fertilizerC;
        
        [SerializeField] private TMP_Text lensesA;
        [SerializeField] private TMP_Text lensesB;
        [SerializeField] private TMP_Text lensesC;
        
        [SerializeField] private TMP_Text clayA;
        [SerializeField] private TMP_Text clayB;
        [SerializeField] private TMP_Text clayC;
        
        [SerializeField] private TMP_Text goldA;
        [SerializeField] private TMP_Text goldB;
        [SerializeField] private TMP_Text goldC;
        
        [SerializeField] private TMP_Text steelA;
        [SerializeField] private TMP_Text steelB;
        [SerializeField] private TMP_Text steelC;
        
        [SerializeField] private TMP_Text woodScore;
        [SerializeField] private TMP_Text insulationScore;
        [SerializeField] private TMP_Text stoneScore;
        [SerializeField] private TMP_Text fertilizerScore;
        [SerializeField] private TMP_Text lensesScore;
        [SerializeField] private TMP_Text clayScore;
        [SerializeField] private TMP_Text goldScore;
        [SerializeField] private TMP_Text steelScore;
        
        [SerializeField] private Image woodHammer;
        [SerializeField] private Image insulationHammer;
        [SerializeField] private Image stoneHammer;
        [SerializeField] private Image lensesHammer;
        [SerializeField] private Image clayHammer;
        [SerializeField] private Image goldHammer;
        [SerializeField] private Image steelHammer;
        
        [SerializeField] private Transform scoreCard;
        [SerializeField] private Transform scoreCardTarget;
        [SerializeField] private float speed;
        private Vector3 _originalPosition;
        private Vector3 _targetPosition;
        
        private GameManager _gameManager;
        private Tribe _player;
        private bool _transitioning = false;
        
        
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
            _player = _gameManager.Player;
            
            FillScoreCard();
            
            _originalPosition = scoreCard.localPosition;
            _targetPosition = _originalPosition;        
        }

        private void FillScoreCard()
        {
            Tribe tribeA = _gameManager.Player;
            Tribe tribeB = _gameManager.Cpu1;
            Tribe tribeC = _gameManager.Cpu2;

            woodA.text = _player.PointTable[(InventoryItems.Wood, tribeA)].ToString();
            woodB.text = _player.PointTable[(InventoryItems.Wood, tribeB)].ToString();
            woodC.text = _player.PointTable[(InventoryItems.Wood, tribeC)].ToString();
            
            insulationA.text = _player.PointTable[(InventoryItems.Insulation, tribeA)].ToString();
            insulationB.text = _player.PointTable[(InventoryItems.Insulation, tribeB)].ToString();
            insulationC.text = _player.PointTable[(InventoryItems.Insulation, tribeC)].ToString();
            
            stoneA.text = _player.PointTable[(InventoryItems.Stone, tribeA)].ToString();
            stoneB.text = _player.PointTable[(InventoryItems.Stone, tribeB)].ToString();
            stoneC.text = _player.PointTable[(InventoryItems.Stone, tribeC)].ToString();
            
            fertilizerA.text = _player.PointTable[(InventoryItems.Fertilizer, tribeA)].ToString();
            fertilizerB.text = _player.PointTable[(InventoryItems.Fertilizer, tribeB)].ToString();
            fertilizerC.text = _player.PointTable[(InventoryItems.Fertilizer, tribeC)].ToString();
            
            lensesA.text = _player.PointTable[(InventoryItems.Lenses, tribeA)].ToString();
            lensesB.text = _player.PointTable[(InventoryItems.Lenses, tribeB)].ToString();
            lensesC.text = _player.PointTable[(InventoryItems.Lenses, tribeC)].ToString();
            
            clayA.text = _player.PointTable[(InventoryItems.Clay, tribeA)].ToString();
            clayB.text = _player.PointTable[(InventoryItems.Clay, tribeB)].ToString();
            clayC.text = _player.PointTable[(InventoryItems.Clay, tribeC)].ToString();
            
            goldA.text = _player.PointTable[(InventoryItems.Gold, tribeA)].ToString();
            goldB.text = _player.PointTable[(InventoryItems.Gold, tribeB)].ToString();
            goldC.text = _player.PointTable[(InventoryItems.Gold, tribeC)].ToString();
            
            steelA.text = _player.PointTable[(InventoryItems.Steel, tribeA)].ToString();
            steelB.text = _player.PointTable[(InventoryItems.Steel, tribeB)].ToString();
            steelC.text = _player.PointTable[(InventoryItems.Steel, tribeC)].ToString();
        }
        
        public void ShowScoreCard(bool isActive)
        {
            scoreCard.gameObject.SetActive(isActive);
        }
        
        public void ToggleScoreCard()
        {
            if(_transitioning) return;
            if (_targetPosition.Compare(_originalPosition, 100))
            {
                _targetPosition = _originalPosition - new Vector3(0,693,0);
                _transitioning = true;
            }
            else
            {
                _targetPosition = _originalPosition;
                _transitioning = true;
            }
        }

        public void UpdateScoreCard(Tribe tribe, InventoryItems resourceBuildingBuild)
        {
            Sprite hammer;
            if (tribe == _gameManager.Cpu1)
            {
                hammer = Resources.Load<Sprite>( "UI/Icons/Green_Hammer_Icon" );
            }
            else if (tribe == _gameManager.Cpu2)
            {
                hammer = Resources.Load<Sprite>( "UI/Icons/Red_Hammer_Icon" );
            }
            else
            {
                hammer = Resources.Load<Sprite>( "UI/Icons/Yellow_Hammer_Icon" );
            }
            
            switch (resourceBuildingBuild)
            {
                case InventoryItems.Wood:
                    woodScore.gameObject.SetActive(true);
                    woodScore.text = _player.PointTable[(InventoryItems.Wood, tribe)].ToString();
                    woodHammer.gameObject.SetActive(true);
                    woodHammer.sprite = hammer;
                    break;
                case InventoryItems.Insulation:
                    insulationScore.gameObject.SetActive(true);
                    insulationScore.text = _player.PointTable[(InventoryItems.Insulation, tribe)].ToString();
                    insulationHammer.gameObject.SetActive(true);
                    insulationHammer.sprite = hammer;
                    break;
                case InventoryItems.Stone:
                    stoneScore.gameObject.SetActive(true);
                    stoneScore.text = _player.PointTable[(InventoryItems.Stone, tribe)].ToString();
                    stoneHammer.gameObject.SetActive(true);
                    stoneHammer.sprite = hammer;
                    break;
                case InventoryItems.Lenses:
                    lensesScore.gameObject.SetActive(true);
                    lensesScore.text = _player.PointTable[(InventoryItems.Lenses, tribe)].ToString();
                    lensesHammer.gameObject.SetActive(true);
                    lensesHammer.sprite = hammer;
                    break;
                case InventoryItems.Clay:
                    clayScore.gameObject.SetActive(true);
                    clayScore.text = _player.PointTable[(InventoryItems.Clay, tribe)].ToString();
                    clayHammer.gameObject.SetActive(true);
                    clayHammer.sprite = hammer;
                    break;
                case InventoryItems.Gold:
                    goldScore.gameObject.SetActive(true);
                    goldScore.text = _player.PointTable[(InventoryItems.Gold, tribe)].ToString();
                    goldHammer.gameObject.SetActive(true);
                    goldHammer.sprite = hammer;
                    break;
                case InventoryItems.Steel:
                    steelScore.gameObject.SetActive(true);
                    steelScore.text = _player.PointTable[(InventoryItems.Steel, tribe)].ToString();
                    steelHammer.gameObject.SetActive(true);
                    steelHammer.sprite = hammer;
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (_transitioning)
            {
                scoreCard.localPosition = Vector3.MoveTowards(scoreCard.localPosition, _targetPosition, speed);
                if (scoreCard.localPosition.Compare(_targetPosition, 100)) _transitioning = false;
            }
        }
    }
}
