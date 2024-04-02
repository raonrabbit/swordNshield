using UnityEngine;

namespace SwordNShield.UI
{
    public class CursorManager : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        
        [SerializeField] private CursorMapping[] cursorMappings = null;
        
        private static CursorManager instance = null;
        public static CursorManager Instance => instance;
        
        
        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            SetCursor(CursorType.Default);
        }
        
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }

            return cursorMappings[0];
        }
        
        public void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
    }
}
