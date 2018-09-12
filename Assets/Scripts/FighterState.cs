using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterState
{
    IDLE, WALK, WALK_BACK, JUMP, CROUCH,
    ATTACK, TAKE_HIT, TAKE_HIT_AIR, TAKE_HIT_DEFEND,
    DEFEND, DEFEND_LOW, LAID_DOWN,
    CELEBRATE, DEAD, NONE
}

