using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueModificationProcessor : AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            Dialogue dialogue =  AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;

            if (dialogue == null) return AssetMoveResult.DidNotMove;

            if (Path.GetDirectoryName(destinationPath) != Path.GetDirectoryName(sourcePath))
            {
                return AssetMoveResult.DidNotMove;
            }

            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);

            return AssetMoveResult.DidNotMove;
        }

    }
}
