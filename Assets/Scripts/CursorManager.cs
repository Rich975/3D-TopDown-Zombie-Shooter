using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D targetingCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero; // The cursor's hotspot

    private void Start()
    {
        // Set the default cursor at the start
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    public void SetTargetingCursor()
    {
        Cursor.SetCursor(targetingCursor, cursorHotspot, CursorMode.Auto);
    }
}

