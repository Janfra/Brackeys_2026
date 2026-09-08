using System;
using UnityEngine;

[Serializable]
public struct PromptSettings
{
    [Tooltip("Prompt to display.")]
    public string Prompt;

    [Tooltip("Input to display on prompt.")]
    public string Input;

    [Tooltip("Sets the position of the prompt. Overriden if following transform position.")]
    public Vector3 Position;

    [Tooltip("Sets the offset to apply to the position. Useful for adjusting location when following a transform position.")]
    public Vector3 PositionOffset;
}