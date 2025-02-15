using UnityEngine;

public class EnemySpawner : Spawner
{
    protected override void Start()
    {
        base.Start();
        GameManager.instance.SetNumberOfEnemies(wantedNumberOfEnemies);
    }
}
