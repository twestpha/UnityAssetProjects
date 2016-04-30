Shader "Hidden/CRTDisplayShader"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_CurrentTime ("Current Time", float) = 0.0
	}
	SubShader {
		// No culling or depth
		// Cull Off ZWrite Off ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

			v2f vert(appdata v){
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			sampler2D _MainTex;
			float _CurrentTime;

			fixed4 frag (v2f i) : SV_Target {
				// Setup
				fixed4 fragColor = tex2D(_MainTex, i.uv);
				float onePixelx = 1.0 / _ScreenParams.x;
				float onePixely = 1.0 / _ScreenParams.y;

				// Ajusted Coordinates
				float xPosScrn = floor((i.screenPos.x * _ScreenParams.x));
				float yPosScrn = floor((i.screenPos.y * _ScreenParams.y));
				float xmod = fmod(xPosScrn, 3);
				float ymod = fmod(yPosScrn, 3);

				// Averaging Neighbors
				float2 plusTwo = float2(i.uv.x + onePixelx * 2.0, i.uv.y - (onePixely * ymod));
				float2 plusOne = float2(i.uv.x + onePixelx, i.uv.y - (onePixely * ymod));
				float2 plusNone = float2(i.uv.x, i.uv.y - (onePixely * ymod));
				float2 minusOne = float2(i.uv.x - onePixelx, i.uv.y - (onePixely * ymod));
				float2 minusTwo = float2(i.uv.x - onePixelx * 2.0, i.uv.y - (onePixely * ymod));

				fixed4 plusOneFragColor = tex2D(_MainTex, plusOne);
				fixed4 plusTwoFragColor = tex2D(_MainTex, plusTwo);
				fixed4 plusNoneFragColor = tex2D(_MainTex, plusNone);
				fixed4 minusOneFragColor = tex2D(_MainTex, minusOne);
				fixed4 minusTwoFragColor = tex2D(_MainTex, minusTwo);

				float red = (plusNoneFragColor.r + plusTwoFragColor.r + plusOneFragColor.r + fragColor.r + minusOneFragColor.r + minusTwoFragColor.r)/5.0;
				float grn = (plusNoneFragColor.g + plusTwoFragColor.g + plusOneFragColor.g + fragColor.g + minusOneFragColor.g + minusTwoFragColor.g)/5.0;
				float blu = (plusNoneFragColor.b + plusTwoFragColor.b + plusOneFragColor.b + fragColor.b + minusOneFragColor.b + minusTwoFragColor.b)/5.0;

				if(xmod == 0.0){
					fragColor = fixed4(red, 0.2, 0.2, 1.0);
				} else if(xmod > 0.0 && xmod < 2.0){
					fragColor = fixed4(0.2, grn, 0.2, 1.0);
				} else {
					fragColor = fixed4(0.2, 0.2, blu, 1.0);
				}

				// Brightness Calculations (from scanline)

				// function that takes position in scan (left to right, top to bottom)
				// and time to compute brightness from scanline. We want to fix time
				// instead of using delta-time for perfect simulation no matter what time it is

				//float scanPosition = (i.screenPos.y * _ScreenParams.y) + i.screenPos.x;
				//float adjustedTime = _CurrentTime;
				//float brightnessIdx = fmod(scanPosition + adjustedTime, 60);
				//float brightness = (brightnessIdx / 240.0) + 0.75;

				return fragColor;
			}
			ENDCG
		}
	}
}
