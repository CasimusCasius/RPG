using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor
{ 

    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow<DialogueEditor>(false,"DialogueEditor");
        }
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId,int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }
    }
}