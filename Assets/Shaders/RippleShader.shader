Shader "Unlit/RippleShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _PrevRT;
            sampler2D _CurrentRT;
            float4 _CurrentRT_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed dir0 = abs(sin(_Time.z));
                fixed dir1 = abs(cos(_Time.z));
                fixed dir2 = -abs(sin(_Time.w));
                fixed dir3 = -abs(cos(_Time.w));

                float3 e = float3(_CurrentRT_TexelSize.xy,0);
                float2 uv = i.uv;
                
                float p10 = tex2D(_CurrentRT, uv - e.zy).x; // down
                float p01 = tex2D(_CurrentRT, uv - e.xz).x; // left
                float p21 = tex2D(_CurrentRT, uv + e.xz ).x; // right
                float p12 = tex2D(_CurrentRT, uv + e.zy).x; // up

                float p11 = tex2D(_PrevRT, uv).x;

                float d;
                d = (p10 + p01 + p21 + p12)/2 - p11;
                
                d *= 0.9;

                //d.x = d.x * step(0.05f, d.x);

                //d.y = step(0.01f, p21 - p01);
                //d.z = step(0.01f, p12 - p10);
                //d.y = step(p01, p21);
                //d.z = step(p10, p12);

                //d.w = 1;

                return d;
            }
            ENDCG
        }
    }
}
