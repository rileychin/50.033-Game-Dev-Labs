using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsummableMushroomSimple : MonoBehaviour
{
    public  Rigidbody2D rigidBody;
    private float speed = 5.0f;
    public Vector2 currentPosition;
    public Vector2 currentDirection;  

    public bool currDirectionState;

    public bool endState;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse); //?
        //rigidBody.AddForce(new Vector2(0,), ForceMode2D.Impulse);
        if (Random.Range(0,2)==0){
            currentDirection = new Vector2(1,0);
            currDirectionState = true; // Mushroom is now moving right
        }
        else{
            currentDirection = new Vector2(-1,0);
            currDirectionState = false; // Mushroom is now moving left
        }
        endState = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMushroom();
    }

    void MoveMushroom(){
        if (endState != true){
            Vector2 nextPosition = rigidBody.position + speed * currentDirection.normalized * Time.fixedDeltaTime;
            rigidBody.MovePosition(nextPosition);
        }
        else{
            rigidBody.velocity = new Vector2(0,0);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.CompareTag("Pipe")){
            Debug.Log(currDirectionState);
            if (currDirectionState == true){
                currentDirection = new Vector2(-1,0); //change mushroom direction
                currDirectionState = false;
            }
            else if (currDirectionState == false){
                currentDirection = new Vector2(1,0);
                currDirectionState = true;
            }
            Debug.Log(currDirectionState);
        }
        else if (col.gameObject.CompareTag("Player")){
            endState = true;
        }
    }

    void  OnBecameInvisible(){
	    Destroy(gameObject);	
    }   
}
