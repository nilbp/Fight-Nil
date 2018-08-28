using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStateBehaviour : StateMachineBehaviour {

    public FighterState behaviourState;

    public float horizontalForce;
    public float verticalForce;
    public float horizontalTransform;
    public float verticalTransform;

    public float hitHorizontalForce;
    public float hitVerticalForce;


    public float damage;
    public bool launcherAttack;

    Fighter fighter;

    float playerFacing;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fighter == null)
        {
            fighter = animator.gameObject.GetComponent<Fighter>();
        }

        fighter.currentState = behaviourState;

        fighter.rb.AddRelativeForce(new Vector2(0, verticalForce));
        fighter.transform.Translate(new Vector2(0, verticalTransform));

        if (behaviourState == FighterState.ATTACK)
        {
            HitBoxCollider.damage = damage;
            HitBoxCollider.launcherAttack = launcherAttack;
            HitBoxCollider.horizontalForce = hitHorizontalForce;
            HitBoxCollider.verticalForce = hitVerticalForce;
        }

        playerFacing = fighter.playerFacing;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (behaviourState == FighterState.TAKE_HIT)
        {
            fighter.rb.AddRelativeForce(new Vector2(horizontalForce * playerFacing, 0));
            fighter.transform.Translate(new Vector2(horizontalTransform * playerFacing, 0));
        }

        else 
        {

            fighter.rb.AddRelativeForce(new Vector2(horizontalForce * playerFacing, 0));
            fighter.transform.Translate(new Vector2(horizontalTransform * playerFacing, 0));
        }


    }
}
