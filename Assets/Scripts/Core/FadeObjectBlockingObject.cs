using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FadeObjectBlockingObject : MonoBehaviour
    {
        [SerializeField]
        private LayerMask LayerMask;
        [SerializeField]
        private Transform Player;
        [SerializeField]
        private Camera Camera;
        [SerializeField]
        private float FadedAlpha = 0.33f;
        [SerializeField]
        private FadeMode FadingMode;

        [SerializeField]
        private float ChecksPerSecond = 10;
        [SerializeField]
        private int FadeFPS = 30;
        [SerializeField]
        private float FadeSpeed = 1;

        [SerializeField]
        private float distanceBehindCamera = 5f;

        [Header("Read Only Data")]
        [SerializeField]
        private List<FadingObject> ObjectsBlockingView = new List<FadingObject>();
        private List<int> IndexesToClear = new List<int>();
        private Dictionary<FadingObject, Coroutine> RunningCoroutines = new Dictionary<FadingObject, Coroutine>();

        private RaycastHit[] Hits = new RaycastHit[10];

        private void Start()
        {
            StartCoroutine(CheckForObjects());
        }

        private IEnumerator CheckForObjects()
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / ChecksPerSecond);

            while (true)
            {
                int hits = Physics.RaycastNonAlloc(Camera.transform.position - (Camera.transform.forward * distanceBehindCamera), (Player.transform.position - Camera.transform.position).normalized, Hits, Vector3.Distance(Camera.transform.position, Player.transform.position), LayerMask);
                if (hits > 0)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        FadingObject fadingObject = GetFadingObjectFromHit(Hits[i]);
                        if (fadingObject != null && !ObjectsBlockingView.Contains(fadingObject))
                        {
                            if (RunningCoroutines.ContainsKey(fadingObject))
                            {
                                if (RunningCoroutines[fadingObject] != null) // may be null if it's already ended
                                {
                                    StopCoroutine(RunningCoroutines[fadingObject]);
                                }

                                RunningCoroutines.Remove(fadingObject);
                            }

                            RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                            ObjectsBlockingView.Add(fadingObject);
                        }
                    }
                }

                FadeObjectsNoLongerBeingHit();

                ClearHits();

                yield return Wait;
            }
        }

        private void FadeObjectsNoLongerBeingHit()
        {
            for (int i = 0; i < ObjectsBlockingView.Count; i++)
            {
                bool objectIsBeingHit = false;
                for (int j = 0; j < Hits.Length; j++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(Hits[j]);
                    if (fadingObject != null && fadingObject == ObjectsBlockingView[i])
                    {
                        objectIsBeingHit = true;
                        break;
                    }
                }

                if (!objectIsBeingHit)
                {
                    if (RunningCoroutines.ContainsKey(ObjectsBlockingView[i]))
                    {
                        if (RunningCoroutines[ObjectsBlockingView[i]] != null)
                        {
                            StopCoroutine(RunningCoroutines[ObjectsBlockingView[i]]);
                        }
                        RunningCoroutines.Remove(ObjectsBlockingView[i]);
                    }

                    RunningCoroutines.Add(ObjectsBlockingView[i], StartCoroutine(FadeObjectIn(ObjectsBlockingView[i])));
                    ObjectsBlockingView.RemoveAt(i);
                }
            }
        }

        private IEnumerator FadeObjectOut(FadingObject FadingObject)
        {
            float waitTime = 1f / FadeFPS;
            WaitForSeconds Wait = new WaitForSeconds(waitTime);
            int ticks = 1;

            for (int i = 0; i < FadingObject.Materials.Count; i++)
            {
                FadingObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); // affects both "Transparent" and "Fade" options
                FadingObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); // affects both "Transparent" and "Fade" options
                FadingObject.Materials[i].SetInt("_ZWrite", 0); // disable Z writing
                if (FadingMode == FadeMode.Fade)
                {
                    FadingObject.Materials[i].EnableKeyword("_ALPHABLEND_ON");
                }
                else
                {
                    FadingObject.Materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                }

                FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }

            if (FadingObject.Materials[0].HasProperty("_Color"))
            {
                while (FadingObject.Materials[0].color.a > FadedAlpha)
                {
                    for (int i = 0; i < FadingObject.Materials.Count; i++)
                    {
                        if (FadingObject.Materials[i].HasProperty("_Color"))
                        {
                            FadingObject.Materials[i].color = new Color(
                                FadingObject.Materials[i].color.r,
                                FadingObject.Materials[i].color.g,
                                FadingObject.Materials[i].color.b,
                                Mathf.Lerp(FadingObject.InitialAlpha, FadedAlpha, waitTime * ticks * FadeSpeed)
                            );
                        }
                    }
                    //FadingObject.GetComponent<Collider>().enabled = false;
                    ticks++;
                    yield return Wait;
                }
            }

            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }
        }

        private IEnumerator FadeObjectIn(FadingObject FadingObject)
        {
            float waitTime = 1f / FadeFPS;
            WaitForSeconds Wait = new WaitForSeconds(waitTime);
            int ticks = 1;

            if (FadingObject.Materials[0].HasProperty("_Color"))
            {
                while (FadingObject.Materials[0].color.a < FadingObject.InitialAlpha)
                {
                    for (int i = 0; i < FadingObject.Materials.Count; i++)
                    {
                        if (FadingObject.Materials[i].HasProperty("_Color"))
                        {
                            FadingObject.Materials[i].color = new Color(
                                FadingObject.Materials[i].color.r,
                                FadingObject.Materials[i].color.g,
                                FadingObject.Materials[i].color.b,
                                Mathf.Lerp(FadedAlpha, FadingObject.InitialAlpha, waitTime * ticks * FadeSpeed)
                            );
                        }
                    }

                    //FadingObject.GetComponent<Collider>().enabled = true;
                    ticks++;
                    yield return Wait;
                }
            }

            for (int i = 0; i < FadingObject.Materials.Count; i++)
            {
                if (FadingMode == FadeMode.Fade)
                {
                    FadingObject.Materials[i].DisableKeyword("_ALPHABLEND_ON");
                }
                else
                {
                    FadingObject.Materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                }
                FadingObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                FadingObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                FadingObject.Materials[i].SetInt("_ZWrite", 1); // re-enable Z Writing
                FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
            }

            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }
        }

        private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
        {
            FadingObject fadingObject = null;
            if (Hit.collider != null)
            {
                fadingObject = Hit.collider.GetComponent<FadingObject>();
                if (!fadingObject)
                {
                    fadingObject = Hit.collider.GetComponentInChildren<FadingObject>();
                }
            }
            return fadingObject;
        }

        private void ClearHits()
        {
            RaycastHit hit = new RaycastHit();
            for (int i = 0; i < Hits.Length; i++)
            {
                Hits[i] = hit;
            }
        }

        public enum FadeMode
        {
            Transparent,
            Fade
        }
    }
}
/*
namespace Assets.Scripts.Core
{
    class FadingObjectBlockingObject : MonoBehaviour
    {

        [SerializeField]
        private LayerMask LayerMask;
        [SerializeField]
        private Transform Player;
        [SerializeField]
        private GameObject Camera;
        [SerializeField]
        private float FadedAlpha = 0.33f;
        [SerializeField]
        private FadeMode FadingMode;

        [SerializeField]
        private float ChecksPerSecond = 10;
        [SerializeField]
        private int FadeFPS = 30;
        [SerializeField]
        private float FadeSpeed = 1;

        [Header("Read Only Data")]
        [SerializeField]
        private List<FadingObject> ObjectsBlockingView = new List<FadingObject>();
        private Dictionary<FadingObject, Coroutine> RunningCoroutines = new Dictionary<FadingObject, Coroutine>();

        private RaycastHit[] Hits = new RaycastHit[10];

        private void Start()
        {
            StartCoroutine(CheckForObjects());
        }

        private IEnumerator CheckForObjects()
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / ChecksPerSecond);

            while (true)
            {
                int hits = Physics.RaycastNonAlloc(Camera.transform.position, (Player.transform.position - Camera.transform.position).normalized, Hits, Vector3.Distance(Camera.transform.position, Player.transform.position), LayerMask);
                if (hits > 0)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        FadingObject fadingObject = GetFadingObjectFromHit(Hits[i]);
                        if (fadingObject != null && !ObjectsBlockingView.Contains(fadingObject))
                        // We have hit a fading object that is new that we do not already know about
                        {
                            if (RunningCoroutines.ContainsKey(fadingObject))
                            // Check to see if it is already fading out
                            {
                                if (RunningCoroutines[fadingObject] != null) // may be null if it's already ended
                                {
                                    // Stop it fading out
                                    StopCoroutine(RunningCoroutines[fadingObject]);
                                }
                                // Remove from our list of known coroutines
                                RunningCoroutines.Remove(fadingObject);
                            }
                            // Add our current one, we are going to start fading this object out
                            RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                            // With the fade starting, add it to the known list of objects blocking our view
                            ObjectsBlockingView.Add(fadingObject);
                        }
                    }
                }
                FadeObjectsNoLongerBeingHit();

                // We have to clear cached hits that existed at the time the player first moved behind an object that was made transparent
                ClearHits();
                yield return Wait;
            }
        }
        /*
         * CoRoutine management and removing objects that are no longer blocking the view of the player
         */
