------------------------------------------------------
zneGIF - GIF 2D Animation for Unity
------------------------------------------------------

GIF�� �۰� �����մϴ�.

zneGIF�� GIF Animation �� Unity���� ��� �����ϰ� ���ݴϴ�.

����:
zneGIF�� Unity3D 4.1.x ���� �׽�Ʈ �Ǿ����ϴ�.
Unity3D 3.5.x ������ �����ų� �������� �������� �ֽ��ϴ�.
zneGIF�� GIF89a �� �����մϴ�.


------------------------------------------------------
����
------------------------------------------------------

1. Ŭ���� ��ӵ�

zneClip ------ zneGifAnimClip
          |___ zneGifMovieClip



MonoBehaviour -- zneGifComponent ----- zneGifAnimation
                                   |__ zneGifMovie

2. ���ϵ�

�����Ǵ� ������Ʈ�� zneGifAnimation, zneGifMovie �̷��� �ΰ��� �Դϴ�.
�ִϸ��̼��� �����Ǿ�� �ϴ� GameObject �� zneGifAnimation �Ǵ� zneGifMovie��
�߰��ϰ� �ν����Ϳ� �ʿ��� ������ ���ָ� GIF �ִϸ��̼��� �� �� �ֽ��ϴ�.

zneGifAnimation �� GIF�� ��� �������� Texture2D ���·� �����
�ִϸ��̼� ��ŵ�ϴ�. 
�� ������Ʈ�� Ư¡�� ���� ����ӵ� �Դϴ�. 
������ �������� �ʹ� ���� GIF�� �� ��ŭ ���� �ؽ��ĸ� ����� ������
���� �޸𸮸� ���� ����ϰ� �˴ϴ�.
zneGifAnimation�� 2D������ ������Ʈ�� ����ϱ⿡ �����մϴ�.

zneGifMovie �� GIF �ִϸ��̼��� �����ϴµ� �� �� ���� Texture2D ���� ����մϴ�.
�׷��� ���� �������� ���� GIF�� ���� �޸𸮷� ������ �����մϴ�.
������ ������ �� ������ �ؽ��ĸ� �����ϱ� ������ zneGifAnimation ����
CPU ��뷮�� �����ϴ�.
zneGifMovie�� ������Ʈ�� ó�� ª�� ������ ����� �ʿ��� ���� ����ϸ� ������ �մϴ�.

zneGifAnimation �� zneGifMovie�� ���������� �ε������� ĳ���ϱ� ������
���� GIF������ ���� ������Ʈ�� �ε��ϰų� ������ �ε��ص� �����̰� ������ �ʽ��ϴ�.
 
�׹ۿ� �ٸ� ���ϵ��� ������ �����ϰڽ��ϴ�.

[Core]
-zneGIF.dll
GIF�� �ε��ϴ� �ٽɱ���� �մϴ�.

[Internal Scripts]
zneClip.cs:
zneGIF.dll �� Stream �� �����Ͽ� GIF�� �ε��ϰ� �ϴ� ������ �մϴ�. 
zneGifAnimClip �� zneGifMovieClip �� �θ�Ŭ���� �Դϴ�.

zneGifAnimClip.cs:
�ε� �� GIF ����Ÿ�� ������ Texture2D�� ����ϴ�.

zneGifMovieClip.cs:
�ε� �� GIF ����Ÿ�� ������ Texture2D�� ���������� ������Ʈ �մϴ�. 

zneGifFrame.cs:
zneGifAnimClip �� zneGifMovieClip ���� ������ ����������(Texture2D,Delay time, comment)�� �����մϴ�. 

zneGifComponent.cs:
zneGifAnimation �� zneGifMovie �� �������� ������ �ִ� �θ� Ŭ���� �Դϴ�.

[Editor]
Editor_zneGifAnimation.cs:
zneGifAnimation �� Ŀ���� �ν����� �Դϴ�.

Editor_zneGifMovie.cs:
zneGifmovie �� Ŀ���� �ν����� �Դϴ�.

[Samples]
zneGifAnimation �� zneGifMovie �� Ȱ�� ������ ��� �ֽ��ϴ�.


�� ���ϵ��� Ŀ���͸���¡�Ͽ� �ڽŸ��� GIF �ִϸ��̼� �ý����� ������� �������Դϴ�.



------------------------------------------------------
��� ���
------------------------------------------------------
1. GameObject �� zneGifAnimation �Ǵ� zneGifMovie ������Ʈ�� �߰��մϴ�.
2. �ν����Ϳ� 'Gif In Streaming Assets' �׸� ����� GIF�� ����� �����ϴ�.
3. �ִϸ��̼ǰ� �޼����� �޾ƾ��ϴ� Ÿ���� �����մϴ�.
4. ��Ÿ �ʿ��� ������ �ν����Ϳ��� �����մϴ�.
5. �����ϸ� GIF �ִϸ��̼��� ���Ǽ� �����̴ϴ�.

[�ν����� ����]

Name: 
GifComponent �̸��� ������ �� �ֽ��ϴ�.

Gif In Streaming Assets : 
�����ϰ��� �ϴ� GIF�� �̰����� ������� �ش� GIF�� �����˴ϴ�.
�ٸ� StreamingAssets ������ �ִ� GIF�� �����մϴ�.
(�ٸ� ������� GIF�� �ε��ϰ� �ʹٸ� 'Custom Load'������ �����ϼ���)

Filename: 
�� �׸��� �б������Դϴ�. GIF�� �����Ǹ� �̰��� �����̸��� ��Ÿ���ϴ�.

Load Method: 
<NO_FRAME_AT_FIRST>
���� �ε� �� �ּҿ� ������ �� �н��ϴ�. 
�������� ���� GIF�ִϸ��̼��� �ε��Ҷ� �����մϴ�.

<ONE_FRAME_AT_FIRST>
���� �ε� �� ù �����ӱ��� �н��ϴ�.
�� ���� ����Ʈ�Դϴ�.

<ALL_FRAME_AT_FIRST>
���� �ε� �� ��� �������� �н��ϴ�.
�߰� �ε����� �ѹ��� ��� �о� ���� �Ѵٸ� �� ���� ����ϸ� �˴ϴ�.

Targets: 
�ִϸ��̼��� ����Ǿ�� �ϴ� Ÿ�ٵ��� �����մϴ�.

Auto Change Texture: 
�� ���� true�̸� �� ������ ���� ������ Targets ���� GUITexure �� 
���͸����� mainTexture �� GIF �̹����� ������ �ݴϴ�.

Send Message On Frame: 
�� ���� true �̸� �� ������ ���� ������ Targets ���� "OnGIF_SetFrame" �޼����� �Բ� zneGifFrame �� ���޵˴ϴ�.
GIF �����ӿ� Comment�� ������ zneGifFrame �� ����Ǿ� ���� ���޵˴ϴ�.

Play On Awake: 
�� ���� true�̸� ���� �� Play()�� ȣ��˴ϴ�.

Loop Type: 
���� Ÿ���� �����մϴ�.

Speed: 
�ִϸ��̼� �ӵ��� �����մϴ�. (�⺻���� 1�Դϴ�.)


-------------------------------------------------- ----
�����մϴ�
-------------------------------------------------- ----
zneGIF�� �̿��� �ּż� �����մϴ�.
���������� Unity3D Ŀ�´�Ƽ�� �̿��� �ֽø�,
���� Ȯ�� �ϴµ��� �亯�ϵ��� �ϰڽ��ϴ�.
