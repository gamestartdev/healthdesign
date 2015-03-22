using UnityEngine;
using System.Collections;

public class Gif_Control : MonoBehaviour {

    public zneGifComponent[] _gifComponents;


    void OnGUI()
    {
        bool is_playing = _gifComponents[0].IsPlaying();
        float speed = _gifComponents[0].GetSpeed();

        // Speed Slider
        Rect rt = new Rect(Screen.width/4, Screen.height - Screen.height / 5,
                           Screen.width/2, Screen.height / 10);

        speed = GUI.HorizontalSlider(rt, speed, 0, 3);

        foreach (zneGifComponent anim in _gifComponents)
            anim.SetSpeed(speed);

        rt.width = Screen.width / 4;
        rt.height /= 2;
        rt.y += rt.height / 2; 
        GUI.Box(rt, "Speed: " + speed);


        rt = new Rect(0, Screen.height - Screen.height / 10, 
                      Screen.width / 10, Screen.height / 10);


        // Buttons
        if (is_playing)
        {
            rt.x = Screen.width / 2 - rt.width * 2 ;
            if (GUI.Button(rt, "STOP"))
            {
                foreach (zneGifComponent anim in _gifComponents)
                    anim.Stop();
                
            }

            rt.x = Screen.width / 2 + rt.width;
            if (GUI.Button(rt, "PAUSE"))
            {
                foreach (zneGifComponent anim in _gifComponents)
                    anim.Pause();

            }
        }
        else
        {
            rt.x = Screen.width / 2 - rt.width / 2;
            if (GUI.Button(rt, "PLAY"))
            {
                foreach (zneGifComponent anim in _gifComponents)
                    anim.Play();
            }
        }
    }
}
