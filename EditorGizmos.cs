using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Utils;
using UnityEngine;

namespace BuilderMenu
{
    public class EditorGizmos : MonoBehaviour
    {
        private GameObject posX;
        private GameObject posY;
        private GameObject posZ;

        private GameObject scX;
        private GameObject scY;
        private GameObject scZ;

        private GameObject rotX;
        private GameObject rotY;
        private GameObject rotZ;

        private bool draggingGizmo;
        private Vector3 draggedGrabPos;
        private Vector3 moveTowardsPos;

        public bool isMoving;
        public float Speed;

        void Start()
        {
            try
            {
                posX = new GameObject("posX");
                posX.SetParent(gameObject);
                posY = new GameObject("posY");
                posY.SetParent(gameObject);
                posZ = new GameObject("posZ");
                posZ.SetParent(gameObject);

                scX = new GameObject("scX");
                scX.SetParent(gameObject);
                scY = new GameObject("scY");
                scY.SetParent(gameObject);
                scZ = new GameObject("scZ");
                scZ.SetParent(gameObject);

                rotX = new GameObject("rotX");
                rotX.SetParent(gameObject);
                rotY = new GameObject("rotY");
                rotY.SetParent(gameObject);
                rotZ = new GameObject("rotZ");
                rotZ.SetParent(gameObject);

                posX.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.PositionX;
                posY.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.PositionY;
                posZ.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.PositionZ;

                scX.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.ScaleX;
                scY.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.ScaleY;
                scZ.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.ScaleZ;

                rotX.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.RotationX;
                rotY.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.RotationY;
                rotZ.AddComponent<Gizmo>().GizmoType = Gizmo.GizmoTypes.RotationZ;

                posX.transform.position = transform.position + Vector3.right;
                posY.transform.position = transform.position + Vector3.up;
                posZ.transform.position = transform.position + Vector3.forward;
                scX.transform.position = transform.position + Vector3.right;
                scY.transform.position = transform.position + Vector3.up;
                scZ.transform.position = transform.position + Vector3.forward;
                rotX.transform.position = transform.position + Vector3.right;
                rotY.transform.position = transform.position + Vector3.up;
                rotZ.transform.position = transform.position + Vector3.forward;

                ChangeEditMode(EditorVariables.EditModes.Position);
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }

        }

