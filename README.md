# VisionPro-ShaderTest
This repository is home to a Unity project with 36 different shaders and 6 different particle systems to be tested all in the same scene on the Apple Vision Pro. This way we can check out what specific shaders are not yet supported on the Apple Vision Pro.

Built scene "Shader_Test" via Unity and XCode to test it out on the Apple Vision Pro.

![Screenshot 2024-04-15 at 16 29 28](https://github.com/360Fabriek/VisionPro-ShaderTest/assets/125959835/4a9de774-36f2-452d-8c21-603d67ca48af)

<br />

**Shaders not supported:**

04 - Smoothness from Metallic-Alpha - not supported :x:

07 - Emission color not supported - Emission breaks as well. :x:

11 - Transparency double sides not rendering double :x:

17 - Voronoi not working :x:

21 - Transparent -  Premultiply (alpha via Base Color) :x:

22 - Transparent - Additive (alpha via Base Color) :x:

23 - Transparent -  (alpha via Base Color) -  :x:

24 - Floor & random range - :x:

25 - Simple noise - :x:

26 - Vertex displacement - Works really bad, non functional right now :x:

34 - Random range node breaks shader :x:

<br />

**Particle system issues:**

- Do not receive light :x:

- Collision :x:

- Trail renderer :x:

- Single burst :x:

- Color by particle system properties limited :x: - Advice : use sprites/base color as much as possible.


# List of shaders:

00 - Transparent White Gradient

01 - Transparent White Gradient with animated tiling (time node)

02 - Transparent White Gradient colored with base color

03 - Transparent White Icon with emission

04 - Smoothness from Metallic-Alpha - currently 03-2024 not supported

05 - Smoothness not from Metallic Alpha

06 - Twirl node (time node)

07 - Transparent White Icon colored with emission color - currently 03-2024 not supported

08 - Fresnel node

09 - Fresnel node with colored emission and base color

10 - Transparent - single sided

11 - Transparent - double sided

12 - Transparent animated (time node)

13 - Base Color image animated Rotate node and Tiling And Offset node (time node)

14 - Base Color image - View Direction node

15 - Base Color image - Sample State - Wrap: Mirror node

16 - Blending node (multiply between two images)

17 - Animated Voronoi node (time node)

18 - Checkerboard node

19 - Polar Coordinates node

20 - Transparent - Alpha (alpha via Base Color)

21 - Transparent -  Premultiply (alpha via Base Color)

22 - Transparent - Additive (alpha via Base Color)

23 - Transparent -  (alpha via Base Color)

24 - Shader Graph with Floor and Random Range nodes

25 - Shader Graph with Simple Noise node

26 - Vertex Displacement via Voronoi node with Simple Noise node as normal and Polyspatial Time node

27 - Base Color image with Color Mask nodes and added Color node

28 - Flipbook node with Split into Metallic and Smoothness

29 - Shader Graph - Metallic and Smoothness texture

30 - Shader Graph - Multiple Shape nodes combined + Replace Color node

31 - Shader Graph - Vertex Color

32 - Shader Graph - Transparent with Metallic

33 - Shader Graph - Transparent with Metallic, One Minus node

34 - Shader Graph - Transparent with Random Range and Color Mask node

35 - Shader Graph - Transparent with Blended Images


# List of particle systems:

P00 - Default White Particle with colored Start Color

P01 - Default White Particle with multiple Colors over Time and Noise

P02 - Default White Particle burst animation

P03 - Default White Particle - World Simulation Space

P04 - Default White Particle with Trail and Collision

P05 - Texture Sheet Particle with Normal and Point light
