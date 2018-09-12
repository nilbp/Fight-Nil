using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStateBehaviour : StateMachineBehaviour {

    public enum HitType {HIGH, MID, LOW };

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
    public float defenseStun;

    public HitType hitType;
    
    public AudioClip soundEffect;

    //public float HorizontalForceReducedEachHitInCombo;
    public float VerticalForceReducedEachHitInCombo;

    public int comboID;

    Fighter fighter;

    float playerFacing;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fighter == null)
        {
            fighter = animator.gameObject.GetComponent<Fighter>();
        }

        if (behaviourState == FighterState.ATTACK || behaviourState == FighterState.TAKE_HIT_AIR
            || behaviourState == FighterState.TAKE_HIT || behaviourState == FighterState.TAKE_HIT_DEFEND)
        {
            HitBoxCollider.fighterState = this;
            fighter.animator.SetBool("Cancel", false);
        }
        else
        {
            fighter.animator.SetBool("Cancel", true);
        }

        fighter.currentState = behaviourState;
		playerFacing = fighter.playerFacing;

        if (soundEffect != null)
        {
            fighter.PlaySound(soundEffect);
        }

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