/*
        private void FadeObjectsNoLongerBeingHit()
        {
            for (int i = 0; i < ObjectsBlockingView.Count; i++)
            {
                bool objectIsBeingHit = false;
                for (int j = 0; j < Hits.Length; j++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(Hits[j]);
                    if (fadingObject != null && fadingObject == ObjectsBlockingView[i])
                    {
                        objectIsBeingHit = true;
                        break;
                    }
                }

                if (!objectIsBeingHit)
                {
                    if (RunningCoroutines.ContainsKey(ObjectsBlockingView[i]))
                    {
                        if (RunningCoroutines[ObjectsBlockingView[i]] != null)
                        {
                            StopCoroutine(RunningCoroutines[ObjectsBlockingView[i]]);
                        }
                        RunningCoroutines.Remove(ObjectsBlockingView[i]);
                    }

                    RunningCoroutines.Add(ObjectsBlockingView[i], StartCoroutine(FadeObjectIn(ObjectsBlockingView[i])));
                    ObjectsBlockingView.RemoveAt(i);
                }
            }
        }

        private IEnumerator FadeObjectOut(FadingObject FadingObject)
        {
            float waitTime = 1f / FadeFPS;
            WaitForSeconds Wait = new WaitForSeconds(waitTime);
            int ticks = 1;
            /*
             * TODO: Change if issues with material shading
             * Now we have to fade the object out based on the shader. This implementation is based on the standard unity shader.
             * 'Changing the Rendering Mode using a script'
             * 'When you change the Rendering Mode, Unity applies a number of changes to the Material. There is no single C# API to change the
             * Rendering Mode of a Material, but you can make the same changes in yoru code.'
             */
