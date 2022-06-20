using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class RedMushroom : MonoBehaviour, ConsumableInterface
{
	public  Texture t;

	public void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public  void  consumedBy(GameObject player){
		// give player jump boost
		player.GetComponent<PlayerController>().upSpeed  +=  10;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator  removeEffect(GameObject player){
		Debug.Log("coroutine stated to remove jump speed");
		yield  return  new  WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().upSpeed  -=  10;
		Destroy(this.gameObject);
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")){
            // update UI
            CentralManager.centralManagerInstance.addPowerup(t, 0, this);
            GetComponent<Collider2D>().enabled  =  false;
        }
    }
}