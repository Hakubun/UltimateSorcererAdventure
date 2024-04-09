using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using System.Threading.Tasks;
using System.IO;


public class AccountDeletion : MonoBehaviour
{
    public GameObject confirmationDialog;
    
    public void ShowConfirmationDialog()
    {
        confirmationDialog.SetActive(true);
    }

    public async void DeleteAccount()
    {
        try
        {
            await AuthenticationService.Instance.DeleteAccountAsync();
            //Debug.Log("Account successfully deleted.");

            // Clear all PlayerPrefs data
            PlayerPrefs.DeleteAll();
            ClearAllFilesInPersistentDataPath();
            // Additional logic for post-deletion

            SceneManager.LoadScene("0_Authentication");
        }
        catch (AuthenticationException authEx)
        {
            Debug.LogError($"Authentication error: {authEx.Message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
        }

        confirmationDialog.SetActive(false);
    }

    public void OnCancelDelete()
    {
        confirmationDialog.SetActive(false);
    }

    public void OnConfirmDeletion()
    {
        DeleteAccount();
    }

    private void ClearAllFilesInPersistentDataPath()
    {
        string persistentDataPath = Application.persistentDataPath;

        // Check if the directory exists
        if (Directory.Exists(persistentDataPath))
        {
            // Get all files in the directory
            string[] files = Directory.GetFiles(persistentDataPath);

            // Delete each file
            foreach (string file in files)
            {
                File.Delete(file);
            }

            //Debug.Log("All files in persistent data path have been removed.");
        }
        else
        {
            // Debug.LogWarning("Persistent data path directory does not exist.");
        }
    }

}
