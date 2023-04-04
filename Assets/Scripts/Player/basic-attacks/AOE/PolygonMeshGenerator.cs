using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolygonMeshGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfSides;
    [SerializeField] private float radius;
    [SerializeField] private float innerRadius;
    [SerializeField] private float height;
    [SerializeField] [Range(0, 1f)] private float angleFraction;
    [SerializeField] private float yRotationRadians;
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        UpdateMesh();
    }

    public void SetParams(AOEParams par)
    {
        radius = innerRadius + par.thickness;
        height = par.height;
        angleFraction = par.converageFraction;
    }

    private Vector3[] GenerateVertices(int sides, float radi, float yOffset = 0)
    {
        var res = new Vector3[sides];
        var angleStep = 2 * Mathf.PI / sides * angleFraction;
        for (int i = 0; i < sides; i++)
        {
            var angle = i * angleStep + yRotationRadians;
            res[i] = new Vector3(Mathf.Cos(angle) * radi, yOffset, Mathf.Sin(angle) * radi);
        }

        return res;
    }


    [ContextMenu("Update mesh")]
    public void UpdateMesh()
    {
        if (numberOfSides < 3 || radius <= 0 || innerRadius < 0) return;
        if (mesh == null)
        {
            mesh = new Mesh();
        }

        var outerVertices = GenerateVertices(numberOfSides, radius);
        var innerVertices = GenerateVertices(numberOfSides, innerRadius);
        var vertices = outerVertices
            .Concat(innerVertices)
            .Concat(GenerateVertices(numberOfSides, radius, height))
            .Concat(GenerateVertices(numberOfSides, innerRadius, height))
            .ToArray();
        var triangleList = new List<int>();

        var upperConversion = 2 * numberOfSides;
        for (int i = 0; i < outerVertices.Length; i++)
        {
            var neighbourIndex = (i + 1) % numberOfSides;
            var upperInnerNeigh = neighbourIndex + upperConversion + numberOfSides;
            var upper = i + upperConversion;
            var inner = i + numberOfSides;

            triangleList.Add(i);
            triangleList.Add(inner);
            triangleList.Add(neighbourIndex);

            triangleList.Add(inner);
            triangleList.Add(numberOfSides + neighbourIndex);
            triangleList.Add(neighbourIndex);

            //Top part
            var upperNeig = neighbourIndex + upperConversion;
            var upperInner = upper + numberOfSides;

            // triangleList.Add(upperInner);
            // triangleList.Add(upper);
            // triangleList.Add(upperNeig);
            //
            // triangleList.Add(numberOfSides + upperNeig);
            // triangleList.Add(upperInner);
            // triangleList.Add(upperNeig);

            triangleList.Add(upper);
            triangleList.Add(upperInner);
            triangleList.Add(upperNeig);
            
            triangleList.Add(upperInner);
            triangleList.Add(numberOfSides + upperNeig);
            triangleList.Add(upperNeig);

            // Outer Walls
            triangleList.Add(i);
            triangleList.Add(upper);
            triangleList.Add(upperNeig);

            triangleList.Add(i);
            triangleList.Add(upperNeig);
            triangleList.Add(neighbourIndex);

            //Inner walls
            triangleList.Add(upperInner);
            triangleList.Add(inner);
            triangleList.Add(upperInnerNeigh);

            triangleList.Add(upperInnerNeigh);
            triangleList.Add(inner);
            triangleList.Add(neighbourIndex + numberOfSides);
        }

        //Start side
        triangleList.Add(0);
        triangleList.Add(numberOfSides);
        triangleList.Add(numberOfSides + upperConversion);

        triangleList.Add(upperConversion);
        triangleList.Add(0);
        triangleList.Add(numberOfSides + upperConversion);

        //End side
        triangleList.Add(numberOfSides - 1 + numberOfSides);
        triangleList.Add(numberOfSides - 1);
        triangleList.Add(numberOfSides - 1 + numberOfSides + upperConversion);

        triangleList.Add(numberOfSides - 1);
        triangleList.Add(numberOfSides - 1 + upperConversion);
        triangleList.Add(numberOfSides - 1 + upperConversion + numberOfSides);

        mesh.Clear();
        mesh.name = "DynamicPolygonMesh";
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleList.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        Destroy(gameObject.GetComponent<MeshCollider>());
        gameObject.AddComponent(typeof(MeshCollider));
    }
}