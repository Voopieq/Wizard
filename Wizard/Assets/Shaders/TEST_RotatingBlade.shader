Shader "Unlit/PolygonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Sides("Sides", Int) = 3
        _Rotation("Rotation", Float) = 0
        _Color("Color", Color) = (0, 0.5, 0.5, 0)
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 position : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            //float4 _MainTex_ST;
            int _Sides;
            float _Rotation;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                o.uv = v.uv; //TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float polygon(float2 pt, float2 center, float radius, int sides, float rotation, float line_width, float edge_thickness)
            {
                float2 pos = pt - center;

                float rad = UNITY_TWO_PI / float(sides); // Distance between vertices
                float theta = atan2(pos.y, pos.x) + rotation + _Time.w; // Test point angle

                float d = cos(floor(theta/rad)* rad - theta) * length(pos);

                return 1.0 - smoothstep(d-line_width-edge_thickness, d-line_width, radius);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = i.position.xy * 2;
                float3 pol = polygon(pos, 0, 0.3, _Sides, _Rotation, 0.01, 0.01) * _Color;
                return float4(pol, 1);
            }
            ENDCG
        }
    }
}
