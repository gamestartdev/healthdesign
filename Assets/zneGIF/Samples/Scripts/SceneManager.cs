using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public int _prevLevel = -1;
    public int _nextLevel = -1;
    public string _page;

    public float updateInterval = 0.5F;
    private float fps = 0; // FPS
    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval


	// Use this for initialization
	void Start () 
    {
        timeleft = updateInterval;  
	}
	
	
    // Update is called once per frame
	void Update () 
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0f )
        {
            // display two fractional digits (f2 format)
            fps = accum / frames;
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
	}

    void OnGUI()
    {
        Rect rt = new Rect(0, 0, Screen.width / 7, Screen.height / 7);
        GUI.Box(rt, System.String.Format("{0:F2} FPS", fps) + "\n" + _page);

        // Previous Button
        rt.y = Screen.height - rt.height;
        if( _prevLevel != -1 && GUI.Button( rt, "<< PREV" ) )
        {
            Application.LoadLevel( _prevLevel );
        }
        

        // Next Button
        rt.x = Screen.width - rt.width;
        if( _nextLevel != -1 && GUI.Button( rt, "NEXT >>" ) )
        {
            Application.LoadLevel(_nextLevel);
        }

#if !UNITY_WEBPLAYER
        // Quit button
        rt.y = 0;
        if (GUI.Button(rt, "Quit"))
        {
            Application.Quit();
        }
#endif
    }

}
