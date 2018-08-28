using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollider : MonoBehaviour {

    Fighter fighterOponent;

    public int comboCounter;
    public float comboCounterPercentage = 0;

    public static float damage;
    public static bool launcherAttack;

    public static float horizontalForce;
    public static float verticalForce;


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
                fighterOponent.life -= (damage * 0.04f);
                return;
            }

            fighterOponent.life -= damage;

            if (fighterOponent.currentState == FighterState.TAKE_HIT_AIR)
            {
                //Està en el aire

                fighterOponent.GetHurt("Air");

                if (!launcherAttack)
                {
                    fighterOponent.rb.AddRelativeForce(new Vector2(0, verticalForce));                  
                }

                comboCounter++;
                comboCounterPercentage += damage;
            }

            else
            {
                comboCounter = 0;
                comboCounterPercentage = 0;


                if (fighterOponent.currentState != FighterState.TAKE_HIT_AIR)
                {
                    if (launcherAttack)
                    {
                        fighterOponent.GetHurt("Air");
                        fighterOponent.rb.AddRelativeForce(new Vector2(0, verticalForce));

                        comboCounter++;
                        comboCounterPercentage += damage;

                    }

                    else if (!launcherAttack)
                    {
                        fighterOponent.GetHurt("Floor");

                    }
                }
            }
        }
    }
}
