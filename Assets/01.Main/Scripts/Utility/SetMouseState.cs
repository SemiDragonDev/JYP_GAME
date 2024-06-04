using UnityEngine;

public class SetMouseState : MonoBehaviour
{
    void Start()
    {
        LockCursor();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            LockCursor();
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}