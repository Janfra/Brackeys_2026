using UnityEngine;

public struct PromptSettings
{
    /// <summary>
    /// Prompt to display.
    /// </summary>
    public string Prompt;

    /// <summary>
    /// Input to display on prompt.
    /// </summary>
    public string Input;
   
    /// <summary>
    /// Sets the position of the prompt. Overriden if following transform position.
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// Sets the offset to apply to the position. Useful for adjusting location when following a transform position.
    /// </summary>
    public Vector3 PositionOffset;
}