using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//heart increases player's health by 50...
public class HeartPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentHealth += 50;
    }
}


