void LinearToGammaSpace_float(float3 linearRGB, out float3 gammaRGB)
{
    linearRGB = max(linearRGB, half3(0.h, 0.h, 0.h));
    gammaRGB = max(1.055h * pow(linearRGB, 0.416666667h) - 0.055h, 0.h);
}