Shader "Custom/MultiTextureShader" {
	Properties {
		[Header(Sub Material 0 (Base Sub Material))]
		_diff0 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm0 ("Normal", 2D) = "" {}
		_metal0 ("Metallic", Range(0,1)) = 0.0
		_gloss0 ("Smoothness", Range(0,1)) = 0.5

		[Header(Sub Material 1)]
		_mask1 ("Mask", 2D) = "black" {}
		[MaterialToggle] _tile1 ("Tile Mask", Float) = 0
		[NoScaleOffset] _diff1 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm1 ("Normal", 2D) = "" {}
		_metal1 ("Metallic", Range(0,1)) = 0.0
		_gloss1 ("Smoothness", Range(0,1)) = 0.5

		[Header(Sub Material 2)]
		_mask2 ("Mask", 2D) = "black" {}
		[MaterialToggle] _tile2 ("Tile Mask", Float) = 0
		[NoScaleOffset] _diff2 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm2 ("Normal", 2D) = "" {}
		_metal2 ("Metallic", Range(0,1)) = 0.0
		_gloss2 ("Smoothness", Range(0,1)) = 0.5

		[Header(Sub Material 3)]
		_mask3 ("Mask", 2D) = "black" {}
		[MaterialToggle] _tile3 ("Tile Mask", Float) = 0
		[NoScaleOffset] _diff3 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm3 ("Normal", 2D) = "" {}
		_metal3 ("Metallic", Range(0,1)) = 0.0
		_gloss3 ("Smoothness", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		// Material 0 (Base Material)
		sampler2D _diff0;
		sampler2D _norm0;
		float _metal0;
		float _gloss0;

		// Material 1
		sampler2D _mask1;
		bool _tile1;
		sampler2D _diff1;
		sampler2D _norm1;
		float _metal1;
		float _gloss1;

		// Material 2
		sampler2D _mask2;
		bool _tile2;
		sampler2D _diff2;
		sampler2D _norm2;
		float _metal2;
		float _gloss2;

		// Material 3
		sampler2D _mask3;
		bool _tile3;
		sampler2D _diff3;
		sampler2D _norm3;
		float _metal3;
		float _gloss3;



		struct Input {
			float2 uv_diff0;
			float2 uv_mask1;
			float2 uv_mask2;
			float2 uv_mask3;
		};

		// Surface Function
		void surf (Input IN, inout SurfaceOutputStandard output) {

			float3 grayscalar = float3(0.3, 0.59, 0.11);

			//###################
			// TILING
			//###################
			float2 uv_final_mask1 = _tile1 ? IN.uv_mask1 : IN.uv_diff0;
			float2 uv_final_mask2 = _tile2 ? IN.uv_mask2 : IN.uv_diff0;
			float2 uv_final_mask3 = _tile3 ? IN.uv_mask3 : IN.uv_diff0;

			//###################
			// DIFFUSE
			//###################
			fixed3 diffuse = lerp(
				tex2D (_diff0, IN.uv_diff0).rgb,
				tex2D (_diff1, IN.uv_mask1).rgb,
				dot(tex2D (_mask1, uv_final_mask1).rgb, grayscalar) * tex2D (_diff1, uv_final_mask1).a
			);

			diffuse = lerp(
				diffuse,
				tex2D (_diff2, IN.uv_mask2).rgb,
				dot(tex2D (_mask2, uv_final_mask2).rgb, grayscalar) * tex2D (_diff2, uv_final_mask2).a
			);

			diffuse = lerp(
				diffuse,
				tex2D (_diff3, IN.uv_mask3).rgb,
				dot(tex2D (_mask3, uv_final_mask3).rgb, grayscalar) * tex2D (_diff3, uv_final_mask3).a
			);

			//###################
			// NORMAL
			//###################
			fixed3 normal = lerp(
				UnpackNormal(tex2D (_norm0, IN.uv_diff0)).rgb,
				UnpackNormal(tex2D (_norm1, IN.uv_mask1)).rgb,
				dot(tex2D (_mask1, uv_final_mask1).rgb, grayscalar) * tex2D (_norm1, uv_final_mask1).a
			);

			normal = lerp(
				normal.rgb,
				UnpackNormal(tex2D (_norm2, IN.uv_mask2)).rgb,
				dot(tex2D (_mask2, uv_final_mask2).rgb, grayscalar) * tex2D (_norm2, uv_final_mask2).a
			);

			normal = lerp(
				normal.rgb,
				UnpackNormal(tex2D (_norm3, IN.uv_mask3)).rgb,
				dot(tex2D (_mask3, uv_final_mask3).rgb, grayscalar) * tex2D (_norm2, uv_final_mask3).a
			);

			//###################
			// Metallic
			//###################
			half metal = lerp(
				_metal0,
				_metal1,
				dot(tex2D (_mask1, uv_final_mask1).rgb, grayscalar)
			);

			metal = lerp(
				metal,
				_metal2,
				dot(tex2D (_mask2, uv_final_mask2).rgb, grayscalar)
			);

			metal = lerp(
				metal,
				_metal3,
				dot(tex2D (_mask3, uv_final_mask3).rgb, grayscalar)
			);

			//###################
			// Gloss
			//###################
			half gloss = lerp(
				_gloss0,
				_gloss1,
				dot(tex2D (_mask1, uv_final_mask1).rgb, grayscalar)
			);

			gloss = lerp(
				gloss,
				_gloss2,
				dot(tex2D (_mask2, uv_final_mask2).rgb, grayscalar)
			);

			gloss = lerp(
				gloss,
				_gloss3,
				dot(tex2D (_mask3, uv_final_mask3).rgb, grayscalar)
			);

			//###################
			// Output
			//###################
			output.Albedo = diffuse;
			output.Normal = normal;
			output.Metallic = metal;
			output.Smoothness = gloss;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
