//this class handles all the setup for every other 
//class in the mod
//
//this is like a huge start method
//












using System.Collections.Generic;
using BuilderCore;
using ModAPI.Attributes;
using UnityEngine;
namespace BuilderMenu
{
    class EditorInitializer : MonoBehaviour
    {
        
        public static void Initialize()
        {
            GameObject obj = new GameObject("EditorMod");
            obj.AddComponent<EditorInitializer>();
        }


        private void Awake()
        {
            try
            {
                InitializeGui();
                InitializeVariables();
                InitializeMaterial();
                InitializeEditorGizmo();
                InitializeInteractions();
                EditorMethods.LoadFavourites();
                Invoke("LoadBlueprintsDelayed",3);
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }
        private void LoadBlueprintsDelayed()
        {
            EditorMethods.LoadBlueprints();
        }
   

        private void InitializeMaterial()
        {
            Shader s = Shader.Find("Standard");
            Material material = new Material(s);
            material.SetColor("_Color", new Color(0, 0.2f, 1f, 0.4f));
            material.SetFloat("_Glossiness", 0);
            material.SetFloat("_Metallic", 1);
            material.SetColor("_EmissionColor", new Color(0, 0.2f, 1f, 0.4f));
            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            EditorVariables.BluePrintGhostMaterial = material;







            Material material2 = new Material(s);
            material2.SetColor("_Color", Color.white);
            material2.SetTexture("_MainTex", ModAPI.Resources.GetTexture("Arrow.PNG"));
            material2.SetFloat("_Glossiness", 0);
            material2.SetFloat("_Metallic", 0);
            material2.SetColor("_EmissionColor", Color.white);
            material2.SetFloat("_Mode", 2);
            material2.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material2.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material2.SetInt("_ZWrite", 0);
            material2.DisableKeyword("_ALPHATEST_ON");
            material2.EnableKeyword("_ALPHABLEND_ON");
            material2.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material2.renderQueue = 3000;
            EditorVariables.GizmoMaterial = material2;
        }
        private void InitializeVariables()
        {
            EditorVariables.Items = new List<ListItem>();
            EditorVariables.sortedItems = new Dictionary<string, List<int>>();
            EditorVariables.SelectedListItem = null;
            EditorVariables.FavouriteItems = new List<int>();
        }
        private void InitializeGui()
        {
            gameObject.AddComponent<EditorGUI>();
        }
        private void InitializeInteractions()
        {
            EditorVariables.pInteraction = new GameObject("PlayerInteraction").AddComponent<PlayerInteraction>();
        }
        private void InitializeEditorGizmo()
        {
            EditorVariables.GizmoEditor = new GameObject("Gizmo Editor").AddComponent<EditorGizmos>();
        }
    }
}
