using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{

    float groundDistance = -1.0f;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("SpawnManager called");
    }
    void Start()
    {
        GameManager.OnCollectCoin += spawnNewEnemy;
        GameManager.OnEnemyDeath += spawnNewEnemy;
        SceneManager.sceneLoaded += startSpawn; // will spawn a new gomba everytime the scene loads

        for (int i = 0; i < 2; i++){
            spawnFromPooler(ObjectType.greenEnemy);
        }
    }

    void startSpawn(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < 1; i++){
            spawnFromPooler(ObjectType.gombaEnemy);
        }
    }

    private void spawnFromPooler(ObjectType i)
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);
        Debug.Log(item);
        if (item != null)
        {
            //get position
            item.transform.localScale = new Vector3(1,1,1);
            item.transform.position = new Vector3(Random.Range(-4.5f,4.5f),groundDistance + item.GetComponent<SpriteRenderer>().bounds.extents.y,0);
            item.SetActive(true);
        }
        else
        {
            Debug.Log("Not enough items in pool");
        }
    }

    public void spawnNewEnemy()
    {
        int i = Random.Range(0,2);
        ObjectType enemy;
        if (i == 0)
        {
            enemy = ObjectType.gombaEnemy;
        }
        else
        {
            enemy = ObjectType.greenEnemy;
        }
        spawnFromPooler(enemy);
    }

    void OnDestroy()
    {
        GameManager.OnCollectCoin -= spawnNewEnemy;
        GameManager.OnEnemyDeath -= spawnNewEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
