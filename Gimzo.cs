using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BuilderMenu
{
    public class Gizmo : MonoBehaviour 
    {
        LineRenderer lr;
        CapsuleCollider col;
        public static float Lenght= 2;
        public static float Witdh = 0.2f;
        public static float Offset = 0.3f;
        public static Transform parent;
        public static Texture2D posImg;
        public static Texture2D scaImg;
        public static Texture2D rotImg;
        public static Color selectColor;
        private static bool isInitialized = false;
        public GizmoTypes GizmoType;
        public enum GizmoTypes
        {
            PositionX,
            PositionY,
            PositionZ,
            RotationX,
            RotationY,
            RotationZ,
            ScaleX,
            ScaleY,
            ScaleZ,
            None
           
        }
        private Color color;
        void Start()
        {
            if (!isInitialized)
            {
                parent = transform.parent;
                posImg = ModAPI.Resources.GetTexture("Pos.PNG");
                scaImg = ModAPI.Resources.GetTexture("Sca.PNG");
                rotImg = ModAPI.Resources.GetTexture("Rot.PNG");
                selectColor = Color.white;


            }
            Material mat = new Material(EditorVariables.GizmoMaterial);
            if (GizmoType == GizmoTypes.PositionX)
            {
                mat.color = Color.red;
                mat.mainTexture = posImg;

            }
            else if (GizmoType == GizmoTypes.PositionY)
            {
                mat.color = Color.green;
                mat.mainTexture = posImg;

            }
            else if (GizmoType == GizmoTypes.PositionZ)
            {
                mat.color = Color.blue;
                mat.mainTexture = posImg;

            }
            else if (GizmoType == GizmoTypes.ScaleX)
            {
                mat.color = Color.red;
                mat.mainTexture = scaImg;

            }
            else if (GizmoType == GizmoTypes.ScaleY)
            {
                mat.color = Color.green;
                mat.mainTexture = scaImg;

            }
            else if (GizmoType == GizmoTypes.ScaleZ)
            {
                mat.color = Color.blue;
                mat.mainTexture = scaImg;

            }
            else if (GizmoType == GizmoTypes.RotationX)
            {
                mat.color = Color.red;
                mat.mainTexture = rotImg;

            }
            else if (GizmoType == GizmoTypes.RotationY)
            {
                mat.color = Color.green;
                mat.mainTexture = rotImg;

            }
            else if (GizmoType == GizmoTypes.RotationZ)
            {
                mat.color = Color.blue;
                mat.mainTexture = rotImg;
            }
            color = mat.color;
            lr = gameObject.AddComponent<LineRenderer>();
            lr.material = mat;
            col = gameObject.AddComponent<CapsuleCollider>();
            col.direction = 2;
            col.radius = 0.4f;
            col.height = 2;
            col.center = Vector3.zero;
            col.isTrigger = false;
        }

        void Update()
        {
            lr.positionCount = 2;
            lr.SetPosition(1, transform.position - transform.forward * Lenght / 2);
            lr.SetPosition(0, transform.position + transform.forward * Lenght / 2);
            transform.LookAt(parent);

        }

        public void SetEnabled()
        {
            lr.material.color = selectColor;
        }
        public void SetDisabled()
        {
            lr.material.color = color;
        }
    }
}
