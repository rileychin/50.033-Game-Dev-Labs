using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class PowerUpManager : MonoBehaviour
{
	public List<GameObject> powerupIcons;
	private List<ConsumableInterface> powerups;

	// Start is called before the first frame update
	void Start()
	{
		powerups  =  new  List<ConsumableInterface>();
		for (int i =  0; i<powerupIcons.Count; i++){
			powerupIcons[i].SetActive(false);
			powerups.Add(null);
		}
	}

    public void addPowerup(Texture texture, int index, ConsumableInterface i)
    {
        Debug.Log("adding powerup");
        if (index  <  powerupIcons.Count){
            powerupIcons[index].SetActive(true);
            Debug.Log(texture.name);
            powerupIcons[index].GetComponent<RawImage>().texture  =  texture;
            powerups[index] =  i;
        }
    }

    public void removePowerup(int index)
    {
        Debug.Log("removing powerup");
        if (index  <  powerupIcons.Count){
        powerupIcons[index].SetActive(false);
        powerups[index] =  null;
        }
    }

    void cast(int i, GameObject p){
        if (powerups[i] !=  null){
            Debug.Log("before");
            powerups[i].consumedBy(p); // interface method
            Debug.Log("after 1");
            removePowerup(i);
            Debug.Log("after 2");
        }
    }

    public  void  consumePowerup(KeyCode k, GameObject player){
        switch(k){
            case  KeyCode.Z: // when z is pressed, eat first slot
                cast(0, player);
                break;
            case  KeyCode.X: // when x is pressed eat second slot
                cast(1, player);
                break;
            default:
                break;
        }
    }
}