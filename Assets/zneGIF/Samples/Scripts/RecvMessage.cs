using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
[RequireComponent(typeof(AudioSource))]
public class RecvMessage : MonoBehaviour {

    GUITexture _gui_texture;
    AudioSource _audio;
    public GUIText _gui_text;

	// Use this for initialization
	void Start () {
        _gui_texture = GetComponent<GUITexture>();
        _audio = GetComponent<AudioSource>();
	}
	
	
    // If check on to 'Send Message On Frame' Of Inspector, 
    // Recv message from zneGifAnimation
    void OnGIF_SetFrame(zneGIF.Frame frame)
    {
        // Texture in frame
        _gui_texture.texture = frame._texture;

        // Play sound by comment
        if (frame._comment != null)
        {
            _gui_text.text = "Recv comment '" + frame._comment + "' at " + (frame._index + 1) + " frame!!";
            _audio.Play();
        }

    }
}

