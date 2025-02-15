using System.Collections.Generic;
using UnityEngine;

public class Asteroid_ObjectPool : ObjectPool
{
    protected override void AdditionalInstantiation(GameObject obj)
    {
        Asteroid asteroidComponent = obj.GetComponent<Asteroid>();
        asteroidComponent.SetObjectPool(this);
    }

    protected override Transform GetPoolStorage()
    {
        return InGameHelper.instance.GetAsteroidsParent();
    }
}
