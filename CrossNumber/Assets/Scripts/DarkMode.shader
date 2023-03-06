Shader "Custom/Refraction" {
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}

	}
		SubShader{
					Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
					zwrite off

					GrabPass{ }

					CGPROGRAM
		#pragma surface surf nolight noambient alpha:fade

					sampler2D _GrabTexture;
				sampler2D _MainTex;
				float _RefStrength;
				float _RefSpeed;

				struct Input {
					float4 color : COLOR;
					float4 screenPos;
					float2 uv_MainTex;

				};

				void surf(Input IN, inout SurfaceOutput o) {

					float3 screenUV = IN.screenPos.rbg / IN.screenPos.a;
					o.Emission = 1 - tex2D(_GrabTexture, IN.uv_MainTex);

				}

				float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten) {
					return float4(0, 0, 0, 1);
				}

				ENDCG
		}
}