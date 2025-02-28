```hlsl

#ifndef LITCHI_COMMON
#define LITCHI_COMMON

//= INCLUDES ======================
#include "common_vertex_pixel.hlsl"
#include "common_buffers.hlsl"
#include "common_samplers.hlsl"
#include "common_textures.hlsl"
//=================================

/*------------------------------------------------------------------------------
    CONSTANTS
------------------------------------------------------------------------------*/
static const float PI                  = 3.14159265f;
static const float PI2                 = 6.28318530f;
static const float PI4                 = 12.5663706f;
static const float INV_PI              = 0.31830988f;
static const float PI_HALF             = PI * 0.5f;
static const float FLT_MIN             = 0.00000001f;
static const float FLT_MAX_10          = 511.0f;
static const float FLT_MAX_11          = 1023.0f;
static const float FLT_MAX_14          = 8191.0f;
static const float FLT_MAX_16          = 32767.0f;
static const float FLT_MAX_16U         = 65535.0f;
static const float ALPHA_THRESHOLD     = 0.6f;
static const float RPC_9               = 0.11111111111f;
static const float RPC_16              = 0.0625f;
static const float RPC_32              = 0.03125f;
static const float ENVIRONMENT_MAX_MIP = 11.0f;
static const uint THREAD_GROUP_COUNT_X = 8;
static const uint THREAD_GROUP_COUNT_Y = 8;
static const uint THREAD_GROUP_COUNT   = 64;

/*------------------------------------------------------------------------------
   COMMON
------------------------------------------------------------------------------*/
float2 get_rt_texel_size()          { return float2(1.0f / pass_get_resolution_out().x, 1.0f /pass_get_resolution_out().y); }
float2 get_tex_noise_normal_scale() { return float2(buffer_frame.resolution_render.x / 256.0f, buffer_frame.resolution_render.y / 256.0f); }
float2 get_tex_noise_blue_scale()   { return float2(buffer_frame.resolution_render.x / 470.0f, buffer_frame.resolution_render.y / 470.0f); }
float3 degamma(float3 color)        { return pow(color, buffer_frame.gamma); }
float3 gamma(float3 color)          { return pow(color, 1.0f / buffer_frame.gamma); }

/*------------------------------------------------------------------------------
    MATH
------------------------------------------------------------------------------*/
float min2(float2 value)                                { return min(value.x, value.y); }
float min3(float3 value)                                { return min(min(value.x, value.y), value.z); }
float min3(float a, float b, float c)                   { return min(min(a, b), c); }
float min4(float a, float b, float c, float d)          { return min(min(min(a, b), c), d); }
float min5(float a, float b, float c, float d, float e) { return min(min(min(min(a, b), c), d), e); }

float max2(float2 value)                                { return max(value.x, value.y); }
float max3(float3 value)                                { return max(max(value.x, value.y), value.z); }
float max4(float a, float b, float c, float d)          { return max(max(max(a, b), c), d); }
float max5(float a, float b, float c, float d, float e) { return max(max(max(max(a, b), c), d), e); }

float pow2(float x)
{
    return x * x;
}

float pow3(float x)
{
    float xx = x*x;
    return xx * x;
}

float pow4(float x)
{
    float xx = x*x;
    return xx * xx;
}

bool is_saturated(float value)  { return value == saturate(value); }
bool is_saturated(float2 value) { return is_saturated(value.x) && is_saturated(value.y); }
bool is_saturated(float3 value) { return is_saturated(value.x) && is_saturated(value.y) && is_saturated(value.z); }
bool is_saturated(float4 value) { return is_saturated(value.x) && is_saturated(value.y) && is_saturated(value.z) && is_saturated(value.w); }

bool is_valid_uv(float2 value) { return (value.x >= 0.0f && value.x <= 1.0f) || (value.y >= 0.0f && value.y <= 1.0f); }

/*------------------------------------------------------------------------------
    SATURATE
------------------------------------------------------------------------------*/
float  saturate_11(float x)  { return clamp(x, 0.0f, FLT_MAX_11); }
float2 saturate_11(float2 x) { return clamp(x, 0.0f, FLT_MAX_11); }
float3 saturate_11(float3 x) { return clamp(x, 0.0f, FLT_MAX_11); }
float4 saturate_11(float4 x) { return clamp(x, 0.0f, FLT_MAX_11); }

float  saturate_16(float x)  { return clamp(x, 0.0f, FLT_MAX_16); }
float2 saturate_16(float2 x) { return clamp(x, 0.0f, FLT_MAX_16); }
float3 saturate_16(float3 x) { return clamp(x, 0.0f, FLT_MAX_16); }
float4 saturate_16(float4 x) { return clamp(x, 0.0f, FLT_MAX_16); }

/*------------------------------------------------------------------------------
    PACKING/UNPACKING
------------------------------------------------------------------------------*/
float3 unpack(float3 value) { return value * 2.0f - 1.0f; }
float3 pack(float3 value)   { return value * 0.5f + 0.5f; }
float2 unpack(float2 value) { return value * 2.0f - 1.0f; }
float2 pack(float2 value)   { return value * 0.5f + 0.5f; }
float  unpack(float value)  { return value * 2.0f - 1.0f; }
float  pack(float value)    { return value * 0.5f + 0.5f; }

float pack_uint32_to_float16(uint i)    { return (float)i / FLT_MAX_16; }
uint  unpack_float16_to_uint32(float f) { return round(f * FLT_MAX_16); }

float pack_float_int(float f, uint i, uint numBitI, uint numBitTarget)
{
    // Constant optimize by compiler
    float precision         = float(1U << numBitTarget);
    float maxi              = float(1U << numBitI);
    float precisionMinusOne = precision - 1.0;
    float t1                = ((precision / maxi) - 1.0) / precisionMinusOne;
    float t2                = (precision / maxi) / precisionMinusOne;

    // Code
    return t1 * f + t2 * float(i);
}

void unpack_float_int(float val, uint numBitI, uint numBitTarget, out float f, out uint i)
{
    // Constant optimize by compiler
    float precision         = float(1U << numBitTarget);
    float maxi              = float(1U << numBitI);
    float precisionMinusOne = precision - 1.0;
    float t1                = ((precision / maxi) - 1.0) / precisionMinusOne;
    float t2                = (precision / maxi) / precisionMinusOne;

    // Code
    // extract integer part
    // + rcp(precisionMinusOne) to deal with precision issue
    i = int((val / t2) + rcp(precisionMinusOne));
    // Now that we have i, solve formula in PackFloatInt for f
    //f = (val - t2 * float(i)) / t1 => convert in mads form
    f = saturate((-t2 * float(i) + val) / t1); // Saturate in case of precision issue
}

/*------------------------------------------------------------------------------
    FAST MATH APPROXIMATIONS
------------------------------------------------------------------------------*/

// Relative error : < 0.7% over full
// Precise format : ~small float
// 1 ALU
float fast_sqrt(float x)
{
    int i = asint(x);
    i = 0x1FBD1DF5 + (i >> 1);
    return asfloat(i);
}

float fast_length(float3 v)
{
    float LengthSqr = dot(v, v);
    return fast_sqrt(LengthSqr);
}

float fast_sin(float x)
{
    const float B = 4 / PI;
    const float C = -4 / PI2;
    const float P = 0.225;

    float y = B * x + C * x * abs(x);
    y = P * (y * abs(y) - y) + y;
    return y;
}

float fast_cos(float x)
{
   return abs(abs(x)  /PI2 % 4 - 2) - 1;
}

/*------------------------------------------------------------------------------
    TRANSFORMATIONS
------------------------------------------------------------------------------*/
float3 world_to_view(float3 x, bool is_position = true)
{
    return mul(float4(x, (float)is_position), buffer_rendererPath.view).xyz;
}

float3 world_to_ndc(float3 x, bool is_position = true)
{
    float4 ndc = mul(float4(x, (float) is_position), buffer_rendererPath.view_projection);
    return ndc.xyz / ndc.w;
}

float3 world_to_ndc(float3 x, float4x4 transform) // shadow mapping
{
    float4 ndc = mul(float4(x, 1.0f), transform);
    return ndc.xyz / ndc.w;
}

float3 view_to_ndc(float3 x, bool is_position = true)
{
    float4 ndc = mul(float4(x, (float) is_position), buffer_rendererPath.projection);
    return ndc.xyz / ndc.w;
}

float2 world_to_uv(float3 x, bool is_position = true)
{
    float4 uv = mul(float4(x, (float) is_position), buffer_rendererPath.view_projection);
    return (uv.xy / uv.w) * float2(0.5f, -0.5f) + 0.5f;
}

float2 view_to_uv(float3 x, bool is_position = true)
{
    float4 uv = mul(float4(x, (float) is_position), buffer_rendererPath.projection);
    return (uv.xy / uv.w) * float2(0.5f, -0.5f) + 0.5f;
}

float2 ndc_to_uv(float2 x)
{
    return x * float2(0.5f, -0.5f) + 0.5f;
}

float2 ndc_to_uv(float3 x)
{
    return x.xy * float2(0.5f, -0.5f) + 0.5f;
}

float3 get_position_ws_from_depth(const float2 uv, const float depth)
{
    float x          = uv.x * 2.0f - 1.0f;
    float y          = (1.0f - uv.y) * 2.0f - 1.0f;
    float4 pos_clip  = float4(x, y, depth, 1.0f);
    float4 pos_world = mul(pos_clip, buffer_rendererPath.view_projection_inverted);
    return             pos_world.xyz / pos_world.w;
}

/*------------------------------------------------------------------------------
    NORMAL
------------------------------------------------------------------------------*/
// Reconstruct normal Z, X and Y components have to be in a -1 to 1 range.
float3 reconstruct_normal_z(float2 normal)
{
    float z = sqrt(saturate(1.0f - dot(normal, normal)));
    return float3(normal, z);
}

// float3 get_normal(uint2 pos)
// {
    // // Load returns 0 for any value accessed out of bounds, so clamp.
    // pos.x = clamp(pos.x, 0, buffer_frame.resolution_render.x);
    // pos.y = clamp(pos.y, 0, buffer_frame.resolution_render.y);
    
    // return tex_normal[pos].xyz;
// }

// float3 get_normal(float2 uv)
// {
    // return tex_normal.SampleLevel(samplers[sampler_point_clamp], uv, 0).xyz;
// }

// float3 get_normal_view_space(uint2 pos)
// {
    // return normalize(mul(float4(get_normal(pos), 0.0f), buffer_rendererPath.view).xyz);
// }

// float3 get_normal_view_space(float2 uv)
// {
    // return normalize(mul(float4(get_normal(uv), 0.0f), buffer_rendererPath.view).xyz);
// }

float3x3 make_tangent_to_world_matrix(float3 n, float3 t)
{
    // re-orthogonalize T with respect to N
    t = normalize(t - dot(t, n) * n);
    // compute bitangent
    float3 b = cross(n, t);
    // create matrix
    return float3x3(t, b, n); 
}

float3x3 make_world_to_tangent_matrix(float3 n, float3 t)
{
    return transpose(make_tangent_to_world_matrix(n, t));
}

/*------------------------------------------------------------------------------
    DEPTH
------------------------------------------------------------------------------*/
float get_depth(uint2 position)
{
     // out of bounds check
    position = clamp(position, uint2(0, 0), uint2(buffer_frame.resolution_render) - uint2(1, 1));
    return tex_depth[position].r;
}

float get_depth(float2 uv)
{
     // effects like screen space shadows, can get artefacts if a point sampler is used
    return tex_depth.SampleLevel(samplers[sampler_bilinear_clamp], uv, 0).r;
}

float get_linear_depth(float z, float near, float far)
{
    float z_b = z;
    float z_n = 2.0f * z_b - 1.0f;
    return 2.0f * far * near / (near + far - z_n * (near - far));
}

float get_linear_depth(float z)
{
    return get_linear_depth(z, buffer_rendererPath.camera_near, buffer_rendererPath.camera_far);
}

// float get_linear_depth(uint2 pos)
// {
    // return get_linear_depth(get_depth(pos));
// }

// float get_linear_depth(float2 uv)
// {
    // return get_linear_depth(get_depth(uv));
// }

/*------------------------------------------------------------------------------
    POSITION
------------------------------------------------------------------------------*/
float3 get_position(float z, float2 uv)
{
    float x             = uv.x * 2.0f - 1.0f;
    float y             = (1.0f - uv.y) * 2.0f - 1.0f;
    float4 pos_clip     = float4(x, y, z, 1.0f);
    float4 pos_world = mul(pos_clip, buffer_rendererPath.view_projection_inverted);
    return pos_world.xyz / pos_world.w;
}

float3 get_position(float2 uv)
{
    return get_position(get_depth(uv), uv);
}

float3 get_position(uint2 pos)
{
    const float2 uv = (pos + 0.5f) / pass_get_resolution_out();
    return get_position(get_depth(pos), uv);
}

float3 get_position_view_space(uint2 pos)
{
    return mul(float4(get_position(pos), 1.0f), buffer_rendererPath.view).xyz;
}

float3 get_position_view_space(float2 uv)
{
    return mul(float4(get_position(uv), 1.0f), buffer_rendererPath.view).xyz;
}

/*------------------------------------------------------------------------------
    VIEW DIRECTION
------------------------------------------------------------------------------*/
float3 get_view_direction(float3 position_world)
{
    return normalize(position_world - buffer_rendererPath.camera_position.xyz);
}

float3 get_view_direction(float depth, float2 uv)
{
    return get_view_direction(get_position(depth, uv));
}

float3 get_view_direction(float2 uv)
{
    return get_view_direction(get_position(uv));
}

float3 get_view_direction(uint2 pos)
{
    const float2 uv = (pos + 0.5f) / pass_get_resolution_out();
    return get_view_direction(uv);
}

float3 get_view_direction_view_space(float2 uv)
{
    return mul(float4(get_view_direction(get_position(uv)), 0.0f), buffer_rendererPath.view).xyz;
}

float3 get_view_direction_view_space(uint2 pos)
{
    const float2 uv = (pos + 0.5f) / pass_get_resolution_out();
    return get_view_direction_view_space(uv);
}

float3 get_view_direction_view_space(float3 position_world)
{
    return mul(float4(get_view_direction(position_world), 0.0f), buffer_rendererPath.view).xyz;
}

/*------------------------------------------------------------------------------
    DIRECTION UV
------------------------------------------------------------------------------*/
float2 direction_sphere_uv(float3 direction)
{
    float n = length(direction.xz);
    float2 uv = float2((n > 0.0000001) ? direction.x / n : 0.0, direction.y);
    uv = acos(uv) * INV_PI;
    uv.x = (direction.z > 0.0) ? uv.x * 0.5 : 1.0 - (uv.x * 0.5);
    uv.x = 1.0 - uv.x;
    
    return uv;
}

uint direction_to_cube_face_index(const float3 direction)
{
    float3 direction_abs = abs(direction);
    float max_coordinate = max3(direction_abs);
    
    if (max_coordinate == direction_abs.x)
    {
        return direction_abs.x == direction.x ? 0 : 1;
    }
    else if (max_coordinate == direction_abs.y)
    {
        return direction_abs.y == direction.y ? 2 : 3;
    }
    else
    {
        return direction_abs.z == direction.z ? 4 : 5;
    }
    
    return 0;
}

/*------------------------------------------------------------------------------
    LUMINANCE
------------------------------------------------------------------------------*/
static const float3 srgb_color_space_coefficient = float3(0.299f, 0.587f, 0.114f);

float luminance(float3 color)
{
    return max(dot(color, srgb_color_space_coefficient), FLT_MIN);
}

float luminance(float4 color)
{
    return max(dot(color.rgb, srgb_color_space_coefficient), FLT_MIN);
}

/*------------------------------------------------------------------------------
    NOISE/RANDOM
------------------------------------------------------------------------------*/
float get_random(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

// An expansion on the interleaved gradient function from Jimenez 2014 http://goo.gl/eomGso
float get_noise_interleaved_gradient(float2 screen_pos, bool animate, bool animate_even_with_taa_off)
{
    // temporal factor
    float animate_    = saturate((float)is_taa_enabled() + (float)animate_even_with_taa_off) * (float)animate;
    float frame_count = (float)buffer_frame.frame;
    float frame_step  = float(frame_count % 16) * RPC_16 * animate_;
    screen_pos.x     += frame_step * 4.7526;
    screen_pos.y     += frame_step * 3.1914;

    float3 magic = float3(0.06711056f, 0.00583715f, 52.9829189f);
    return frac(magic.z * frac(dot(screen_pos, magic.xy)));
}

// float get_noise_blue(float2 screen_pos)
// {
    // // Temporal factor - animated blue noise image
    // //float taaOn      = (float)is_taa_enabled();
    // //float frameCount = (float)g_frame;
    // //float frameStep  = taaOn * float(frameCount % 16) * RPC_16;
    // //screen_pos.x     += frameStep * 4.7526;
    // //screen_pos.y    += frameStep * 3.1914;

    // // Temporal factor - alternate between blue noise images
    // float slice = (buffer_frame.frame % 8) * (float)is_taa_enabled();

    // float2 uv = (screen_pos + 0.5f) * get_tex_noise_blue_scale();
    // return tex_noise_blue.SampleLevel(samplers[sampler_point_wrap], float3(uv.x, uv.y, slice), 0).r;
// }

// float3 get_noise_normal(uint2 screen_pos)
// {
    // float2 uv = (screen_pos + 0.5f) * get_tex_noise_normal_scale();
    // return normalize(tex_noise_normal.SampleLevel(samplers[sampler_point_wrap], uv, 0).xyz);
// }

/*------------------------------------------------------------------------------
    OCCLUSION/SHADOWING
------------------------------------------------------------------------------*/
// The Technical Art of Uncharted 4 - http://advances.realtimerendering.com/other/2016/naughty_dog/index.html
float microw_shadowing_nt(float n_dot_l, float ao)
{
    float aperture = 2.0f * ao * ao;
    return saturate(abs(n_dot_l) + aperture - 1.0f);
}

// Chan 2018, "Material Advances in Call of Duty: WWII"
float microw_shadowing_cod(float n_dot_l, float visibility)
{
    float aperture = rsqrt(1.0 - visibility);
    float microShadow = saturate(n_dot_l * aperture);
    return microShadow * microShadow;
}

float3 project_onto_paraboloid(float3 light_to_vertex_view, float near_plane, float far_plane)
{
    float3 ndc = 0.0f;

    // normalize the light to vertex vector
    float d = length(light_to_vertex_view);
    light_to_vertex_view /= d;

    // project onto paraboloid
    ndc.xy = light_to_vertex_view.xy / (light_to_vertex_view.z + 1.0f);

     // calculate reverse depth
    ndc.z = (far_plane - d) / (far_plane - near_plane);

    // if the vertex is behind the light, clamp it to the edge of the circular paraboloid
    float is_valid = step(0.0f, light_to_vertex_view.z);
    float radius_squared = dot(ndc.xy, ndc.xy);
    float clamped_radius = sqrt(clamp(radius_squared, 0.0f, 1.0f));
    ndc.xy = is_valid * ndc.xy + (1.0f - is_valid) * (ndc.xy / clamped_radius);

    return ndc;
}
/*------------------------------------------------------------------------------
    PRIMITIVES
------------------------------------------------------------------------------*/
float draw_line(float2 p1, float2 p2, float2 uv, float a)
{
    float r = 0.0f;
    float one_px = 1. / pass_get_resolution_out().x; // not really one px

    // get dist between points
    float d = distance(p1, p2);

    // get dist between current pixel and p1
    float duv = distance(p1, uv);

    // if point is on line, according to dist, it should match current uv 
    r = 1.0f - floor(1.0f - (a * one_px) + distance(lerp(p1, p2, clamp(duv / d, 0.0f, 1.0f)), uv));

    return r;
}

float draw_line_view_space(float3 p1, float3 p2, float2 uv, float a)
{
    float2 p1_uv = view_to_uv(p1);
    float2 p2_uv = view_to_uv(p2);
    return draw_line(p1_uv, p2_uv, uv, a);
}

float draw_circle(float2 origin, float radius, float2 uv)
{
    return (distance(origin, uv) <= radius) ? 1.0f : 0.0f;
}

float draw_circle_view_space(float3 origin, float radius, float2 uv)
{
    float2 origin_uv = view_to_uv(origin);
    return draw_circle(origin_uv, radius, uv);
}

/*------------------------------------------------------------------------------
    MISC
------------------------------------------------------------------------------*/
float3 compute_diffuse_energy(float3 F, float metallic)
{
    float3 kS = F;          // The energy of light that gets reflected - Equal to Fresnel
    float3 kD = 1.0f - kS;  // Remaining energy, light that gets refracted
    kD *= 1.0f - metallic;  // Multiply kD by the inverse metalness such that only non-metals have diffuse lighting
    
    return kD;
}

float screen_fade(float2 uv)
{
    float2 fade = max(0.0f, 12.0f * abs(uv - 0.5f) - 5.0f);
    return saturate(1.0f - dot(fade, fade));
}

// Find good arbitrary axis vectors to represent U and V axes of a plane,
// given just the normal. Ported from UnMath.h
void find_best_axis_vectors(float3 In, out float3 Axis1, out float3 Axis2)
{
    const float3 N = abs(In);

    // Find best basis vectors.
    if (N.z > N.x && N.z > N.y)
    {
        Axis1 = float3(1, 0, 0);
    }
    else
    {
        Axis1 = float3(0, 0, 1);
    }

    Axis1 = normalize(Axis1 - In * dot(Axis1, In));
    Axis2 = cross(Axis1, In);
}

static const float3 hemisphere_samples[64] =
{
    float3(0.04977, -0.04471, 0.04996),
    float3(0.01457, 0.01653, 0.00224),
    float3(-0.04065, -0.01937, 0.03193),
    float3(0.01378, -0.09158, 0.04092),
    float3(0.05599, 0.05979, 0.05766),
    float3(0.09227, 0.04428, 0.01545),
    float3(-0.00204, -0.0544, 0.06674),
    float3(-0.00033, -0.00019, 0.00037),
    float3(0.05004, -0.04665, 0.02538),
    float3(0.03813, 0.0314, 0.03287),
    float3(-0.03188, 0.02046, 0.02251),
    float3(0.0557, -0.03697, 0.05449),
    float3(0.05737, -0.02254, 0.07554),
    float3(-0.01609, -0.00377, 0.05547),
    float3(-0.02503, -0.02483, 0.02495),
    float3(-0.03369, 0.02139, 0.0254),
    float3(-0.01753, 0.01439, 0.00535),
    float3(0.07336, 0.11205, 0.01101),
    float3(-0.04406, -0.09028, 0.08368),
    float3(-0.08328, -0.00168, 0.08499),
    float3(-0.01041, -0.03287, 0.01927),
    float3(0.00321, -0.00488, 0.00416),
    float3(-0.00738, -0.06583, 0.0674),
    float3(0.09414, -0.008, 0.14335),
    float3(0.07683, 0.12697, 0.107),
    float3(0.00039, 0.00045, 0.0003),
    float3(-0.10479, 0.06544, 0.10174),
    float3(-0.00445, -0.11964, 0.1619),
    float3(-0.07455, 0.03445, 0.22414),
    float3(-0.00276, 0.00308, 0.00292),
    float3(-0.10851, 0.14234, 0.16644),
    float3(0.04688, 0.10364, 0.05958),
    float3(0.13457, -0.02251, 0.13051),
    float3(-0.16449, -0.15564, 0.12454),
    float3(-0.18767, -0.20883, 0.05777),
    float3(-0.04372, 0.08693, 0.0748),
    float3(-0.00256, -0.002, 0.00407),
    float3(-0.0967, -0.18226, 0.29949),
    float3(-0.22577, 0.31606, 0.08916),
    float3(-0.02751, 0.28719, 0.31718),
    float3(0.20722, -0.27084, 0.11013),
    float3(0.0549, 0.10434, 0.32311),
    float3(-0.13086, 0.11929, 0.28022),
    float3(0.15404, -0.06537, 0.22984),
    float3(0.05294, -0.22787, 0.14848),
    float3(-0.18731, -0.04022, 0.01593),
    float3(0.14184, 0.04716, 0.13485),
    float3(-0.04427, 0.05562, 0.05586),
    float3(-0.02358, -0.08097, 0.21913),
    float3(-0.14215, 0.19807, 0.00519),
    float3(0.15865, 0.23046, 0.04372),
    float3(0.03004, 0.38183, 0.16383),
    float3(0.08301, -0.30966, 0.06741),
    float3(0.22695, -0.23535, 0.19367),
    float3(0.38129, 0.33204, 0.52949),
    float3(-0.55627, 0.29472, 0.3011),
    float3(0.42449, 0.00565, 0.11758),
    float3(0.3665, 0.00359, 0.0857),
    float3(0.32902, 0.0309, 0.1785),
    float3(-0.08294, 0.51285, 0.05656),
    float3(0.86736, -0.00273, 0.10014),
    float3(0.45574, -0.77201, 0.00384),
    float3(0.41729, -0.15485, 0.46251),
    float3(-0.44272, -0.67928, 0.1865)
};

//= INCLUDES =================
#include "common_structs.hlsl"
//============================

#endif // SPARTAN_COMMON

```

