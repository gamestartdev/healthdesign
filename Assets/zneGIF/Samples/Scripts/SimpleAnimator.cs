using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class SimpleAnimator : MonoBehaviour
{
    public zneGifAnimation[] _animations;
    Dictionary<string, zneGifAnimation> _anim_dic = new Dictionary<string, zneGifAnimation>();
    zneGifAnimation _current_animation = null;
    void Awake()
    {
        // Make animation dictionary
        if (_animations != null && _animations.Length > 0 )
        {
            foreach (zneGifAnimation anim in _animations)
            {
                if (anim._name == null && anim._name == "")
                {
                    Debug.LogError("zneGifAnimation name is none. zneGifAnimation need name in SimpleAnimator!");
                    continue;
                }

                anim._name = anim._name.ToLower();
                anim._autoChangeTexture = true;
                anim.Stop();
                _anim_dic.Add(anim._name, anim);
            }

            // Set first animation to start animation
            SetAnimation(_animations[0]._name);
        }
    }

    public void SetAnimation(string name)
    {
        zneGifAnimation new_anim = null;
        if( _anim_dic.TryGetValue(name, out new_anim) )
        {
            if( new_anim != _current_animation && _current_animation != null )
            {
                _current_animation.Stop();
            }

            new_anim.Play();
            _current_animation = new_anim;
        }
    }

    public zneGifAnimation GetCurrentAnimation()
    {
        return _current_animation;
    }
}