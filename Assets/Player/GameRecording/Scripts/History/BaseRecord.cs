#if RECORDING
public abstract class BaseRecord
{
    public abstract void Undo();
    public abstract void Redo();

    public virtual void OnRemove() { }
}

public abstract class Record<T> : BaseRecord where T : class
{
    protected readonly T actor;

    protected Record(T actor)
    {
        this.actor = actor;
    }
} 
#endif

