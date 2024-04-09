using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.SceneManagement;

public class SignOutButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void SignOut()
    {
        // Assuming you have a method in your authentication service to handle sign-out
        AuthenticationService.Instance.SignOut();

        // Optionally, you might want to perform additional actions after signing out
        // For example, load a login scene or update the UI
        ClearAllFilesInPersistentDataPath();
        SceneManager.LoadScene(0);
    }

    void ClearAllFilesInPersistentDataPath()
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
