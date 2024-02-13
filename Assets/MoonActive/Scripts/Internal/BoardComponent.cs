using UnityEngine;

namespace MoonActive.Scripts
{
    public class BoardComponent : MonoBehaviour
    {
        [SerializeField] private BoardView _boardView;
        [SerializeField] private BoardEvents _boardEvents;

        private GameBoardLogic _gameBoardLogic;

        public void Awake()
        {
            _boardView.Initialize(GetComponentsInChildren<BoardTileView>());
            
            _gameBoardLogic = new GameBoardLogic(_boardView, _boardEvents);
            _gameBoardLogic.Initialize(3, 3);
        }
        
        private void OnDestroy()
        {
            _gameBoardLogic.DeInitialize();
        }
        
        public void OnStartGameClicked()
        {
            _boardEvents.OnStartGameClicked();
        }
    
        public void OnTileClicked(BoardTileView tileView)
        {
            _boardEvents.OnTileClicked(tileView);
        }
    
        public void OnSaveGameStateClicked()
        {
            _boardEvents.OnSaveGameStateClicked();
        }
    
        public void OnLoadGameStateClicked()
        {
            _boardEvents.OnLoadGameStateClicked();
        }
    }
}