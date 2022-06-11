using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use this class to define the data structure of an Object in the pool
public class ExistingPoolItem
{
	public  GameObject gameObject;
	public  ObjectType type;

	// constructor
	public  ExistingPoolItem(GameObject gameObject, ObjectType type){
		// reference input
		this.gameObject  =  gameObject;
		this.type  =  type;
	}
}

// Use this class to define the data structure of an Object metadata to be spawned into the pool
[System.Serializable]
public class ObjectPoolItem
{
	public  int amount; // how many objects to instantiate
	public  GameObject prefab; // reference to gameobject
	public  bool expandPool; // if we did not instantiate enough objects in pool
	public  ObjectType type; // type of enemy (0 for goomba, 1 for green turtle)
}

// named integers
public enum ObjectType{
	gombaEnemy =  0,
	greenEnemy =  1
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool; // types of different object to pool
    public List<ExistingPoolItem> pooledObjects; // a list of all objects in the pool, of all types

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        SharedInstance = this;
        pooledObjects = new List<ExistingPoolItem>();
        Debug.Log("ObjectPooler Awake");

        foreach (ObjectPoolItem item in  itemsToPool)
        {
            for (int i =  0; i  <  item.amount; i++)
            {
                // this 'pickup' a local variable, but Unity will not remove it since it exists in the scene
                GameObject pickup = (GameObject)Instantiate(item.prefab);
                pickup.SetActive(false);
                pickup.transform.parent = this.transform;

                // One liner code
                // pooledObjects.Add(new  ExistingPoolItem(pickup, item.type));

                ExistingPoolItem e = new ExistingPoolItem(pickup, item.type);
                pooledObjects.Add(e);
            }
        }
    }

    // modified from original
    public GameObject GetPooledObject(ObjectType type)
    {
        Debug.Log("attemping to spawn of type");
        Debug.Log(type);
        // return inactive pooled object if it matches the type

        for (int i =  0; i  <  pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                Debug.Log("Found an inactive item");
                return  pooledObjects[i].gameObject;
            }
        }

        // this will be called when no more active object is present, item to expand pool if required
        if (pooledObjects.Count < 5)
        {
            foreach (ObjectPoolItem item in itemsToPool)
            {

                Debug.Log(item.type);
                if (item.type == type)
                {
                    if (item.expandPool)
                    {
                        GameObject pickup = (GameObject)Instantiate(item.prefab);
                        pickup.SetActive(false);
                        pickup.transform.parent  =  this.transform;
                        pooledObjects.Add(new  ExistingPoolItem(pickup, item.type));
                        Debug.Log("adding items to pool");
                        Debug.Log(pooledObjects.Count);
                        return  pickup;
                    }
                }
            }
        }

        // this line will be executed if pool is full, and all objects are active, then we just instatiante an object and send in
        

        // return null IFF type doesn't match with what is defined in the itemsToPool.
        return null;
    }
}
