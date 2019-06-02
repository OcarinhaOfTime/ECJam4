Shader "VShaders/UI/TiledSprite"
{
	Properties
	{		
        [PerRendererData]_MainTex("Main Texture", 2D) = "white" {}
        [PerRendererData]_ScaleOffset("Scale Offset", Vector) = (1,1,0,0)
	}
	SubShader
	{
		Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
			};

            sampler2D _MainTex;
            float4 _ScaleOffset;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				
				return tex2D(_MainTex, i.uv * _ScaleOffset.xy  +_ScaleOffset.zw) * i.color;
			}
			ENDCG
		}
	}
}
