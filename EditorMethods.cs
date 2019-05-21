using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BuilderCore;
using System.IO;
using TheForest.Utils;

namespace BuilderMenu
{
    public static class EditorMethods
    {
        public static void Sort()
        {
            try
            {
                EditorVariables.sortedItems.Clear();
                for (int i = 0; i < EditorVariables.Items.Count; i++)
                {
                    string category = EditorVariables.Items[i].Category;
                    if (EditorVariables.sortedItems.ContainsKey(category))
                    {
                        if (!EditorVariables.sortedItems[category].Contains(i))
                        {
                            EditorVariables.sortedItems[category].Add(i);
                        }
                    }
                    else
                    {
                        List<int> abc = new List<int> { i };
                        EditorVariables.sortedItems.Add(category, abc);
                    }
                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        public static void DisableEditing()
        {
            try
            {
                EditorVariables.isEditing = false;
                EditorVariables.EditedTransform = null;
                EditorVariables.GizmoEditor.ChangeEditMode(EditorVariables.EditModes.Position);
                EditorVariables.SelectedGizmo = Gizmo.GizmoTypes.None;
                ModAPI.Console.Write("Setting to none");
                EditorVariables.GizmoEditor.DisableEditing();
                EditorVariables.gizmo.SetDisabled();
                EditorVariables.gizmo = null;
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        public static void EnableEditing(Blueprint b)
        {
            
            EditorVariables.isEditing = true;
            EditorVariables.EditedTransform = b.transform;
        }
    public static void AddBuilding(ListItem i)
        {
            try
            {
                
                ModAPI.Log.Write("Trying to implement " + i.Name);
                if (!EditorVariables.Items.Contains(i))
                {
                  
                    EditorVariables.Items.Add(i);
                    Sort();
                }
                else
                {
                    ModAPI.Log.Write("Failed implementing " + i.Name + ". Already added");
                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }
        public static void AddBuildings(ListItem[] i)
        {
            try
            {
                for (int a = 0; a < i.Length; a++)
                {
                    if (!EditorVariables.Items.Contains(i[a]))
                    {
                        EditorVariables.Items.Add(i[a]);
                    }
                    else
                    {
                        ModAPI.Log.Write("Failed implementing " + i[a].Name + ". Already added");
                    }

                }
                Sort();
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }
        public static GameObject PlaceBlueprint(int id,Vector3 pos)
        {
            GameObject obj = null;
            try
            {
                TheForest.Utils.LocalPlayer.Inventory.EquipPreviousWeapon();
                TheForest.Utils.LocalPlayer.FpCharacter.UnLockView();
                EditorVariables.isMenuOpened = false;
                obj = Core.Instantiate(id, pos, out uint PlacedIndex);


                Blueprint b = obj.AddComponent<Blueprint>();
                ItemRecipe r = obj.AddComponent<ItemRecipe>();
                ItemVariables v = obj.AddComponent<ItemVariables>();
                v.blueprint = b;
                v.Recipe = r;
                v.ItemOnList = EditorVariables.SelectedListItem;
                v.ItemOnListIndex = EditorVariables.SelectedListItemIndex;
                b.vars = v;
                r.vars = v;
                Dictionary<int, int> dict = new Dictionary<int, int>(EditorVariables.SelectedListItem.Ingredients);


                SerializableBluePrint bluePrint = new SerializableBluePrint();
               // ModAPI.Console.Write(EditorVariables.SelectedListItem.Ingredients.Keys.Count + "---" + dict.Keys.Count);
                r.Ingredients = dict;

                bluePrint.Ingredients = dict;
                bluePrint.Built = false;
                bluePrint.ListItemID = EditorVariables.SelectedListItemIndex;

                EditorVariables.SerializableBlueprints.Add(PlacedIndex, bluePrint);
                v.SerializableBlueprint = bluePrint;
                v.Index = PlacedIndex;
               
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
            return obj;
        }
        public static GameObject PlaceBlueprint()
        {
            GameObject obj = null;
            try
            {
                TheForest.Utils.LocalPlayer.Inventory.EquipPreviousWeapon();
                TheForest.Utils.LocalPlayer.FpCharacter.UnLockView();
                EditorVariables.isMenuOpened = false;
                obj = Core.Instantiate(EditorVariables.SelectedListItem.PrefabID, EditorVariables.pInteraction.GetLookAtPos(),out uint PlacedIndex);
              

                Blueprint b = obj.AddComponent<Blueprint>();
                ItemRecipe r = obj.AddComponent<ItemRecipe>();
                ItemVariables v = obj.AddComponent<ItemVariables>();
                v.blueprint = b;
                v.Recipe = r;
                v.ItemOnList = EditorVariables.SelectedListItem;
                v.ItemOnListIndex = EditorVariables.SelectedListItemIndex;
                b.vars = v;
                r.vars = v;
                Dictionary<int, int> dict = new Dictionary<int, int>(EditorVariables.SelectedListItem.Ingredients);

                
                SerializableBluePrint bluePrint = new SerializableBluePrint();
                ModAPI.Console.Write(EditorVariables.SelectedListItem.Ingredients.Keys.Count + "---" + dict.Keys.Count);
                r.Ingredients = dict;
            
                bluePrint.Ingredients = dict;
                bluePrint.Built = false;
                bluePrint.ListItemID = EditorVariables.SelectedListItemIndex;

                EditorVariables.SerializableBlueprints.Add(PlacedIndex, bluePrint);
                v.SerializableBlueprint = bluePrint;
                v.Index = PlacedIndex;
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
            return obj;

        }
        public static void SaveFavourites()
        {
            string path = "Mods/Hazard's Mods/BuilderMenu";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "/Favourites.boob";
            MemoryStream stream = new MemoryStream();
            BinaryWriter buf = new BinaryWriter(stream);
            buf.Write(EditorVariables.FavouriteItems.Count);
            for (int i = 0; i < EditorVariables.FavouriteItems.Count; i++)
            {
                buf.Write(EditorVariables.FavouriteItems[i]);
            }
            buf.Close();
            File.WriteAllBytes(path, stream.ToArray());

        }
        public static void LoadFavourites()
        {
            string path = "Mods/Hazard's Mods/BuilderMenu/Favourites.boob";
            if (File.Exists(path))
            {
                EditorVariables.FavouriteItems = new List<int>();
                BinaryReader buf = new BinaryReader(new MemoryStream(File.ReadAllBytes(path)));
                int a = buf.ReadInt32();
                for (int i = 0; i < a; i++)
                {
                    EditorVariables.FavouriteItems.Add(buf.ReadInt32());
                }
                buf.Close();
            }
            ModAPI.Console.Write("Loaded" + EditorVariables.FavouriteItems.Count);
        }

        public static void SaveBlueprints()
        {
            string path = "Mods/Hazard's Mods/BuilderMenu/";

            if (GameSetup.IsSinglePlayer)
            {
                path += "Singleplayer saves/";
            }
            else
            {
                path += "Multiplayer saves/";
            }
            path += GameSetup.Slot.ToString();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "/Blueprints.ass";

            MemoryStream stream = new MemoryStream();
            BinaryWriter buf = new BinaryWriter(stream);
            buf.Write(EditorVariables.SerializableBlueprints.Count);
            foreach (KeyValuePair<uint,SerializableBluePrint> item in EditorVariables.SerializableBlueprints)
            {

                buf.Write(item.Key);
                buf.Write(item.Value.Built);
                buf.Write(item.Value.ListItemID);
                buf.Write(item.Value.Ingredients.Count);
                foreach (KeyValuePair<int,int> pair in item.Value.Ingredients)
                {
                    buf.Write(pair.Key);
                    buf.Write(pair.Value);
                }
               

            }
            buf.Close();
            File.WriteAllBytes(path, stream.ToArray());
            

            ModAPI.Console.Write("Saved");

        }
        public static void LoadBlueprints()
        {
            try
            {


                string path = "Mods/Hazard's Mods/BuilderMenu/";

                if (GameSetup.IsSinglePlayer)
                {
                    path += "Singleplayer saves/";
                }
                else
                {
                    path += "Multiplayer saves/";
                }
                path += GameSetup.Slot.ToString();
                path += "/Blueprints.ass";

                if (File.Exists(path))
                {
                    ModAPI.Console.Write("Loading bp");
                    EditorVariables.SerializableBlueprints = new Dictionary<uint, SerializableBluePrint>();
                    BinaryReader buf = new BinaryReader(new MemoryStream(File.ReadAllBytes(path)));
                    int maxI = buf.ReadInt32();
                    for (int i = 0; i < maxI; i++)
                    {
                        uint key = buf.ReadUInt32();
                        SerializableBluePrint bp = new SerializableBluePrint();
                        bp.Built = buf.ReadBoolean();
                        bp.ListItemID = buf.ReadInt32();
                        int b = buf.ReadInt32();
                        bp.Ingredients = new Dictionary<int, int>();
                        for (int a = 0; a < b; a++)
                        {
                            int x = buf.ReadInt32();
                            int y = buf.ReadInt32();
                            bp.Ingredients.Add(x, y);
                        }
                        EditorVariables.SerializableBlueprints.Add(key, bp);
                    }
                    buf.Close();


                    ModAPI.Console.Write("bp count " + EditorVariables.SerializableBlueprints.Count);
                    foreach (KeyValuePair<uint, SerializableBluePrint> pair in EditorVariables.SerializableBlueprints)
                    {
                        ModAPI.Console.Write("Doing foreach");
                        if (!pair.Value.Built)
                        {
                            GameObject obj = Core.placedBuildingGameObjects[pair.Key].PlacedGameObject;
                            ModAPI.Console.Write(obj.ToString() + obj.name);
                            Blueprint b = obj.AddComponent<Blueprint>();
                            ItemRecipe r = obj.AddComponent<ItemRecipe>();
                            ItemVariables v = obj.AddComponent<ItemVariables>();
                            b.Finished = pair.Value.Built;
                            b.vars = v;
                            r.vars = v;
                            v.blueprint = b;
                            v.Recipe = r;
                            v.ItemOnList = EditorVariables.Items[pair.Value.ListItemID];
                            v.ItemOnListIndex = pair.Value.ListItemID;
                            r.Ingredients = pair.Value.Ingredients;
                            v.Index = pair.Key;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                ModAPI.Console.Write(e.ToString());
            }
        }
}
}
