using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollider : MonoBehaviour {

    Fighter fighterOponent;

    public int comboCounter;
    public float comboDamageCounter = 0;

	public static FighterStateBehaviour fighterState;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Fighter>() != null)
        {         
            fighterOponent = collision.GetComponent<Fighter>();

            if (fighterOponent.currentState == FighterState.LAID_DOWN)
                return;

            if (fighterOponent.currentState == FighterState.DEFEND)
            {
                fighterOponent.animator.SetTrigger("Defend Hit");
                fighterOponent.life -= (fighterState.damage * 0.04f);
                return;
            }
			if (fighterOponent.currentState != FighterState.TAKE_HIT && fighterOponent.currentState != FighterState.TAKE_HIT_AIR) 
			{
				comboCounter = 0;
				comboDamageCounter = 0;
			}

			//L'oponent no està en el terra ni defensan-se
            fighterOponent.life -= fighterState.damage;

			//Combo incrementa cada Hit
			comboCounter++;
			comboDamageCounter += fighterState.damage;
			fighterState.numberOfTimesTheHitWasDoneInCombo++;

			Debug.Log ("Times" + fighterState.numberOfTimesTheHitWasDoneInCombo);

			Debug.Log ("Hits" + comboCounter);

			//L'oponent està en el aire
            if (fighterOponent.currentState == FighterState.TAKE_HIT_AIR)
            {
                
                fighterOponent.GetHurt("Air");


				fighterOponent.rb.AddRelativeForce(new Vector2(0, fighterState.hitVerticalForce - fighterOponent.rb.velocity.y * 30)); 
				fighterOponent.rb.AddRelativeForce(new Vector2(fighterState.hitHorizontalForce * fighterOponent.playerFacing * (-1), 0));

            }

			//L'oponent no està en l'aire
			else if (fighterOponent.currentState != FighterState.TAKE_HIT_AIR)
            {            
				//l'oponent està al terra i acaba de ser colpejat per un launcher attack
				if (fighterState.launcherAttack)
                {
                    fighterOponent.GetHurt("Air");
                    fighterOponent.rb.AddRelativeForce(new Vector2(0, fighterState.hitVerticalForce));
					fighterOponent.rb.AddRelativeForce(new Vector2(fighterState.hitHorizontalForce * fighterOponent.playerFacing * (-1), 0));


                }

				//L'oponent està al terra i no acaba de ser colpejat per un launcher attack
                else if (!fighterState.launcherAttack)
                {
                    fighterOponent.GetHurt("Floor");
					fighterOponent.stunTime = fighterState.stunTime;
                }
                
            }
        }
    }
}
