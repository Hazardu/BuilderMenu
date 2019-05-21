using BuilderCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Items;
using TheForest.Items.Inventory;
using TheForest.Utils;
using UnityEngine;

namespace BuilderMenu
{
    public class EditorGUI : MonoBehaviour
    {
        #region Textures
        private Texture2D backGroundImage;
        private Texture2D favouriteON;
        private Texture2D favouriteOFF;
        private Texture2D recipeImage;
        private Texture2D titleImage;
        private Texture2D listBorder;
        private Texture2D[] listItemBG;
        private Texture2D RemoveImage;
        #endregion

        #region Floats
        private float Ratio;
        #endregion

        private System.Random PRNG;


        private Rect MenuRect;
        private Rect ListRect;
        private Rect DetailRect;
        private Rect DetailRecipeRect;
        private Rect DetailImageRect;
        private Rect DetailNameRect;
        private Rect DetailDescRect;
        private Rect DetailInfoRect;
        private Rect DetailFavouriteRect;
        private Rect RecipeRect;

        private Vector2 ScrollPosition;


        private void Start()
        {
            try {
                Ratio = (float)Screen.height / 1000;
                Ratio *= 0.9f;
                backGroundImage = ModAPI.Resources.GetTexture("Background.PNG");
                favouriteON = ModAPI.Resources.GetTexture("FavouriteON.PNG");
                favouriteOFF = ModAPI.Resources.GetTexture("FavouriteOFF.PNG");
                recipeImage = ModAPI.Resources.GetTexture("RecipeImage.PNG");
                titleImage = ModAPI.Resources.GetTexture("Title.PNG");
                listBorder = ModAPI.Resources.GetTexture("ListBorder.PNG");
                listItemBG = new Texture2D[]
                    {
                ModAPI.Resources.GetTexture("List1.PNG"),
                ModAPI.Resources.GetTexture("List2.PNG"),
                ModAPI.Resources.GetTexture("List3.PNG"),
                ModAPI.Resources.GetTexture("List4.PNG")
                    };
                RemoveImage = new Texture2D(1, 1);
                RemoveImage.SetPixel(0, 0, Color.red);
                RemoveImage.Apply();
                MenuRect = new Rect(0, 0, 1300 * Ratio, 1000 * Ratio);
                MenuRect.center = new Vector2(Screen.width / 2, Screen.height / 2);
                ListRect = new Rect(MenuRect.x, MenuRect.y, 300 * Ratio, 1000 * Ratio);
                DetailRect = new Rect(MenuRect.x + 300 * Ratio, MenuRect.y, 1000 * Ratio, 1000 * Ratio);
                DetailRecipeRect = new Rect(DetailRect.x + 800 * Ratio, DetailRect.y + 0, 200 * Ratio, 600 * Ratio);
                DetailImageRect = new Rect(DetailRect.x + 200 * Ratio, DetailRect.y + 300 * Ratio, 600 * Ratio, 500 * Ratio);
                DetailNameRect = new Rect(DetailRect.x + 150 * Ratio, DetailRect.y + 0, 700 * Ratio, 300 * Ratio);
                DetailDescRect = new Rect(DetailRect.x + 100 * Ratio, DetailRect.y + 800 * Ratio, 800 * Ratio, 200 * Ratio);
                DetailInfoRect = new Rect(DetailRect.x, DetailRect.y + 100 * Ratio, 200 * Ratio, 550 * Ratio);
                DetailFavouriteRect = new Rect(DetailRect.x, DetailRect.y, 100 * Ratio, 100 * Ratio);
                RecipeRect = new Rect(0, 0, 800 * Ratio, 1200 * Ratio);
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        private void OnGUI()
        {
            try
            {

        
            if (EditorVariables.isMenuOpened)
            {
                GUI.DrawTexture(MenuRect, backGroundImage);
                DrawListMenu();
                if(EditorVariables.SelectedListItem != null)
                {
                    DrawDetailMenu();
                }

            }
            else if (!EditorVariables.isEditing)
            {
                DrawBlueprintMenu();
            }    }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        private void Update()
        {
            try
            {


                if (ModAPI.Input.GetButtonDown("Menu"))
                {
                    EditorVariables.isMenuOpened = !EditorVariables.isMenuOpened;
                    if (EditorVariables.isMenuOpened)
                    {
                        LocalPlayer.FpCharacter.LockView(true);
                        LocalPlayer.Inventory.StashEquipedWeapon(false);
                    }
                    else
                    {
                        LocalPlayer.Inventory.EquipPreviousWeapon();
                        LocalPlayer.FpCharacter.UnLockView();
                    }
                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }
        private void DrawBlueprintMenu()
            {
                Blueprint b = EditorVariables.pInteraction.GetLookAtBlueprint();
            if (b != null &&!b.Finished)
            {


                float Dist = Vector3.Distance(LocalPlayer.Transform.position, b.transform.position);
                Rect rect = new Rect(RecipeRect);
                rect.size = new Vector2(rect.width / Dist, rect.height / Dist);
                    Vector2 center= LocalPlayer.MainCam.WorldToScreenPoint(b.transform.position);
                rect.center = new Vector2(center.x,Screen.height - center.y);
                GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                GUILayout.BeginArea(rect, boxStyle);

                GUIStyle recipeStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = Mathf.RoundToInt(140 / Dist),
                    alignment = TextAnchor.LowerCenter
                };
                GUILayout.Label(b.vars.ItemOnList.Name, recipeStyle);
                foreach (KeyValuePair<int, int> pair in b.vars.Recipe.Ingredients)
                {
                    string ItemName = ItemDatabase.ItemById(pair.Key)._name;
                    GUILayout.Label(" " + ItemName + "   x" + pair.Value, recipeStyle);
                }
                GUILayout.EndArea();
                GUI.DrawTexture(new Rect(rect.x, rect.yMax, rect.width* ( EditorVariables.TimeToRemove/EditorVariables.MaxTimeToRemove), 20 * Ratio), RemoveImage);
                if (ModAPI.Input.GetButton("Delete"))
                {
                    EditorVariables.TimeToRemove = Mathf.Clamp(EditorVariables.TimeToRemove + Time.deltaTime, 0, EditorVariables.MaxTimeToRemove);
                    if(EditorVariables.TimeToRemove >= EditorVariables.MaxTimeToRemove)
                    {
                        EditorVariables.SerializableBlueprints.Remove(b.vars.Index);
                        Core.Remove(b.gameObject);
                        EditorVariables.TimeToRemove = 0;

                    }
                }
                else
                {
                    EditorVariables.TimeToRemove = 0;
                }


                if (ModAPI.Input.GetButtonDown("Put"))
                {
                    if (b.vars.Recipe.PlaceItem())
                    {
                        // play audio
                    }

                }
                else if (ModAPI.Input.GetButtonDown("Edit"))
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                      GameObject obj =  EditorMethods.PlaceBlueprint(b.vars.ItemOnList.PrefabID, EditorVariables.pInteraction.GetLookAtPos());
                        obj.transform.localScale = b.transform.localScale;
                        obj.transform.rotation = b.transform.rotation;

                    }
                    else
                    {

                    }
                    EditorMethods.EnableEditing(b);
                }
            }
        }

        private void DrawDetailMenu()
        {
            ListItem item = EditorVariables.SelectedListItem;
            ///////////////////////////////////Image
            GUI.DrawTexture(DetailImageRect, item.Image);
            ///////////////////////////////////////////////////////////
            ///////////////////////////////////name
            GUIStyle NameStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(40 * Ratio),
                alignment = TextAnchor.UpperCenter,
                fontStyle = FontStyle.Italic
            };
            GUILayout.BeginArea(DetailNameRect);
            GUILayout.Label(titleImage, NameStyle, GUILayout.Height(70));
            GUILayout.Label(item.Name, NameStyle);
            GUILayout.EndArea();
            ////////////////////////////////////////////////////////////
            ///////////////////////////////////recipe

            GUIStyle RecipeStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(25 * Ratio),
                alignment = TextAnchor.UpperRight
            };
            GUILayout.BeginArea(DetailRecipeRect,RecipeStyle);

            GUILayout.Label(recipeImage, RecipeStyle, GUILayout.Height(60));
            foreach (KeyValuePair<int, int> pair in item.Ingredients)
            {
                string ItemName = ItemDatabase.ItemById(pair.Key)._name;
                GUILayout.Label("      " + ItemName + "   x" + pair.Value, RecipeStyle);
            }
            GUILayout.EndArea();
            ////////////////////////////////////////////////////////////
            ///////////////////////////////////description
            GUIStyle DescStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(26 * Ratio),
                alignment = TextAnchor.UpperCenter
            };
            GUI.Label(DetailDescRect, item.Description, DescStyle);
            ////////////////////////////////////////////////////////////
            ///////////////////////////////////info
            GUIStyle InfoStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(28 * Ratio),
                alignment = TextAnchor.UpperLeft,                
            };
            GUILayout.BeginArea(DetailInfoRect);
            GUILayout.Label("Author", InfoStyle);
            GUILayout.Label(item.AuthorName, InfoStyle);
            GUILayout.Space(30 * Ratio);
            GUILayout.Label("Category", InfoStyle);
            GUILayout.Label(item.Category, InfoStyle);
            GUILayout.Space(30 * Ratio);
            GUILayout.Label("Mod name", InfoStyle);
            GUILayout.Label(item.ModName, InfoStyle);
            GUILayout.EndArea();
            ////////////////////////////////////////////////////////////
            ///////////////////////////////////favourite button
            if (EditorVariables.FavouriteItems.Contains(EditorVariables.SelectedListItemIndex))
            {
                if(GUI.Button(DetailFavouriteRect,favouriteON))
                if (DetailFavouriteRect.Contains(new Vector2(UnityEngine.Input.mousePosition.x, Screen.height - UnityEngine.Input.mousePosition.y)))
                    { 
                        EditorVariables.FavouriteItems.Remove(EditorVariables.SelectedListItemIndex);
                        EditorMethods.SaveFavourites();
                }
            }
            else
            {
                if (GUI.Button(DetailFavouriteRect, favouriteOFF))
                {
                    EditorVariables.FavouriteItems.Add(EditorVariables.SelectedListItemIndex);
                    EditorMethods.SaveFavourites();

                }
            }
            ////////////////////////////////////////////////////////////
            ///////////////////////////////////place blueprint button
           if(GUI.Button(DetailImageRect, "", RecipeStyle))
            {
                EditorMethods.PlaceBlueprint();
            }
        }


        private void DrawListMenu()
        {
           // Rect r;
            PRNG = new System.Random(100002);
            GUILayout.BeginArea(ListRect);
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Width(300 * Ratio), GUILayout.Height(1000 * Ratio));
            ScrollPosition += new Vector2(0, UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 2);
            GUIStyle CategoryStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(22 * Ratio),
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = false,
                
            };
            GUIStyle ItemStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(18 * Ratio),
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = false,
               

            };
            GUILayout.Label("♥ Favourites ♥", CategoryStyle);
            

            for (int i = 0; i < EditorVariables.FavouriteItems.Count; i++)
            {
               // r = GUILayoutUtility.GetRect(300 * Ratio, 18 * Ratio);
               // Rect rr = new Rect(r);
             //  rr.width+= 18 * Ratio;
            //    GUI.DrawTexture(rr, listItemBG[PRNG.Next(0, listItemBG.Length)]);
                if(GUILayout.Button("     • "+EditorVariables.Items[EditorVariables.FavouriteItems[i]].Name, ItemStyle))
                {
                    EditorVariables.SelectedListItem = EditorVariables.Items[EditorVariables.FavouriteItems[i]];
                    EditorVariables.SelectedListItemIndex = EditorVariables.FavouriteItems[i];
                }
            }

            foreach (KeyValuePair<string,List<int>> pair in EditorVariables.sortedItems)
            {
                GUILayout.Label(pair.Key, CategoryStyle);
                for (int i = 0; i < pair.Value.Count; i++)
                {
                  //  r = GUILayoutUtility.GetRect(300 * Ratio, 18 * Ratio);
                  //  Rect rr = new Rect(r);
                 //   rr.y+= 18 * Ratio;
                 //   GUI.DrawTexture(rr, listItemBG[PRNG.Next(0, listItemBG.Length)]);
                    if (GUILayout.Button("      " + EditorVariables.Items[pair.Value[i]].Name, ItemStyle))
                    {
                        EditorVariables.SelectedListItem = EditorVariables.Items[pair.Value[i]];
                        EditorVariables.SelectedListItemIndex = pair.Value[i];
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();

        }
    }
}
