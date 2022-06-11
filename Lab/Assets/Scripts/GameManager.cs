using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Text score;
    private int playerScore =  0;

    // Start is called before the first frame update

    // void Awake()
    // {
    //   if (__instance && null)
    //   {
    //     Destroy(this.gameObject);

    //   }
    // }

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