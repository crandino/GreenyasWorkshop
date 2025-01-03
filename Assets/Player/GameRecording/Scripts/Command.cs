using HexaLinks.Events.Arguments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public abstract class BaseCommand
{
    public abstract void Undo();
    public abstract void Redo();
}

public abstract class Command<T> : BaseCommand where T : class
{
    protected readonly T actor;

    protected Command(T actor)
    {
        this.actor = actor;
    }

    public abstract void Execute();
}

public class ModifyScoreCommand : Command<Score>
{
    private int scoreVariation = 0;

    public ModifyScoreCommand(Score score, int scoreVariation) : base(score)
    {
        this.scoreVariation = scoreVariation;
    }

    public override void Execute()
    {
        actor.Value += scoreVariation;
    }

    public override void Redo()
    {
        Execute();
    }

    public override void Undo()
    {
        actor.Value -= scoreVariation;
    }
}
