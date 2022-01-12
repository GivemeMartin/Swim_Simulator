using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Addition-Post-processing/PaintingPostProcess")]
public class PaintingPostProcess : PostProcessBase
{
    public ClampedFloatParameter radius = new ClampedFloatParameter(0f, 0, 5);
    public ClampedFloatParameter resolutionValue = new ClampedFloatParameter(0f, 0, 5);

}