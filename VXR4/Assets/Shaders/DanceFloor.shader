Shader "Custom/DanceFloor"
{
	Properties
	{
		_Color0 ("Red", Color) = (1,0,0,1)
		_Color1 ("Green", Color) = (0,1,0,1)
		_Color2 ("Blue", Color) = (0,0,1,1)
		_Color3 ("Yellow", Color) = (1,1,0,1)
		_Color4 ("Cyan", Color) = (0,1,1,1)
		_Color5 ("Magenta", Color) = (1,0,1,1)
		_Color6 ("White", Color) = (1,1,1,1)
		_Color7 ("Black", Color) = (0,0,0,1)
		_GridSize ("Grid Size", Float) = 4
		_Speed ("Animation Speed", Float) = 1
		_BorderThickness ("Border Thickness", Range(0.01,0.5)) = 0.08
		// ...existing code...
		_EmissionStrength ("Emission Strength", Range(0,5)) = 1
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

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _Color0, _Color1, _Color2, _Color3, _Color4, _Color5, _Color6, _Color7;
			float _GridSize;
			float _Speed;
			float _BorderThickness;
			float _EmissionStrength;
			// ...existing code...

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			// Simple hash function for pseudo-randomness
			int hash(int x, int y, int t) {
				return abs((x * 73856093 ^ y * 19349663 ^ t * 83492791)) % 8;
			}

			float4 frag (v2f i) : SV_Target {
				float time = _Time.y * _Speed;
				float2 grid = floor(i.uv * _GridSize);
				float2 cellUV = frac(i.uv * _GridSize);
				int x = (int)grid.x;
				int y = (int)grid.y;
				float4 colors[8] = {
					_Color0, _Color1, _Color2, _Color3,
					_Color4, _Color5, _Color6, _Color7
				};
				float border = _BorderThickness;
				if (cellUV.x < border || cellUV.x > 1.0 - border || cellUV.y < border || cellUV.y > 1.0 - border)
				{
					return float4(0,0,0,1);
				}
				int idx = hash(x, y, (int)time);
				float4 col = colors[idx];
				// Emission: make squares glow
				float3 emission = col.rgb * _EmissionStrength;
				return float4(emission, col.a);
			}
			ENDCG
		}
	}
}