```c

#include "common_textures.hlsl"
#include "common_samplers.hlsl"

struct FrameBufferData
{
    //matrix view;
    //matrix projection;
    //matrix view_projection;
    //matrix view_projection_inverted;
    //matrix view_projection_orthographic;
    //matrix view_projection_unjittered;
    //matrix view_projection_previous;

    float2 resolution_render;
    float2 resolution_output;

    float2 taa_jitter_current;
    float2 taa_jitter_previous;
    
    float delta_time;
    uint frame;
    float gamma;
    uint options;
    
    //float3 camera_position;
    //float camera_near;
    
    //float3 camera_direction;
    //float camera_far;
};

struct RendererPathBufferData
{
    matrix view;
    matrix projection;
    matrix view_projection;
    matrix view_projection_inverted;
    matrix view_projection_orthographic;
    matrix view_projection_unjittered;
    matrix view_projection_previous;
    
    float3 camera_position;
    float camera_near;
    
    float3 camera_direction;
    float camera_far;
};

struct LightBufferData
{
    matrix view_projection[6];
    
    float4 color;

    float3 position;
    float intensity;

    float3 forward;
    float range;
    
    float angle;
    uint flags;
    float2 padding;

    
    // lighting properties
    bool light_is_directional()
    {
        return flags & uint(1U << 0);
    }
    bool light_is_point()
    {
        return flags & uint(1U << 1);
    }
    bool light_is_spot()
    {
        return flags & uint(1U << 2);
    }
    bool light_has_shadows()
    {
        return flags & uint(1U << 3);
    }
    bool light_has_shadows_transparent()
    {
        return flags & uint(1U << 4);
    }
    bool light_is_volumetric()
    {
        return flags & uint(1U << 5);
    }
    
    float compare_depth(float3 uv, float compare)
    {
        return tex_light_depth.SampleCmpLevelZero(samplers_comparison[sampler_compare_depth], uv, compare).r;
    }
    
    float sample_depth(float3 uv)
    {
        return tex_light_depth.SampleLevel(samplers[sampler_bilinear_clamp_border], uv, 0).r;
    }
    
    float3 sample_color(float3 uv)
    {
        return tex_light_color.SampleLevel(samplers[sampler_bilinear_clamp_border], uv, 0).rgb;
    }
    
    float2 compute_resolution()
    {
        float2 resolution;
        uint layer_count;
        tex_light_depth.GetDimensions(resolution.x, resolution.y, layer_count);

        return resolution;
    }

    float3 compute_direction(float3 fragment_position)
    {
        float3 direction = 0.0f;
        
        if (light_is_directional())
        {
            direction = normalize(forward.xyz);
        }
        else if (light_is_point() || light_is_spot())
        {
            direction = normalize(fragment_position - position);
        }

        return direction;
    }

    // attenuation over distance
    float compute_attenuation_distance(const float3 surface_position)
    {
        float distance_to_pixel = length(surface_position - position);
        float attenuation = saturate(1.0f - distance_to_pixel / range);
        return attenuation * attenuation;
    }

    float compute_attenuation_angle(float3 surface_position)
    {

        float3 to_pixel = compute_direction(surface_position);
        float cos_outer = cos(angle);
        float cos_inner = cos(angle * 0.9f);
        float cos_outer_squared = cos_outer * cos_outer;
        float scale = 1.0f / max(0.001f, cos_inner - cos_outer);
        float offset = -cos_outer * scale;
        float cd = dot(to_pixel, forward);
        float attenuation = saturate(cd * scale + offset);
        
        return attenuation * attenuation;
    }

    float compute_attenuation(const float3 surface_position)
    {
        float attenuation = 0.0f;
        
        if (light_is_directional())
        {
            attenuation = saturate(dot(-forward, float3(0.0f, 1.0f, 0.0f)));
        }
        else if (light_is_point())
        {
            attenuation = compute_attenuation_distance(surface_position);
        }
        else if (light_is_spot())
        {
            attenuation = compute_attenuation_distance(surface_position) * compute_attenuation_angle(surface_position);
        }

        return attenuation;
    }


};
RWStructuredBuffer<LightBufferData> buffer_lights : register(u1);

struct MaterialBufferData
{
    float4 color;

    float2 tiling;
    float2 offset;

    float roughness;
    float metallness;
    float normal;
    float height;

    uint properties;
    float clearcoat;
    float clearcoat_roughness;
    float anisotropic;
    
    float anisotropic_rotation;
    float sheen;
    float sheen_tint;
    float padding3;
};

const static int MaxLight = 15;
struct LightBufferDataArr
{
    int lightCount;
    int3 padding;
    LightBufferData lightBufferDataArr[MaxLight];
};

const static int MaxBone = 512;
struct BoneDataArr
{
    //int boneCount;
    //int3 padding;
    matrix boneTransformArr[MaxBone];
};

cbuffer BufferFrame : register(b0)
{
    FrameBufferData buffer_frame;
}; // low frequency    - updates once per frame
cbuffer BufferLight : register(b1)
{
    LightBufferData buffer_light;
}; // medium frequency - updates per light
cbuffer BufferMaterial : register(b2)
{
    MaterialBufferData buffer_material;
}; // medium frequency - updates per material during the g-buffer pass
//cbuffer BufferLightArr : register(b3)
//{
//    LightBufferDataArr light_buffer_data_arr;
//}
cbuffer BoneDataArr : register(b4)
{
    BoneDataArr bone_data_arr;
}
cbuffer BufferRendererPath : register(b5)
{
    RendererPathBufferData buffer_rendererPath;
}

struct PassBufferData
{
    matrix transform;
    matrix m_value;
};

[[vk::push_constant]]
PassBufferData buffer_pass;

// g-buffer texture properties
bool has_single_texture_roughness_metalness()
{
    return buffer_material.properties & uint(1U << 0);
}
bool has_texture_height()
{
    return buffer_material.properties & uint(1U << 1);
}
bool has_texture_normal()
{
    return buffer_material.properties & uint(1U << 2);
}
bool has_texture_albedo()
{
    return buffer_material.properties & uint(1U << 3);
}
bool has_texture_roughness()
{
    return buffer_material.properties & uint(1U << 4);
}
bool has_texture_metalness()
{
    return buffer_material.properties & uint(1U << 5);
}
bool has_texture_alpha_mask()
{
    return buffer_material.properties & uint(1U << 6);
}
bool has_texture_emissive()
{
    return buffer_material.properties & uint(1U << 7);
}
bool has_texture_occlusion()
{
    return buffer_material.properties & uint(1U << 8);
}
bool material_is_terrain()
{
    return buffer_material.properties & uint(1U << 9);
}
bool material_is_water()
{
    return buffer_material.properties & uint(1U << 10);
}

// frame properties
bool is_taa_enabled()
{
    return any(buffer_frame.taa_jitter_current);
}
bool is_ssr_enabled()
{
    return buffer_frame.options & uint(1U << 0);
}
bool is_ssgi_enabled()
{
    return buffer_frame.options & uint(1U << 1);
}
bool is_screen_space_shadows_enabled()
{
    return buffer_frame.options & uint(1U << 2);
}
bool is_fog_enabled()
{
    return buffer_frame.options & uint(1U << 3);
}
bool is_fog_volumetric_enabled()
{
    return buffer_frame.options & uint(1U << 4);
}

// pass properties
matrix pass_get_transform_previous()
{
    return buffer_pass.m_value;
}
float2 pass_get_resolution_in()
{
    return float2(buffer_pass.m_value._m03, buffer_pass.m_value._m22);
}
float2 pass_get_resolution_out()
{
    return float2(buffer_pass.m_value._m23, buffer_pass.m_value._m30);
}
float3 pass_get_f3_value()
{
    return float3(buffer_pass.m_value._m00, buffer_pass.m_value._m01, buffer_pass.m_value._m02);
}
float3 pass_get_f3_value2()
{
    return float3(buffer_pass.m_value._m20, buffer_pass.m_value._m21, buffer_pass.m_value._m31);
}
float4 pass_get_f4_value()
{
    return float4(buffer_pass.m_value._m10, buffer_pass.m_value._m11, buffer_pass.m_value._m12, buffer_pass.m_value._m13);
}
bool pass_is_transparent()
{
    return buffer_pass.m_value._m33;
}
bool pass_is_reflection_probe_available()
{
    return pass_get_f4_value().x == 1.0f;
} // this is more risky
bool pass_is_opaque()
{
    return !pass_is_transparent();
}
// m32 is free

```

