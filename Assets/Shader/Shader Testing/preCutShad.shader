Shader "Unlit/preCutShad"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+1"}
        LOD 100

		 ColorMask 0

		ZWrite off
		Stencil{
			Ref 1
			Comp always
			Pass replace
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
           

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
			   return half4(1,1,0,1);
            }
            ENDCG
        }
    }
}
