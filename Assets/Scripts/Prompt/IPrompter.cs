public interface IPrompter
{
    public void SetPrompt(PromptSettings promptSettings);
    public PromptSettings GetPrompt();
    public void HidePrompt();
}
