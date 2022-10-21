/*
 * 
 * 
 * Photoshop Like Blend Modes
The following are pre-processor definitions for blending two colors with a photoshop like blending mode approach. 
These make use of the max(sign(f - 0.5), 0) trick to create an interpolant that is either 0 or 1 to avoid branch related artifacts.
 * 
// Result is 0 for values under 0.5, 1 for values equal or greater than 0.5
#ifndef HardFactor
#define HardFactor(factor) max(sign(##factor - 0.5), 0)
#endif
 
// Chooses one or the other value by following the rules established by HardFactor on each component
#ifndef Choice
#define Choice(less, greater, factor) lerp(##less, ##greater, HardFactor(##factor))
#endif
 
#ifndef Blend
 
#define Blend_Normal(base, blend) ##base
#define Blend_Add(base, blend) min(##base + ##blend, 1)
#define Blend_Subtract(base, blend) max(##base + ##blend - 1, 0)
#define Blend_Average(base, blend) ((##base + ##blend) * 0.5)
 
#define Blend_Darken(base, blend) min(##base, ##blend)
#define Blend_Multiply(base, blend) (##base * ##blend)
#define Blend_ColorBurn(base, blend) (1 - ((1 - ##base) / (##blend + Epsilon)))
#define Blend_LinearBurn(base, blend) Blend_Subtract(##base, ##blend)
 
#define Blend_Lighten(base, blend) max(##base, ##blend)
#define Blend_Screen(base, blend) (1 - ((1 - ##base) * (1 - ##blend)))
#define Blend_ColorDodge(base, blend) (##base / (1 - ##blend + Epsilon))
#define Blend_LinearDodge(base, blend) Blend_Add(##base, ##blend)
 
#define Blend_Overlay(base, blend) \
	Choice(2 * ##base * ##blend, \
		   1 - 2 * (1 - ##base) * (1 - ##blend), \
		   ##base)
#define Blend_SoftLight(base, blend) \
	Choice(2 * ##base * ##blend + ##base * ##base * (1 - 2 * ##blend), \
		   sqrt(##base) * (2 * ##blend - 1) + 2 * ##base * (1 - ##blend), \
		   ##blend)
#define Blend_HardLight(base, blend) Blend_Overlay(##blend, ##base)
#define Blend_VividLight(base, blend) \
	Choice(Blend_ColorBurn(##base, (2 * ##blend)), \
		   Blend_ColorDodge(##base, (2 * (##blend - 0.5))), \
		   ##blend)
#define Blend_LinearLight(base, blend) \
	Choice(Blend_LinearBurn(##base, 2 * ##blend), \
		   Blend_LinearDodge(##base, 2 * (##blend - 0.5)), \
		   ##blend)
#define Blend_PinLight(base, blend) \
	Choice(Blend_Darken(##base, 2 * ##blend), \
		   Blend_Lighten(##base, 2 * (##blend - 0.5)), \
		   ##blend)
 
#define Blend_Difference(base, blend) abs(##base - ##blend)
#define Blend_Exclusion(base, blend) (##base + ##blend - 2 * ##base * ##blend)
#define Blend_Reflect(base, blend) (##base * ##base / (1 - ##blend + Epsilon))
#define Blend_Phoenix(base, blend) (min(##base, ##blend) - max(##base, ##blend) + 1)
#define Blend_HardMix(base, blend)  max(sign(Blend_VividLight(##base, ##blend) - 0.5), 0)
 
#define Blend(base, blend, function) Blend_##function(##base, ##blend)
 
#endif

*/