        public void ChangeEditMode(EditorVariables.EditModes EditMode)
        {
            EditorVariables.EditMode = EditMode;
            if (EditorVariables.EditMode == EditorVariables.EditModes.Position)
            {
                posX.SetActive(true);
                posY.SetActive(true);
                posZ.SetActive(true);

                scX.SetActive(false);
                scY.SetActive(false);
                scZ.SetActive(false);

                rotX.SetActive(false);
                rotY.SetActive(false);
                rotZ.SetActive(false);
            }
            else if (EditorVariables.EditMode == EditorVariables.EditModes.Rotation)
            {
                posX.SetActive(false);
                posY.SetActive(false);
                posZ.SetActive(false);

                scX.SetActive(false);
                scY.SetActive(false);
                scZ.SetActive(false);

                rotX.SetActive(true);
                rotY.SetActive(true);
                rotZ.SetActive(true);
            }
            else if (EditorVariables.EditMode == EditorVariables.EditModes.Scale)
            {
                posX.SetActive(false);
                posY.SetActive(false);
                posZ.SetActive(false);

                scX.SetActive(true);
                scY.SetActive(true);
                scZ.SetActive(true);

                rotX.SetActive(false);
                rotY.SetActive(false);
                rotZ.SetActive(false);
            }
        }
        public void ChangeEditMode()
        {
            EditorVariables.EditMode = (EditorVariables.EditModes)((int)(EditorVariables.EditMode + 1) % Enum.GetNames(typeof(EditorVariables.EditModes)).Length);
            if (EditorVariables.EditMode == EditorVariables.EditModes.Position)
            {
                posX.SetActive(true);
                posY.SetActive(true);
                posZ.SetActive(true);

                scX.SetActive(false);
                scY.SetActive(false);
                scZ.SetActive(false);

                rotX.SetActive(false);
                rotY.SetActive(false);
                rotZ.SetActive(false);
            }
            else if (EditorVariables.EditMode == EditorVariables.EditModes.Rotation)
            {
                posX.SetActive(false);
                posY.SetActive(false);
                posZ.SetActive(false);

                scX.SetActive(false);
                scY.SetActive(false);
                scZ.SetActive(false);

                rotX.SetActive(true);
                rotY.SetActive(true);
                rotZ.SetActive(true);
            }
            else if (EditorVariables.EditMode == EditorVariables.EditModes.Scale)
            {
                posX.SetActive(false);
                posY.SetActive(false);
                posZ.SetActive(false);

                scX.SetActive(true);
                scY.SetActive(true);
                scZ.SetActive(true);

                rotX.SetActive(false);
                rotY.SetActive(false);
                rotZ.SetActive(false);
            }
        }
        public void DisableEditing()
        {
            isMoving = false;
            draggingGizmo = false;

        }
        public void Update()
        {
            try
            {
                if (EditorVariables.isEditing)
                {
                    if (UnityEngine.Input.GetMouseButtonDown(1))
                    {
                        if (isMoving)
                        {
                            isMoving = false;
                        }
                        else
                        {
                            EditorMethods.DisableEditing();
                        }
                    }
                    if (ModAPI.Input.GetButtonDown("Toggle"))
                    {
                        if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                            {
                            ChangeEditMode();
                        }
                        ChangeEditMode();
                    }
                    //displaying gizmos 
                    if (EditorVariables.EditedTransform != null)
                    {

                        transform.position = EditorVariables.EditedTransform.position;
                        if (EditorVariables.EditMode == EditorVariables.EditModes.Position)
                        {
                            transform.rotation = Quaternion.identity;
                        }
                        else
                        {
                        transform.rotation = EditorVariables.EditedTransform.rotation;
                        }

                            //editing the obj transform

                            SelectGizmo();
                        ReleaseGizmo();
                        if (draggingGizmo)
                        {
                            //DragGizmo();
                        }
                        else 
                        {
                            if (EditorVariables.SelectedGizmo != Gizmo.GizmoTypes.None)
                            {
                                EditTransform();
                            }
                            else
                            {
                              //  ModAPI.Console.Write("no selected gizmo");
                            }
                        }

                    }
                    else
                    {
                        EditorVariables.isEditing = false;
                    }
                }
                else
                {
                    transform.position = Vector3.up * -10000;
                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        private static void EditTransform()
        {
            if (EditorVariables.GizmoEditor.isMoving)
            {
                Vector3 movetowards = EditorVariables.EditedTransform.position;

                if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionX)
                {

                    movetowards = new Vector3(EditorVariables.GizmoEditor.moveTowardsPos.x, EditorVariables.EditedTransform.position.y, EditorVariables.EditedTransform.position.z);
                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionY)
                {
                    movetowards = new Vector3(EditorVariables.EditedTransform.position.x, EditorVariables.GizmoEditor.moveTowardsPos.y, EditorVariables.EditedTransform.position.z);

                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionZ)
                {
                    movetowards = new Vector3(EditorVariables.EditedTransform.position.x, EditorVariables.EditedTransform.position.y, EditorVariables.GizmoEditor.moveTowardsPos.z);

                }
                float s = Time.deltaTime * EditorVariables.GizmoEditor.Speed;
                if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                {
                    s *= 4;
                }
                EditorVariables.EditedTransform.position = Vector3.MoveTowards(EditorVariables.EditedTransform.position, movetowards, s);
                EditorVariables.GizmoEditor.Speed = Mathf.Clamp(EditorVariables.GizmoEditor.Speed + Time.deltaTime / 2, 0, 5);
            }
            else
            {
                EditorVariables.GizmoEditor.Speed = 0;
            }
            float F = 0;
            if (ModAPI.Input.GetButton("Increase"))
            {
               // ModAPI.Console.Write("+F");
                EditorVariables.GizmoEditor.isMoving = false;
                F = 1;
                
            }
            else if (ModAPI.Input.GetButton("Decrease"))
            {
              // ModAPI.Console.Write("-F");
                EditorVariables.GizmoEditor.isMoving = false;
                F = -1;
            }
            if (F != 0)
            {
               // ModAPI.Console.Write("editing " + EditorVariables.SelectedGizmo.ToString());

                if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionX)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F *= 4;
                    }
                    EditorVariables.EditedTransform.position += Vector3.right * F * Time.deltaTime;
                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionY)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F *= 4;
                    }
                    EditorVariables.EditedTransform.position += Vector3.up * F * Time.deltaTime;

                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionZ)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F *= 4;
                    }
                    EditorVariables.EditedTransform.position += Vector3.forward * F * Time.deltaTime;

                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.ScaleX)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F /= 10;
                    }
                    Vector3 sc = EditorVariables.EditedTransform.localScale;
                    sc.x += F * 0.05f;
                    EditorVariables.EditedTransform.localScale = sc;
                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.ScaleY)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F /= 10;
                    }
                    Vector3 sc = EditorVariables.EditedTransform.localScale;
                    sc.y += F * 0.05f;
                    EditorVariables.EditedTransform.localScale = sc;
                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.ScaleZ)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F /= 10;
                    }
                    Vector3 sc = EditorVariables.EditedTransform.localScale;
                    sc.z += F * 0.05f;
                    EditorVariables.EditedTransform.localScale = sc;
                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.RotationX)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F /= 10;
                    }
                    EditorVariables.EditedTransform.Rotate(EditorVariables.EditedTransform.right * F, Space.World);
                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.RotationY)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F /= 10;
                    }
                    EditorVariables.EditedTransform.Rotate(EditorVariables.EditedTransform.up * F,Space.World);
                }
                else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.RotationZ)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        F /= 10;
                    }
                    EditorVariables.EditedTransform.Rotate(EditorVariables.EditedTransform.forward * F, Space.World);
                }
            }
        }

        private void DragGizmo()
        {
           // ModAPI.Console.Write("Dragging gizmo " + EditorVariables.SelectedGizmo.ToString());
            if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionX)
            {
                Vector3 lookat = EditorVariables.pInteraction.GetLookAtPos();

                float NewX = lookat.x + draggedGrabPos.x - EditorVariables.EditedTransform.position.x;
                EditorVariables.EditedTransform.position = Vector3.Slerp(EditorVariables.EditedTransform.position, new Vector3(NewX, EditorVariables.EditedTransform.position.y, EditorVariables.EditedTransform.position.z), Time.deltaTime * 2);
            }
            else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionY)
            {
                Vector3 lookat = EditorVariables.pInteraction.GetLookAtPos();

                float NewY = lookat.y + draggedGrabPos.y - EditorVariables.EditedTransform.position.y;
                EditorVariables.EditedTransform.position = Vector3.Slerp(EditorVariables.EditedTransform.position, new Vector3(EditorVariables.EditedTransform.position.x, NewY, EditorVariables.EditedTransform.position.z), Time.deltaTime * 2);
            }
            else if (EditorVariables.SelectedGizmo == Gizmo.GizmoTypes.PositionZ)
            {
                Vector3 lookat = EditorVariables.pInteraction.GetLookAtPos();

                float NewZ = lookat.z + draggedGrabPos.z - EditorVariables.EditedTransform.position.z;
                EditorVariables.EditedTransform.position = Vector3.Slerp(EditorVariables.EditedTransform.position, new Vector3(EditorVariables.EditedTransform.position.x, EditorVariables.EditedTransform.position.y, NewZ), Time.deltaTime * 2);
            }
        }

        private void ReleaseGizmo()
        {
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                draggingGizmo = false;

            }
        }

        private void SelectGizmo()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Vector3 hitPos= EditorVariables.pInteraction.SelectGizmo();
                if (hitPos != Vector3.zero)
                {
                    draggingGizmo = true;
                    draggedGrabPos = hitPos;
                    EditorVariables.gizmo.SetEnabled();
                }
                else
                {
                    moveTowardsPos = EditorVariables.pInteraction.GetLookAtPos();
                    isMoving = true;
                }
            }
        }
    }
}
