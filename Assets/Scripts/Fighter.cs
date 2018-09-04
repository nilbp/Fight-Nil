using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {


    public enum PlayerType
    {
        PLAYER1, PLAYER2, JOYSTICK, IA
    }

    public enum Name
    {
        GIRL1, KUNGFU, WOLF
    }

    public Name characterName;

    public CharacterController2D controller;
    public Animator animator;

    static float MAX_LIFE = 100;

    //Vida
    public float life = MAX_LIFE;

    //Variable moviment 
    float horizontalMove = 0f;

	float verticalAxes = 0f;

	//Variable Stun
	public float stunTime;

    //Defense recover
    public float defenseRecoverTime;

	//Variables per atacs especials
	float downSpecial;
	float forwardSpecial;
	float backSpecial;

	static float SPECIAL_KEYPRESS_TIME = 0.15f;

    public int playerFacing; //1 right, -1 left

    public Rigidbody2D rb;

    //Jugador controlat per...
    public PlayerType playerType;

    //Oponent
    public Fighter oponent;

    //Estat del jugador
    public FighterState currentState = FighterState.IDLE;

    public bool IsGrounded;

    //Transformation Wolf variables
    static float TRANSFORMATION_TIME = 15F;
    private float transformationTimer = TRANSFORMATION_TIME;
    private bool transformated = false;

    //Colliders 
    public Collider HurtBoxTorso;
    public Collider HurtBoxLegs;
    public Collider2D groundCheck;
    public Collider2D floorCollider;

    //Variables controlar els diferents jugadors player 1 i player 2
    string horizontalMovement;
	string verticalMovement;
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
			verticalMovement = "Vertical"; //UP, DOWN
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
		else if (playerType == PlayerType.JOYSTICK)
		{
			horizontalMovement = "Horizontal_J";
			verticalMovement = "Vertical_J";
			stand1 = "Stand1_J"; 
			stand2 = "Stand2_J"; 
			defend = "Defend_J"; 

		}
        else
        {
            //IA
        }

	}
	
	void Update () {

        horizontalMove = Input.GetAxisRaw(horizontalMovement);

		if(playerType == PlayerType.JOYSTICK)
			verticalAxes = Input.GetAxisRaw (verticalMovement);

        IsGrounded = groundCheck.IsTouching(floorCollider);

        animator.SetBool("IsGrounded", IsGrounded);

        //facing each other 

        if (gameObject.transform.position.x > oponent.gameObject.transform.position.x)
        {
            if (characterName == Name.WOLF)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }

            else
            {       
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }

            playerFacing = -1;
        }

        else
        {
            if (characterName == Name.WOLF)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }

            else
            {
               
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }

            playerFacing = 1;
        }

        //WALKING FORKARD AND BACKWARDS
        if (horizontalMove * playerFacing > 0.1f)
        {
            animator.SetBool("Walk", true);
			forwardSpecial = SPECIAL_KEYPRESS_TIME;
        }
        else if (horizontalMove * playerFacing < -0.1f)
        {
            animator.SetBool("Walk Back", true);
			backSpecial = SPECIAL_KEYPRESS_TIME;
        }
        else
        {
            animator.SetBool("Walk Back", false);
            animator.SetBool("Walk", false);

        }

		//Jumping and crouching Joystick control
		if (playerType == PlayerType.JOYSTICK) {
			
			if (verticalAxes < -0.1f) 
			{
				if (currentState == FighterState.IDLE ||
					currentState == FighterState.WALK ||
					currentState == FighterState.WALK_BACK) 
				{
					animator.SetTrigger ("IsJumping");
				}
			} 

			else if (verticalAxes > 0.1f) 
			{
				animator.SetBool ("IsCrouching", true);
				downSpecial = SPECIAL_KEYPRESS_TIME;
			} 

			else 
			{
				animator.SetBool ("IsCrouching", false);
			}
		} 

		//Jumping and crouching keyboard control
		else {
			
			if (Input.GetButtonDown (jump) ) 
			{
				if (currentState == FighterState.IDLE ||
				    currentState == FighterState.WALK ||
				    currentState == FighterState.WALK_BACK) 
				{
					animator.SetTrigger ("IsJumping");
				}
			}

			if (Input.GetButtonDown (crouch)) 
			{
				animator.SetBool ("IsCrouching", true);
				downSpecial = SPECIAL_KEYPRESS_TIME;
			} 
			else if (Input.GetButtonUp (crouch)) 
			{
				animator.SetBool ("IsCrouching", false);
			}
		}

		if (Input.GetButtonDown(defend))
        {
            animator.SetBool("IsDefending", true);
        }
        else if (Input.GetButtonUp(defend))
        {
            animator.SetBool("IsDefending", false);
        }

        //Defence recover
        if (defenseRecoverTime > 0)
        {
            animator.SetBool("Defense Recover", false);
            defenseRecoverTime-= 1 * Time.deltaTime;
        }
        else
            animator.SetBool("Defense Recover", true);

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

		//Activar IsInDefenceStance quan està en la posició de defença
		if (currentState == FighterState.DEFEND)
			animator.SetBool ("IsInDefenceStance", true);
		else 
			animator.SetBool ("IsInDefenceStance", false);


		//posa a TRUE Stuned quan el stunTime sigui 0
		if (stunTime <= 0 && currentState == FighterState.TAKE_HIT)
			animator.SetBool ("Stuned", false);
		
		//Set Trigger from Special attakcs
		if (downSpecial > 0 && backSpecial > 0 && downSpecial < backSpecial) 
		{
			animator.SetBool ("Down Back", true);
		}
		else if (downSpecial > 0 && forwardSpecial > 0 && downSpecial < forwardSpecial) 
		{
			animator.SetBool ("Down Forward", true);
		}
		else if (forwardSpecial > 0 && backSpecial > 0 && backSpecial < forwardSpecial) 
		{
			animator.SetBool ("Back Forward", true);
		}
		else if (downSpecial > 0 && backSpecial > 0 && forwardSpecial > 0 
			&& downSpecial < backSpecial && backSpecial < forwardSpecial) 
		{
			animator.SetBool ("Down Back Forward", true);
		}


		//Decrementar les variables de Special Attack
		if (downSpecial > 0)
			downSpecial -= 1 * Time.deltaTime;
		if (forwardSpecial > 0)
			forwardSpecial -= 1 * Time.deltaTime;
		if (backSpecial > 0)
			backSpecial -= 1 * Time.deltaTime;

		if(downSpecial <= 0)
		{
			animator.SetBool ("Down Forward", false);
			animator.SetBool ("Down Back", false);
		}
		if (backSpecial <= 0) 
		{
			animator.SetBool ("Back Forward", false);

		}

		//Decrementar variable stunTime
		if (stunTime > 0)
			stunTime -= 1 * Time.deltaTime;

        if (characterName == Name.WOLF)
        {
            if (transformated)
            {
                if (transformationTimer <= 0)
                    animator.SetBool("Transformed", false);

                transformationTimer -= 1 * Time.deltaTime;
            }
        }

    }

    public void GetHurt(string mode)
    {
        if (mode == "Floor")
        {
            animator.SetTrigger("Hurt Floor");
           
        }

        else if (mode == "Air")
        {
			animator.SetBool("HurtAir", true);
        }

        if (characterName == Name.WOLF)
        {
            animator.SetBool("Transformed", false);
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

    public void Stand1Stand2_False()
    {
        animator.SetBool("Stand1", false);
        animator.SetBool("Stand2", false);
    }

    public void Transformation()
    {
        transformationTimer = TRANSFORMATION_TIME;
        transformated = true;
    }

}
