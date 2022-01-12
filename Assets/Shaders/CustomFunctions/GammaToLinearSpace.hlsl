void GammaToLinearSpace_float(float3 gammaRGB, out float3 linearRGB)
{
    linearRGB = gammaRGB * (gammaRGB * (gammaRGB * 0.305306011h + 0.682171111h) + 0.012522878h);
}