using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base for all melee weapon behaviors
//meant to be placed on a prefab of a melee weapon

public class MeleeWeaponBehavior : MonoBehaviour
{

    public float destroyAfterSeconds;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

}