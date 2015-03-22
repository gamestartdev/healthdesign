using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class LoadingInfo : MonoBehaviour {

    public zneGifComponent _gifComponent;
    GUIText _gui_text;

	// Use this for initialization
	void Start () 
    {
        _gui_text = GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {

        if( _gifComponent == null )
            return;

        if (_gifComponent.IsFileOpening())
        {

            string text = string.Format("Downloading {0:F1} %", _gifComponent.GetFileOpeningProgress() * 100.0f);
            _gui_text.text = text;
        }

        else if( _gifComponent._clip != null )
        {
            if (_gifComponent._clip.HasError())
            {
                _gui_text.text = "Error: " + _gifComponent._clip.GetError();
            }
            else if (_gifComponent._clip.IsLoading())
            {
                _gui_text.text = "Loading: " + _gifComponent._clip.GetFrameCount() + " frame";
            }
            else if (_gifComponent._clip.IsDone())
            {
                int w = _gifComponent._clip.GetWidth();
                int h = _gifComponent._clip.GetHeight();
                int cf = _gifComponent._frame != null ? (_gifComponent._frame._index + 1) : 0;
                int tf = _gifComponent._clip.GetFrameCount();
                _gui_text.text = "Done (" + w + "X" + h + ") [ frame " + cf + "/" + tf + " ]";
                _gui_text.text += "\n" + tf + " Textures created!!";

                if (_gifComponent.IsPlaying())
                    _gui_text.text += "\n[State: Playing]";
                else if( _gifComponent._frame == null )
                    _gui_text.text += "\n[State: Stop]";
                else
                    _gui_text.text += "\n[State: Pause]";

            }
        }
	}
}
