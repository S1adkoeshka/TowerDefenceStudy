using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    public enum DamageType:byte
    {
        neutral = 0,
        fire = 1,
        frost = 2,
        shock = 3,
        nature = 4,
        toxic = 5,
        pure = 6
    }

    public enum AttackType : byte
    {
        target = 0,
        aoe = 1,
        air = 2
    }


}

