public abstract class BaseCommand
{
    public abstract void Undo();
    public abstract void Redo();
}

public abstract class CommandRecord<T> : BaseCommand where T : class
{
    protected readonly T actor;

    protected CommandRecord(T actor)
    {
        this.actor = actor;
    }
}

// Create maximum number of steps recorded. Remove all other commands

