using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Call : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject loadingTap;
    [SerializeField] private GameObject butTap;

    private void Start()
    {
        loadingScreen.SetActive(false); // Hide loading screen initially
        loadingTap.SetActive(true);
        butTap.SetActive(true);
    }

    // Function to be invoked when the button is clicked
    public void StartLoadingMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        loadingScreen.SetActive(true); // Show loading screen

        // Simulate loading time
        yield return new WaitForSeconds(2f); // Adjust the time as needed

        // Check and request permission
        AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.ACTIVITY_RECOGNITION");
        if (result == AndroidRuntimePermissions.Permission.Granted)
        {
            Debug.Log("We have permission to access the step counter");
            
        }
        else
        {
        
            Debug.Log("Permission state: " + result); // No permission
        }

        // Unload the current scene
        SceneManager.LoadScene("World", LoadSceneMode.Single);

    }
}   
