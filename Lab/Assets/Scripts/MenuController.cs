using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
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
      //Time.timeScale = 0.0f;
      foreach (Transform eachChild in transform)
      {
          if (eachChild.name == "Score" || eachChild.name == "Powerups")
          {

          }
          else
          {
              // disable all other child
              eachChild.gameObject.SetActive(false);
              Time.timeScale = 1.0f;
          }
      }
    }

    public void StartButtonClicked()
    {

    }
}