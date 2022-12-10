namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public DialogueNode[] NextDialogueNodes;
    }
}