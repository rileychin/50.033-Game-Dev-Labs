using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    ORANGEMUSHROOM = 0,
    REDMUSHROOM = 1
}
public class PowerupManagerEV : MonoBehaviour
{
  // reference of all player stats affected
  public IntVariable marioJumpSpeed;
  public IntVariable marioMaxSpeed;
  public PowerupInventory powerupInventory;
  public List<GameObject> powerupIcons;

  void Start()
  {
      if (!powerupInventory.gameStarted)
      {
          powerupInventory.gameStarted = true;
          powerupInventory.Setup(powerupIcons.Count);
          resetPowerup();
      }
      else
      {
          // re-render the contents of the powerup from the previous time
          for (int i = 0; i < powerupInventory.Items.Count; i++)
          {
              Powerup p = powerupInventory.Get(i);
              if (p != null)
              {
                  AddPowerupUI(i, p.powerupTexture);
              }
          }
      }
  }
    
  public void resetPowerup()
  {
      for (int i = 0; i < powerupIcons.Count; i++)
      {
          powerupIcons[i].SetActive(false);
      }
  }
    
  void AddPowerupUI(int index, Texture t)
  {
      powerupIcons[index].GetComponent<RawImage>().texture = t;
      powerupIcons[index].SetActive(true);
  }

  void RemovePowerupUI(int index, Texture t)
  {
      powerupIcons[index].GetComponent<RawImage>().texture = null;
      powerupIcons[index].SetActive(false);
  }

  public void AddPowerup(Powerup p)
  {
      powerupInventory.Add(p, (int)p.index);
      AddPowerupUI((int)p.index, p.powerupTexture);
  }

  public void RemovePowerup(Powerup p)
  {
      powerupInventory.Remove((int)p.index);
      RemovePowerupUI((int)p.index, p.powerupTexture);
  }

  public void OnApplicationQuit()
  {
      ResetValues();
  }

  void ResetValues()
  {
    powerupInventory.Clear();
  }
  
  public void AttemptConsumePowerup(KeyCode K)
  {
    if (K == KeyCode.Z)
    {
        if (powerupIcons[0].activeSelf == true)
        {
            Debug.Log("Power up z is available");
            Powerup p = powerupInventory.Get(0);
            marioJumpSpeed.ApplyChange(p.absoluteJumpBooster);
            marioMaxSpeed.ApplyChange(p.aboluteSpeedBooster);
            RemovePowerup(p);
            StartCoroutine(DisablepowerUp(p));
        }
        else
        {
            Debug.Log("Z not available");
        }
    }
    else if (K == KeyCode.X)
    {
        if (powerupIcons[1].activeSelf == true)
        {
            Debug.Log("Power up x is available");
            Powerup p = powerupInventory.Get(1);
            marioJumpSpeed.ApplyChange(p.absoluteJumpBooster);
            marioMaxSpeed.ApplyChange(p.aboluteSpeedBooster);
            RemovePowerup(p);
            StartCoroutine(DisablepowerUp(p));
        }
        else
        {
            Debug.Log("X not available");
        }
    }
  }

  IEnumerator DisablepowerUp(Powerup p)
  {
    yield return new WaitForSeconds(p.duration);
    marioJumpSpeed.ApplyChange(-p.absoluteJumpBooster);
    marioMaxSpeed.ApplyChange(-p.aboluteSpeedBooster);
  }


 }