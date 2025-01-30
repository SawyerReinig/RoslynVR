// Made with Amplify Shader Editor v1.9.1.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Polyart/Dreamscape/Builtin/Tree Wind"
{
	Properties
	{
		[Enum(Back,2,Front,1,Off,0)]_Culling("Culling", Int) = 2
		[Header(Foliage)][Header(.)][SingleLineTexture]_FoliageColorMap("Foliage Color Map", 2D) = "white" {}
		[Normal][SingleLineTexture]_FoliageNormalMap("Foliage Normal Map", 2D) = "bump" {}
		_MaskClipValue("Mask Clip Value", Range( 0 , 1)) = 0.5
		_FoliageSize("Foliage Size", Float) = 100
		_FoliageColorTop("Foliage Color Top", Color) = (1,1,1,0)
		_FoliageColorBottom("Foliage Color Bottom", Color) = (0,0,0,0)
		_GradientOffset("Gradient Offset", Float) = 0
		_GradientFallout("Gradient Fallout", Float) = 0
		_FoliageEmissiveColor("Foliage Emissive Color", Color) = (0,0,0,0)
		_FoliageNormalIntensity("Foliage Normal Intensity", Range( 0 , 1)) = 1
		_FoliageRoughness("Foliage Roughness", Range( 0 , 1)) = 0.85
		_FoliageEmissiveIntensity("Foliage Emissive Intensity", Range( 0 , 20)) = 0
		[Header (Trunk)][Toggle(_USEDFORTRUNK_ON)] _UsedforTrunk("Used for Trunk?", Float) = 0
		[SingleLineTexture]_ColorMap("Color Map", 2D) = "white" {}
		[SingleLineTexture]_NormalMap("Normal Map", 2D) = "white" {}
		[SingleLineTexture]_ORMMap("ORM Map", 2D) = "white" {}
		[SingleLineTexture]_EmissiveMap("Emissive Map", 2D) = "white" {}
		_TextureSize("Texture Size", Float) = 100
		_ColorTint("Color Tint", Color) = (1,1,1,0)
		_NormalIntensity("Normal Intensity", Range( 0 , 1)) = 1
		_RoughnessIntensity("Roughness Intensity", Range( 0 , 2)) = 1
		_AOIntensity("AO Intensity", Range( 0 , 2)) = 1
		_EmissiveColor("Emissive Color", Color) = (0,0,0,0)
		_EmissiveIntensity("Emissive Intensity", Range( 0 , 20)) = 0
		[Header(WIND RUSTLE)][Toggle(_USEGLOBALWINDSETTINGS_ON)] _UseGlobalWindSettings("Use Global Wind Settings?", Float) = 0
		[HideInInspector][SingleLineTexture]_NoiseTexture("NoiseTexture", 2D) = "white" {}
		_WindScrollSpeed("Wind Scroll Speed", Range( 0 , 0.5)) = 0.05
		_WindJitterSpeed("Wind Jitter Speed", Range( 0 , 0.5)) = 0.05
		_WindOffsetIntensity("Wind Offset Intensity", Range( 0 , 1)) = 1
		_WindRustleSize("Wind Rustle Size", Range( 0 , 0.2)) = 0.035
		[Header(WIND SWAY)][Toggle(_USESGLOBALWINDSETTINGS_ON)] _UsesGlobalWindSettings("Uses Global Wind Settings?", Float) = 0
		_WindSwayDirection("Wind Sway Direction", Vector) = (1,0,0,0)
		_WIndSwayIntensity("WInd Sway Intensity", Float) = 1
		_WIndSwayFrequency("WInd Sway Frequency", Float) = 1
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TreeTransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull [_Culling]
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _USEDFORTRUNK_ON
		#pragma shader_feature_local _USEGLOBALWINDSETTINGS_ON
		#pragma shader_feature_local _USESGLOBALWINDSETTINGS_ON
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Translucency;
		};

		uniform sampler2D _NoiseTexture;
		uniform int _Culling;
		uniform float _WindScrollSpeed;
		uniform float varWindRustleScrollSpeed;
		uniform float _WindRustleSize;
		uniform float _WindJitterSpeed;
		uniform float Float0;
		uniform float _WindOffsetIntensity;
		uniform float _WIndSwayIntensity;
		uniform float varWindSwayIntensity;
		uniform float2 _WindSwayDirection;
		uniform float2 varWindDirection;
		uniform float _WIndSwayFrequency;
		uniform float varWindSwayFrequency;
		uniform sampler2D _FoliageNormalMap;
		uniform float _FoliageSize;
		uniform float _FoliageNormalIntensity;
		uniform sampler2D _NormalMap;
		uniform float _TextureSize;
		uniform float _NormalIntensity;
		uniform float4 _FoliageColorBottom;
		uniform float4 _FoliageColorTop;
		uniform float _GradientOffset;
		uniform float _GradientFallout;
		uniform sampler2D _FoliageColorMap;
		uniform float4 _ColorTint;
		uniform sampler2D _ColorMap;
		uniform float4 _FoliageEmissiveColor;
		uniform float _FoliageEmissiveIntensity;
		uniform sampler2D _EmissiveMap;
		uniform float4 _EmissiveColor;
		uniform float _EmissiveIntensity;
		uniform float _FoliageRoughness;
		uniform sampler2D _ORMMap;
		uniform float _RoughnessIntensity;
		uniform float _AOIntensity;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float _MaskClipValue;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float temp_output_18_0_g87 = _WindScrollSpeed;
			#ifdef _USEGLOBALWINDSETTINGS_ON
				float staticSwitch25_g87 = ( temp_output_18_0_g87 * varWindRustleScrollSpeed );
			#else
				float staticSwitch25_g87 = temp_output_18_0_g87;
			#endif
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult4_g87 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_7_0_g87 = ( appendResult4_g87 * _WindRustleSize );
			float2 panner9_g87 = ( ( staticSwitch25_g87 * _Time.y ) * float2( 1,1 ) + temp_output_7_0_g87);
			float temp_output_19_0_g87 = _WindJitterSpeed;
			#ifdef _USEGLOBALWINDSETTINGS_ON
				float staticSwitch26_g87 = ( temp_output_19_0_g87 * Float0 );
			#else
				float staticSwitch26_g87 = temp_output_19_0_g87;
			#endif
			float2 panner13_g87 = ( ( _Time.y * staticSwitch26_g87 ) * float2( 1,1 ) + ( temp_output_7_0_g87 * float2( 2,2 ) ));
			float4 lerpResult30_g87 = lerp( float4( float3(0,0,0) , 0.0 ) , ( pow( tex2Dlod( _NoiseTexture, float4( panner9_g87, 0, 0.0) ) , 1.0 ) * tex2Dlod( _NoiseTexture, float4( panner13_g87, 0, 0.0) ) ) , _WindOffsetIntensity);
			float3 ase_vertex3Pos = v.vertex.xyz;
			float temp_output_27_0_g83 = _WIndSwayIntensity;
			#ifdef _USESGLOBALWINDSETTINGS_ON
				float staticSwitch33_g83 = ( temp_output_27_0_g83 * varWindSwayIntensity );
			#else
				float staticSwitch33_g83 = temp_output_27_0_g83;
			#endif
			float temp_output_14_0_g83 = ( ( ase_vertex3Pos.y * ( staticSwitch33_g83 / 100.0 ) ) + 1.0 );
			float temp_output_16_0_g83 = ( temp_output_14_0_g83 * temp_output_14_0_g83 );
			#ifdef _USESGLOBALWINDSETTINGS_ON
				float2 staticSwitch41_g83 = varWindDirection;
			#else
				float2 staticSwitch41_g83 = _WindSwayDirection;
			#endif
			float2 clampResult10_g83 = clamp( staticSwitch41_g83 , float2( -1,-1 ) , float2( 1,1 ) );
			float4 transform1_g83 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float4 appendResult3_g83 = (float4(transform1_g83.x , 0.0 , transform1_g83.z , 0.0));
			float dotResult4_g84 = dot( appendResult3_g83.xy , float2( 12.9898,78.233 ) );
			float lerpResult10_g84 = lerp( 1.0 , 1.01 , frac( ( sin( dotResult4_g84 ) * 43758.55 ) ));
			float mulTime9_g83 = _Time.y * lerpResult10_g84;
			float temp_output_29_0_g83 = _WIndSwayFrequency;
			#ifdef _USESGLOBALWINDSETTINGS_ON
				float staticSwitch34_g83 = ( temp_output_29_0_g83 * varWindSwayFrequency );
			#else
				float staticSwitch34_g83 = temp_output_29_0_g83;
			#endif
			float2 break26_g83 = ( ( ( temp_output_16_0_g83 * temp_output_16_0_g83 ) - temp_output_16_0_g83 ) * ( ( ( staticSwitch41_g83 * float2( 4,4 ) ) + sin( ( ( clampResult10_g83 * mulTime9_g83 ) * staticSwitch34_g83 ) ) ) / float2( 4,4 ) ) );
			float4 appendResult25_g83 = (float4(break26_g83.x , 0.0 , break26_g83.y , 0.0));
			float4 temp_output_246_0 = appendResult25_g83;
			#ifdef _USEDFORTRUNK_ON
				float4 staticSwitch100 = temp_output_246_0;
			#else
				float4 staticSwitch100 = ( ( float4( ase_vertexNormal , 0.0 ) * lerpResult30_g87 ) + temp_output_246_0 );
			#endif
			float4 vWind116 = staticSwitch100;
			v.vertex.xyz += vWind116.rgb;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !defined(DIRECTIONAL)
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float temp_output_235_0 = ( _FoliageSize / 100.0 );
			float2 appendResult234 = (float2(temp_output_235_0 , temp_output_235_0));
			float2 uv_TexCoord236 = i.uv_texcoord * appendResult234;
			float3 vFoliageNormal226 = UnpackScaleNormal( tex2D( _FoliageNormalMap, uv_TexCoord236 ), _FoliageNormalIntensity );
			float2 temp_output_19_0_g88 = ( i.uv_texcoord / ( _TextureSize / 100.0 ) );
			#ifdef _USEDFORTRUNK_ON
				float3 staticSwitch97 = UnpackScaleNormal( tex2D( _NormalMap, temp_output_19_0_g88 ), _NormalIntensity );
			#else
				float3 staticSwitch97 = vFoliageNormal226;
			#endif
			o.Normal = staticSwitch97;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 lerpResult115 = lerp( _FoliageColorBottom , _FoliageColorTop , saturate( ( ( ase_vertex3Pos.y + _GradientOffset ) * ( _GradientFallout * 2 ) ) ));
			float4 tex2DNode124 = tex2D( _FoliageColorMap, uv_TexCoord236 );
			float4 vFoliageColor172 = ( lerpResult115 * tex2DNode124 );
			#ifdef _USEDFORTRUNK_ON
				float4 staticSwitch82 = ( _ColorTint * tex2D( _ColorMap, temp_output_19_0_g88 ) );
			#else
				float4 staticSwitch82 = vFoliageColor172;
			#endif
			o.Albedo = staticSwitch82.rgb;
			float4 vFoliageEmissive225 = ( _FoliageEmissiveColor * _FoliageEmissiveIntensity );
			#ifdef _USEDFORTRUNK_ON
				float4 staticSwitch228 = ( ( tex2D( _EmissiveMap, temp_output_19_0_g88 ) * _EmissiveColor ) * _EmissiveIntensity );
			#else
				float4 staticSwitch228 = vFoliageEmissive225;
			#endif
			o.Emission = staticSwitch228.rgb;
			float4 tex2DNode9_g89 = tex2D( _ORMMap, temp_output_19_0_g88 );
			#ifdef _USEDFORTRUNK_ON
				float staticSwitch98 = ( 1.0 - ( tex2DNode9_g89.g * _RoughnessIntensity ) );
			#else
				float staticSwitch98 = ( 1.0 - _FoliageRoughness );
			#endif
			o.Smoothness = staticSwitch98;
			float lerpResult10_g89 = lerp( 1.0 , tex2DNode9_g89.r , _AOIntensity);
			#ifdef _USEDFORTRUNK_ON
				float staticSwitch99 = lerpResult10_g89;
			#else
				float staticSwitch99 = 1.0;
			#endif
			o.Occlusion = staticSwitch99;
			float3 temp_cast_4 = (1.0).xxx;
			o.Translucency = temp_cast_4;
			o.Alpha = 1;
			float vFoliageOpacity173 = tex2DNode124.a;
			#ifdef _USEDFORTRUNK_ON
				float staticSwitch182 = 1.0;
			#else
				float staticSwitch182 = vFoliageOpacity173;
			#endif
			clip( staticSwitch182 - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=19102
Node;AmplifyShaderEditor.RangedFloatNode;233;-3416.064,-1881.969;Inherit;False;Property;_FoliageSize;Foliage Size;4;0;Create;True;0;0;0;False;0;False;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;235;-3240.15,-1876.368;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;234;-3105.964,-1883.67;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;122;-3225.14,916.776;Inherit;False;1580.315;688.4131;Comment;11;194;212;116;100;246;77;101;104;102;252;256;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-3364.521,-2127.726;Inherit;False;Property;_GradientOffset;Gradient Offset;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-3366.52,-2044.726;Inherit;False;Property;_GradientFallout;Gradient Fallout;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;212;-3035.043,1195.038;Inherit;False;Property;_WindJitterSpeed;Wind Jitter Speed;36;0;Create;True;0;0;0;False;0;False;0.05;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;256;-3037.416,1113.652;Inherit;False;Property;_WindRustleSize;Wind Rustle Size;38;0;Create;True;0;0;0;False;0;False;0.035;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;194;-3040.178,955.6157;Inherit;False;Property;_WindScrollSpeed;Wind Scroll Speed;35;0;Create;True;0;0;0;False;0;False;0.05;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;252;-3038.711,1039.359;Inherit;False;Property;_WindOffsetIntensity;Wind Offset Intensity;37;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-2974.226,1274.111;Inherit;False;Property;_WIndSwayIntensity;WInd Sway Intensity;42;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;104;-2975.226,1433.112;Inherit;False;Property;_WindSwayDirection;Wind Sway Direction;41;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;112;-3015.271,-2301.595;Inherit;False;Property;_FoliageColorTop;Foliage Color Top;5;1;[Header];Create;True;0;0;0;False;0;False;1,1,1,0;1,0,0.7909455,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;236;-2948.401,-1905.561;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;174;-2688.444,-1929.97;Inherit;True;Property;_FoliageNormalMap;Foliage Normal Map;2;2;[Normal];[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.FunctionNode;131;-3076.101,-2118.791;Inherit;False;PA_SF_ObjectGradient;-1;;52;f7566061dd2a41c4bbc5f0e0ea7b5f5b;0;2;8;FLOAT;0;False;9;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;114;-3026.939,-2483.921;Inherit;False;Property;_FoliageColorBottom;Foliage Color Bottom;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.7909455,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;123;-2704.77,-2184.466;Inherit;True;Property;_FoliageColorMap;Foliage Color Map;1;2;[Header];[SingleLineTexture];Create;True;2;Foliage;.;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;102;-2988.226,1355.111;Inherit;False;Property;_WIndSwayFrequency;WInd Sway Frequency;43;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;175;-2432.265,-1930.658;Inherit;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;246;-2715.526,1201.211;Inherit;False;PA_SF_WindSway;39;;83;bc8ec8a781a3c384e9042e29b2eae6d5;0;3;27;FLOAT;0;False;29;FLOAT;1;False;30;FLOAT2;1,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;257;-2726.183,1062.027;Inherit;False;PA_SF_WindRustleNoise;32;;87;7733c52bc6ce2e94b9c81cb72dee5854;0;4;18;FLOAT;0;False;33;FLOAT;1;False;35;FLOAT;0.035;False;19;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;83;-3049.222,-875.8011;Inherit;False;1072.199;1674.577;Comment;12;95;94;93;92;91;90;89;88;87;86;85;84;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-2412.641,-1743.529;Inherit;False;Property;_FoliageNormalIntensity;Foliage Normal Intensity;10;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;124;-2448.591,-2185.154;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;224;-2686.941,-1481.076;Inherit;False;Property;_FoliageEmissiveIntensity;Foliage Emissive Intensity;12;0;Create;True;0;0;0;False;0;False;0;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;222;-2686.453,-1661.345;Inherit;False;Property;_FoliageEmissiveColor;Foliage Emissive Color;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;115;-2669.975,-2319.941;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;89;-2938.222,-98.80125;Inherit;True;Property;_ORMMap;ORM Map;22;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;87;-2881.222,-826.8011;Inherit;False;Property;_TextureSize;Texture Size;24;0;Create;True;0;0;0;False;0;False;100;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;94;-2936.834,349.4665;Inherit;True;Property;_EmissiveMap;Emissive Map;23;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;-2381.069,-1656.414;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-2980.222,106.1985;Inherit;False;Property;_RoughnessIntensity;Roughness Intensity;27;0;Create;True;0;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;84;-2941.222,-738.8015;Inherit;True;Property;_ColorMap;Color Map;20;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;93;-2985.696,191.0175;Inherit;False;Property;_AOIntensity;AO Intensity;29;0;Create;True;0;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-2982.374,268.7941;Inherit;False;Property;_MetallicIntensity;Metallic Intensity;28;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-2104.469,-2320.945;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;95;-2918.222,-546.8016;Inherit;False;Property;_ColorTint;Color Tint;25;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-2377.781,1061.776;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;176;-2091.523,-1925.919;Inherit;False;Tangent;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;90;-2911.356,713.1755;Inherit;False;Property;_EmissiveIntensity;Emissive Intensity;31;0;Create;True;0;0;0;False;0;False;0;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-2984.222,-173.8012;Inherit;False;Property;_NormalIntensity;Normal Intensity;26;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;92;-2919.356,540.1759;Inherit;False;Property;_EmissiveColor;Emissive Color;30;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;85;-2937.222,-372.8016;Inherit;True;Property;_NormalMap;Normal Map;21;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;True;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;225;-2175.064,-1660.412;Inherit;False;vFoliageEmissive;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;220;-1830.553,34.33481;Inherit;False;Property;_FoliageRoughness;Foliage Roughness;11;0;Create;True;0;0;0;False;0;False;0.85;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;255;-2387.722,-260.9431;Inherit;False;PA_SF_BasePBR_OpaqueORM;14;;88;a21dcaebf1379e5439b421c5da1cd710;0;12;20;FLOAT;100;False;9;SAMPLER2D;0;False;15;COLOR;0,0,0,0;False;22;SAMPLER2D;;False;28;FLOAT;1;False;59;SAMPLER2D;;False;38;FLOAT;1;False;73;FLOAT;0;False;39;FLOAT;0;False;52;SAMPLER2D;;False;53;COLOR;0,0,0,0;False;54;FLOAT;0;False;6;COLOR;0;FLOAT3;7;COLOR;6;FLOAT;5;FLOAT;3;FLOAT;8
Node;AmplifyShaderEditor.StaticSwitch;100;-2201.238,1170.427;Inherit;False;Property;_TrunkMaterial;Trunk Material?;13;0;Create;True;0;0;0;False;1;Header(TRUNK);False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;82;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;173;-2071.848,-2087.301;Inherit;False;vFoliageOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;226;-1863.922,-1931.901;Inherit;False;vFoliageNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;172;-1890.849,-2326.3;Inherit;False;vFoliageColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;229;-1722.231,-128.673;Inherit;False;225;vFoliageEmissive;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;231;-1890.55,167.2511;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;187;-554.4885,146.7166;Inherit;False;Constant;_Float1;Float 1;34;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;227;-1586.681,-288.1677;Inherit;False;226;vFoliageNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-1951.639,1170.127;Inherit;False;vWind;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;232;-1865.879,90.96267;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;230;-1559.062,40.15799;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;183;-619.5778,71.2254;Inherit;False;173;vFoliageOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;237;-1831.389,-49.0491;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-1527.62,162.4118;Inherit;False;Constant;_AO;AO;34;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;181;-1580.058,-441.5723;Inherit;False;172;vFoliageColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;228;-1386.606,-89.33887;Inherit;False;Property;_TrunkMaterial;Trunk Material?;13;0;Create;True;0;0;0;False;1;Header(TRUNK);False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;82;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;182;-369.5775,72.2254;Inherit;False;Property;_TrunkMaterial;Trunk Material?;13;0;Create;True;0;0;0;False;1;Header(TRUNK);False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;82;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;99;-1387.107,162.772;Inherit;False;Property;_TrunkMaterial;Trunk Material?;13;0;Create;True;0;0;0;False;1;Header(TRUNK);False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;82;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;98;-1389.094,36.90163;Inherit;False;Property;_TrunkMaterial;Trunk Material?;13;0;Create;True;0;0;0;False;1;Header(TRUNK);False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;82;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-706.6788,-1057.925;Inherit;False;Property;_MaskClipValue;Mask Clip Value;3;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;82;-1388.657,-440.5339;Inherit;False;Property;_UsedforTrunk;Used for Trunk?;13;0;Create;True;0;0;0;False;1;Header (Trunk);False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;117;-304.8967,175.2606;Inherit;False;116;vWind;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;97;-1388.557,-270.4312;Inherit;False;Property;_TrunkMaterial;Trunk Material?;13;0;Create;True;0;0;0;False;1;Header(TRUNK);False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;82;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.IntNode;109;-704.7553,-965.1958;Inherit;False;Property;_Culling;Culling;0;1;[Enum];Create;True;0;3;Back;2;Front;1;Off;0;0;True;0;False;2;0;True;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;170;-306.6799,-10.38589;Inherit;False;Constant;_Translucency;Translucency;7;0;Create;True;0;0;0;False;0;False;1;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-16.38549,-288.2648;Float;False;True;-1;2;;0;0;Standard;Polyart/Dreamscape/Builtin/Tree Wind;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;TreeTransparentCutout;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;44;-1;-1;0;False;0;0;True;_Culling;-1;0;True;_MaskClipValue;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;235;0;233;0
WireConnection;234;0;235;0
WireConnection;234;1;235;0
WireConnection;236;0;234;0
WireConnection;131;8;110;0
WireConnection;131;9;111;0
WireConnection;175;0;174;0
WireConnection;175;1;236;0
WireConnection;246;27;101;0
WireConnection;246;29;102;0
WireConnection;246;30;104;0
WireConnection;257;18;194;0
WireConnection;257;33;252;0
WireConnection;257;35;256;0
WireConnection;257;19;212;0
WireConnection;124;0;123;0
WireConnection;124;1;236;0
WireConnection;115;0;114;0
WireConnection;115;1;112;0
WireConnection;115;2;131;0
WireConnection;223;0;222;0
WireConnection;223;1;224;0
WireConnection;125;0;115;0
WireConnection;125;1;124;0
WireConnection;77;0;257;0
WireConnection;77;1;246;0
WireConnection;176;0;175;0
WireConnection;176;1;177;0
WireConnection;225;0;223;0
WireConnection;255;20;87;0
WireConnection;255;9;84;0
WireConnection;255;15;95;0
WireConnection;255;22;85;0
WireConnection;255;28;88;0
WireConnection;255;59;89;0
WireConnection;255;38;86;0
WireConnection;255;73;93;0
WireConnection;255;39;91;0
WireConnection;255;52;94;0
WireConnection;255;53;92;0
WireConnection;255;54;90;0
WireConnection;100;1;77;0
WireConnection;100;0;246;0
WireConnection;173;0;124;4
WireConnection;226;0;176;0
WireConnection;172;0;125;0
WireConnection;231;0;255;8
WireConnection;116;0;100;0
WireConnection;232;0;255;5
WireConnection;230;0;220;0
WireConnection;237;0;255;6
WireConnection;228;1;229;0
WireConnection;228;0;237;0
WireConnection;182;1;183;0
WireConnection;182;0;187;0
WireConnection;99;1;221;0
WireConnection;99;0;231;0
WireConnection;98;1;230;0
WireConnection;98;0;232;0
WireConnection;82;1;181;0
WireConnection;82;0;255;0
WireConnection;97;1;227;0
WireConnection;97;0;255;7
WireConnection;0;0;82;0
WireConnection;0;1;97;0
WireConnection;0;2;228;0
WireConnection;0;4;98;0
WireConnection;0;5;99;0
WireConnection;0;7;170;0
WireConnection;0;10;182;0
WireConnection;0;11;117;0
ASEEND*/
//CHKSM=ECAD5827A047B66EA06D628716FB3C5EF9CDBBE2