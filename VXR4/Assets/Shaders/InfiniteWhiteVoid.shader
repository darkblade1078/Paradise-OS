Shader "Skybox/InfiniteWhiteVoid"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (1, 1, 1, 1)
        _Exposure ("Exposure", Range(0, 8)) = 1.3
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off 
        ZWrite Off
        ZTest LEqual

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            half4 _Tint;
            half _Exposure;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                float4 pos = UnityObjectToClipPos(v.vertex);
                o.vertex = pos;
                
                // Use the world position for skybox coordinates
                o.texcoord = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normalize the direction vector
                float3 worldDir = normalize(i.texcoord);
                
                // Create a very subtle gradient to add some depth
                float gradient = saturate(worldDir.y * 0.1 + 0.95);
                
                // Base white color with tint and exposure
                half3 color = _Tint.rgb * gradient * _Exposure;
                
                return half4(color, 1.0);
            }
            ENDCG
        }
    }
    
    Fallback "Skybox/Procedural"
}