Shader "VShaders/Shapes/CustomRect"
{
	Properties
	{
        [PerRendererData] _MainTex("Main Texture", 2D) = "white"{}
        _PointsAB("Points A B", Vector) = (0, 1, 1, 1)
        _PointsCD("Points C D", Vector) = (1, 0, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend DstColor SrcColor
        ZWrite Off
		AlphaToMask On
		Cull Off

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
                fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
			};
			
            float4 _PointsAB;
            float4 _PointsCD;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.color = v.color;
				return o;
			}

			//d=(x−x1)(y2−y1)−(y−y1)(x2−x1)

			float line_sign(float2 a, float2 b, float2 p){
				float d = (p.x - a.x) * (b.y - a.y) - (p.y - a.y) * (b.x - a.x);
				return d;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 p = i.uv;

				float2 a = _PointsAB.xy;
				float2 b = _PointsAB.zw;
				float2 c = _PointsCD.xy;
				float2 d = _PointsCD.zw;

				float ab = step(0, line_sign(a, b, p));
				float bc = step(0, line_sign(b, c, p));
				float cd = step(0, line_sign(c, d, p));
				float da = step(0, line_sign(d, a, p));

				float alpha = ab * bc * cd * da;

                fixed4 col = i.color;
				clip(alpha - .5);

                return col;
			}
			ENDCG
		}
	}
}
