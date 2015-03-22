using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        const string projectId = "d1f9f021-f08d-4c81-9979-ecbad58b42b8";
        UnityAnalytics.StartSDK(projectId);
        UnityAnalytics.Transaction("12345abcde", 0.99m, "USD", null, null);
        int totalPotions = 5;
        int totalCoins = 100;
        UnityAnalytics.CustomEvent("gameOver", new Dictionary<string, object>
        {
            { "potions", totalPotions },
            { "coins", totalCoins }
        });

        SexEnum gender = SexEnum.F;
        UnityAnalytics.SetUserGender(gender);
        int birthYear = 2014;
        UnityAnalytics.SetUserBirthYear(birthYear);
    }

}