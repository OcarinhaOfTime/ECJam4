Shader "VShaders/UI/OutlineDissolve"
{
	Properties
	{		
        [PerRendererData]_MainTex("Main Texture", 2D) = "white" {}
        _OutlineDist ("Outline Sample Distance", Range(0, 50)) = 2.0
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _Randomness("_Randomness", Float) = 1
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
            #include "../noiseutils.cginc"

            sampler2D _MainTex;
            float2 _MainTex_TexelSize;
            float _OutlineDist;
            fixed4 _OutlineColor;
            float _Randomness;

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
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.color = v.color;
				return o;
			}

            float Outline(float2 uv, float size){
                float2 disc[16] =
				{
					float2(0, 1),
					float2(0.3826835, 0.9238796),
					float2(0.7071069, 0.7071068),
					float2(0.9238796, 0.3826834),
					float2(1, 0),
					float2(0.9238795, -0.3826835),
					float2(0.7071068, -0.7071068),
					float2(0.3826833, -0.9238796),
					float2(0, -1),
					float2(-0.3826835, -0.9238796),
					float2(-0.7071069, -0.7071067),
					float2(-0.9238797, -0.3826832),
					float2(-1, 0),
					float2(-0.9238795, 0.3826835),
					float2(-0.7071066, 0.707107),
					float2(-0.3826834, 0.9238796)
				};

                float max_alpha = 1;
                for(int k=0; k<16; k++){
                    float sample_alpha = tex2D(_MainTex, uv + disc[k] * _MainTex_TexelSize * size).a;
                    max_alpha = min(sample_alpha, max_alpha);
                }

                return max_alpha;
            }
			
			fixed4 frag (v2f i) : SV_Target
			{				
                float d =  fbm(i.uv * _Randomness);
                clip(i.color.a - d);
                fixed4 vcol = i.color;
                vcol.a = 1;
				fixed4 col = tex2D(_MainTex, i.uv) * vcol;
                float outline = Outline(i.uv, _OutlineDist);
                col.rgb = lerp(_OutlineColor, col, outline * col.a);
                col.a = max(col.a, outline);
                return col;
			}
			ENDCG
		}
	}
}
