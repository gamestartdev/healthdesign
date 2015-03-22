using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour
{
    void Start()
    {
        const string projectId = "d1f9f021-f08d-4c81-9979-ecbad58b42b8";
        UnityAnalytics.StartSDK(projectId);
    }

}