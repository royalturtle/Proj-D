using UnityEngine;

public class EffectBase
{
    protected GameObject Obj;
    public EffectBase(GameObject obj)
    {
        Obj = obj;
    }

    public virtual void Work() {}
}
