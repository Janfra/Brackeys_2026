using UnityEngine;

[CreateAssetMenu(fileName = "New Prompter", menuName = "Scriptable Objects/Prompter")]
public class PromptSO : ScriptableObject
{
    public IPrompter Prompter { get; private set; }

    public void SetPrompter(IPrompter prompter)
    {
        Prompter = prompter;
    }

    public bool TryGetPrompter(out IPrompter prompter)
    {
        prompter = Prompter;
        return prompter != null;
    }
}
