using UnityEngine;

public interface IPrompter
{
    public void SetPrompt(PromptSettings promptSettings);
    public void SetFollowingPrompt(PromptSettings promptSettings, Transform transform);
    public PromptSettings GetPrompt();
    public void HidePrompt();
}
