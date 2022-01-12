Shader "Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
		Tags { "RenderPipeline" = "UniversalPipeline" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;	
			float _Range;

			float RandomRange_float(float2 Seed, float Min, float Max)
			{
				float randomno = frac(sin(dot(Seed, float2(12.9898, 78.233))) * 43758.5453);
				return lerp(Min, Max, randomno);
			}

			fixed4 frag (v2f_img i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				half3 c = col.xyz;
				c = LinearToGammaSpace(c);

				c.r += 1.0f / 256.0f * RandomRange_float(i.uv, -_Range, _Range);
				c.g += 1.0f / 256.0f * RandomRange_float(i.uv + 0.01, -_Range, _Range);
				c.b += 1.0f / 256.0f * RandomRange_float(i.uv + 0.02, -_Range, _Range);

				c = GammaToLinearSpace(c);
				col.xyz = c.xyz;

				return col;
			}
			ENDCG
		}
    }
}