/*
            for (int i = 0; i < FadingObject.Materials.Count; i++)
            {
                FadingObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); // affects both "Transparent" and "Fade" options
                FadingObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); // affects both "Transparent" and "Fade" options
                FadingObject.Materials[i].SetInt("_ZWrite", 0); // disable Z writing
                if (FadingMode == FadeMode.Fade)
                {
                    FadingObject.Materials[i].EnableKeyword("_ALPHABLEND_ON");
                } else
                {
                    FadingObject.Materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                }

                FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }

            if(FadingObject.Materials[0].HasProperty("_COLOR")){
                while (FadingObject.Materials[0].color.a > FadedAlpha)
                {
                    for (int i =0; i < FadingObject.Materials.Count; i++)
                    {
                        if (FadingObject.Materials[i].HasProperty("_COLOR"))
                        {
                            FadingObject.Materials[i].color = new Color(
                                FadingObject.Materials[i].color.r,
                                FadingObject.Materials[i].color.g,
                                FadingObject.Materials[i].color.b,
                                Mathf.Lerp(FadingObject.InitialAlpha, FadedAlpha, waitTime * ticks)
                                );
                        }
                    }

                    ticks++;
                    yield return Wait;
                }
            }

            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }

        }

        private IEnumerator FadeObjectIn(FadingObject FadingObject)
        {
            float waitTime = 1f / FadeFPS;
            WaitForSeconds Wait = new WaitForSeconds(waitTime);
            int ticks = 1;

            if (FadingObject.Materials[0].HasProperty("_COLOR"))
            {
                while(FadingObject.Materials[0].color.a < FadingObject.InitialAlpha)
                {
                    for (int i = 0; i < FadingObject.Materials.Count; i++)
                    {
                        FadingObject.Materials[i].color = new Color(
                            FadingObject.Materials[i].color.r,
                            FadingObject.Materials[i].color.g,
                            FadingObject.Materials[i].color.b,
                            Mathf.Lerp(FadedAlpha, FadingObject.InitialAlpha, waitTime * ticks)
                            );
                    }

                    ticks++;
                    yield return Wait;
                }
            }

            for (int i = 0; i < FadingObject.Materials.Count; i++)
            {
                if (FadingMode == FadeMode.Fade)
                {
                    FadingObject.Materials[i].DisableKeyword("_ALPHABLEND_ON");
                }
                else
                {
                    FadingObject.Materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                }
                FadingObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                FadingObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                FadingObject.Materials[i].SetInt("_ZWrite", 1); // re-enable Z writing
                FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
            }

            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }
        }

        private void ClearHits()
        {
            RaycastHit hit = new RaycastHit();
            for (int i =0; i < Hits.Length; i++)
            {
                Hits[i] = hit;
            }
        }

        private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
        {
            return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
        }

        public enum FadeMode
        {
            Transparent,
            Fade
        }
    }
}
*/