```c

// for details, read my blog post: https://panoskarabelas.com/blog/posts/hdr_in_under_10_minutes/

static const float gamma_sdr = 2.2f; // SDR monitors are likely old, and aim for a simplistic gamma 2.2 curve
static const float gamma_hdr = 2.4f; // HDR monitors are more likely to aim for the actual sRGB standard, which has a curve that for mid tones to high lights resembles a gamma of 2.4

float3 srgb_to_linear(float3 color)
{
    float gamma        = lerp(gamma_sdr, gamma_hdr, false);
    float3 linear_low  = color / 12.92;
    float3 linear_high = pow((color + 0.055) / 1.055, gamma);
    float3 is_high     = step(0.0404482362771082, color);
    return lerp(linear_low, linear_high, is_high);
}

float3 linear_to_srgb(float3 color)
{
    float gamma = lerp(gamma_sdr, gamma_hdr, false);
    float3 srgb_low  = color * 12.92;
    float3 srgb_high = 1.055 * pow(color, 1.0 / gamma) - 0.055;
    float3 is_high   = step(0.00313066844250063, color);
    return lerp(srgb_low, srgb_high, is_high);
}

float3 linear_to_hdr10(float3 color, float white_point)
{
    // convert Rec.709 (similar to srgb) to Rec.2020 color space
    {
        static const float3x3 from709to2020 =
        {
            { 0.6274040f, 0.3292820f, 0.0433136f },
            { 0.0690970f, 0.9195400f, 0.0113612f },
            { 0.0163916f, 0.0880132f, 0.8955950f }
        };
        
        color = mul(from709to2020, color);
    }

    // normalize HDR scene values ([0..>1] to [0..1]) for the ST.2084 curve
    const float st2084_max = 10000.0f;
    color *= white_point / st2084_max;

    // apply ST.2084 (PQ curve) for HDR10 standard
    {
        static const float m1 = 2610.0 / 4096.0 / 4;
        static const float m2 = 2523.0 / 4096.0 * 128;
        static const float c1 = 3424.0 / 4096.0;
        static const float c2 = 2413.0 / 4096.0 * 32;
        static const float c3 = 2392.0 / 4096.0 * 32;
        float3 cp             = pow(abs(color), m1);
        color                 = pow((c1 + c2 * cp) / (1 + c3 * cp), m2);
    }

    return color;
}

```

