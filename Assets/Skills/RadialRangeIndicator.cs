using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialRangeIndicator : MonoBehaviour
{
    int quality = 15;

    Mesh mesh;
    Mesh castTimeMesh;

    public Material indicatorMaterial;

    Vector3[] normals;
    Vector2[] uv;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Init must be called before any other functions can be used.
    /// Initialises the meshes and array sizes
    /// </summary>
    /// <param name="angle"></param>
    /// The parameter angle is 
    public void Init(float angle)
    {
        quality = Mathf.RoundToInt(angle * 0.5f);

        mesh = new Mesh();
        mesh.vertices = new Vector3[4 * quality];
        mesh.triangles = new int[3 * 2 * quality];

        castTimeMesh = new Mesh();
        castTimeMesh.vertices = new Vector3[4 * quality];
        castTimeMesh.triangles = new int[3 * 2 * quality];

        normals = new Vector3[4 * quality];
        uv = new Vector2[4 * quality];

        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(0, 0);
        }
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = new Vector3(0, 1, 0);
        }

        mesh.uv = uv;
        mesh.normals = normals;

        castTimeMesh.uv = uv;
        castTimeMesh.normals = normals;
    }

    public void DrawIndicator(Transform zoneStart, float angle, float minRange, float maxRange)
    {
        float angleLookAt = GetForwardAngle(zoneStart);

        float angleStart = angleLookAt - angle;
        float angleEnd = angleLookAt + angle;
        float angleDelta = (angleEnd - angleStart) / quality;

        float angleCurrent = angleStart;
        float angleNext = angleStart + angleDelta;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        Vector3[] vertices = new Vector3[4 * quality];
        int[] triangles = new int[3 * 2 * quality];

        for (int i = 0; i < quality; i++)
        {
            Vector3 sphereCurrent = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleCurrent)), 0,
                                                Mathf.Cos(Mathf.Deg2Rad * (angleCurrent)));

            Vector3 sphereNext = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleNext)), 0,
                                             Mathf.Cos(Mathf.Deg2Rad * (angleNext)));

            posCurrentMin = transform.position + sphereCurrent * minRange;
            posCurrentMax = transform.position + sphereCurrent * maxRange;

            posNextMin = transform.position + sphereNext * minRange;
            posNextMax = transform.position + sphereNext * maxRange;

            int a = 4 * i;
            int b = 4 * i + 1;
            int c = 4 * i + 2;
            int d = 4 * i + 3;

            vertices[a] = posCurrentMin;
            vertices[b] = posCurrentMax;
            vertices[c] = posNextMax;
            vertices[d] = posNextMin;

            triangles[6 * i] = a;
            triangles[6 * i + 1] = b;	// b
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = d;	// d
            triangles[6 * i + 5] = a;

            angleCurrent += angleDelta;
            angleNext += angleDelta;
        }
        //for (int i = 0; i < uv.Length; i++)
        //{
        //uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        //mesh.uv = uv;
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Bounds should be automatically recalculated when setting triangles
        mesh.RecalculateBounds();

        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    public void DrawCastTimeIndicator(Transform zoneStart, float angle, float minRange, float drawDistance)
    {
        float angleLookAt = GetForwardAngle(zoneStart);

        float angleStart = angleLookAt - angle;
        float angleEnd = angleLookAt + angle;
        float angleDelta = (angleEnd - angleStart) / quality;

        float angleCurrent = angleStart;
        float angleNext = angleStart + angleDelta;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        Vector3[] vertices = new Vector3[4 * quality];
        int[] triangles = new int[3 * 2 * quality];

        for (int i = 0; i < quality; i++)
        {
            Vector3 sphereCurrent = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleCurrent)), 0,
                                                Mathf.Cos(Mathf.Deg2Rad * (angleCurrent)));

            Vector3 sphereNext = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleNext)), 0,
                                             Mathf.Cos(Mathf.Deg2Rad * (angleNext)));

            posCurrentMin = transform.position + sphereCurrent * minRange;
            posCurrentMax = transform.position + sphereCurrent * drawDistance;

            posNextMin = transform.position + sphereNext * minRange;
            posNextMax = transform.position + sphereNext * drawDistance;

            int a = 4 * i;
            int b = 4 * i + 1;
            int c = 4 * i + 2;
            int d = 4 * i + 3;

            vertices[a] = posCurrentMin;
            vertices[b] = posCurrentMax;
            vertices[c] = posNextMax;
            vertices[d] = posNextMin;

            triangles[6 * i] = a;
            triangles[6 * i + 1] = b;
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = d;
            triangles[6 * i + 5] = a;

            angleCurrent += angleDelta;
            angleNext += angleDelta;
        }

        //for (int i = 0; i < uv.Length; i++)
        //{
        //uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        castTimeMesh.vertices = vertices;
        castTimeMesh.triangles = triangles;
        //castTimeMesh.uv = uv;
        //castTimeMesh.normals = normals;
        castTimeMesh.RecalculateBounds();

        Graphics.DrawMesh(castTimeMesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    float GetForwardAngle(Transform t)
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(t.forward.z, t.forward.x);
    }
}
