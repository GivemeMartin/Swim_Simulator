Shader "Unlit/shoreShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv = i.uv;
                float3 e = float3(_MainTex_TexelSize.xy,0);
                float down = 1-tex2D(_MainTex, i.uv-e.zy); //down
                float up = 1-tex2D(_MainTex, i.uv+e.zy); //up
                float left = 1-tex2D(_MainTex, i.uv-e.xz); //left
                float right = 1-tex2D(_MainTex, i.uv+e.xz); //right

                fixed middle = tex2D(_MainTex,i.uv);
                //如果middle为0 且 四周有1 则 middle 改为 1
                middle = step(0.1, middle + down + up + left + right);
                fixed res = middle;
                
                return fixed4(res,res,res,1);
            }
            ENDCG
        }
    }
}
