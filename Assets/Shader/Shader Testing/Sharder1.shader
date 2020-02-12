Shader "Unlit/Sharder1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("color",Color) = (1,1,1,1)
		
    }
    SubShader
    {
        Pass
        {
			
            CGPROGRAM

			
            #pragma vertex vertFunc
            #pragma fragment fragFunc
          
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

		struct v2f
		{
			float4 position : SV_POSITION;
			float2 uv : TEXCOORD0;
		};


			//var
			fixed4 _Color;
            sampler2D _MainTex;

			
            v2f vertFunc (appdata IN)
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
            }
            ENDCG
        }
    }
}
