using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerControllerEV : MonoBehaviour
{

    private float force;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;

    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private Rigidbody2D marioBody;

    private bool onGroundState = true;

    public Text scoreText;
    public Text restartText;

    // flags to be used for the game
    private bool isDead = false;
    private bool isADKeyUp = true;
    private bool isSpacebarUp = true;

    private Animator marioAnimator;
    private AudioSource marioAudio;
    
    public ParticleSystem dustCloud;

    public UnityEvent<KeyCode> OnPlayerCast;

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate =  60;
        marioBody = GetComponent<Rigidbody2D>();
        restartText.text = ""; //set restart text to be nothing when game started
        marioAnimator  =  GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
        dustCloud = GameObject.Find("DustCloud").GetComponent<ParticleSystem>(); // cannot use GetComponent<ParticleSystem>(); directly
        // GameManager.OnPlayerDeath  +=  PlayerDiesSequence; // when the player dies do a death sequence
        
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerStartingMaxSpeed );
        force = gameConstants.playerDefaultForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead){
            // toggle state
            if (Input.GetKeyDown("a") && faceRightState){
                isADKeyUp = false;
                faceRightState = false;
                marioSprite.flipX = true; //Flips x direction
                    if (Mathf.Abs(marioBody.velocity.x) >  0.05) 
                    {
                        marioAnimator.SetTrigger("onSkid");
                    }
            }

            if (Input.GetKeyDown("d") && !faceRightState){
                isADKeyUp = false;
                faceRightState = true;
                marioSprite.flipX = false; //Does not flip x direction
                if (Mathf.Abs(marioBody.velocity.x) >  0.05) 
                {
                    marioAnimator.SetTrigger("onSkid");
                }
            }

            if (Input.GetKeyDown("a") || Input.GetKeyDown("d")){
                isADKeyUp = false;
            }

            if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
                isADKeyUp = true;
            }

            if (Input.GetKeyDown("space")){
                // Mario is on the ground, we add a force to go up
            isSpacebarUp = false;
            }
            
            if (Input.GetKeyUp("space")){
                // Mario is on the ground, we add a force to go up
            isSpacebarUp = true;
            }

            if (!onGroundState)
            {
                dustCloud.Play(); // TODO find out where to put this
            }
            // Assigns value to Mario skidding condition
            marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x)); //Assigns value to xSpeed
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            //check if a or d is pressed currently
            if (!isADKeyUp)
            {
                float direction;
                // float direction = faceRightState ? 1.0f : -1.0f;
                if (faceRightState)
                {
                    direction = 1.0f;
                }
                else
                {
                    direction = -1.0f;
                }
                Vector2 movement = new Vector2(force * direction, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
                    marioBody.AddForce(movement);
            }
            else
            {
                marioBody.velocity = new Vector2(0.0f,marioBody.velocity.y);
            }

            if (!isSpacebarUp && onGroundState)
            {
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                // part 2
                marioAnimator.SetBool("onGround", onGroundState);
            }
            if (Input.GetKeyDown("z")){
                OnPlayerCast.Invoke(KeyCode.Z);
            }

            if (Input.GetKeyDown("x")){
                OnPlayerCast.Invoke(KeyCode.X);
            }
            
        }
        else
        {
            marioBody.velocity = new Vector2(0.0f,marioBody.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If colliding with ground or obstacles
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) {
        onGroundState = true;
        // scoreText.text = "Score: " + score.ToString();
        marioAnimator.SetBool("onGround", onGroundState); //Assigns value to onGround
        }
    }
    void  PlayJumpSound(){
	    marioAudio.PlayOneShot(marioAudio.clip);
    }


    void OnDestroy()
    {
        // GameManager.OnPlayerDeath  -=  PlayerDiesSequence;
    }

    public void PlayerDiesSequence()
    {
        isDead = true;
        marioAnimator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        marioBody.AddForce(Vector3.up * 30, ForceMode2D.Impulse);
        marioBody.gravityScale = 30;
        StartCoroutine(dead());
    }
    
    IEnumerator dead()
    {
        yield return new WaitForSeconds(1.0f);
        marioBody.bodyType = RigidbodyType2D.Static;
    }

}
