Shader "Custom/Fresnel sharder"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Smoothness("Smoothness", Range(0, 1)) = 0
		[HDR] _Emission("Emission", color) = (0,0,0)

		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		[PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.1, 5)) = 1
    }
    SubShader
    {
	   Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}
       

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
			INTERNAL_DATA
        };

		sampler2D _MainTex;
		half _Smoothness;
        half _Metallic;
        fixed4 _Color;
		half3 _Emission;

		float3 _FresnelColor;
		float _FresnelExponent;

        

        void surf (Input _IN, inout SurfaceOutputStandard _OUT)
        {
            // Albedo comes from a texture tinted by color
			fixed4 col = tex2D(_MainTex, _IN.uv_MainTex);
			col *= _Color;
			_OUT.Albedo = tex2D(_MainTex, _IN.uv_MainTex).rgb;
            
			// Metallic and smoothness come from slider variables
			_OUT.Metallic = _Metallic;
			_OUT.Smoothness = _Smoothness;
           
			float fresnel = dot(_IN.worldNormal, _IN.viewDir);
			//invert the fresnel so the big values are on the outside
			fresnel = saturate(1 - fresnel);
			//raise the fresnel value to the exponents power to be able to adjust it
			fresnel = pow(fresnel, _FresnelExponent);
			//combine the fresnel value with a color
			float3 fresnelColor = fresnel * _FresnelColor;
			//apply the fresnel value to the emission
			_OUT.Emission = _Emission + fresnelColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