```c
  
#include "common.hlsl"
#include "shadow_mapping.hlsl"

/**
 * \brief Light Render Model For BilinnPhong
 * \param viewDir 
 * \param normal 
 * \param diffuseTex 
 * \param specularTex 
 * \param shininess 
 * \param lightDir 
 * \param lightColor 
 * \param luminosity 
 * \return 
 */
float3 BilinnPhong(float3 viewDir, float3 normal, float3 diffuseTex, float3 specularTex, float shininess, float3 lightDir, float3 lightColor, float luminosity)
{
    const float3 halfwayDir = normalize(lightColor + viewDir);
    const float diffuseCoefficient = max(dot(normal, lightDir), 0.0);
    const float specularCoefficient = pow(max(dot(normal, halfwayDir), 0.0), shininess * 2.0);

    return lightColor * diffuseTex.rgb * diffuseCoefficient * luminosity + ((luminosity > 0.0) ?
    (lightColor * specularTex.rgb * specularCoefficient * luminosity) : float3(0, 0, 0));
}

float3 CalcPointLight(float3 viewDir, float3 normal, float3 diffuseTex, float3 specularTex, float shininess, LightBufferData light_buffer_data)
{
    float3 lightPosition = light_buffer_data.position.xyz;
    float3 lightDir = light_buffer_data.forward.xyz;
    float3 lightColor = light_buffer_data.color.rgb;
    float luminosity = light_buffer_data.intensity;


    return BilinnPhong(viewDir, normal, diffuseTex, specularTex, shininess, lightDir, lightColor, luminosity);
}


float3 CalcDirectionalLight(float3 viewDir, float3 normal, float3 diffuseTex, float3 specularTex, float shininess, LightBufferData light_buffer_data, float luminosity)
{
    return BilinnPhong(viewDir, normal, diffuseTex, specularTex, shininess, light_buffer_data.forward.xyz, light_buffer_data.color.rgb, luminosity);
}


// return shadow ratio
float ShadowCalculation(float3 fragWorldNormal, float3 fragWorldPos)
{
    // default shadow value for fully lit (no shadow)
    float shadow = 1.0f;
    int mainLightIndex = 0;
    if (buffer_lights[mainLightIndex].light_has_shadows())
    {
        LightBufferData light = buffer_lights[mainLightIndex];
        
        // process only if the pixel is within the light's effective range
        float distance_to_pixel = length(fragWorldPos - light.position);
        if (distance_to_pixel < light.range)
        {
            
            // project to light space
            // uint slice_index = light.light_is_point() ? direction_to_cube_face_index(light.to_pixel) : 0;
            uint slice_index = 0;
        
            float3 light_to_pixel = light.compute_direction(fragWorldPos);
            float light_n_dot_l = saturate(dot(fragWorldNormal, -light_to_pixel));
        
            float2 resolution = light.compute_resolution();
            float2 texel_size = 1.0f / resolution;

            // compute world position with normal offset bias to reduce shadow acne
            //float3 normal_offset_bias = surface.normal * (1.0f - saturate(light.n_dot_l)) * light.normal_bias * get_shadow_texel_size();
            //float3 normal_offset_bias = fragWorldNormal * (1.0f - saturate(light_n_dot_l)) * light.normal_bias * 1.0f;
            float3 normal_offset_bias = fragWorldNormal * (1.0f - saturate(light_n_dot_l)) * texel_size.x * 200.0f;
            // float3 normal_offset_bias = fragWorldNormal * light.normal_bias;
            float3 position_world = fragWorldPos + normal_offset_bias;
        
	        // project into light space
            float3 pos_ndc = world_to_ndc(position_world, light.view_projection[slice_index]);
            float2 pos_uv = ndc_to_uv(pos_ndc);

            if (is_valid_uv(pos_uv))
            {
                // ȡ�����������(ʹ��[0,1]��Χ�µ�fragPosLight������)
                shadow = SampleShadowMap(light, float3(pos_uv, slice_index), pos_ndc.z);
            }

            // blend with the far cascade for the directional lights
            float cascade_fade = saturate((max(abs(pos_ndc.x), abs(pos_ndc.y)) - g_shadow_cascade_blend_threshold) * 4.0f);
            if (light.light_is_directional() && cascade_fade > 0.0f)
            {
            // sample shadow map
                slice_index = 1;
                pos_ndc = world_to_ndc(position_world, light.view_projection[slice_index]);
                pos_uv = ndc_to_uv(pos_ndc);
                float shadow_far = SampleShadowMap(light, float3(pos_uv, slice_index), pos_ndc.z);

            // blend/lerp
                shadow = lerp(shadow, shadow_far, cascade_fade);
            }
        }

    }

    return shadow;
}


// return shadow ratio
float ShadowCalculation2(float3 fragWorldNormal, float3 fragWorldPos, int lightIndex)
{
    float shadow = 1.0f;
    LightBufferData light = buffer_lights[lightIndex];
    if (light.light_has_shadows())
    {
        // process only if the pixel is within the light's effective range
        float distance_to_pixel = length(fragWorldPos - light.position);
        if (distance_to_pixel < light.range)
        {
            float3 light_to_pixel = light.compute_direction(fragWorldPos);
            float light_n_dot_l = saturate(dot(fragWorldNormal, -light_to_pixel));
        
            float2 resolution = light.compute_resolution();
            float2 texel_size = 1.0f / resolution;
            float3 normal_offset_bias = fragWorldNormal * (1.0f - saturate(light_n_dot_l)) * texel_size.x;
            float3 position_world = fragWorldPos + normal_offset_bias;
        

            if (light.light_is_point())
            {
             // compute paraboloid coordinates and depth
                uint light_slice_index = dot(light.forward, light_to_pixel) < 0.0f; // 0 = front, 1 = back
                uint slice_index = 2 * lightIndex + light_slice_index;
                float3 pos_view = mul(float4(position_world, 1.0f), light.view_projection[light_slice_index]).xyz;
                float3 light_to_vertex_view = pos_view;
                float3 ndc = project_onto_paraboloid(light_to_vertex_view, 0.01, light.range);

            // sample shadow map
                float3 sample_coords = float3(ndc_to_uv(ndc.xy), slice_index);
                shadow = SampleShadowMap(light, sample_coords, ndc.z);

            // // handle transparent shadows if necessary
            //if (shadow.a > 0.0f && light.has_shadows_transparent())
            //{
            //    shadow.rgb = Technique_Vogel_Color(light, surface, sample_coords);
            //}
            }
            else
            {
                // project to light space
                uint light_slice_index = 2 * lightIndex + 0;
                uint slice_index = 2 * lightIndex + light_slice_index;

                // project into light space
                float3 pos_ndc = world_to_ndc(position_world, light.view_projection[light_slice_index]);
                float2 pos_uv = ndc_to_uv(pos_ndc);

                if (is_valid_uv(pos_uv))
                {
                    shadow = SampleShadowMap(light, float3(pos_uv, slice_index), pos_ndc.z);

                    //if (shadow.a > 0.0f && light.has_shadows_transparent())
                    //{
                    //    shadow.rgb = Technique_Vogel_Color(light, surface, sample_coords);
                    //}

                    // blend with the far cascade for the directional lights
                    float cascade_fade = saturate((max(abs(pos_ndc.x), abs(pos_ndc.y)) - g_shadow_cascade_blend_threshold) * 4.0f);
                    if (light.light_is_directional() && cascade_fade > 0.0f)
                    {
                        // sample shadow map
                        light_slice_index = 2 * lightIndex + 1;
                        slice_index = 2 * lightIndex + light_slice_index;
                        pos_ndc = world_to_ndc(position_world, light.view_projection[light_slice_index]);
                        pos_uv = ndc_to_uv(pos_ndc);
                        float shadow_far = SampleShadowMap(light, float3(pos_uv, slice_index), pos_ndc.z);

                        // blend/lerp
                        shadow = lerp(shadow, shadow_far, cascade_fade);
                    }
                }
            }

        }
       
    }

 
    return shadow;
}



float3 CalcOneLightColorPBR(
float3 fragPos,
float4 albedo, float metallic, float squareRoughness, float lerpSquareRoughness,
float3 normal, float3 viewDir, float nv,
int light_index)
{
    float PI = 3.14;
    float3 colorSpaceDielectricSpecRgb = float3(0.04, 0.04, 0.04);

    LightBufferData lightBufferData = buffer_lights[light_index];
    float3 light_to_pixel = lightBufferData.compute_direction(fragPos);
    float3 lightDir = -light_to_pixel;
    float attenuation = lightBufferData.compute_attenuation(fragPos);
    float3 halfVector = normalize(lightDir + viewDir);

    float nl = max(saturate(dot(normal, lightDir)), 0.000001);
    float lh = max(saturate(dot(lightDir, halfVector)), 0.000001);
    float vh = max(saturate(dot(viewDir, halfVector)), 0.000001);
    float nh = max(saturate(dot(normal, halfVector)), 0.000001);

    float3 randiance = lightBufferData.color.xyz * lightBufferData.intensity * attenuation * nl * 1.0f;;
    
	// temp code
    float shadow = 0;
    bool t = pass_is_transparent();
    if (t)
    {
        shadow = ShadowCalculation2(normal, fragPos, light_index);
    }
    else
    {
        shadow = ShadowCalculation2(normal, fragPos, light_index);
    }

    randiance *= shadow;

	// calculate D F G for Specular
    float D = lerpSquareRoughness / (pow((pow(nh, 2) * (lerpSquareRoughness - 1) + 1), 2) * PI);

    float3 F0 = lerp(colorSpaceDielectricSpecRgb, albedo.xyz, metallic);
    float3 F = F0 + (1 - F0) * exp2((-5.55473 * vh - 6.98316) * vh);

    float kInDirectLight = pow(squareRoughness + 1, 2) / 8;
    float kInIBL = pow(squareRoughness, 2) / 8;
    float GLeft = nl / lerp(nl, 1, kInDirectLight);
    float GRight = nv / lerp(nv, 1, kInDirectLight);
    float G = GLeft * GRight;

    float3 kd = (1 - F) * (1 - metallic);

	// calculate directLightResult with diffuse and specular
    float3 diffColor = kd * albedo.xyz / PI;
    float3 specColor = (D * G * F) / (4 * nv * nl);
    float3 directLightResult = (diffColor + specColor) * randiance;

	// calculate indirectLightResult todo
    float3 iblDiffuseResult = 0;
    float3 iblSpecularResult = 0;
    float3 indirectResult = iblDiffuseResult + iblSpecularResult;

    return directLightResult + indirectResult;
}


```

