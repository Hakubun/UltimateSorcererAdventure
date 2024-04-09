using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class ChangePwScript : MonoBehaviour
{
    // Start is called before the first frame update


    public TMP_InputField _oldpw;
    public TMP_InputField _newpw;
    public Toggle showPW;
    public GameObject LogInPageErrorPopUp;
    public TextMeshProUGUI ErrorMessage_LogIn;
    public AudioSource LogedInSFX;
    public AudioSource ErrorSFX;

    private void OnEnable()
    {
        _oldpw.text = null;
        _newpw.text = null;
        if (showPW.isOn)
        {
            _oldpw.contentType = TMP_InputField.ContentType.Standard;
            _newpw.contentType = TMP_InputField.ContentType.Standard;

            // Force update the input field to apply the content type change
            _oldpw.ForceLabelUpdate();
            _newpw.ForceLabelUpdate();
        }
        else
        {
            _oldpw.contentType = TMP_InputField.ContentType.Password;
            _newpw.contentType = TMP_InputField.ContentType.Password;

            // Force update the input field to apply the content type change
            _oldpw.ForceLabelUpdate();
            _newpw.ForceLabelUpdate();
        }


    }

    public void storeCurrentPW()
    {

        Debug.Log(_oldpw.text);

    }

    public void storeNewPW()
    {
        Debug.Log(_newpw.text);
    }

    public async void UpdatePw()
    {
        await UpdatePasswordAsync(_oldpw.text, _newpw.text);
    }
    public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
            UserLogIn data = SaveSystem.LoadLogIn();
            SaveSystem.SaveLogIn(data.username, newPassword);
            this.gameObject.SetActive(false);
            LogedInSFX.Play();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            LogInPageErrorPopUp.SetActive(true);
            ErrorMessage_LogIn.text = ex.Message;
            ErrorSFX.Play();
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            LogInPageErrorPopUp.SetActive(true);
            ErrorMessage_LogIn.text = ex.Message;
            ErrorSFX.Play();
        }
    }

    public void CancelUpdatePW()
    {

        this.gameObject.SetActive(false);
    }

    public void displayContent()
    {
        if (showPW.isOn)
        {
            _oldpw.contentType = TMP_InputField.ContentType.Standard;
            _newpw.contentType = TMP_InputField.ContentType.Standard;

            // Force update the input field to apply the content type change
            _oldpw.ForceLabelUpdate();
            _newpw.ForceLabelUpdate();
        }
        else
        {
            _oldpw.contentType = TMP_InputField.ContentType.Password;
            _newpw.contentType = TMP_InputField.ContentType.Password;

            // Force update the input field to apply the content type change
            _oldpw.ForceLabelUpdate();
            _newpw.ForceLabelUpdate();
        }
    }
}
