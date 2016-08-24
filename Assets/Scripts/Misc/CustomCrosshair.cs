using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class CustomCrosshair : MonoBehaviour
    {

        public Texture2D crosshairTexture;
        public CursorMode crosshairMode;
        public Vector2 hotSpot = Vector2.zero;

        void OnMouseEnter()
        {
            Cursor.SetCursor(crosshairTexture, hotSpot, crosshairMode);
        }

        void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, crosshairMode);
        }
    }
}