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
    private bool deadState = false;

    private Animator marioAnimator;
    private AudioSource marioAudio;
    
    public ParticleSystem dustCloud;

    // get Player transform to init position
    private Transform player;

    // Singleton Pattern
    private  static  PlayerController _instance;
    // Getter
    public  static  PlayerController Instance
    {
      get { return  _instance; }
    }

    // Start is called before the first frame update

    // how to spawn the object at the start??
    void Awake()
    {
        // check if the _instance is not this, means it's been set before, return
        if (_instance  !=  null  &&  _instance  !=  this)
        {
            // Set transform position of player to new position
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            player.transform.position = new Vector3(-8.1f, 2.81f, 0);

            Destroy(this.gameObject);
            return;
        }
        
        // otherwise, this is the first time this instance is created
        _instance  =  this;
        // add to preserve this object open scene loading
        DontDestroyOnLoad(this.gameObject); // only works on root gameObjects

    }

    void  Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate =  60;
        marioBody = GetComponent<Rigidbody2D>();
        restartText.text = ""; //set restart text to be nothing when game started
        marioAnimator  =  GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        dustCloud = GameObject.Find("DustCloud").GetComponent<ParticleSystem>(); // cannot use GetComponent<ParticleSystem>(); directly
        
        GameManager.OnPlayerDeath  +=  PlayerDiesSequence; // when the player dies do a death sequence
    }

    // Update is called once per frame
    void Update()
    {
        if (!deadState){
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

            if (!onGroundState)
            {
                dustCloud.Play(); // TODO find out where to put this
            }
            // Assigns value to Mario skidding condition
            marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x)); //Assigns value to xSpeed
        }
        // Restart mechanic
        if(Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(0);
        }
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (!deadState){
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
            marioAnimator.SetBool("onGround", onGroundState); //Assigns value to onGround
            }

            if (Input.GetKeyDown("z")){
                CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
            }

            if (Input.GetKeyDown("x")){
                CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
            }
        }
    }

    // TODO: Uncomment when want to let mario increase score by jumping over
    // called when the mario hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        // If colliding with ground or obstacles
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) {
        onGroundState = true;
        // scoreText.text = "Score: " + score.ToString();
        marioAnimator.SetBool("onGround", onGroundState); //Assigns value to onGround
        }
    }

    //TODO: Uncomment when want to set him to die
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         restartText.gameObject.SetActive(true); // Set active to be true for restart text to tell people how to restart
    //         restartText.text = "Press 'R' to restart";
            
    //         Time.timeScale = 0.0f; // Restart 

    //     }
    // }

    // Playing jump sound
    void  PlayJumpSound(){
	    marioAudio.PlayOneShot(marioAudio.clip);
    }

    // called when the player dies
    void  PlayerDiesSequence(){
        // Mario dies
        Debug.Log("Mario dies");
        // do whatever you want here, animate etc
        // ...
        restartText.gameObject.SetActive(true); // Set active to be true for restart text to tell people how to restart
        restartText.text = "Press 'R' to restart";
        marioAnimator.SetTrigger("isDead");

        marioBody.velocity = new Vector2(0,0);
        deadState = true;
        StartCoroutine(marioDeathDance());
        Debug.Log("Mario dies");
    }

    IEnumerator marioDeathDance()
    {
        Debug.Log("Mario starts dying");
        while (true)
        {
            marioSprite.flipY = true;
            yield return new WaitForSeconds(0.5f);
            marioSprite.flipY = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnDestroy()
    {
         GameManager.OnPlayerDeath  -=  PlayerDiesSequence;
    }

}
