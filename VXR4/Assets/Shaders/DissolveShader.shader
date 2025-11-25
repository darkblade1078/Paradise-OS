Shader "Custom/DissolveElectric"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.5
        _EdgeWidth ("Edge Width", Range(0, 0.5)) = 0.1
        _EdgeColor ("Edge Color", Color) = (0, 0.8, 1, 1)
        _ElectricIntensity ("Electric Intensity", Range(0, 2)) = 1.0
        _ElectricSpeed ("Electric Speed", Range(0, 3)) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _DissolveTex;
            sampler2D _NoiseTex;
            
            float4 _MainTex_ST;
            float4 _DissolveTex_ST;
            float4 _NoiseTex_ST;
            float _DissolveAmount;
            float _EdgeWidth;
            float4 _EdgeColor;
            float _ElectricIntensity;
            float _ElectricSpeed;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv_main : TEXCOORD0;
                float2 uv_dissolve : TEXCOORD1;
                float2 uv_noise : TEXCOORD2;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_main = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv_dissolve = TRANSFORM_TEX(v.uv, _DissolveTex);
                
                float2 noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex);
                noiseUV.x += _Time.y * _ElectricSpeed * 0.3;
                noiseUV.y += _Time.y * _ElectricSpeed * 0.2;
                o.uv_noise = noiseUV;
                
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 mainCol = tex2D(_MainTex, i.uv_main);
                float dissolve = tex2D(_DissolveTex, i.uv_dissolve).r;
                float noise1 = tex2D(_NoiseTex, i.uv_noise).r;
                float noise2 = tex2D(_NoiseTex, i.uv_noise * 0.7).r;
                float noise3 = tex2D(_NoiseTex, i.uv_noise * 1.3).r;
                
                float electricNoise = noise1 * 0.4 + noise2 * 0.3 + noise3 * 0.3;
                float wave = sin(i.uv_dissolve.y * 15.0 + _Time.y * 3.0) * 0.15;
                electricNoise = electricNoise + wave;
                
                float dissolveValue = dissolve + electricNoise * 0.2;
                float alpha = step(_DissolveAmount, dissolveValue);
                
                float edgeMask = smoothstep(_DissolveAmount - _EdgeWidth, _DissolveAmount + _EdgeWidth * 0.3, dissolveValue);
                edgeMask = edgeMask * (1.0 - alpha);
                
                float glowIntensity = edgeMask * electricNoise * _ElectricIntensity;
                
                float3 finalColor = mainCol.rgb;
                finalColor = lerp(finalColor, _EdgeColor.rgb, edgeMask);
                finalColor = finalColor + (_EdgeColor.rgb * glowIntensity);
                
                float finalAlpha = alpha * mainCol.a + glowIntensity * 0.5;
                
                return float4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }
    
    Fallback "Mobile/Diffuse"
}