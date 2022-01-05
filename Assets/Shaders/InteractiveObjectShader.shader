Shader "Unlit/InteractiveObjectShader"
{
    Properties
    {
        _Radius ("Radius", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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
            
            float4 _Pos;
            float _Radius;
            float _Ratio;
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                return max(_Radius - length(uv - float2(0.5, 0.5)) / _Radius,0);
            }
            ENDCG
        }
    }
}
