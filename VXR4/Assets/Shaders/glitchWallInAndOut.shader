Shader "Custom/GlitchWallInAndOut"
{
    Properties
    {
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.5
        _GlitchSpeed ("Glitch Speed", Range(0,10)) = 2.0
        _FlashSpeed ("Flash Speed", Range(0,10)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Lighting Off
        Cull Off

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

            float _GlitchIntensity;
            float _GlitchSpeed;
            float _FlashSpeed;

            // Color palette
            fixed4 GetPaletteColor(float n)
            {
                if (n < 0.2) return fixed4(1,0,0,1);      // Red
                if (n < 0.4) return fixed4(0,1,0,1);      // Green
                if (n < 0.6) return fixed4(0,0.5,1,1);    // Blue
                if (n < 0.8) return fixed4(1,1,0,1);      // Yellow
                return fixed4(1,0,1,1);                   // Magenta
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y * _GlitchSpeed;
                // Flashing logic: when fully out, shader is 100% transparent for 5 seconds
                float flashOutHold = 5.0; // seconds to stay fully out (invisible)
                float flashPeriod = 2.0; // seconds for one full flash cycle (in+out)
                float cycleTime = fmod(_Time.y, flashPeriod + flashOutHold);
                if (cycleTime >= flashPeriod)
                {
                    // Fully out: shader is invisible
                    return fixed4(0,0,0,0);
                }
                // Normal flash: 0..1..0 over flashPeriod
                float phase = cycleTime / flashPeriod; // 0..1
                float flash = 0.5 + 0.5 * sin(phase * 3.1415926); // 0..1..0
                float modulatedIntensity = lerp(0.0, _GlitchIntensity, flash);
                float glitchLine = step(rand(float2(i.uv.y, t)), modulatedIntensity);

                if (glitchLine > 0.5)
                {
                    float offset = (rand(float2(t, i.uv.y)) - 0.5) * 0.2;
                    float glitch = step(0.5, rand(float2(i.uv.x + offset, t)));
                    if (glitch > 0.5)
                    {
                        float colorSeed = rand(float2(i.uv.y * 10.0, t));
                        return GetPaletteColor(colorSeed);
                    }
                }
                // Transparent elsewhere
                return fixed4(0,0,0,0);
            }
            ENDCG
        }
    }
    FallBack Off
}