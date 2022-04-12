// #ifndef SOBELOUTLINES_INCLUDED
// #define SOBELOUTLINES_INCLUDED

// /*
//  * Uses sobel edge detection algorithm
//  * Samples the texture around a point/pixel, multiplied by a convolution
//  * matrix weight and added. Length of the vector of the hori and vert parts
//  * is the final sobel value. Higher num means more of an edge.
//  */

// // Sample offsets for each cell in matrix, relative to center point/pixel
// static float2 sobelSamplePoints[9] = {
//   float2(-1, 1), float2(0, 1), float2(1, 1),
//   float2(-1, 0), float2(0, 0), float2(1, 0),
//   float2(-1, -1), float2(0, -1), float2(1, -1),
// };

// // Weights for x component
// static float sobelXMatrix[9] = {
//   1, 0, -1,
//   2, 0, -2,
//   1, 0, -1
// };

// // Weights for y component
// static float sobelYMatrix[9] = {
//   1, 2, 1,
//   0, 0, 0,
//   -1, -2, -1
// };

// /*
//  * This function runs the sobel algo over the depth texture
//  * UV - position to place matrix over
//  * Thickness - sample distance of matrix
//  * Out - final sobel value output
//  */
// void DepthSobel_float(float2 UV, float Thickness, out float Out) {

//   // Holds sobel value. x/hori, y/vert
//   float2 sobel = 0;

//   // Loop to compute matrix cells
//   // unroll tells compiler can optimize, bc constant num of interations
//   // also removes i = 4 iteration which is always zero, wow!
//   [unroll] for (int i = 0; i < 9; i++) {
//     float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobelSamplePoints[i] * Thickness);
//     sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
//   }

//   // Get final sobel value
//   Out = length(sobel);
// }

// // // This function runs the sobel algo over the opaque texture
// // void ColorSobel_float(float2 UV, float Thickness, out float Out) {
// //   // Have to run algo over RGB channels separately
// //   float2 sobelR = 0;
// //   float2 sobelG = 0;
// //   float2 sobelB = 0;
// //   // Unroll loop for efficiency, compiler removes i=4 which is always 0
// //   [unroll] for (int i = 0; i < 9; i++) {
// //     // Sample the scene color texture
// //     float3 rgb = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV + sobelSamplePoints[i] * Thickness);
// //     // Create kernel for iteration
// //     float2 kernel = float2(sobelXMatrix[i], sobelYMatrix[i]);
// //     // Accumulate samples for each color
// //     sobelR += rgb.r * kernel;
// //     sobelG += rgb.g * kernel;
// //     sobelB += rgb.b * kernel;
// //   }
// //   // Get final sobel value
// //   // Combine RGB values by taking the one with largest sobel value
// //   Out = max(length(sobelR), max(length(sobelG), length(sobelB)));
// // }

// #endif