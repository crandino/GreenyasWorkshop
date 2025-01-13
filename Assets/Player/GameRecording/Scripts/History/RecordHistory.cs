using System.Collections.Generic;
using UnityEngine;

public class RecordHistory
{
    private class TurnHistory
    {
        public List<BaseRecord> commands = new List<BaseRecord>();  
        
        public void Remove()
        {
            foreach (BaseRecord command in commands)
                command.OnRemove();
        }
    }

    private readonly List<TurnHistory> turnHistories = new List<TurnHistory>();
    private int currentTurn;
    private TurnHistory currentRecordingTurn;

    private bool HistorySynchronized => currentTurn == turnHistories.Count;

    public bool MovingThroughHistoryAllowed => turnHistories.Count != 0 && currentRecordingTurn.commands.Count == 0;
    public bool UndoIsPossible => currentTurn > 0;
    public bool RedoIsPossible => currentTurn < turnHistories.Count;

    public RecordHistory()
    {
        currentTurn = 0;

        turnHistories = new List<TurnHistory>();
        currentRecordingTurn = new TurnHistory();
    }

    public void Save()
    {
        if(!HistorySynchronized)
        {
            List<TurnHistory> historiesToRemove = turnHistories.GetRange(currentTurn, turnHistories.Count - currentTurn);
            foreach (TurnHistory history in historiesToRemove)
                history.Remove();
            turnHistories.RemoveRange(currentTurn, turnHistories.Count - currentTurn);
        }

        turnHistories.Add(currentRecordingTurn);
        currentRecordingTurn = new TurnHistory();
        currentTurn++;
    }
  
    public void RecordCommand<T>(Record<T> command) where T : class
    {
        currentRecordingTurn.commands.Add(command);
    }

    public void Undo()
    {
        if (!UndoIsPossible)
            return;

        TurnHistory turnHistory = turnHistories[currentTurn - 1];
        foreach(BaseRecord command in turnHistory.commands)
        {
            command.Undo();
        }

        Debug.Log($"UNDOING Turn {currentTurn} of {turnHistories.Count}. Now on {--currentTurn}");
    }

    public void Redo()
    {
        if (!RedoIsPossible)
            return;

        TurnHistory turnHistory = turnHistories[currentTurn];
        foreach (BaseRecord command in turnHistory.commands)
        {
            command.Redo();
        }

        Debug.Log($"REDOING Turn {++currentTurn} of {turnHistories.Count}. Now on {currentTurn}");
    }
}
