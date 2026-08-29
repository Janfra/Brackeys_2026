using Janito.EditorExtras;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Timer gameDuration;

    [SerializeField]
    [CreateButton(savePath: PathUtils.ProjectScriptableObjectsPath)]
    [InlineInspector]
    private GameStateSO gameState;

    private void Awake()
    {
        gameDuration.IsLooping = false;
        gameState.GameTimer = gameDuration;
    }

    private void OnEnable()
    {
        gameDuration.IsRunning = true;
    }

    private void Update()
    {
        gameDuration.Update(Time.deltaTime);
    }

    private void OnDisable()
    {
        gameDuration.IsRunning = false;
    }
}

public class GameStateSO : ScriptableObject
{
    [field: SerializeField]
    public int MinDeliveries { get; private set; } = 3;

    public IReadOnlyTimer GameTimer;
}
