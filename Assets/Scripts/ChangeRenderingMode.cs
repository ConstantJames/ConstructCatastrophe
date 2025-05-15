using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderingMode : MonoBehaviour
{
    public void ChangeRenderMode(Material objectMat, bool turnTransparent)
    {
        if (turnTransparent)
        {
            // Transparent
            objectMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            objectMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            objectMat.SetInt("_ZWrite", 0);
            objectMat.DisableKeyword("_ALPHATEST_ON");
            objectMat.DisableKeyword("_ALPHABLEND_ON");
            objectMat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            objectMat.renderQueue = 3000;
        }
        else
        {
            // Opaque
            objectMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            objectMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            objectMat.SetInt("_ZWrite", 1);
            objectMat.DisableKeyword("_ALPHATEST_ON");
            objectMat.DisableKeyword("_ALPHABLEND_ON");
            objectMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            objectMat.renderQueue = -1;
        }
    }
}
