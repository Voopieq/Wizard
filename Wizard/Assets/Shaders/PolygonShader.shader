Shader "Unlit/PolygonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Sides("Sides", Int) = 3
        _Rotation("Rotation", Float) = 0
        _Color("Color", Color) = (0, 0.5, 0.5, 0)
        _Radius("Radius", Float) = 0.5
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
            int _Sides;
            float _Rotation;
            float4 _Color;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                o.uv = v.uv;
                return o;
            }

            float polygon(float2 pt, float2 center, float radius, int sides, float rotation, float line_width, float edge_thickness)
            {
                float2 pos = pt - center;

                float rad = UNITY_TWO_PI / float(sides); // Distance between vertices
                float theta = atan2(pos.y, pos.x) + rotation; // Test point angle

                float d = cos(floor(theta/rad + 0.5)* rad - theta) * length(pos);

                return 1.0 - smoothstep(radius-line_width-edge_thickness, radius-line_width, d);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = frac(i.position * 4);
                float3 pol = polygon(pos, _Radius, 0.45, _Sides, _Rotation, 0.01, 0.01) * _Color;
                return float4(pol, 1);
            }
            ENDCG
        }
    }
}
