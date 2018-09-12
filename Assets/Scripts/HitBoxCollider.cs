using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollider : MonoBehaviour {

    Fighter fighterOponent;
    public Fighter fighter;

    public int comboCounter;
    public float comboDamageCounter = 0;

    private float forceModifier;
    private int numberOfHits = 0;

    public List<int> comboHitData;

    //Hit particles
    public GameObject hitParticles;
    public GameObject defendHitParticles;

    //Sound hit effect
    public AudioClip soundEffect;
    public AudioClip defendHitSoundEffect;

    public Collider2D selfCollider;

    private float realDamage;

	public static FighterStateBehaviour fighterState;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Fighter>() != null)
        {         
            fighterOponent = collision.GetComponent<Fighter>();

            float yOffset = selfCollider.transform.position.y - collision.transform.position.y;

            if (yOffset > 0.3)
                yOffset = 0.3f;
            else if (yOffset < -0.2)
                yOffset = 0.2f;

            if (fighterOponent.currentState == FighterState.LAID_DOWN)
                return;

            if (fighterOponent.currentState == FighterState.DEFEND
                || fighterOponent.currentState == FighterState.TAKE_HIT_DEFEND)
            {
                if (fighterState.hitType == FighterStateBehaviour.HitType.HIGH || fighterState.hitType == FighterStateBehaviour.HitType.MID)
                {
                    DefendHit(collision, yOffset);
                    return;
                }
            }

            else if (fighterOponent.currentState == FighterState.DEFEND_LOW)
            {
                if (fighterState.hitType == FighterStateBehaviour.HitType.MID || fighterState.hitType == FighterStateBehaviour.HitType.LOW)
                {
                    DefendHit(collision, yOffset);
                    return;
                }
            }

			if (fighterOponent.currentState != FighterState.TAKE_HIT && fighterOponent.currentState != FighterState.TAKE_HIT_AIR) 
			{
				comboCounter = 0;
				comboDamageCounter = 0;
                comboHitData.Clear();
			}

            //reduce force of the hit if there was more hits like this in the combo
            foreach (int a in comboHitData)
            {
                if (a == fighterState.comboID)
                    numberOfHits++;
            }

            realDamage = fighterState.damage - (numberOfHits * 1.2f);

            if (realDamage < 1)
                realDamage = 1;

            //L'oponent no està en el terra ni defensan-se
            fighterOponent.life -= realDamage;

            //Combo incrementa cada Hit
            comboCounter++;
			comboDamageCounter += realDamage ;
            comboHitData.Add(fighterState.comboID);

            //Instantiate particles
            GameObject particles =  Instantiate(hitParticles, collision.transform.position + new Vector3(fighterOponent.playerFacing * 0.12f, yOffset, -1), Quaternion.identity);
            particles.transform.SetParent(fighterOponent.transform);

            //Sound Effect
            if (soundEffect != null)
            {
                /*int a = Random.Range(0, 2);
                if (a == 0)
                    fighter.PlaySound(soundEffect);
                else if(a == 1)
                    fighter.PlaySound(soundEffect2);
                else
                    fighter.PlaySound(soundEffect3);*/

                fighterOponent.PlaySound(soundEffect);

            }

            //L'oponent està en el aire
            if (fighterOponent.currentState == FighterState.TAKE_HIT_AIR )
            {
                
                fighterOponent.GetHurt("Air");


                fighterOponent.rb.AddRelativeForce(new Vector2(0, fighterState.hitVerticalForce - (fighterOponent.rb.velocity.y * 30) - (numberOfHits * fighterState.VerticalForceReducedEachHitInCombo))); 
				fighterOponent.rb.AddRelativeForce(new Vector2(fighterState.hitHorizontalForce * fighterOponent.playerFacing * (-1), 0));

            }

            //Si colpejes l'oponent a l'aire però no està Hurt
            else if (!fighterOponent.animator.GetBool("IsGrounded"))
            {

                fighterOponent.GetHurt("Air");


                fighterOponent.rb.AddRelativeForce(new Vector2(0, fighterState.hitVerticalForce - (fighterOponent.rb.velocity.y * 30) - (numberOfHits * fighterState.VerticalForceReducedEachHitInCombo)));
                fighterOponent.rb.AddRelativeForce(new Vector2((fighterState.hitHorizontalForce + 100) * fighterOponent.playerFacing * (-1) , 0));

            }

            //L'oponent no està en l'aire
            else if (fighterOponent.currentState != FighterState.TAKE_HIT_AIR)
            {
                //l'oponent està al terra i acaba de ser colpejat per un launcher attack
                if (fighterState.launcherAttack)
                {
                    fighterOponent.GetHurt("Air");
                    fighterOponent.rb.AddRelativeForce(new Vector2(0, fighterState.hitVerticalForce - (numberOfHits * fighterState.VerticalForceReducedEachHitInCombo)));
                    fighterOponent.rb.AddRelativeForce(new Vector2(fighterState.hitHorizontalForce * fighterOponent.playerFacing * (-1), 0));


                }

                //L'oponent està al terra i no acaba de ser colpejat per un launcher attack
                else if (!fighterState.launcherAttack)
                {
                    fighterOponent.GetHurt("Floor");
                    fighterOponent.stunTime = fighterState.stunTime;
                }

            }
            //Reset hit variable
            numberOfHits = 0;
        }
    }

    void DefendHit(Collider2D collision, float yOffset)
    {
        fighterOponent.animator.SetTrigger("Defend Hit");
        fighterOponent.defenseRecoverTime = fighterState.defenseStun;
        fighterOponent.life -= (fighterState.damage * 0.04f);

        if (defendHitSoundEffect != null && fighterOponent.currentState != FighterState.LAID_DOWN)
        {
            fighterOponent.PlaySound(defendHitSoundEffect);

            GameObject defendParticles = Instantiate(defendHitParticles, collision.transform.position + new Vector3(fighterOponent.playerFacing * 0.12f, yOffset, -1), defendHitParticles.transform.rotation);
            defendParticles.transform.SetParent(fighterOponent.transform);
        }
    }
}
