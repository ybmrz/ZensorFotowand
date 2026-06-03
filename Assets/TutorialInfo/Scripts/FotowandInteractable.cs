using UnityEngine;

/// <summary>
///  Attach this script to a Fotowand GameObject.
///  Handles proximity detection, outline highlighting, and triggering the UI panel.
///  
/// Setup:
///     1. Add a Collider to this GameObject and set it to "Is Trigger"
///     2. Assign a FotowandData asset in the Inspector
///     3. Make sure the Player GameObject is tagged "Player"
///     4. Assign the FotowandUI reference in the Inspector
/// </summary>

public class FotowandInteractable : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("The ScriptableObject containing this wall's photo and info")]
    public FotowandData data;

    [Header("References")]
    [Tooltip("Reference to the shared FotowandUI panel in the scene")]
    public FotowandUI ui;

    [Header("Interaction Settings")]
    [Tooltip("Key the player presses to open this Fotowand")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Emission color shown when the player is in range")]
    public Color outlineColor = Color.yellow;

    // -- internal state --
    private bool _playerInRange = false;
    private Renderer[] _renderers;

    // -- Unity lifecycle -- 
    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        SetOutline(false);
    }

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(interactKey))
        {
            ui.Open(data);
        }
    }

    // -- Trigger detection --
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger hit by: " + other.gameObject.name + " | Tag: " + other.tag);

        if (!other.CompareTag("Player")) return;

        _playerInRange = true;
        SetOutline(true);
        ui.ShowHint(interactKey);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = false;
        SetOutline(false);
        ui.HideHint();
    }

    // -- Outline helpers --
    private void SetOutline(bool enabled)
    {
        foreach (var r in _renderers)
        {
            if (enabled)
            {
                r.material.EnableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", outlineColor * 0.4f);
            }
            else
            {
                r.material.DisableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", Color.black);
            }
        }

    }
}
