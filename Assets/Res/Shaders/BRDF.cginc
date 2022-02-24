#ifndef BRDF_CGINC
#define BRDF_CGINC
#include "MobileGGX.cginc"
#include "UnityCG.cginc"

half3 EnvBRDFApprox(half3 SpecularColor, half roughness, half nov)
{
	// [ Lazarov 2013, "Getting More Physical in Call of Duty: Black Ops II" ]
	// Adaptation to fit our G term.
	const half4 c0 = { -1, -0.0275, -0.572, 0.022 };
	const half4 c1 = { 1, 0.0425, 1.04, -0.04 };
	half4 r = roughness * c0 + c1;
	half a004 = min(r.x * r.x, exp2(-9.28 * nov)) * r.x + r.y;
	half2 AB = half2(-1.04, 1.04) * a004 + r.zw;

	// Anything less than 2% is physically impossible and is instead considered to be shadowing
	// Note: this is needed for the 'specular' show flag to work, since it uses a SpecularColor of 0
	AB.y *= saturate(50.0 * SpecularColor.g);

	return SpecularColor * AB.x + AB.yyy;
}

half EnvBRDFApproxNonmetal(half roughness, half nov)
{
	// Same as EnvBRDFApprox( 0.04, roughness, nov )
	const half2 c0 = { -1, -0.0275 };
	const half2 c1 = { 1, 0.0425 };
	half2 r = roughness * c0 + c1;
	return min(r.x * r.x, exp2(-9.28 * nov)) * r.x + r.y;
}

half PhongApprox(half roughness, half rol)
{
	half a = roughness * roughness;			// 1 mul
	//!! Ronin Hack?
	a = max(a, 0.008);						// avoid underflow in FP16, next sqr should be bigger than 6.1e-5
	half a2 = a * a;						// 1 mul
	half rcp_a2 = rcp(a2);					// 1 rcp
	//half rcp_a2 = exp2( -6.88886882 * roughness + 6.88886882 );

	// Spherical Gaussian approximation: pow( x, n ) ~= exp( (n + 0.775) * (x - 1) )
	// Phong: n = 0.5 / a2 - 0.5
	// 0.5 / ln(2), 0.275 / ln(2)
	half c = 0.72134752 * rcp_a2 + 0.39674113;	// 1 mad
	half p = rcp_a2 * exp2(c * rol - c);		// 2 mad, 1 exp2, 1 mul
	// Total 7 instr
	return min(p, rcp_a2);						// Avoid overflow/underflow on Mali GPUs
}


half MobileGGX(half3 n, half3 v, half3 l, half roughness)
{
	half3 h = normalize(v + l);
	half loh = saturate(dot(l, h));
	half noh = saturate(dot(n, h));

	half a2 = roughness * roughness;
	//a2 = max(a2, 0.002h);
	half a4 = a2 * a2;
	half d = (noh * a4 - noh) * noh + 1.00001h;
	half specular_term = (a4 / (4.0f * max(0.1h, loh * loh) * (roughness + 0.5h) * (d * d)));
	specular_term = clamp(specular_term - 1e-4h, 0.0, 100.0);

	return specular_term;
}

//half3 Fresnel(half3 SpecularColor, half VoH)
//{
//	half t = exp2((-5.55473 * VoH - 6.98316) * VoH);
//	return SpecularColor + (1.0f - SpecularColor) * t;
//}

half2 EnvUV2D(half3 rotR)
{
	half lengthxz = length(half2(rotR.x, rotR.z)) + 1E-06f;
	half2 proj_angle = acos(half2(rotR.x / lengthxz, rotR.y)) / 3.1415926;
	half frontback = step(rotR.z, 0.0f);
	half t = (frontback * 2.0f - 1.0f) * proj_angle.x * 0.5f;
	half u = frontback - t;
	return half2(u, proj_angle.y);
}

half GetEnvMapLod(half roughness)
{
	if (roughness < 0.05)
		return 0;
	//return (roughness * 12.0 - pow(roughness, 6.0) * 1.5);
	const int MaxMip = 7;
	return clamp(roughness * MaxMip, 0, MaxMip);
}


half3 GetEnvMap2D(half roughness, half3 r, sampler2D envSampler)
{
	half2 uv = EnvUV2D(r);
	half lod = GetEnvMapLod(roughness);
	return tex2Dlod(envSampler, half4(uv, 0.0, lod)).rgb;
}

half3 GetSHEvalLinearL0L1(half3 normal)
{
	return SHEvalLinearL0L1(half4(normal, 1.0));
}

half Lum(half3 val)
{
	return dot(val, half3(0.299, 0.587, 0.114));
}

half3 GammaToLinear(in half3 v)
{
#if defined(UNITY_COLORSPACE_GAMMA)
	return v * v;
#else
	return v;
#endif
}

half3 LinearToGamma(in half3 v)
{
#if defined(UNITY_COLORSPACE_GAMMA)
	return sqrt(v);
#else
	return v;
#endif
}

half3 GetEnvMapCube(half roughness, half3 r, UNITY_ARGS_TEXCUBE(tex))
{
	half mip = GetEnvMapLod(roughness);

	half4 rgbm = UNITY_SAMPLE_TEXCUBE_LOD(tex, r, mip);
	return GammaToLinear(rgbm.rgb);
	//half3 env = DecodeHDR(rgbm, unity_SpecCube0_HDR);
	//return env;
}


#endif