using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;

public class IAPButtonFunctions : MonoBehaviour
{

    public GameObject ErrorPopUp;
    public TextMeshProUGUI errorMessage;
    public void OnPurchaseComplete(Product _product)
    {

    }

    public void OnPurchaseFailed(Product _product, PurchaseFailureReason _reason)
    {
        errorMessage.text = "Pruchasing failed due to: " + _reason;
        ErrorPopUp.SetActive(true);
    }
   

}
