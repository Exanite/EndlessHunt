using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class PolygonColliderToShadow : MonoBehaviour
{
    [Header("Runtime")]
    public PolygonCollider2D PolygonCollider;
    public ShadowCaster2D ShadowCaster;

    private FieldInfo meshField;
    private FieldInfo shapePathField;
    private MethodInfo generateShadowMeshMethod;

    private void OnEnable()
    {
        meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
        shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
        generateShadowMeshMethod = typeof(ShadowCaster2D)
            .Assembly
            .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
            .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);

        UpdateShadow();
    }

    public void UpdateShadow()
    {
        PolygonCollider = GetComponent<PolygonCollider2D>();
        ShadowCaster = GetComponent<ShadowCaster2D>();

        if (!PolygonCollider || !ShadowCaster)
        {
            return;
        }

        var referenceVertices = PolygonCollider.points;
        var vertices = new Vector3[referenceVertices.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = referenceVertices[i];
        }

        shapePathField.SetValue(ShadowCaster, vertices);
        meshField.SetValue(ShadowCaster, new Mesh());
        generateShadowMeshMethod.Invoke(ShadowCaster, new object[]
        {
            meshField.GetValue(ShadowCaster),
            shapePathField.GetValue(ShadowCaster),
        });

        ShadowCaster.Update();
    }
}