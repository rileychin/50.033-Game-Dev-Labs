using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour
{

    private bool broken = false;

    private AudioSource brickAudio;
    public Rigidbody2D prefab;
    public  GameObject consummablePrefab;
    public GameConstants gameConstants;

    // Start is called before the first frame update
    void Start()
    {
        brickAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") &&  !broken){
            broken  =  true;
            // assume we have 5 debris per box
            for (int x =  0; x<gameConstants.spawnNumberOfDebris; x++){
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled  =  false;
            gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled  =  false;
            GetComponent<EdgeCollider2D>().enabled  =  false;
            brickAudio.PlayOneShot(brickAudio.clip); // plays the break brick sound
            Instantiate(consummablePrefab, 
            new  Vector3(this.transform.position.x, 
            this.transform.position.y  +  1.5f, 
            this.transform.position.z), 
            Quaternion.identity);
        }
    }
}