using System;
using UnityEngine;
using System.IO;
using Gif2Textures;
using System.Collections;

public enum PlayerAnimState
{
    IDLE,
    JUMP,
    MOVE,
}

struct Frame
{
    public Sprite Sprite;
    public float Delay;
}

class Gif
{
    public readonly string url;
    public GifFrames Frames;

    public Gif(string url)
    {
        this.url = url;
        this.Frames = null;
    }

    public IEnumerator Download()
    {
        WWW www = new WWW(this.url);
        yield return www;
        this.Frames = new GifFrames();
        this.Frames.Load(new MemoryStream(www.bytes), true);
    }

    public Frame GetNextFrame()
    {
        float delay = 0.5f;
        Sprite sprite = null;
        if (this.Frames != null)
        {
            Texture2D currentFrame;
            this.Frames.GetNextFrame(out currentFrame, out delay);
            sprite = Sprite.Create(currentFrame, new Rect(0, 0, currentFrame.width, currentFrame.height), Vector2.zero, 100f);
        }
        return new Frame { Delay = delay, Sprite = sprite };
    }
}

public class GifAnimation : MonoBehaviour
{
    public PlayerAnimState CurrentState;
    public string idle = "http://piskel-imgstore-b.appspot.com/img/ef558485-d037-11e4-bba4-5bfbb68c2d8f.gif";
    public string move = "http://piskel-imgstore-b.appspot.com/img/09e606fa-d038-11e4-911f-5bfbb68c2d8f.gif";
    public string jump = "http://piskel-imgstore-b.appspot.com/img/1d45404f-d038-11e4-be50-5bfbb68c2d8f.gif";

    IEnumerator Start()
    {
        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
        Gif[] _gifs = new[] { new Gif(idle), new Gif(move), new Gif(jump) };

        foreach (var gif in _gifs)
        {
            StartCoroutine(gif.Download());
        }

        while (true)
        {
            var frame =  _gifs[(int)CurrentState].GetNextFrame();
            renderer.sprite = frame.Sprite;
            yield return new WaitForSeconds(frame.Delay);
        }
    }


}
