using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xpDrop : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private int xpamount;
    [SerializeField] private int multiplier;




    private void OnTriggerEnter(Collider other) {
        //Debug.Log(other);
        if(other.tag == "Player"){
            other.GetComponent<player>().xpUp(xpamount);
            Destroy(gameObject);
        }
        
    }

    public void SetUpXP(int amount)
    {
        xpamount = amount;
        xpamount *= multiplier;
    }
}
