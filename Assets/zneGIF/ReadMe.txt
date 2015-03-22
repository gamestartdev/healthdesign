
Sorry about that.My English is not good.
Thank you for your understanding.

------------------------------------------------------
zneGIF - GIF 2D Animation for Unity
------------------------------------------------------

GIF is a small and easy.

You can using GIF animated in Unity3D by zneGIF.

Note:
zneGIF was tested on Unity3D 4.1.x.
Maybe zneGIF is slow or not run in Unity3D 3.5.x. 
zneGIF supports only GIF89a.


------------------------------------------------------
Contents
------------------------------------------------------


1. Class inheritance

zneClip ------ zneGifAnimClip
          |___ zneGifMovieClip



MonoBehaviour -- zneGifComponent ----- zneGifAnimation
                                   |__ zneGifMovie


2. Component

There are two components zneGifAnimation and zneGifMovie.

zneGifAnimation makes textures for all frame of GIF to animation.
The advantage of this is a faster playback speed.
But the more animation frames, the more needs video memory.
It is good for 2D game or GUI.

zneGifMovie makes just one texture to animation.
The advantage of this is to using small memory.
But this is need more CPU costs more than zneGifAnimation.
It is good for short video playback. 
For example, the game intro or ending video.


------------------------------------------------------
How to use
------------------------------------------------------
1. Add zneGifAnimation or zneGifMovie to GameObject.
2. GIF drag-and-drop from StreamingAssets to 'Gif In Streaming Assets' in inspector.
3. Setup targets in inspector.
4. Run!!! you can see GIF animated.


------------------------------------------------------
Inspector items
------------------------------------------------------

Name: The name of GifComponent.

Gif In Streaming Assets: You have to drag-and-drop GIF from StreamingAssets to this.
If you want to load GIF from another location. you can see 'Custom load' sample.

Load Method: Decide to How much to loading GIF at first.

Targets: Targets to receive the message or apply texture by GifComponent.

Auto Change Texture: If it is true, GifComponent changes textures of targets at GIF frame move.

Send Message On Frame:  If it is true, GifComponent send "OnGIF_SetFrame" message with zneGifFrame to targets at GIF frame move.

Play On Awake:  If it is true, Call Play() of GifComponent on Awake.

Loop Type: Loop type of animation.

Speed: Speed of animation. default is 1.0


Note.
If you see samples, you will be more understand it.

------------------------------------------------------
Thank you
------------------------------------------------------

If you have question, using community of Unity3D.
I will check commuity and answer to you.

