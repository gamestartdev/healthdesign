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

[AddComponentMenu("zneGIF/zneGIFAnimation")]
[ExecuteInEditMode]
public class zneGifAnimation : zneGifComponent
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
            zneGIF.AnimClip.DelClipInCache((zneGIF.AnimClip)_clip);
            _clip = null;
        }
    }

    public zneGIF.AnimClip GetAnimClip()
    {
        return (zneGIF.AnimClip)_clip;
    }
     
    internal override void Update_Once()
    {
        if (_frame != null && _frame._is_last_frame)
        {
            Pause();
            return;
        }

        zneGIF.Frame next_frame = GetAnimClip().GetNextFrame(_frame, _frame_time);

        if (next_frame != null)
            SetFrame(next_frame);
    }

    internal override void Update_ReverseOnce()
    {
        if (_frame != null && _frame._index == 0)
        {
            Pause();
            return;
        }

        zneGIF.Frame prev_frame = GetAnimClip().GetPreviousFrame(_frame, _frame_time);

        if (prev_frame != null)
            SetFrame(prev_frame);
    }

    internal override void Update_Loop()
    {
        zneGIF.Frame next_frame = GetAnimClip().GetNextFrame(_frame, _frame_time);

        if (next_frame != null)
            SetFrame(next_frame);
    }

    internal override void Update_ReverseLoop()
    {
        zneGIF.Frame prev_frame = GetAnimClip().GetPreviousFrame(_frame, _frame_time);

        if (prev_frame != null)
            SetFrame(prev_frame);
    }

    internal override void Update_PingPong()
    {
        if (_pingpong_way_normal)
        {
            zneGIF.Frame next_frame = GetAnimClip().GetNextFrame(_frame, _frame_time);

            if (next_frame != null)
            {
                SetFrame(next_frame);

                // change way
                if (next_frame._is_last_frame)
                    _pingpong_way_normal = !_pingpong_way_normal;
            }
        }
        else
        {
            zneGIF.Frame prev_frame = GetAnimClip().GetPreviousFrame(_frame, _frame_time);

            if (prev_frame != null)
            {
                SetFrame(prev_frame);

                // change way
                if (prev_frame._index == 0)
                    _pingpong_way_normal = !_pingpong_way_normal;
            }
        }
        
    }

    internal override void Update_ReversePingPong()
    {
        Update_PingPong();
    }

    // Load Gif Clip from filename    
    public override bool LoadGifClipByFilename(string filename)
    {
        
        if (filename != null)
        {
            _error = null;
            _frame_time = 0.0f;
            _frame = null;

            _clip = zneGIF.AnimClip.Create(filename, _loadMethod, _textureFilterMode, _textureWrapMode);
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

            _clip = zneGIF.AnimClip.Create(stream, _loadMethod, _textureFilterMode, _textureWrapMode);
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
            _clip = zneGIF.AnimClip.Create(url, _loadMethod, _textureFilterMode, _textureWrapMode);
        }

        return (_clip != null && !_clip.HasError());
    }

    public override void Play()
    {
        _is_playing = true;
    }

}