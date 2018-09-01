using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : MonoBehaviour {

    public Text ComboHitsCounterPlayer1;
    public Text ComboHitsCounterPlayer2;

    public Text ComboDamageCounterPlayer1;
    public Text ComboDamageCounterPlayer2;

    public Fighter player1;
    public Fighter player2;

    public HitBoxCollider player1HitBoxCollider;
    public HitBoxCollider player2HitBoxCollider;

    public Image player1Life;
    public Image player2Life;

    public Text player1Wins;
    public Text player2Wins;

	public Text flawlessVictory;

    public static bool isGroundedEndComboCounter;

	public bool practice;

    private void Update()
    {
		if (practice) 
		{
			if (player1.life < 100)
				player1.life += 0.15f;

			if (player2.life < 100)
				player2.life += 0.15f;
		}

        if (player1.life <= 0)
        {
            player2Wins.gameObject.SetActive(true);
            Time.timeScale = 0;
            player1.life = 0;

			if (player2.life == 100) 
			{
				flawlessVictory.gameObject.SetActive (true);
			}
        }

        else if (player2.life <= 0)
        {
            player1Wins.gameObject.SetActive(true);
            Time.timeScale = 0;
            player2.life = 0;

			if (player1.life == 100) 
			{
				flawlessVictory.gameObject.SetActive (true);
			}
        }


        player1Life.transform.localScale = new Vector2 (player1.life / 100, 1);

        player2Life.transform.localScale = new Vector2(player2.life / 100, 1);

        if (player1.IsGrounded)
        {
            ComboHitsCounterPlayer1.gameObject.SetActive(false);
            ComboDamageCounterPlayer1.gameObject.SetActive(false);

        }

        if (player2.IsGrounded)
        {
            ComboHitsCounterPlayer2.gameObject.SetActive(false);
            ComboDamageCounterPlayer2.gameObject.SetActive(false);

        }

        if (player1HitBoxCollider.comboCounter > 1)
        {
            ComboHitsCounterPlayer1.gameObject.SetActive(true);
            ComboDamageCounterPlayer1.gameObject.SetActive(true);
            ComboHitsCounterPlayer1.text = "Hits " + player1HitBoxCollider.comboCounter;
            ComboDamageCounterPlayer1.text = "Damage " + (int)player1HitBoxCollider.comboDamageCounter + " %";

        }

        if (player2HitBoxCollider.comboCounter > 1)
        {
            ComboHitsCounterPlayer2.gameObject.SetActive(true);
            ComboDamageCounterPlayer2.gameObject.SetActive(true);
            ComboHitsCounterPlayer2.text = "Hits " + player2HitBoxCollider.comboCounter;
            ComboDamageCounterPlayer2.text = "Damage " + (int)player2HitBoxCollider.comboDamageCounter + " %";
        }
    }
}