```c

// comparsion
static const int sampler_compare_depth    = 0;

// regular
static const uint sampler_point_clamp_edge = 0;
static const uint sampler_point_clamp_border = 1;
static const uint sampler_point_wrap = 2;
static const uint sampler_bilinear_clamp = 3;
static const uint sampler_bilinear_clamp_border = 4;
static const uint sampler_bilinear_wrap = 5;
static const uint sampler_trilinear_clamp = 6;
static const uint sampler_anisotropic_wrap = 7;
// samplers
SamplerComparisonState samplers_comparison[] : register(s0, space1);
SamplerState samplers[]                      : register(s1, space2);

```

```c

// // G-buffer
Texture2D tex_depth             : register(t16);

 // Light depth/color maps
Texture2DArray tex_light_depth : register(t20);
Texture2DArray tex_light_color : register(t21);

// // Misc
Texture2D tex_frame              : register(t34);
Texture2D tex                    : register(t35);
Texture2D tex_font_atlas         : register(t37);

// // Storage
// RWTexture2D<float4> tex_uav                                : register(u0);
// RWTexture2D<float4> tex_uav2                               : register(u1);
// RWTexture2D<float4> tex_uav3                               : register(u2);
// globallycoherent RWStructuredBuffer<uint> g_atomic_counter : register(u3);
// globallycoherent RWTexture2D<float4> tex_uav_mips[12]      : register(u4);
// RWTexture2DArray<float4> tex_uav4                          : register(u5);

// SkyBox
TextureCube tex_skyBox : register(t40);

```

