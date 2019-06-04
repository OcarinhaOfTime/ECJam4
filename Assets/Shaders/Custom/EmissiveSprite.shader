Shader "VShaders/UI/EmissiveSprite"
{
	Properties
	{		
        [PerRendererData]_MainTex("Main Texture", 2D) = "white" {}
        [PerRendererData]_Emission("Emission", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "../vutils.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                half4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
                half4 color : COLOR;
			};

			sampler2D _MainTex;
            half _Emission;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.color = v.color;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{				
                half4 col = i.color * tex2D(_MainTex, i.uv);
				return col * (1 +_Emission * col.a);
			}
			ENDCG
		}
	}
}
