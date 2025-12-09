Shader "Custom/GlitchWall"
{
    Properties
    {
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.5
        _GlitchSpeed ("Glitch Speed", Range(0,10)) = 2.0
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
                // Stay fully out for 5 seconds, then flash in and out
                float flashDuration = 5.0; // seconds fully out
                float flash = 0.0;
                if (_Time.y > flashDuration)
                {
                    // After 5 seconds, start flashing in and out
                    float flashSpeed = 1.0; // flashes per second
                    float phase = (_Time.y - flashDuration) * flashSpeed;
                    flash = 0.5 + 0.5 * sin(phase * 6.2831853); // 0..1
                }
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