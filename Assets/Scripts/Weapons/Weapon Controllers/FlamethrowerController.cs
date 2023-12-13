using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedBullet = Instantiate(weaponData.Prefab);
        spawnedBullet.transform.position = transform.position;
        spawnedBullet.GetComponent<FlamethrowerBehavior>().DirectionChecker(pm.lastMovedDirVector);
    }
}