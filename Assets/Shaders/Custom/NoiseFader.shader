Shader "Unlit/NoiseFader"
{
	Properties
	{
		[PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        _Randomness("_Randomness", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
            #include "../noiseutils.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
            float _Randomness;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;

				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed a = fbm(i.uv * _Randomness);
				fixed4 col = i.color;
				col.a = a <= i.color.a ? 1 : 0;
				col.a *= ceil(i.color.a);
				return col;
			}
			ENDCG
		}
	}
}
