using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolygonMeshGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfSides;
    [SerializeField] private float radius;
    [SerializeField] private float innerRadius;
    [SerializeField] private float height;
    [SerializeField] private MeshFilter walls;
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        UpdateMesh();
    }

    private Vector3[] GenerateVertices(int sides, float radi, float yOffset = 0)
    {
        var res = new Vector3[sides];
        var angleStep = 2 * Mathf.PI / sides;
        for (int i = 0; i < sides; i++)
        {
            var angle = i * angleStep;
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

        var upperIndex = 2 * numberOfSides;
        for (int i = 0; i < outerVertices.Length; i++)
        {
            var neighbourIndex = (i + 1) % numberOfSides;
            var upperInnerNeigh = neighbourIndex + upperIndex + numberOfSides; 
            var upper = i + upperIndex;
            var inner = i + numberOfSides;

            triangleList.Add(i);
            triangleList.Add(inner);
            triangleList.Add(neighbourIndex);

            triangleList.Add(inner);
            triangleList.Add(numberOfSides + neighbourIndex);
            triangleList.Add(neighbourIndex);

            //Top part
            var upperNeig = neighbourIndex + upperIndex;
            var upperInner = upper + numberOfSides;

            triangleList.Add(upperInner);
            triangleList.Add(upper);
            triangleList.Add(upperNeig);

            triangleList.Add(numberOfSides + upperNeig);
            triangleList.Add(upperInner);
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

        mesh.Clear();
        mesh.name = "DynamicPolygonMesh";
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleList.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}