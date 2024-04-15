using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderExamplesList : MonoBehaviour
{
    [TextAreaAttribute(5, 35)]
    public string shaderList =
        "00 - Transparent White Gradient\n" +
        "01 - Transparent White Gradient with animated tiling (time node)\n" +
        "02 - Transparent White Gradient colored with base color\n" +
        "03 - Transparent White Icon with emission\n" +
        "04 - Smoothness from Metallic-Alpha - currently 03-2024 not supported\n" +
        "05 - Smoothness not from Metallic Alpha\n" +
        "06 - Twirl node (time node)\n" +
        "07 - Transparent White Icon colored with emission color - currently 03-2024 not supported\n" +
        "08 - Fresnel node\n" +
        "09 - Fresnel node with colored emission and base color\n" +
        "10 - Transparent - single sided\n" +
        "11 - Transparent - double sided\n" +
        "12 - Transparent animated (time node)\n" +
        "13 - Base Color image animated Rotate node and Tiling And Offset node (time node)\n" +
        "14 - Base Color image - View Direction node\n" +
        "15 - Base Color image - Sample State - Wrap: Mirror node\n" +
        "16 - Blending node (multiply between two images)\n" +
        "17 - Animated Voronoi node (time node)\n" +
        "18 - Checkerboard node\n" +
        "19 - Polar Coordinates node\n" +
        "20 - Transparent - Alpha (alpha via Base Color)\n" +
        "21 - Transparent -  Premultiply (alpha via Base Color)\n" +
        "22 - Transparent - Additive (alpha via Base Color)\n" +
        "23 - Transparent -  (alpha via Base Color)\n" +
        "24 - Shader Graph with Floor and Random Range nodes\n" +
        "25 - Shader Graph with Simple Noise node\n" +
        "26 - Vertex Displacement via Voronoi node with Simple Noise node as normal and Polyspatial Time node\n" +
        "27 - Base Color image with Color Mask nodes and added Color node\n" +
        "28 - Flipbook node with Split into Metallic and Smoothness\n" +
        "29 - Shader Graph - Metallic and Smoothness texture\n" +
        "30 - Shader Graph - Multiple Shape nodes combined + Replace Color node\n" +
        "31 - Shader Graph - Vertex Color\n" +
        "32 - Shader Graph - Transparent with Metallic\n" +
        "33 - Shader Graph - Transparent with Metallic, One Minus node\n" +
        "34 - Shader Graph - Transparent with Random Range and Color Mask node\n" +
        "35 - Shader Graph - Transparent with Blended Images";


    [TextAreaAttribute(5, 50)]
    public string particleList =
        "P00 - Default White Particle with colored Start Color\n" +
        "P01 - Default White Particle with multiple Colors over Time and Noise\n" +
        "P02 - Default White Particle burst animation\n" +
        "P03 - Default White Particle - World Simulation Space\n" +
        "P04 - Default White Particle with Trail and Collision\n" +
        "P05 - Texture Sheet Particle with Normal and Point light\n";
}

