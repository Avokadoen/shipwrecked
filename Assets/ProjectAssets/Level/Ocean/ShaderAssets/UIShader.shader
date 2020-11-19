Shader "Unlit/UIShader"
{
    Properties
    {
		_MainTex ("Texture", 2D) = "white" {}
        _GameTex ("Texture", 2D) = "white" {}
		_UITex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _GameTex;
			sampler2D _UITex;

            float4 _MainTex_ST;

			fixed4 _Black = fixed4(0, 0, 0, 0);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 gameCol = tex2D(_GameTex, i.uv);
				fixed4 uiCol = tex2D(_UITex, i.uv);
				
				fixed4 col;
				col[0] = lerp(gameCol[0], uiCol[0], uiCol[3]);
				col[1] = lerp(gameCol[1], uiCol[1], uiCol[3]);
				col[2] = lerp(gameCol[2], uiCol[2], uiCol[3]);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
