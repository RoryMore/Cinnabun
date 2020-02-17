// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: commented out 'float4x4 _World2Object', a built-in variable
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/Sharder 3"
{
	Properties
	{
		 [NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Color("color",Color) = (1,1,1,1)

		[HDR] _Emission("Emission", color) = (0,0,0)

		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		[PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.25, 4)) = 1
	}
		SubShader
		{
			Pass
			{
			Tags{"LightMode" = "ForwardBase"}
				CGPROGRAM
				#pragma vertex vertFunc
				#pragma fragment fragFunc

				#include "UnityCG.cginc"
			 #include "UnityLightingCommon.cginc"


				struct vertexOutput
				{
				float2 uv: TEXCOORD0;
				float4 col : COLOR0;
				float4 position : SV_POSITION;
				};


		//Vertez shader
	vertexOutput vertFunc(appdata_base IN)
	{
		vertexOutput Out;
		Out.position = UnityObjectToClipPos(IN.vertex);
		Out.uv = IN.texcoord;
		//light
		half3 normalworld = UnityObjectToWorldNormal(IN.normal);

		half normalLight = max(0.0 , dot(normalworld, _WorldSpaceLightPos0.xyz));

		Out.col = normalLight * _LightColor0;
		return Out;
	}

	sampler2D _MainTex;

	//fragment shader
	fixed4 fragFunc(vertexOutput IN) : SV_Target
	{

		fixed4 colo = tex2D(_MainTex, IN.uv);
	// multiply by lighting
		colo *= IN.col;

		return	colo;


	}
	ENDCG
}
		}
}