using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAlarmManager : MonoBehaviour
{
    public static bool PushAlarm = false;


    public static void CallNotification()
    {
        //Just a placeholder function until we get Notifications set up. Then we can call
        //SendNotification(string message) from this function.
        if (PushAlarm == true)
        {
            SendNotification("Test Notification: ");
        }
    }


    public static void SendNotification(string message)
    {
        #if UNITY_ANDROID
        // Android push notification implementation using Firebase Cloud Messaging (FCM)
        // You need to set up FCM in your project and handle the logic for sending push notifications
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass firebaseMessaging = new AndroidJavaClass("com.example.FirebaseMessagingHandler");
        firebaseMessaging.CallStatic("SendNotification", unityActivity, message);
       
        #elif UNITY_IOS
        // iOS push notification implementation using a native plugin or a third-party service
        // You need to set up push notifications for iOS and handle the logic for sending push notifications
        //IOSPushNotification.SendNotification(message);
        
        #else
        Debug.Log("Push notifications not supported on this platform");
        #endif
    }
}
