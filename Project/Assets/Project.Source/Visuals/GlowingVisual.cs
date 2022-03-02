using UnityEngine;

namespace Project.Source.Visuals
{
    public class GlowingVisual : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public AnimationCurve emissionCurve;

        [Header("Runtime")]
        public Renderer myRenderer;

        private MaterialPropertyBlock block;

        private void Awake()
        {
            block = new MaterialPropertyBlock();
        }

        private void Start()
        {
            myRenderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            var emissionMultiplier = emissionCurve.Evaluate(Time.time);
            var baseEmissionColor = myRenderer.sharedMaterial.GetColor(EmissionColor);

            block.SetColor(EmissionColor, baseEmissionColor * emissionMultiplier);
            myRenderer.SetPropertyBlock(block);
        }
    }
}