using System.Collections.Generic;
using UnityEngine;

public static class CommandHistory
{
    private class TurnHistory
    {
        public List<BaseCommand> commands = new List<BaseCommand>();      
    }

    private readonly static List<TurnHistory> turnHistories = new List<TurnHistory>();
    private static int currentTurn;
    private static TurnHistory currentRecordingTurn;

    private static bool HistorySynchronized => currentTurn == turnHistories.Count;

    static CommandHistory()
    {
        currentTurn = 0;

        turnHistories = new List<TurnHistory>();
        currentRecordingTurn = new TurnHistory();
    }

    public static void Save()
    {
        if(!HistorySynchronized)
        {
            turnHistories.RemoveRange(currentTurn + 1, turnHistories.Count - currentTurn);
        }

        turnHistories.Add(currentRecordingTurn);
        currentRecordingTurn = new TurnHistory();
        currentTurn++;
    }
  
    public static void AddCommand<T>(Command<T> command) where T : class
    {
        command.Execute();
        currentRecordingTurn.commands.Add(command);
    }

    public static void Undo()
    {
        if (currentTurn == 0)
            return;

        TurnHistory turnHistory = turnHistories[currentTurn - 1];
        foreach(BaseCommand command in turnHistory.commands)
        {
            command.Undo();
        }

        Debug.Log($"UNDOING Turn {currentTurn} of {turnHistories.Count}. Now on {--currentTurn}");
    }

    public static void Redo()
    {
        if (currentTurn + 1 > turnHistories.Count)
            return;

        TurnHistory turnHistory = turnHistories[currentTurn];
        foreach (BaseCommand command in turnHistory.commands)
        {
            command.Redo();
        }

        Debug.Log($"REDOING Turn {++currentTurn} of {turnHistories.Count}. Now on {currentTurn}");
    }
}
