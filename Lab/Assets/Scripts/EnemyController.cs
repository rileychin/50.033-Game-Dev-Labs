using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer enemySprite;
    private float originalX;
    private int moveRight;
    private Vector2 velocity;
    public GameConstants gameConstants;
    private Rigidbody2D enemyBody;


    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;

        moveRight = Random.Range(0,2) == 0 ? -1 : 1;
        ComputeVelocity();
        GameManager.OnPlayerDeath  +=  EnemyRejoice; // reference to static class, enemy will rejoice if player dies 
    }


    void ComputeVelocity(){
        velocity = new Vector2((moveRight)*gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
    }


    void MoveEnemy(){
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }


    // Update is called once per frame
   void Update()
   {
        if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
        {// move gomba
            MoveEnemy();
        }
        else{
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            MoveEnemy();
        }
   }

	void OnTriggerEnter2D(Collider2D other){
		// check if it collides with Mario
		if (other.gameObject.tag  ==  "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y  -  this.transform.position.y);
			if (yoffset  >  1.0f){
				KillSelf(); // enemy dies
                CentralManager.centralManagerInstance.damageEnemy();
			}
			else{
				CentralManager.centralManagerInstance.damagePlayer(); // enemy lives
			}
		}
	}

    void  KillSelf(){
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore();
		StartCoroutine(flatten());
        //Destroy(gameObject);
	}

    IEnumerator  flatten(){
		int steps =  1;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		this.gameObject.SetActive(false);
		yield  break;
	}

    // animation when player is dead
    void  EnemyRejoice(){
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
        // ...

        StartCoroutine(enemyDance());

    }

    
    IEnumerator enemyDance()
    {
        if (gameObject.activeSelf)
        {
            velocity = new Vector2(0,0);
            while (true)
            {
                //Debug.Log("Flipping");
                enemySprite.flipX = true;
                yield return new WaitForSeconds(0.5f);
                //Debug.Log("Flipping back");
                enemySprite.flipX = false;
                yield return new WaitForSeconds(0.5f);
            }
            
        }
    }

    void OnDestroy(){
        GameManager.OnPlayerDeath  -=  EnemyRejoice;
    }
}
