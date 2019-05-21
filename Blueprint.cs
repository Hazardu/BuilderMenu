using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace BuilderMenu
{
  public  class Blueprint : MonoBehaviour
    {
        public ItemVariables vars;
        public bool Edited;
        public bool Finished;
        private List<MeshCollider> MeshColliders;
        private List<Collider> Colliders;

        private Dictionary<MeshRenderer, Material[]> OriginalMaterials;

        private void Start()
        {
            try
            {
                if (!Finished)
                {
                    MeshColliders = new List<MeshCollider>();
                    Colliders = new List<Collider>();
                    OriginalMaterials = new Dictionary<MeshRenderer, Material[]>();

                    #region CollidersToTriggers
                    MeshCollider thisColliderMesh = GetComponent<MeshCollider>();
                    MeshCollider[] childCollidersMesh = GetComponentsInChildren<MeshCollider>();

                    Collider thisCollider = GetComponent<Collider>();
                    Collider[] childColliders = GetComponentsInChildren<Collider>();

                    foreach (Collider col in childColliders)
                    {
                        Colliders.Add(col);
                    }
                    foreach (MeshCollider col in childCollidersMesh)
                    {
                        
                        MeshColliders.Add(col);
                    }
                    if (thisColliderMesh != null)
                    {
                        MeshColliders.Add(thisColliderMesh);
                    }
                    if (thisCollider != null)
                    {
                        Colliders.Add(thisCollider);
                    }

                    foreach (MeshCollider col in MeshColliders)
                    {
                        col.isTrigger = true;
                    }
                    foreach (Collider col in Colliders)
                    {
                        col.isTrigger = true;
                    }
                    #endregion

                    MeshRenderer thisRenderer = GetComponent<MeshRenderer>();
                    MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer rend in childRenderers)
                    {
                        if (!OriginalMaterials.ContainsKey(rend))
                            OriginalMaterials.Add(rend, rend.materials);
                    }
                    if (thisRenderer != null)
                    {
                        if(!OriginalMaterials.ContainsKey(thisRenderer))
                        OriginalMaterials.Add(thisRenderer, thisRenderer.materials);

                    }
                    foreach (KeyValuePair<MeshRenderer, Material[]> pair in OriginalMaterials)
                    {
                        pair.Key.materials = new Material[]
                        {
                        EditorVariables.BluePrintGhostMaterial
                        };
                    }
                    vars.SerializableBlueprint.Built = false;

                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        public void Finish()
        {
            Finished = true;
            vars.EnableAgain();
            foreach (KeyValuePair<MeshRenderer, Material[]> pair in OriginalMaterials)
            {
                pair.Key.materials = pair.Value;
            }
            foreach (Collider col in Colliders)
            {
                col.isTrigger = false;
            }
            foreach (MeshCollider col in MeshColliders)
            {
                col.isTrigger = false;
            }
        }
    }
}
