Shader "zneGIF/2D Shadow" {
	Properties {
		_Color ("Color Tint", Color) = (1,1,1,1)
		_MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white"
	}
	Category {
	   Lighting On
	   ZWrite Off
	   Cull Front
	   Blend SrcAlpha OneMinusSrcAlpha
	   Tags {Queue=Transparent}
	   SubShader {
            Material {
               Diffuse [_Color]
	           Emission [_Color]
            }
            Pass {
	           SetTexture [_MainTex] {
	                  Combine Texture * Primary, Texture * Primary
                }
            }
        } 
    }
}