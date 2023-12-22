Shader "Unlit/Test3"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _NoiseForce("Noise Force", Range(0, 500)) = 10
        _NoiseScale("Noise Scale", Vector) = (10, 10, 0, 0)
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" }

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

                sampler2D _MainTex;
                sampler2D _NoiseTex;
                float _NoiseForce;
                float2 _NoiseScale;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float3 getNoise(float2 uv)
                {
                    float2 scale = 10;
                    float force = _NoiseForce;
                    return tex2D(_NoiseTex, uv * _NoiseScale) / force;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    col.rgb += getNoise(i.uv);
                    return col;
                }
                ENDCG
            }
        }
}
