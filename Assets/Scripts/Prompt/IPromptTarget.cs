using UnityEngine;

public interface IPromptTarget
{
    public Transform Transform { get; }
    public PromptSettings GetPromptSettings();
}
