using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {


    public enum PlayerType
    {
        PLAYER1, PLAYER2, IA
    }

    public CharacterController2D controller;
    public Animator animator;

    static float MAX_LIFE = 100;

    //Vida
    public float life = MAX_LIFE;

    //Variable moviment 
    float horizontalMove = 0f;

    public int playerFacing; //1 right, -1 left

    public Rigidbody2D rb;

    //Jugador controlat per...
    public PlayerType playerType;

    //Oponent
    public Fighter oponent;

    public float horizontalForce;
    public float verticalForce;

    //Estat del jugador
    public FighterState currentState = FighterState.IDLE;


    //Colliders 
    public Collider HurtBoxTorso;
    public Collider HurtBoxLegs;
    public Collider2D groundCheck;
    public Collider2D floorCollider;

    //Variables controlar els diferents jugadors player 1 i player 2
    string horizontalMovement;
    string crouch;
    string jump;
    string stand1;
    string stand2;
    string defend;

    void Start () {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (playerType == PlayerType.PLAYER1)
        {
            horizontalMovement = "Horizontal"; //LEFT, RIGHT
            crouch = "Crouch"; //DOWN
            jump = "Jump"; //UP
            stand1 = "Stand1"; //, (COMA)
            stand2 = "Stand2"; // . (POINT)
            defend = "Defend"; // - (GUIÓ)

        }
        else if (playerType == PlayerType.PLAYER2)
        {
            horizontalMovement = "Horizontal_2"; //A, D
            crouch = "Crouch_2"; //S
            jump = "Jump_2"; //W
            stand1 = "Stand1_2"; //1
            stand2 = "Stand2_2"; //2
            defend = "Defend_2"; //3

        }
        else
        {
            //IA
        }

	}
	
	void Update () {

        horizontalMove = Input.GetAxisRaw(horizontalMovement);
        animator.SetBool("IsGrounded", groundCheck.IsTouching(floorCollider));

        //facing each other handdle
        if (gameObject.transform.position.x > oponent.gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            playerFacing = -1;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            playerFacing = 1;
        }



        if (horizontalMove * playerFacing > 0.1f)
        {
            animator.SetBool("Walk", true);
        }
        else if (horizontalMove * playerFacing < -0.1f)
        {
            animator.SetBool("Walk Back", true);
        }
        else
        {
            animator.SetBool("Walk Back", false);
            animator.SetBool("Walk", false);

        }

        if (Input.GetButtonDown(jump))
        {
            animator.SetTrigger("IsJumping");
        }

        if (Input.GetButtonDown(crouch))
        {
            animator.SetBool("IsCrouching", true);
        }
        else if (Input.GetButtonUp(crouch))
        {
            animator.SetBool("IsCrouching", false);
        }

        if (Input.GetButtonDown(defend))
        {
            animator.SetBool("IsDefending", true);
        }
        else if (Input.GetButtonUp(defend))
        {
            animator.SetBool("IsDefending", false);
        }

        //STAND 1
        if (Input.GetButtonDown(stand1))
        {
            animator.SetTrigger("Stand1");
        }

        //STAND 2
        else if (Input.GetButtonDown(stand2))
        {
            animator.SetBool("Stand2", true);   
        }

    }

    public void GetHurt(string mode, float horizontalForce, float verticalForce)
    {
        if (mode == "Floor")
        {
            animator.SetTrigger("Hurt Floor");
           
        }

        else if (mode == "Air")
        {
            animator.SetTrigger("Hurt Air Flopy");
        }

    }

    public void EndAnimation(string animName)
    {
        animator.SetBool(animName, false);
    }

    public void SpecialTimerTrue(string animName)
    {
        animator.SetBool(animName, true);
    }

}
