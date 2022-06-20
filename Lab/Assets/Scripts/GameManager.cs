using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
  
    // Singleton Pattern
    private  static  GameManager _instance;
    // Getter
    public  static  GameManager Instance
    {
      get { return  _instance; }
    }


    public Text score;
    private int playerScore =  0;
    public GameObject panel;
    public GameObject restart;

    // Start is called before the first frame update

    private  void  Awake()
    {
      // check if the _instance is not this, means it's been set before, return
      if (_instance  !=  null  &&  _instance  !=  this)
      {
        Destroy(this.gameObject);
        return;
      }
      
      // otherwise, this is the first time this instance is created
      _instance  =  this;
      // add to preserve this object open scene loading
      DontDestroyOnLoad(this.gameObject); // only works on root gameObjects
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  increaseScore()
    {
      playerScore  +=  1;
      score.text  =  "SCORE: "  +  playerScore.ToString();
	  }

    public  void  damagePlayer()
    {
      OnPlayerDeath();
    }

    public void damageEnemy()
    {
      OnEnemyDeath();
    }

    public void collectCoin()
    {
      OnCollectCoin();
    }

    public  delegate  void gameEvent();
    public  static  event  gameEvent OnPlayerDeath; //stores methods of events  
    public  static  event  gameEvent OnEnemyDeath; //stores methods of events 
    public  static  event  gameEvent OnCollectCoin; //stores methods of events
}