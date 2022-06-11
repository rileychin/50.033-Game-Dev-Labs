using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsummableMushroomSimple : MonoBehaviour
{
    private  Rigidbody2D rigidBody;
    private float speed = 5.0f;
    private Vector2 currentPosition;
    private Vector2 currentDirection;  

    private bool currDirectionState;

    private bool endState;
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
            if (currDirectionState == true){
                currentDirection = new Vector2(-1,0); //change mushroom direction
                currDirectionState = false;
            }
            else if (currDirectionState == false){
                currentDirection = new Vector2(1,0);
                currDirectionState = true;
            }
        }
        else if (col.gameObject.CompareTag("Player")){
            endState = true;
            GetComponent<Collider2D>().enabled = false; // disable collision
            StartCoroutine("ScaleOut"); // Consume animation
        }
    }

    // TODO remove this if becomes invisible
    void  OnBecameInvisible(){
	    // Destroy(gameObject);
        GetComponent<SpriteRenderer>().enabled = false;
    }   

    IEnumerator  ScaleOut(){
        this.transform.localScale += new Vector3(0.4f,0.4f,0); // scale it out
        // wait for next frame
        yield  return  null;

        // render for 0.5 second
        for (int step =  0; step  < 10; step++)
        {
            this.transform.localScale  =  this.transform.localScale  -  new Vector3(0.09f,0.09f,0);
            // wait for next frame
            yield  return  null;
        }
        this.transform.localScale = new Vector3(0,0,0); // then reset to 0

    }
}
