Shader "VShaders/UI/EmissiveImage"
{
	Properties
	{		
        [PerRendererData]_MainTex("Main Texture", 2D) = "white" {}
        _Emission("Emission", Range(0, 10)) = 0

        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil ("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
		[HideInInspector]_ColorMask ("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

        Stencil
         {
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

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
				return col * (1 + _Emission * col.a);
			}
			ENDCG
		}
	}
}
