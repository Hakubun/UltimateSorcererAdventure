using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{

    public AudioSource LogedInSFX;
    public AudioSource ErrorSFX;
    public Text LogTxt;
    public GameObject Panel;
    public GameObject LogInPageErrorPopUp;
    public TextMeshProUGUI ErrorMessage_LogIn;
    public TMP_InputField _username;
    public TMP_InputField _pw;
    public loadingscene loading;

    [Header("LogIn")]
    public TMP_InputField _Username_login;
    public TMP_InputField _pw_login;
    //TMP_InputField
    private string user;
    private string password;
    private UserLogIn data;

    public GameObject AutoLoginLoading;

    private void Awake()
    {
        data = SaveSystem.LoadLogIn();
        //Panel.SetActive(true);

        //DontDestroyOnLoad(this);
    }

    public async void OnTapToStart()
    {
        if (data != null)
        {
            AutoLoginLoading.SetActive(true);
            await SignInWithUsernamePassword(user, password);
        }
    }


    async void Start()
    {
        await UnityServices.InitializeAsync();
        //await AuthenticationService.Instance.ClearSessionToken();
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(AuthenticationService.Instance.IsSignedIn);
        if (data != null)
        {
            user = data.username;
            password = data.pw;

        }

    }

    public async void SignIn()
    {
        await signInAnonymous();
    }

    public async void SignUpUserNamePw()
    {

        await SignUpWithUsernamePassword(_username.text, _pw.text);

    }

    public async void LogIn()
    {
        await SignInWithUsernamePassword(_Username_login.text, _pw_login.text);
    }

    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print("Sign in Success");
            print("Player ID: " + AuthenticationService.Instance.PlayerId);
            LogTxt.text = "Player ID: " + AuthenticationService.Instance.PlayerId;
        }
        catch (AuthenticationException ex)
        {

            print("sign in failed!");
            // Debug.LogException(ex);
        }
    }

    async Task SignUpWithUsernamePassword(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            // Debug.Log("SignUp is successful.");
            SaveSystem.SaveLogIn(username, password);

            //SceneManager.LoadScene(1);
            loading.LoadScene(1);
            LogedInSFX.Play();


        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            // Debug.LogException(ex);

            LogInPageErrorPopUp.SetActive(true);
            AutoLoginLoading.SetActive(false);
            ErrorMessage_LogIn.text = ex.Message;
            ErrorSFX.Play();
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            // Debug.LogException(ex);
            LogInPageErrorPopUp.SetActive(true);
            AutoLoginLoading.SetActive(false);
            ErrorMessage_LogIn.text = ex.Message;
            ErrorSFX.Play();
        }
    }

    async Task SignInWithUsernamePassword(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            // Debug.Log("SignIn is successful.");
            SaveSystem.SaveLogIn(username, password);
            loading.LoadScene(1);
            //SceneManager.LoadScene(1);
            //LogedInSFX.Play();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            // Debug.LogException(ex);
            //Debug.Log(ex.ErrorCode == 400);
            // Debug.Log("authFail:");
            LogInPageErrorPopUp.SetActive(true);
            AutoLoginLoading.SetActive(false);
            ErrorMessage_LogIn.text = ex.Message;
            ErrorSFX.Play();

        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            //Debug.LogException(ex);
            //Debug.Log("requestFail:");
            //Debug.Log(ex.Message);
            LogInPageErrorPopUp.SetActive(true);
            AutoLoginLoading.SetActive(false);
            ErrorMessage_LogIn.text = ex.Message;
            ErrorSFX.Play();


        }
    }

    async Task AddUsernamePasswordAsync(string username, string password) //DONT USE THIS FUNCTION
    {
        try
        {
            await AuthenticationService.Instance.AddUsernamePasswordAsync(username, password);
            // Debug.Log("Username and password added.");
            Panel.SetActive(false);
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            // Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            // Debug.LogException(ex);
        }
    }

    async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
            // Debug.Log("Password updated.");
            string username = SaveSystem.LoadLogIn().username;
            SaveSystem.SaveLogIn(username, newPassword);

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            // Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            // Debug.LogException(ex);
        }
    }


}
