------------------------------------------------------
zneGIF - GIF 2D Animation for Unity
------------------------------------------------------

GIF는 작고 간편합니다.

zneGIF는 GIF Animation 을 Unity에서 사용 가능하게 해줍니다.

참고:
zneGIF는 Unity3D 4.1.x 에서 테스트 되었습니다.
Unity3D 3.5.x 에서는 느리거나 구현되지 않을수도 있습니다.
zneGIF는 GIF89a 만 지원합니다.


------------------------------------------------------
내용
------------------------------------------------------

1. 클래스 상속도

zneClip ------ zneGifAnimClip
          |___ zneGifMovieClip



MonoBehaviour -- zneGifComponent ----- zneGifAnimation
                                   |__ zneGifMovie

2. 파일들

지원되는 컴포넌트는 zneGifAnimation, zneGifMovie 이렇게 두가지 입니다.
애니메이션이 구현되어야 하는 GameObject 에 zneGifAnimation 또는 zneGifMovie를
추가하고 인스펙터에 필요한 설정만 해주면 GIF 애니메이션을 볼 수 있습니다.

zneGifAnimation 은 GIF의 모든 프레임을 Texture2D 형태로 만들어
애니메이션 시킵니다. 
이 컴포넌트의 특징은 빠른 재생속도 입니다. 
단점은 프레임이 너무 많은 GIF는 그 만큼 많은 텍스쳐를 만들기 때문에
비디오 메모리를 많이 사용하게 됩니다.
zneGifAnimation는 2D게임의 오브젝트로 사용하기에 적당합니다.

zneGifMovie 은 GIF 애니메이션을 구현하는데 단 한 장의 Texture2D 만을 사용합니다.
그래서 많은 프레임을 가진 GIF라도 적은 메모리로 구현이 가능합니다.
하지만 단점은 매 프레임 텍스쳐를 갱신하기 때문에 zneGifAnimation 보다
CPU 사용량이 많습니다.
zneGifMovie는 게임인트로 처럼 짧은 동영상 재생이 필요한 곳에 사용하면 좋을듯 합니다.

zneGifAnimation 와 zneGifMovie는 내부적으로 로딩파일을 캐싱하기 때문에
같은 GIF파일을 여러 오브젝트가 로딩하거나 여러번 로딩해도 딜레이가 생기지 않습니다.
 
그밖에 다른 파일들은 간단히 설명하겠습니다.

[Core]
-zneGIF.dll
GIF를 로딩하는 핵심기능을 합니다.

[Internal Scripts]
zneClip.cs:
zneGIF.dll 에 Stream 을 전달하여 GIF를 로드하게 하는 역할을 합니다. 
zneGifAnimClip 과 zneGifMovieClip 의 부모클래스 입니다.

zneGifAnimClip.cs:
로드 된 GIF 데이타를 각각의 Texture2D로 만듭니다.

zneGifMovieClip.cs:
로드 된 GIF 데이타를 한장의 Texture2D에 지속적으로 업데이트 합니다. 

zneGifFrame.cs:
zneGifAnimClip 와 zneGifMovieClip 에서 생성된 프레임정보(Texture2D,Delay time, comment)를 저장합니다. 

zneGifComponent.cs:
zneGifAnimation 과 zneGifMovie 의 공통기능을 가지고 있는 부모 클래스 입니다.

[Editor]
Editor_zneGifAnimation.cs:
zneGifAnimation 의 커스텀 인스펙터 입니다.

Editor_zneGifMovie.cs:
zneGifmovie 의 커스텀 인스펙터 입니다.

[Samples]
zneGifAnimation 과 zneGifMovie 의 활용 예제가 들어 있습니다.


이 파일들을 커스터마이징하여 자신만의 GIF 애니메이션 시스템을 만들수도 있을것입니다.



------------------------------------------------------
사용 방법
------------------------------------------------------
1. GameObject 에 zneGifAnimation 또는 zneGifMovie 컴포넌트를 추가합니다.
2. 인스펙터에 'Gif In Streaming Assets' 항목에 사용할 GIF를 끌어다 놓습니다.
3. 애니메이션과 메세지를 받아야하는 타겟을 설정합니다.
4. 기타 필요한 설정을 인스펙터에서 설정합니다.
5. 실행하면 GIF 애니메이션을 보실수 있을겁니다.

[인스펙터 설명]

Name: 
GifComponent 이름을 설정할 수 있습니다.

Gif In Streaming Assets : 
설정하고자 하는 GIF를 이곳으로 끌어오면 해당 GIF가 설정됩니다.
다만 StreamingAssets 폴더에 있는 GIF만 가능합니다.
(다른 방법으로 GIF를 로딩하고 싶다면 'Custom Load'예제를 참고하세요)

Filename: 
이 항목은 읽기전용입니다. GIF가 설정되면 이곳에 파일이름이 나타납니다.

Load Method: 
<NO_FRAME_AT_FIRST>
최초 로딩 시 최소에 데이터 만 읽습니다. 
프레임이 많은 GIF애니메이션을 로딩할때 적당합니다.

<ONE_FRAME_AT_FIRST>
최초 로딩 시 첫 프레임까지 읽습니다.
이 값이 디폴트입니다.

<ALL_FRAME_AT_FIRST>
최초 로딩 시 모든 프레임을 읽습니다.
중간 로딩없이 한번에 모두 읽어 들어야 한다면 이 값을 사용하면 됩니다.

Targets: 
애니메이션이 적용되어야 하는 타겟들을 설정합니다.

Auto Change Texture: 
이 값이 true이면 매 프레임 마다 설정된 Targets 에게 GUITexure 나 
메터리얼의 mainTexture 를 GIF 이미지로 변경해 줍니다.

Send Message On Frame: 
이 값이 true 이면 매 프레임 마다 설정된 Targets 에게 "OnGIF_SetFrame" 메세지와 함께 zneGifFrame 가 전달됩니다.
GIF 프레임에 Comment가 있으면 zneGifFrame 에 저장되어 같이 전달됩니다.

Play On Awake: 
이 값이 true이면 실행 시 Play()가 호출됩니다.

Loop Type: 
루프 타입을 설정합니다.

Speed: 
애니메이션 속도를 설정합니다. (기본값은 1입니다.)


-------------------------------------------------- ----
감사합니다
-------------------------------------------------- ----
zneGIF를 이용해 주셔서 감사합니다.
질문사항은 Unity3D 커뮤니티를 이용해 주시면,
제가 확인 하는데로 답변하도록 하겠습니다.
