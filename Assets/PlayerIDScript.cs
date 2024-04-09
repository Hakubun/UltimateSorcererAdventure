using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class PlayerIDScript : MonoBehaviour
{
    public TextMeshProUGUI LogTxt;
    private void Awake() {
        LogTxt.text = "Player ID: " + AuthenticationService.Instance.PlayerId;
    }
}
