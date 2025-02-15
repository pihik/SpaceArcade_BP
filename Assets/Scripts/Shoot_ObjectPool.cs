using System.Collections.Generic;
using UnityEngine;

public class Shoot_ObjectPool : ObjectPool
{
    protected override void AdditionalInstantiation(GameObject obj)
    {
        Projectile projectileComponent = obj.GetComponent<Projectile>();
        projectileComponent.SetObjectPool(this);
        projectileComponent.SetInstigator(transform.parent.gameObject);
    }

    protected override Transform GetPoolStorage()
    {
        return InGameHelper.instance.GetProjectilesParent();
    }
}
