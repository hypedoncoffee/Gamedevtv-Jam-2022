using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Core
{
    public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
    {
        /*
        private Shader m_OldShader = null;
        private Color m_OldColor = Color.black;
        private float m_Transparency = 0.3f;
        private const float m_TargetTransparancy = 0.3f;
        private const float m_FallOff = 0.1f; // returns to 100% in 0.1 sec
        private Renderer renderer;
        */

        public List<Renderer> Renderers = new List<Renderer>();
        public Vector3 Position;
        public List<Material> Materials = new List<Material>();
        public float InitialAlpha;

        void Awake()
        {
            Position = transform.position;
            if (Renderers.Count == 0)
            {
                // Add all renderers attached to this object
                Renderers.AddRange(GetComponentsInChildren<Renderer>());
            }
            for (int i = 0; i < Renderers.Count; i++)
            {
                Materials.AddRange(Renderers[i].materials);
            }

            InitialAlpha = Materials[0].color.a;
        }

        // Should be implemented in any case you implement IEquatable that will be stored in a generic collection
        public bool Equals(FadingObject other)
        {
            return Position.Equals(other.Position);
        }
        // Should be implemented in any case you implement IEquatable that will be stored in a generic collection
        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
        /* 
        public void BeTransparent()
        {
            // reset the transparency;
            m_Transparency = m_TargetTransparancy;
            if (m_OldShader == null)
            {
                // Save the current shader
                m_OldShader = renderer.material.shader;
                m_OldColor = renderer.material.color;
                renderer.material.shader = Shader.Find("Transparent/Diffuse");


            }
        }
        void Update()
        {
            if (m_Transparency < 1.0f)
            {
                Color C = renderer.material.color;
                C.a = m_Transparency;
                renderer.material.color = C;
            }
            else
            {
                // Reset the shader
                renderer.material.shader = m_OldShader;
                renderer.material.color = m_OldColor;
                // And remove this script
                Destroy(this);
            }
            m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;
        }
        */
    }

}