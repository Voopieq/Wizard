Shader "Unlit/SpriteRendererShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off
            Lighting Off
            SetTexture [_MainTex]
            {
                Combine texture * primary
            }
        }
    }
}
