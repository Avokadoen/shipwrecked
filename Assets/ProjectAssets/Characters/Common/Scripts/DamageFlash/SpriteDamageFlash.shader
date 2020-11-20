Shader "Unlit/SpriteDamageFlash"
{
    Properties
    {
		// TODO: Color field
		_Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
		_TargetColor ("TargetColor", Color) = (1, 1, 1, 1)
		_FlashTime ("FlashTime", float) = 0.4
		_FlashPos ("FlashPosition", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#pragma target 3.0

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4 _Color;
			float4 _TargetColor;
			float _FlashTime;
			float _FlashPos;

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
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				
				float lerpPos =  _FlashPos / _FlashTime;
				col[0] = lerp(col[0], _TargetColor[0], lerpPos);
				col[1] = lerp(col[1], _TargetColor[1], lerpPos);
				col[2] = lerp(col[2], _TargetColor[2], lerpPos);


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
