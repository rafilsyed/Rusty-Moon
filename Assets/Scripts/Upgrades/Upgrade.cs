using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public abstract Sprite Icon { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Level { get; }
    public abstract int Price { get; }

    public abstract void OnUpgrade();

    public virtual void reset()
    {
        
    }
}