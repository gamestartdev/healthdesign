// (c) Copyright 'ZNE Edu' 2013. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace zneGIF
{
   
    public class AnimClip : Clip
    {

#region AnimClip Cache System
        static Dictionary<string, AnimClip> s_clip_cache_dic = new Dictionary<string, AnimClip>();

        static internal AnimClip GetClipInCache( string key )
        {
            if (s_clip_cache_dic.ContainsKey(key))
                return s_clip_cache_dic[key];

            return null;
        }

        static internal bool AddClipInCache( string key, AnimClip clip )
        {
            if (!s_clip_cache_dic.ContainsKey(key))
            {
                s_clip_cache_dic.Add(key, clip);
                return true;
            }

            return false;
        }

        static internal bool DelClipInCache( string key )
        {
            if (!s_clip_cache_dic.ContainsKey(key))
            {
                s_clip_cache_dic.Remove(key);
                return true;
            }

            return false;
        }

        static internal bool DelClipInCache( AnimClip clip )
        {
            foreach( KeyValuePair <string, AnimClip> p in s_clip_cache_dic )
            {
                if( p.Value == clip )
                {
                    s_clip_cache_dic.Remove(p.Key);
                    return true;
                }
            }
            return false;
        }

        static public AnimClip Create( string filename, LOAD_METHOD load_method,
                                        FilterMode filter_mode = FilterMode.Bilinear,
                                        TextureWrapMode wrap_mode = TextureWrapMode.Clamp)
        {
            string key = filename + "_@" + filter_mode.ToString() + "_@" + wrap_mode.ToString();
            AnimClip clip = (AnimClip)GetClipInCache(key);
            if( clip == null )
            {
                clip = new AnimClip();

                if( clip.Load( filename, load_method, filter_mode, wrap_mode ) )
                {
                    AddClipInCache(key, clip);
                }
            }

            return clip;
        }

        static public AnimClip Create( Stream stream, LOAD_METHOD load_method,
                                          FilterMode filter_mode = FilterMode.Bilinear,
                                          TextureWrapMode wrap_mode = TextureWrapMode.Clamp)
        {
            string hash_code = stream.GetHashCode().ToString();
            string key = hash_code + "_@" + filter_mode.ToString() + "_@" + wrap_mode.ToString();

            AnimClip clip = (AnimClip)GetClipInCache( key );
            if( clip == null )
            {
                clip = new AnimClip();

                if( clip.Load( stream, load_method, filter_mode, wrap_mode ) )
                    AddClipInCache(key, clip);
            }

            return clip;
        }
#endregion




#region Values & Functions

        internal Color32[]       _pixels         = null;


        // It will override at child
        public override bool CreateNextFrame()
        {
            if (IsLoading())
            {
                _loader.AsyncNextLoad();

                if (_loader.HasError())
                {
                    OnLoadFail(_loader.GetLastError().ToString());
                    return false;
                }

                if (_loader.GetStatus() >= zneGif.STATUS.LOADED_SCREEN_DESC)
                {
                    _width = _loader._logical_screen_desc.image_width;
                    _height = _loader._logical_screen_desc.image_height;
                }

                int frame_index = _frame_count;
                if (frame_index < _loader._frames.Count)
                {
                    zneGif.Frame src_frame = _loader._frames[frame_index];
                    zneGIF.Frame dst_frame = new zneGIF.Frame();

                    if (_pixels == null)
                    {
                        _pixels = new Color32[_width * _height];
                        for (int i = 0; i < _pixels.Length; i++)
                            _pixels[i].a = 0;
                    }

                    dst_frame._index = frame_index;
                    dst_frame._comment = src_frame._CME_data.comment;
                    dst_frame._delay_time = (float)src_frame._GCE_data.delay_time * 0.01f;
                    dst_frame._texture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
                    dst_frame._texture.filterMode = _filter_mode;
                    dst_frame._texture.wrapMode = _wrap_mode;
                    dst_frame._is_last_frame = false;


                    int ix = src_frame._image.desc.image_left;
                    int iy = src_frame._image.desc.image_top;
                    int iw = src_frame._image.desc.image_width;
                    int ih = src_frame._image.desc.image_height;


                    // If disposal_method is 3, backup pixels
                    int disposal_method = src_frame._GCE_data.disposal_method;
                    Color32[] backup_pixels = null;
                    if (disposal_method == 3)
                    {
                        backup_pixels = new Color32[iw * ih];

                        for (int i = 0; i < ih; i++)
                        {
                            int src_index = (i + iy) * _width + ix;
                            int dst_index = i * iw;

                            System.Array.Copy(_pixels, src_index, backup_pixels, dst_index, iw);
                        }
                    }


                    // Make current frame pixel
                    zneGif.Color[] color_table = null;
                    if (src_frame._image.desc.local_color_table_flag)
                        color_table = src_frame._image.desc.local_color_table;
                    else
                        color_table = _loader._global_color_table;


                    int data_index = 0;
                    bool transparent = src_frame._GCE_data.transparent_color_flag;
                    int transparent_index = src_frame._GCE_data.transparent_color;
                    for (int py = iy; py < (iy + ih); py++)
                    {
                        for (int px = ix; px < (ix + iw); px++)
                        {
                            int table_index = src_frame._image.data[data_index++];

                            if (transparent && transparent_index == table_index)
                                continue;

                            zneGif.Color zneColor = color_table[table_index];

                            int pixel_index = py * _width + px;
                            _pixels[pixel_index].r = zneColor.r;
                            _pixels[pixel_index].g = zneColor.g;
                            _pixels[pixel_index].b = zneColor.b;
                            _pixels[pixel_index].a = 255;
                        }
                    }


                    dst_frame._texture.SetPixels32(_pixels);
                    dst_frame._texture.Apply(false, true);


                    if (disposal_method == 2)
                    {
                        // Make background color
                        Color32 bg_color = Color.clear;
                        int bg_color_index = _loader._logical_screen_desc.background_color;
                        if (!transparent && bg_color_index < color_table.Length)
                        {
                            zneGif.Color zne_color = color_table[bg_color_index];
                            bg_color.r = zne_color.r;
                            bg_color.g = zne_color.g;
                            bg_color.b = zne_color.b;
                            bg_color.a = 255;
                        }
                        /*
                        // Fill pixels with background color
                        for (int i = 0; i < _pixels.Length; i++)
                            _pixels[i] = bg_color;
                         */

                        for (int py = iy; py < (iy + ih); py++)
                        {
                            for (int px = ix; px < (ix + iw); px++)
                            {
                                int pixel_index = py * _width + px;
                                _pixels[pixel_index] = bg_color;
                            }
                        }

                    }
                    else if (disposal_method == 3)
                    {
                        // back to previous image
                        for (int i = 0; i < ih; i++)
                        {
                            int src_index = i * iw;
                            int dst_index = (i + iy) * _width + ix;

                            System.Array.Copy(backup_pixels, src_index, _pixels, dst_index, iw);
                        }
                    }


                    // add new frame
                    zneGIF.Frame[] new_frame = new zneGIF.Frame[frame_index + 1];

                    if (frame_index > 0)
                        System.Array.Copy(_frames, 0, new_frame, 0, frame_index);

                    new_frame[frame_index] = dst_frame;
                    _frames = new_frame;
                    _frame_count = _frames.Length;
                }

                // Is Load Complete?
                if (_loader.IsDoneWithoutError() &&
                     _frame_count == _loader._frames.Count)
                {
                    _frames[_frame_count - 1]._is_last_frame = true;
                    OnLoadComplete();
                }

                return true;
            }
            return false;
        }

        public Frame GetPreviousFrame( Frame frame, float delayed_time)
        {
            if (HasError())
                return null;

            bool time_over = true;
            int index = _frame_count;
            if (frame != null)
            {
                index = frame._index;
                time_over = (frame._delay_time <= delayed_time);
            }

            if( IsDone() )
            { 
                if (time_over)
                {
                    index--;
                    if (index < 0)
                        index = _frame_count-1;

                    return GetFrame(index);
                }
            }
            
            return null;
        }

        public Frame GetNextFrame( Frame frame, float delayed_time)
        {
            if (HasError())
                return null;

            bool time_over = true;
            int index = -1;
            if (frame != null)
            {
                index = frame._index;
                time_over = (frame._delay_time <= delayed_time);
            }

            if ( IsLoading() )
            {
                if (time_over)
                    return GetFrame(index + 1);
            }
            else
            { 
                if (time_over)
                {
                    index++;
                    if (index >= _frame_count)
                        index = 0;

                    return GetFrame(index);
                }
            }
            
            return null;
        }

#endregion


    }
}