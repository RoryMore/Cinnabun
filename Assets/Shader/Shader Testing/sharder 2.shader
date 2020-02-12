Shader "Unlit/Sharder2"
{
	Properties
	{
		_Color("color",Color) = (0,0,0,1)
		_MainTex("Texture", 2D) = "white" {}
		
		
		_Smoothness("Smoothness", Range(0, 1)) = 0
		_Metallic("Metalness", Range(0, 1)) = 0

			[HDR] _Emission("Emission", color) = (0,0,0)

		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		[PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.25, 4)) = 1
	}
		SubShader
		{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

				CGPROGRAM

				#pragma surface surf Standard fullforwardshadows
				#pragma target 3.0

			/* #pragma vertex vertFunc
			 #pragma fragment fragFunc*/

			 #include "UnityCG.cginc"

			 /*struct appdata
			 {
				 float4 vertex : POSITION;
				 float2 uv : TEXCOORD0;
			 };

		 struct v2f
		 {
			 float4 position : SV_POSITION;
			 float2 uv : TEXCOORD0;
		 };*/

			 struct Input {
				 float2 uv;
				 float3 WorldNormals;
				 float3 ViewDir;
				 INTERNAL_DATA
			 };



		//var
		fixed4 _Color;
		sampler2D _MainTex;

		half _Smoothness;
		half _Metallic;
		half3 _emission;

		float3 _FresnelColor;
		float _FresnelExponent;


		void surf(Input i, inout surfaceOutputStandard o) {
			fixed4 col = tex2D(_MainTex, i.uv);
			col *= _Color;
			o.Albedo = col.rgb;

			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;


			float fresnel = dot(i.WorldNormals, i.ViewDir);

			fresnel = saturate(1 - fresnel);

			fresnel = pow(fresnel, _fresnelExponent);

			float3 FresnelColor = fresnel * _fresnelColor;

			o.emission = _Emission + FresnelColor;
		}


		/* v2f vertFunc (appdata IN)
		 {
			 v2f Out;
			 Out.position = UnityObjectToClipPos(IN.vertex);
			 Out.uv = IN.uv;

			 return Out;
		 }

		 fixed4 fragFunc(v2f IN) : SV_Target
		 {

			 fixed4 colo = tex2D(_MainTex, IN.uv);

		 return colo * _Color;
		 }*/
		 ENDCG
	 }
	 FallBack "Standard"
		
}
