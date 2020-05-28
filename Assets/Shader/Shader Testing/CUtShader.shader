Shader "Custom/CUtShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		 _MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		
		
		Stencil{
			Ref 1
			Comp notequal
			Pass keep
		
		}

        CGPROGRAM
        #pragma surface surf Lambert alpha  
		 
		sampler2D _MainTex;
		 float4 _Color;
		 
        struct Input
        {
			float2 uv_MainTex;
        };

      
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
        }
        ENDCG
    }
}
