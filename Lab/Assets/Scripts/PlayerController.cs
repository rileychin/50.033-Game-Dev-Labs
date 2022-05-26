using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public float speed;
    private Rigidbody2D marioBody;

    public float maxSpeed = 10;
    public float upSpeed = 6;

    private bool onGroundState = true;

    public Transform enemyLocation;
    public Text scoreText;
    public Text restartText;
    private int score = 0;
    private bool countScoreState = false;

    private Animator marioAnimator;
    private AudioSource marioAudio;

    // Start is called before the first frame update
    void  Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate =  60;
        marioBody = GetComponent<Rigidbody2D>();
        restartText.text = ""; //set restart text to be nothing when game started
        marioAnimator  =  GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      // toggle state
      if (Input.GetKeyDown("a") && faceRightState){
          faceRightState = false;
          marioSprite.flipX = true; //Flips x direction
            if (Mathf.Abs(marioBody.velocity.x) >  0.05) 
            {
                marioAnimator.SetTrigger("onSkid");
            }
      }

      if (Input.GetKeyDown("d") && !faceRightState){
          faceRightState = true;
          marioSprite.flipX = false; //Does not flip x direction
          if (Mathf.Abs(marioBody.velocity.x) >  0.05) 
          {
              marioAnimator.SetTrigger("onSkid");
          }
      }

      if (!onGroundState && countScoreState)
      {
          if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
          {
              countScoreState = false;
              score++;
          }
      }

      // Restart mechanic
      if(Input.GetKeyDown("r"))
      {
        SceneManager.LoadScene(0);
      }

      // Assigns value to Mario skidding condition

	  marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x)); //Assigns value to xSpeed
      

    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }

        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            // marioBody.velocity = Vector2.zero; //Set all velocity to 0
            marioBody.velocity = new Vector2(0.0f,marioBody.velocity.y); // Set x velocity to 0, keep y velocity constant
        }

        if (Input.GetKeyDown("space") && onGroundState){
            // Mario is on the ground, we add a force to go up
          marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
          onGroundState = false;
          countScoreState = true; //check if Gomba is underneath
          marioAnimator.SetBool("onGround", onGroundState); //Assigns value to onGround
        }
    }

    // called when the mario hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        // If colliding with ground or obstacles
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) {
        onGroundState = true;
        countScoreState = false; // reset score state
        scoreText.text = "Score: " + score.ToString();
        marioAnimator.SetBool("onGround", onGroundState); //Assigns value to onGround
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            restartText.gameObject.SetActive(true); // Set active to be true for restart text to tell people how to restart
            restartText.text = "Press 'R' to restart";
            Time.timeScale = 0.0f; // Restart 
        }
    }

    // Playing jump sound
    void  PlayJumpSound(){
	    marioAudio.PlayOneShot(marioAudio.clip);
    }

}
