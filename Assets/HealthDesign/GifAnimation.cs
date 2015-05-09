using System;
using UnityEngine;
using System.IO;
using Gif2Textures;
using System.Collections;
using System.Collections.Generic;

public enum PlayerAnimState
{
    IDLE,
    MOVE,
    JUMP,
}

struct Frame
{
    public Sprite Sprite;
    public float Delay;
}

class Gif {
    public readonly string url;
    public GifFrames Frames;

    public Gif(string url)
    {
        this.url = url;
        this.Frames = null;
    }

    public IEnumerator Download() {
        Debug.Log(url);
        WWW www = new WWW(this.url);
        yield return www;
        var memoryStream = new MemoryStream(www.bytes);
        this.Frames = new GifFrames();
        this.Frames.Load(memoryStream, true);
    }

    public Frame GetNextFrame()
    {
        float delay = 0.5f;
        Sprite sprite = null;
        if (this.Frames != null)
        {
            Texture2D currentFrame;
            this.Frames.GetNextFrame(out currentFrame, out delay);
            sprite = Sprite.Create(currentFrame, new Rect(0, 0, currentFrame.width, currentFrame.height), new Vector2(0.5f, 0.5f), 600f);
        }
        return new Frame { Delay = delay, Sprite = sprite };
    }
}

public class GifAnimation : MonoBehaviour
{
    public PlayerAnimState CurrentState
    {
        get { return _currentState; }
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
                NextFrame();
            }
        }
    }
    private PlayerAnimState _currentState;

    public string idle = "http://piskel-imgstore-b.appspot.com/img/ef558485-d037-11e4-bba4-5bfbb68c2d8f.gif";
    public string move = "http://piskel-imgstore-b.appspot.com/img/09e606fa-d038-11e4-911f-5bfbb68c2d8f.gif";
    public string jump = "http://piskel-imgstore-b.appspot.com/img/1d45404f-d038-11e4-be50-5bfbb68c2d8f.gif";
    
    Gif[] _gifs;
    SpriteRenderer _spriteRenderer;

    IEnumerator Start() {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(!_spriteRenderer)
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _gifs = new[] { new Gif(idle), new Gif(move), new Gif(jump) };

        foreach (var gif in _gifs)
        {
            StartCoroutine(gif.Download());
        }

        while (true)
        {
            yield return new WaitForSeconds(NextFrame().Delay);
        }
    }

    private Frame NextFrame()
    {
        var frame = _gifs[(int) _currentState].GetNextFrame();
        if (_spriteRenderer)
            _spriteRenderer.sprite = frame.Sprite;
        return frame;
    }

    public void UpdateIdle(string value) {
        UpdateGif(value, 0);
    }
    public void UpdateMove(string value) {
        UpdateGif(value, 1);
    }
    public void UpdateJump(string value) {
        UpdateGif(value, 2);
    }

    private void UpdateGif(string value, int index) {
        var gif = new Gif(value);
        StartCoroutine(gif.Download());
        _gifs[index] = gif;
    }
}