```c

// Vertex
struct Vertex_Pos
{
    float4 position : POSITION0;
};

struct Vertex_PosUv
{
    float4 position : POSITION0;
    float2 uv       : TEXCOORD0;
};

struct Vertex_PosColor
{
    float4 position : POSITION0;
    float4 color    : COLOR0;
};

struct Vertex_Pos2dUvColor
{
    float2 position : POSITION0;
    float2 uv       : TEXCOORD0;
    float4 color    : COLOR0;
};

struct Vertex_PosUvNorTan
{
    float4 position           : POSITION0;
    float2 uv                 : TEXCOORD0;
    float3 normal             : NORMAL0;
    float3 tangent            : TANGENT0;
    #if INSTANCED
    matrix instance_transform : INSTANCE_TRANSFORM0;
    #endif
};

struct Vertex_PosUvNorTanBone
{
    float4 position : POSITION0;
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL0;
    float3 tangent : TANGENT0;
    uint4 boneIndices : BLENDINDEX;
    float3 boneWeights : BLENDWEIGHT;
};

// Pixel
struct Pixel_Pos
{
    float4 position : SV_POSITION;
};

struct Pixel_PosUv
{
    float4 position : SV_POSITION;
    float2 uv       : TEXCOORD;
};

struct Pixel_PosColor
{
    float4 position : SV_POSITION;
    float4 color    : COLOR0;
};


struct Pixel_PosNor
{
    float4 position : SV_POSITION;
    float3 normal   : NORMAL;
};

struct Pixel_PosColUv
{
    float4 position : SV_POSITION;
    float4 color    : COLOR0;
    float2 uv       : TEXCOORD;
};

struct Pixel_PosUvNorTan
{
    float4 position : SV_POSITION;
    float2 uv       : TEXCOORD;
    float3 normal   : NORMAL;
    float3 tangent  : TANGENT;
};

```


