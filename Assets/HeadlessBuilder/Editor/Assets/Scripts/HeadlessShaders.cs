/* 
 * Headless Builder
 * (c) Salty Devs, 2020
 * 
 * Please do not publish or pirate this code.
 * We worked really hard to make it.
 * 
 */


#if UNITY_2018_2_OR_NEWER

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

// Simple example of stripping of a debug build configuration
class HeadlessShader : IPreprocessShaders
{

    // Multiple callback may be implemented. 
    // The first one executed is the one where callbackOrder is returning the smallest number.
    public int callbackOrder { get { return 1024; } }

    public void OnProcessShader(
        Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData)
    {
        // In development, don't strip debug variants
        if (!Headless.IsBuildingHeadless() || !HeadlessBuilder.ShouldStripShaders())
            return;

        int shaderCount = shaderCompilerData.Count;

        for (int i = 0; i < shaderCompilerData.Count; ++i)
        {
            shaderCompilerData.RemoveAt(i);
            --i;
        }

        HeadlessBuilder.shaderCount += shaderCount;
    }
}

#endif