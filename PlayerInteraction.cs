//added onto main camera
//shoots rays to determine collisions with blueprints, gizmos
//gets positions to spawn objects


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Utils;
using UnityEngine;

namespace BuilderMenu
{
    public class PlayerInteraction : MonoBehaviour
    {

        public Transform t;
        private void Update()
        {
            if (t == null)
            {
                t = Camera.main.transform;
            }
            else
            {
                transform.position = t.position;
                transform.rotation = t.rotation;
            }
        }
        public Vector3 GetLookAtPos()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward * 20, 20);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.root != this.transform.root || hits[i].transform.root != LocalPlayer.Transform.root)
                {
                    
                        return hits[i].point;
                    }
                
            }
            return transform.position+transform.forward*20;
        }
        public Vector3 SelectGizmo()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward * 50, 50);
            for (int i = 0; i < hits.Length; i++)
            {
                Gizmo g = hits[i].transform.GetComponent<Gizmo>();
                if (g != null)
                {
                    Gizmo oldGizmo = EditorVariables.gizmo;
                    ModAPI.Console.Write("hit gizmo " + g.gameObject.name);

                    EditorVariables.SelectedGizmo = g.GizmoType;
                    ModAPI.Console.Write("selected gizmo " + EditorVariables.SelectedGizmo.ToString());
                    g.SetEnabled();
                    EditorVariables.gizmo = g;
                    oldGizmo.SetDisabled();

                    return hits[i].point;
                }
            }
            return Vector3.zero;
        }
        public void SelectEdit()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward * 50, 50, 0, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hits.Length; i++)
            {
                Blueprint b = hits[i].transform.root.GetComponent<Blueprint>();
                if (b != null)
                {
                    EditorVariables.EditedTransform = b.transform.root;
                    EditorVariables.isEditing = true;
                    b.Edited = true;
                   
                }
            }
        }



        public Blueprint GetLookAtBlueprint()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward * 50, 50, 1, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hits.Length; i++)
            {
                Blueprint b = hits[i].transform.root.GetComponent<Blueprint>();
                if (b != null)
                {
                    return b;

                }
            }
            return null;
        }
    }
}
