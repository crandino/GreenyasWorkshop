using UnityEngine.InputSystem;

namespace HexaLinks.Turn
{
    public partial class TurnManager
    {
#if UNITY_EDITOR && DEBUG
        public RecordHistory History { private set; get; } = new RecordHistory();

        private void Update()
        {
            if (!History.MovingThroughHistoryAllowed)
                return;

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame && History.UndoIsPossible)
            {
                steps.FinalizeTurn(true);
                History.Undo();
                ChangePlayer();
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame && History.RedoIsPossible)
            {
                steps.FinalizeTurn(true);
                History.Redo();
                ChangePlayer();
            }
        }
#endif
    } 
}
