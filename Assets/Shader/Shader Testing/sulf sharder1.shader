Shader "Custom/sulf sharder1"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Smoothness("Smoothness", Range(0, 1)) = 0
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

        

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			col *= _Color;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
           

			float fresnel = dot(IN.worldNormal, IN.viewDir);
			//invert the fresnel so the big values are on the outside
			fresnel = saturate(1 - fresnel);
			//raise the fresnel value to the exponents power to be able to adjust it
			fresnel = pow(fresnel, _FresnelExponent);
			//combine the fresnel value with a color
			float3 fresnelColor = fresnel * _FresnelColor;
			//apply the fresnel value to the emission
			o.Emission = _Emission + fresnelColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