```c

//= INLUCES =============
#include "common.hlsl"
//=======================

//==================================

/*------------------------------------------------------------------------------
    SETTINGS
------------------------------------------------------------------------------*/
// technique
#define SampleShadowMap Technique_Vogel

// technique - all
static const uint   g_shadow_samples                 = 4;
static const float g_shadow_filter_size = 6.0f;
static const float  g_shadow_cascade_blend_threshold = 0.7f;
// technique - vogel
static const uint   g_penumbra_samples               = 8;
static const float  g_penumbra_filter_size           = 128.0f;
// technique - pre-calculated
static const float g_pcf_filter_size    = (sqrt((float)g_shadow_samples) - 1.0f) / 2.0f;
static const float g_shadow_samples_rpc = 1.0f / (float) g_shadow_samples;

/*------------------------------------------------------------------------------
    SHADER VARIABLES
------------------------------------------------------------------------------*/

float get_shadow_resolution()
{
    return pass_get_f3_value().y;
}

float get_shadow_texel_size()
{
    return (1.0f / get_shadow_resolution());
}

/*------------------------------------------------------------------------------
    LIGHT SHADOW MAP SAMPLING
------------------------------------------------------------------------------*/

/*------------------------------------------------------------------------------
    PENUMBRA
------------------------------------------------------------------------------*/
float2 vogel_disk_sample(uint sample_index, uint sample_count, float angle)
{
    const float golden_angle = 2.399963f; // radians
    float r                  = sqrt(sample_index + 0.5f) / sqrt(sample_count);
    float theta              = sample_index * golden_angle + angle;
    float sine, cosine;
    sincos(theta, sine, cosine);
    
    return float2(cosine, sine) * r;
}

float compute_penumbra(LightBufferData light, float vogel_angle, float3 uv, float compare)
{
    float penumbra = 1.0f;
    float blocker_depth_avg = 0.0f;
    uint blocker_count = 0;

    for (uint i = 0; i < g_penumbra_samples; i++)
    {
        float2 offset = vogel_disk_sample(i, g_penumbra_samples, vogel_angle) * get_shadow_texel_size() * g_penumbra_filter_size;
        float depth = light.sample_depth(uv + float3(offset, 0.0f));

        if (depth > compare)
        {
            blocker_depth_avg += depth;
            blocker_count++;
        }
    }

    if (blocker_count != 0)
    {
        blocker_depth_avg /= (float) blocker_count;

        // compute penumbra
        penumbra = (compare - blocker_depth_avg) / (blocker_depth_avg + FLT_MIN);
        penumbra *= penumbra;
        penumbra *= 10.0f;
    }
    
    return saturate_16(penumbra);
}

///*------------------------------------------------------------------------------
//    TECHNIQUE - VOGEL
//------------------------------------------------------------------------------*/
float Technique_Vogel(LightBufferData light ,float3 uv, float compare)
{
    float shadow = 0.0f;
    float temporal_offset = get_noise_interleaved_gradient(uv.xy * pass_get_resolution_out(), true, false);
    float temporal_angle = temporal_offset * PI2;
    float penumbra = light.light_is_directional() ? 1.0f : compute_penumbra(light,temporal_angle, uv, compare);

    // todo: in the case of the point light, the uv is the forward, filtering works ok but I could improved it.

    for (uint i = 0; i < g_shadow_samples; i++)
    {
        // float2 offset = vogel_disk_sample(i, g_shadow_samples, temporal_angle) * get_shadow_texel_size() * g_shadow_filter_size * penumbra;
        // float2 offset = vogel_disk_sample(i, g_shadow_samples, temporal_angle) * 1.0f * g_shadow_filter_size * penumbra;
        float2 offset = float2(0.0,0.0f);
        shadow += light.compare_depth(uv + float3(offset, 0.0f), compare);
    }

    return shadow * g_shadow_samples_rpc;
}

//float3 Technique_Vogel_Color(Surface surface, float3 uv)
//{
//    float3 shadow     = 0.0f;
//    float vogel_angle = get_noise_interleaved_gradient(surface.uv * pass_get_resolution_out(), true, false) * PI2;

//    // todo: in the case of the point light, the uv is the forward, filtering works ok but I could improved it.
    
//    for (uint i = 0; i < g_shadow_samples; i++)
//    {
//        float2 offset = vogel_disk_sample(i, g_shadow_samples, vogel_angle) * get_shadow_texel_size() * g_shadow_filter_size;
//        shadow        += shadow_sample_color(uv + float3(offset, 0.0f));
//    } 

//    return shadow * g_shadow_samples_rpc;
//}

/*------------------------------------------------------------------------------
    TECHNIQUE - POISSON
------------------------------------------------------------------------------*/
static const float2 poisson_disk[64] =
{
    float2(-0.5119625f, -0.4827938f),
    float2(-0.2171264f, -0.4768726f),
    float2(-0.7552931f, -0.2426507f),
    float2(-0.7136765f, -0.4496614f),
    float2(-0.5938849f, -0.6895654f),
    float2(-0.3148003f, -0.7047654f),
    float2(-0.42215f, -0.2024607f),
    float2(-0.9466816f, -0.2014508f),
    float2(-0.8409063f, -0.03465778f),
    float2(-0.6517572f, -0.07476326f),
    float2(-0.1041822f, -0.02521214f),
    float2(-0.3042712f, -0.02195431f),
    float2(-0.5082307f, 0.1079806f),
    float2(-0.08429877f, -0.2316298f),
    float2(-0.9879128f, 0.1113683f),
    float2(-0.3859636f, 0.3363545f),
    float2(-0.1925334f, 0.1787288f),
    float2(0.003256182f, 0.138135f),
    float2(-0.8706837f, 0.3010679f),
    float2(-0.6982038f, 0.1904326f),
    float2(0.1975043f, 0.2221317f),
    float2(0.1507788f, 0.4204168f),
    float2(0.3514056f, 0.09865579f),
    float2(0.1558783f, -0.08460935f),
    float2(-0.0684978f, 0.4461993f),
    float2(0.3780522f, 0.3478679f),
    float2(0.3956799f, -0.1469177f),
    float2(0.5838975f, 0.1054943f),
    float2(0.6155105f, 0.3245716f),
    float2(0.3928624f, -0.4417621f),
    float2(0.1749884f, -0.4202175f),
    float2(0.6813727f, -0.2424808f),
    float2(-0.6707711f, 0.4912741f),
    float2(0.0005130528f, -0.8058334f),
    float2(0.02703013f, -0.6010728f),
    float2(-0.1658188f, -0.9695674f),
    float2(0.4060591f, -0.7100726f),
    float2(0.7713396f, -0.4713659f),
    float2(0.573212f, -0.51544f),
    float2(-0.3448896f, -0.9046497f),
    float2(0.1268544f, -0.9874692f),
    float2(0.7418533f, -0.6667366f),
    float2(0.3492522f, 0.5924662f),
    float2(0.5679897f, 0.5343465f),
    float2(0.5663417f, 0.7708698f),
    float2(0.7375497f, 0.6691415f),
    float2(0.2271994f, -0.6163502f),
    float2(0.2312844f, 0.8725659f),
    float2(0.4216993f, 0.9002838f),
    float2(0.4262091f, -0.9013284f),
    float2(0.2001408f, -0.808381f),
    float2(0.149394f, 0.6650763f),
    float2(-0.09640376f, 0.9843736f),
    float2(0.7682328f, -0.07273844f),
    float2(0.04146584f, 0.8313184f),
    float2(0.9705266f, -0.1143304f),
    float2(0.9670017f, 0.1293385f),
    float2(0.9015037f, -0.3306949f),
    float2(-0.5085648f, 0.7534177f),
    float2(0.9055501f, 0.3758393f),
    float2(0.7599946f, 0.1809109f),
    float2(-0.2483695f, 0.7942952f),
    float2(-0.4241052f, 0.5581087f),
    float2(-0.1020106f, 0.6724468f),
};

//float Technique_Poisson(Surface surface, float3 uv, float compare)
//{
//    float shadow          = 0.0f;
//    float temporal_offset = get_noise_interleaved_gradient(uv.xy * get_shadow_resolution(), true, false); // helps with noise if TAA is active

//    for (uint i = 0; i < g_shadow_samples; i++)
//    {
//        uint index    = uint(g_shadow_samples * get_random(uv.xy * i)) % g_shadow_samples; // A pseudo-random number between 0 and 15, different for each pixel and each index
//        float2 offset = (poisson_disk[index] + temporal_offset) * get_shadow_texel_size() * g_shadow_filter_size;
//        shadow        += shadow_compare_depth(uv + float3(offset, 0.0f), compare);
//    }   

//    return shadow * g_shadow_samples_rpc;
//}

///*------------------------------------------------------------------------------
//    TECHNIQUE - PCF
//------------------------------------------------------------------------------*/
//float Technique_Pcf(Surface surface, float3 uv, float compare)
//{
//    float shadow = 0.0f;

//    for (float y = -g_pcf_filter_size; y <= g_pcf_filter_size; y++)
//    {
//        for (float x = -g_pcf_filter_size; x <= g_pcf_filter_size; x++)
//        {
//            float2 offset = float2(x, y) * get_shadow_texel_size();
//            shadow        += shadow_compare_depth(uv + float3(offset, 0.0f), compare);
//        }
//    }
    
//    return shadow * g_shadow_samples_rpc;
//}

///*------------------------------------------------------------------------------
//    BIAS
//------------------------------------------------------------------------------*/
//inline void auto_bias(Surface surface, inout float3 position, Light light, float bias_mul = 1.0f)
//{
//    //// Receiver plane bias (slope scaled basically)
//    //float3 du                   = ddx(position);
//    //float3 dv                   = ddy(position);
//    //float2 receiver_plane_bias  = mul(transpose(float2x2(du.xy, dv.xy)), float2(du.z, dv.z));
    
//    //// Static depth biasing to make up for incorrect fractional sampling on the shadow map grid
//    //float sampling_error = min(2.0f * dot(g_shadow_texel_size, abs(receiver_plane_bias)), 0.01f);

//    // Scale down as the user is interacting with much bigger, non-fractional values (just a UX approach)
//    float fixed_factor = 0.0001f;
    
//    // Slope scaling
//    float slope_factor = (1.0f - saturate(light.n_dot_l));

//    // Apply bias
//    position.z += fixed_factor * slope_factor * light.bias * (bias_mul + 1.0f);
//}
/*------------------------------------------------------------------------------
    BIAS
------------------------------------------------------------------------------*/
inline void auto_bias(inout float3 position,float light_n_dot_l = 0.0f,float light_bias = 0.005f ,float bias_mul = 1.0f)
{
    //// Receiver plane bias (slope scaled basically)
    //float3 du                   = ddx(position);
    //float3 dv                   = ddy(position);
    //float2 receiver_plane_bias  = mul(transpose(float2x2(du.xy, dv.xy)), float2(du.z, dv.z));
    
    //// Static depth biasing to make up for incorrect fractional sampling on the shadow map grid
    //float sampling_error = min(2.0f * dot(g_shadow_texel_size, abs(receiver_plane_bias)), 0.01f);

    // Scale down as the user is interacting with much bigger, non-fractional values (just a UX approach)
    float fixed_factor = 0.0001f;
    
    // Slope scaling
    float slope_factor = (1.0f - saturate(light_n_dot_l));

    // Apply bias
    position.z += fixed_factor * slope_factor * light_bias * (bias_mul + 1.0f);
}

//inline float3 bias_normal_offset(Surface surface, Light light, float3 normal)
//{
//    return normal * (1.0f - saturate(light.n_dot_l)) * light.normal_bias * get_shadow_texel_size() * 10;
//}

///*------------------------------------------------------------------------------
//    ENTRYPOINT
//------------------------------------------------------------------------------*/
//float4 Shadow_Map(Surface surface, Light light)
//{ 
//    float3 position_world = surface.position + bias_normal_offset(surface, light, surface.normal);
//    float4 shadow         = 1.0f;

//    if (light_is_directional())
//    {
//        // TODO send from CPU constbuffer Light instead
//        uint cascade_count = 3;
//        for (uint cascade_index = 0; cascade_index < cascade_count; cascade_index++)
//        {
//            // project into light space
//            float3 pos_ndc = world_to_ndc(position_world, buffer_light.view_projection[cascade_index]);
//            float2 pos_uv  = ndc_to_uv(pos_ndc);

//            // ensure not out of bound
//            if (is_saturated(pos_uv))
//            {
//                // sample primary cascade
//                auto_bias(surface, pos_ndc, light, cascade_index);
//                shadow.a = SampleShadowMap(surface, float3(pos_uv, cascade_index), pos_ndc.z);

//                if (light_has_shadows_transparent())
//                {
//                    if (shadow.a > 0.0f && surface.is_opaque())
//                    {
//                        shadow.rgb *= Technique_Vogel_Color(surface, float3(pos_uv, cascade_index));
//                    }
//                }

//                // if we are close to the edge a secondary cascade exists, lerp with it.
//                float cascade_fade = (max2(abs(pos_ndc.xy)) - g_shadow_cascade_blend_threshold) * 4.0f;
//                uint cascade_index_next = cascade_index + 1;

//                if (cascade_fade > 0.0f && cascade_index_next < cascade_count - 1)
//                {
//                    // project into light space
//                    pos_ndc = world_to_ndc(position_world, buffer_light.view_projection[cascade_index_next]);
//                    pos_uv  = ndc_to_uv(pos_ndc);

//                    // sample secondary cascade
//                    auto_bias(surface, pos_ndc, light, cascade_index_next);
//                    float shadow_secondary = SampleShadowMap(surface, float3(pos_uv, cascade_index_next), pos_ndc.z);

//                    // blend cascades
//                    shadow.a = lerp(shadow.a, shadow_secondary, cascade_fade);
                    
//                    if (light_has_shadows_transparent())
//                    {
//                        if (shadow.a > 0.0f && surface.is_opaque())
//                        {
//                            shadow.rgb = min(shadow.rgb, Technique_Vogel_Color(surface, float3(pos_uv, cascade_index_next)));
//                        }
//                    }
//                }

//                break;
//            }
//        }
//    }
//    else if (light_is_point())
//    {
//        if (light.distance_to_pixel < light.far)
//        {
//            // project into light space
//            uint slice_index  = direction_to_cube_face_index(light.to_pixel);
//            float3 pos_ndc    = world_to_ndc(position_world, buffer_light.view_projection[slice_index]);

//            auto_bias(surface, pos_ndc, light);
//            shadow.a = SampleShadowMap(surface, light.to_pixel, pos_ndc.z);
            
//            if (light_has_shadows_transparent())
//            {
//                if (shadow.a > 0.0f && surface.is_opaque())
//                {
//                    shadow.rgb *= Technique_Vogel_Color(surface, light.to_pixel);
//                }
//            }
//        }
//    }
//    else if (light_is_spot())
//    {
//        if (light.distance_to_pixel < light.far)
//        {
//            // project into light space
//            float3 pos_ndc  = world_to_ndc(position_world, buffer_light.view_projection[0]);
//            float3 pos_uv   = float3(ndc_to_uv(pos_ndc), 0);

//            // ensure not out of bound
//            if (is_saturated(pos_uv.xy))
//            {
//                auto_bias(surface, pos_ndc, light);
//                shadow.a = SampleShadowMap(surface, pos_uv, pos_ndc.z);

//                if (light_has_shadows_transparent())
//                {
//                    if (shadow.a > 0.0f && surface.is_opaque())
//                    {
//                        shadow.rgb *= Technique_Vogel_Color(surface, pos_uv);
//                    }
//                }
//            }
//        }
//    }

//    return shadow;
//}

```