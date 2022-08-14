// This shader is modified from Chris Nolet's Shader Code.
// Big thanks to Chris Nolet for his awesome asset
// https://github.com/chrisnolet/QuickOutline
// Modified by Ari Utomo using HLSL

Shader "Gura Games/Map Mask"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+100"}

        Pass
        {
            Name "Map Mask"
            Cull Off
            ZTest Always
            ZWrite Off
            ColorMask 0

            Stencil {
                Ref 3
                Comp Always
                Pass Replace
            }
        }
    }
}
