Shader "Custom/OilPaintEffect" {
	Properties{
		_MainTex("MainTex",2D)="white"{}
	    //迭代次数
		_Radius("_Radius",Range(0.0,5.0)) = 2.0
		//模糊程度
		_ResolutionValue("_ResolutionValue",Range(0.0,5.0)) = 1.0
		//原始图片宽的像素
		_Width("Width",Int) = 0
		//原始图片高的像素
		_Height("Height", Int) = 0
	}
		SubShader{
		Pass{

		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		int _Radius;
		float _ResolutionValue;
		int _Width;
		int _Height;

		float4 frag(v2f_img i) :SV_Target{
			//_ResolutionValue = _ResolutionValue + sin(_Time.z);
			float2 src_size = float2(_ResolutionValue / _Width,_ResolutionValue / _Height);
			float2 uv = i.uv;
			float n = (_Radius + 1)*(_Radius + 1);
			float3 m0 = 0.0;
			float3 m1 = 0.0;
			float3 s0 = 0.0;
			float3 s1 = 0.0;
			float3 color = 0.0;
			for (int j = -_Radius;j <= 0;++j) {
				for (int k = -_Radius;k <= 0; ++k) {
					color = tex2D(_MainTex, uv +float2(k,j)*src_size).rgb;
					m0 += color;
					s0 += color * color;
				}
			}
			for (int j = 0;j <= _Radius; ++j) {
				for (int k = 0;k <= _Radius; ++k) {
					color = tex2D(_MainTex, uv + float2(k, j)*src_size).rgb;
					m1 += color;
					s1 += color * color;
				}
			}
			float4 finalColor = 0.0;
			float min_sigma2 = 100;
			m0 /= n;
			s0 = abs(s0 / n - m0 * m0);
			float sigma2 = s0.r + s0.g + s0.b;
			if (sigma2 < min_sigma2) 
			{
				min_sigma2 = sigma2;
				finalColor = float4(m0, 1.0);
			}
			m1 /= n;
			s1 = abs(s1 / n - m1 * m1);
			sigma2 = s1.r + s1.g + s1.b;
			if (sigma2 < min_sigma2) 
			{
				min_sigma2 = sigma2;
				finalColor = float4(m1, 1.0);
			}
			return finalColor;		
		}
        ENDCG
        }		
	}
}

