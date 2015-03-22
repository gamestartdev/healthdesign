using UnityEngine;
using System.Collections;

public class Gif_CustomLoad : MonoBehaviour {

    // test url = http://upload.wikimedia.org/wikipedia/commons/5/55/Tesseract.gif

    public zneGifComponent _gifComponent;
    public string _url = "http://upload.wikimedia.org/wikipedia/commons/5/55/Tesseract.gif";

    void OnGUI()
    {

        // Input text area
        Rect rt = new Rect(Screen.width/7, Screen.height - Screen.height / 4,
                           (int)((float)Screen.width/1.5f), Screen.height / 7);

        _url = GUI.TextArea(rt, _url);


        // Open button
        rt = new Rect(0, Screen.height - Screen.height / 10, 
                      Screen.width / 5, Screen.height / 10);

        rt.x = Screen.width / 2 - rt.width / 2;

        if ( !_gifComponent.IsLoading() )
        {
            if (GUI.Button(rt, "OPEN"))
            {
                _gifComponent.LoadGifClipByUrl(_url);
                _gifComponent.Play();
            }
        }
    }
}
