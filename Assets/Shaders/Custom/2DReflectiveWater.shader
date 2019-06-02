Shader "Custom/2DReflectiveWater"
{
	Properties
	{
		[PerRendererData] _MainTex("Main", 2D) = "white" {}
		_NoiseTex("Surface Noise", 2D) = "white" {}
		_Width("Wave Width", Range(4, 32)) = 8
		_SurfaceOutlineThickness("SurfaceOutlineThickness", Range(0, 1)) = 2
        _SurfaceOutlineTint ("Surface Outline Tint", Color) = (0, 0, 1, 1)
		_Freq("Wave Freq", Range(0, 100)) = 50
		_Amp("Wave Amp", Range(0, 0.1)) = .02
		_NoiseMag("Wave Randomness", Range(0, 1)) = .2

        _DisplacementMap("Refraction Map", 2D) = "white" {}
        _DisplacementMag("Refraction Magnitude", Range(0, .1)) = .01
        _Speed("Refraction Speed", Range(-2, 2)) = 1
        _ReflectionTint ("Refraction Tint", Color) = (0, 0, 1, 1)
		_Cutout("Cutout", Range(0, 2)) = 1.8
	}
	SubShader
	{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Zwrite Off
			ZTest Always
			Tags{ "RenderType" = "Transparent" "Queue" = "Transparent"}

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
				half4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 noise_uv : TEXCOORD1;
                float2 screen_uv : TEXCOORD2;
				half4 color : COLOR;
			};

            sampler2D _GlobalReflectionTex;
			sampler2D _MainTex;
			sampler2D _NoiseTex;
            sampler2D _DisplacementMap;

            half4 _ReflectionTint;
			half4 _SurfaceOutlineTint;

			float4 _NoiseTex_ST;
			float4 _MainTex_ST;
			float4 _DisplacementMap_ST;

			float _Width;
			float _Amp;
			float _Freq;
			float _NoiseMag;
            float _Speed;
            float _DisplacementMag;
			float _SurfaceOutlineThickness;
			float _Cutout;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.noise_uv = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.screen_uv = (o.vertex + 1) * .5;
				o.screen_uv = TRANSFORM_TEX(o.screen_uv, _DisplacementMap);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float noise = tex2D(_NoiseTex, i.noise_uv).r * 2 - 1;
				noise *= _NoiseMag;
				i.uv.y = i.uv.y + cos(i.uv.x * _Width + _Time.x * _Freq) * (_Amp + noise);

				if (i.uv.y > _Cutout)
					return half4(0,0,0,0);
				if (i.uv.y > (_Cutout - .1) + ((1 - _SurfaceOutlineThickness) * 0.1))
					return _SurfaceOutlineTint;

                float2 displacement = tex2D(_DisplacementMap, i.uv + _Time.x * _Speed);
				float2 diplacedUV = (displacement * 2 - 1) * _DisplacementMag;

                half4 reflex = tex2D(_GlobalReflectionTex, i.screen_uv + diplacedUV)* _ReflectionTint * _ReflectionTint.a;
				half4 surface = i.color.a * i.color * tex2D(_MainTex, i.uv);

				return  reflex + surface;
			}
			ENDCG
		}
	}
}
