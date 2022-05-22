using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Core
{
    public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
    {

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
    }

}