// (c) Copyright 'ZNE Edu' 2013. All rights reserved.

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

[AddComponentMenu("zneGIF/zneGIFMovie")]
[ExecuteInEditMode]
public class zneGifMovie : zneGifComponent
{

    void Awake()
    {
        GifComponent_Awake();
    }

    void Start()
    {
        GifComponent_Start();
    }

    void Update()
    {
        GifComponent_Update();
    }

    void OnDestroy()
    {
        if (_removeCashOnDestroy && _clip != null)
        {
            zneGIF.MovieClip.DelClipInCache((zneGIF.MovieClip)_clip);
            _clip = null;
        }
    }

    public zneGIF.MovieClip GetMovieClip()
    {
        return (zneGIF.MovieClip)_clip;
    }

    internal override void Update_Once()
    {
        if (_frame == null)
        {
            zneGIF.MovieClip movie_clip = GetMovieClip();

            _frame = movie_clip.GetFrame();

            // Sync speed, play state
            movie_clip.SetSpeed(_speed);

            if (_is_playing)
                movie_clip.Play();
            else
                movie_clip.Pause();
        }


        _clip.Update();

        if (_frame._index != _last_set_frame_index)
        {
            // frame changed
            SetFrame(_frame);
        }

        if (_frame._is_last_frame)
            Stop();
    }

    internal override void Update_ReverseOnce()
    {
        // zneGifMovie no support ReverseOnce
        _loopType = LoopType.Once;
        Update_Once();
    }

    internal override void Update_Loop()
    {
        if (_frame == null)
        {
            zneGIF.MovieClip movie_clip = GetMovieClip();

            _frame = movie_clip.GetFrame();

            // Sync speed, play state
            movie_clip.SetSpeed(_speed);

            if (_is_playing)
                movie_clip.Play();
            else
                movie_clip.Pause();
        }

         _clip.Update();

        if (_frame._index != _last_set_frame_index)
        {
            // frame changed
            SetFrame(_frame);
        }
    }

    internal override void Update_ReverseLoop()
    {
        // zneGifMovie no support ReverseLoop
        _loopType = LoopType.Loop;
        Update_Loop();
    }

    internal override void Update_PingPong()
    {
        // zneGifMovie no support PingPong
        _loopType = LoopType.Loop;
        Update_Loop();
    }

    internal override void Update_ReversePingPong()
    {
        // zneGifMovie no support PingPong
        _loopType = LoopType.Loop;
        Update_Loop();
    }

    // Load Gif Clip from filename    
    public override bool LoadGifClipByFilename(string filename)
    {
        if (filename != null)
        {
            _error = null;
            _frame_time = 0.0f;
            _frame = null;

            _clip = zneGIF.MovieClip.Create(filename, _loadMethod, _textureFilterMode, _textureWrapMode);
        }

        return (_clip != null && !_clip.HasError());
    }

    // Load Gif Clip by stream
    public override bool LoadGifClipByStream(Stream stream)
    {
        if (stream != null)
        {
            _error = null;
            _frame_time = 0.0f;
            _frame = null;

            _clip = zneGIF.MovieClip.Create(stream, _loadMethod, _textureFilterMode, _textureWrapMode);
        }
        return (_clip != null && !_clip.HasError());
    }

    // Load Gif Clip by Url
    public override bool LoadGifClipByUrl(string url)
    {
        if (url != null)
        {
            _error = null;
            _frame_time = 0.0f;
            _frame = null;

            _loadMethod = zneGIF.LOAD_METHOD.NO_FRAME_AT_FIRST;
            _clip = zneGIF.MovieClip.Create(url, _loadMethod, _textureFilterMode, _textureWrapMode);
        }

        return (_clip != null && !_clip.HasError());
    }

    public override void SetSpeed(float speed)
    {
        _speed = speed;

        if (_clip != null)
            GetMovieClip().SetSpeed(_speed);
    }

    public override float GetSpeed()
    {
        return _speed;
    }

    public override void Play()
    {
        _is_playing = true;

        if (_clip != null)
            GetMovieClip().Play();
    }

    public override bool IsPlaying()
    {
        _is_playing = GetMovieClip().IsPlaying();
        return _is_playing;
    }

    public override void Stop()
    {
        _is_playing = false;

        if (_clip != null)
            GetMovieClip().Stop();
    }

    public override void Pause()
    {
        _is_playing = false;

        if (_clip != null)
            GetMovieClip().Pause();
    }

   
}