using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollider : MonoBehaviour {

    Fighter fighterOponent;

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

            if (!launcherAttack)
            {
                fighterOponent.GetHurt("Floor", horizontalForce, verticalForce);
            }

            else
            {
                fighterOponent.GetHurt("Air", horizontalForce, verticalForce);

                fighterOponent.verticalForce = 100f;

            }

            Debug.Log(fighterOponent.life);

        }
    }
}
