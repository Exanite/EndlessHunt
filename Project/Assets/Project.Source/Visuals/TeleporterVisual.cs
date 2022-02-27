using UnityEngine;

namespace Project.Source.Visuals
{
    public class TeleporterVisual : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public AnimationCurve emissionCurve;

        [Header("Runtime")]
        public Renderer renderer;

        private MaterialPropertyBlock block;

        private void Awake()
        {
            block = new MaterialPropertyBlock();
        }

        private void Start()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            var emissionMultiplier = emissionCurve.Evaluate(Time.time);
            var baseEmissionColor = renderer.sharedMaterial.GetColor(EmissionColor);

            block.SetColor(EmissionColor, baseEmissionColor * emissionMultiplier);
            renderer.SetPropertyBlock(block);
        }
    }
}