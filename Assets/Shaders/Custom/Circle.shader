Shader "VShaders/Shapes/Circle"
{
	Properties
	{
		_Scale("_Scale", Float) = .5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

            float _Scale;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}            
			
			fixed4 frag (v2f i) : SV_Target
			{				
				float2 st = i.uv;
                float d = 1 - distance(st, .5);
                return smoothstep(_Scale, _Scale + .01, d);				
			}
			ENDCG
		}
	}
}
