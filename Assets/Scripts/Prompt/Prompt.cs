using Janito.EditorExtras;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Prompt : MonoBehaviour, IPrompter
{
    [SerializeField]
    private TMP_Text prompt;

    [SerializeField]
    private TMP_Text input;

    [SerializeField]
    [CreateButton(namingFormat: "{name} Prompter", savePath: PathUtils.ProjectScriptableObjectsPath + "/Prompters")]
    private PromptSO promptSource;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (prompt == null)
        {
            this.LogErrorInDevelopment($"Prompt ({nameof(TMP_Text)}) reference is null. Unable to set prompt text. Please assign it.");
            return;
        }

        if (input == null)
        {
            this.LogErrorInDevelopment($"Input ({nameof(TMP_Text)}) reference is null. Unable to set input text. Please assign it.");
            return;
        }

        if (promptSource.Prompter != null)
        {
            this.LogWarningInDevelopment($"Old prompter will be replaced with this instance ({name}) inside {promptSource.name}");
        }
        promptSource.SetPrompter(this);

        // For now assume that prompts should never be interactable
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void HidePrompt()
    {
        canvasGroup.alpha = 0.0f;
    }

    public PromptSettings GetPrompt()
    {
        return new PromptSettings { Input = input.text, Prompt = prompt.text };
    }

    public void SetPrompt(PromptSettings promptSettings)
    {
        if (string.IsNullOrEmpty(promptSettings.Prompt))
        {
            this.LogWarningInDevelopment($"Empty prompt has been provided for display.");
        }

        prompt.text = promptSettings.Prompt;
        prompt.text = promptSettings.Input;
        canvasGroup.alpha = 1.0f;
    }
}
