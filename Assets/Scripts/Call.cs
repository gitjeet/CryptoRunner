using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Call : MonoBehaviour
{
    private void Awake()
    {
        AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.ACTIVITY_RECOGNITION");
        if (result == AndroidRuntimePermissions.Permission.Granted)
        {
            Debug.Log("We have permission to access the stepcounter");
            Debug.LogError("PERMISSION GRANTED");
        }
        else
        {

            Debug.LogError("NO PERMISSION");

            Debug.Log("Permission state: " + result); // No permission

        }
    }
}
