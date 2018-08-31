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
	public float stunTime;

	public float HorizontalForceReducedEachHitInCombo;
	public float VerticalForceReducedEachHitInCombo;

	public int numberOfTimesTheHitWasDoneInCombo = 0;

    Fighter fighter;

    float playerFacing;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fighter == null)
        {
            fighter = animator.gameObject.GetComponent<Fighter>();
        }

		if (behaviourState == FighterState.ATTACK)
		{
			//reduce force if there was more hits like this in the combo
			//hitHorizontalForce += numberOfTimesTheHitWasDoneInCombo * HorizontalForceReducedEachHitInCombo; //more horizontalForce farther you launch the oponent and is more dificult to continue combo
			hitVerticalForce -= numberOfTimesTheHitWasDoneInCombo * VerticalForceReducedEachHitInCombo;

			HitBoxCollider.fighterState = this;
		}

        fighter.currentState = behaviourState;
		playerFacing = fighter.playerFacing;


		//Si és colpejat per qualsevol attack sigui launcher o no però es manté en l'aire
        if (behaviourState == FighterState.TAKE_HIT_AIR)
        {
            fighter.rb.AddRelativeForce(new Vector2(0, fighter.rb.velocity.y * -1));
			fighter.rb.AddRelativeForce(new Vector2(horizontalForce * playerFacing * (-1), 0));
        }       
			
		fighter.rb.AddRelativeForce(new Vector2(0, verticalForce));
		fighter.rb.AddRelativeForce(new Vector2(horizontalForce * playerFacing, 0));

			
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (behaviourState == FighterState.TAKE_HIT)
        {          
            fighter.transform.Translate(new Vector2(horizontalTransform * playerFacing, 0));
        }

        else 
        {

            //fighter.rb.AddRelativeForce(new Vector2(horizontalForce * playerFacing, 0));
            fighter.transform.Translate(new Vector2(horizontalTransform * playerFacing, 0));
        }


    }
}
