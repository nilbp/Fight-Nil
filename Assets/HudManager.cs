using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : MonoBehaviour {

    public Fighter player1;
    public Fighter player2;

    public Image player1Life;
    public Image player2Life;

    public Text player1Wins;
    public Text player2Wins;

    private void Update()
    {
        if (player1.life <= 0)
        {
            player2Wins.gameObject.SetActive(true);
            Time.timeScale = 0;
            player1.life = 0;
        }

        else if (player2.life <= 0)
        {
            player1Wins.gameObject.SetActive(true);
            Time.timeScale = 0;
            player2.life = 0;
        }


        player1Life.transform.localScale = new Vector2 (player1.life / 100, 1);

        player2Life.transform.localScale = new Vector2(player2.life / 100, 1);

        
    }
}
