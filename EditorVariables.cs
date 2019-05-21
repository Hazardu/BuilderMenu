using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace BuilderMenu
{
   public static class EditorVariables
    {
        public static Material BluePrintGhostMaterial;
        public static Material GizmoMaterial;

        public static bool isMenuOpened;
        public static bool isEditing;

        public static int SelectedListItemIndex;

        public static PlayerInteraction pInteraction;
        public static EditorGizmos GizmoEditor;
        public enum EditModes
        {
            Position=0,
            Rotation=1,
            Scale=2,
        }
        public static EditModes EditMode;

        public static List<ListItem> Items= new List<ListItem>();
        public static ListItem SelectedListItem = null;
        public static List<int> FavouriteItems = new List<int>();

        public static Dictionary<string, List<int>> sortedItems= new Dictionary<string, List<int>>();

        public static float TimeToRemove = 0;
        public static float MaxTimeToRemove = 0.5f;

        public static Gizmo.GizmoTypes SelectedGizmo;
        public static Gizmo gizmo;
        public static Transform EditedTransform = null;

        public static Dictionary<uint,SerializableBluePrint> SerializableBlueprints = new Dictionary<uint, SerializableBluePrint>();
    }
}
