using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BuilderMenu
{
    public class ListItem
    {
        public int PrefabID;
        public string Category;
        public string Name;
        public string AuthorName;
        public string Description;
        public string ModName;
        public Texture2D Image;
        public Dictionary<int, int> Ingredients;
    }
}
