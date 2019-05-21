using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;

namespace BuilderMenu
{
    public class ItemRecipe : MonoBehaviour
    {
        public ItemVariables vars;
        public Dictionary<int,int> Ingredients;
        public bool PlaceItem()
        {
            foreach (KeyValuePair<int,int> pair in Ingredients)
            {
                if (LocalPlayer.Inventory.RemoveItem(pair.Key))
                {
                    Ingredients[pair.Key]--;
                    if (Ingredients[pair.Key] <= 0)
                    {
                        Ingredients.Remove(pair.Key);
                       
                    }




                    return true;
                }
            }
            if (Ingredients.Count <= 0)
                        {
                            vars.blueprint.Finish();
                        }
            return false;
        }
        public void Finish()
        {

        }
    }
}
