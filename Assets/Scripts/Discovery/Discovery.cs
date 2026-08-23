using UnityEngine;
using Janito.EditorExtras;
public class Discovery : MonoBehaviour
{
    [SerializeField] private GameObject[] contentToDiscover;
    [SerializeField] private PlayerMovement player;

[Button(ButtonExecutionModes.PlayMode)]
    private void OpenContent()
    {
        if (contentToDiscover.Length == 0)
        {
            return;
        }

        GameObject content = contentToDiscover[Random.Range(0, contentToDiscover.Length)];
        gameObject.SetActive(false);
        content.SetActive(true);
        content.transform.position = transform.position;
        content.transform.rotation = transform.rotation;
        content.transform.localScale = Vector3.one;

        if (player != null)
        {
            content.transform.SetParent(player.transform, true);
        }
    }
}
