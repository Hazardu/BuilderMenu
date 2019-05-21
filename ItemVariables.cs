using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace BuilderMenu
{
    public class ItemVariables : MonoBehaviour
    {
        public ItemRecipe Recipe;
        public Blueprint blueprint;
        public ListItem ItemOnList;
        public int ItemOnListIndex;
        public SerializableBluePrint SerializableBlueprint;
        private List<Behaviour> disabledComponents;
        public uint Index;
        private void Start()
        {
            if (!blueprint.Finished)
            {
                disabledComponents = new List<Behaviour>();
                Component[] thisComps = GetComponents(typeof(Behaviour));
                Component[] childComps = GetComponentsInChildren(typeof(Behaviour));
                foreach (Behaviour c in thisComps)
                {
                    if (c.GetType() != typeof(ItemRecipe) || c.GetType() != typeof(Blueprint) || c.GetType() != typeof(MeshFilter) || c.GetType() != typeof(Renderer) || c.GetType() != typeof(MeshCollider) || c.GetType() != typeof(Collider))
                    {
                        disabledComponents.Add(c);
                        c.enabled = false;
                    }
                }
            }
        }

        public void EnableAgain()
        {
            foreach (Behaviour b in disabledComponents)
            {
                b.enabled = true;
            }
        }
    }
}
