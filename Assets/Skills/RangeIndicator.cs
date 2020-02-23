using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RangeIndicator", order = 1)]
public class RangeIndicator : MonoBehaviour
{
    enum IndicatorShape
    {
        RECTANGULAR,
        RADIAL
    }

    IndicatorShape shape;

    int quality;

    [HideInInspector] public Mesh mesh;
    Mesh castTimeMesh;
    public Material indicatorMaterial;

    Vector3[] normals;
    Vector2[] uv;

    [HideInInspector] public Vector3 corner1;
    [HideInInspector] public Vector3 corner2;
    [HideInInspector] public Vector3 corner3;
    [HideInInspector] public Vector3 corner4;

    Player player;

    public void Init(BaseSkill.SkillShape skillShape, float angle = 0)
    {
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            Debug.Log("Range Indicator set player");
        }
        switch (skillShape)
        {
            case BaseSkill.SkillShape.RADIAL:
                shape = IndicatorShape.RADIAL;
                break;

            case BaseSkill.SkillShape.RECTANGULAR:
                shape = IndicatorShape.RECTANGULAR;
                break;
            default:
                break;
        }

        switch (shape)
        {
            case IndicatorShape.RADIAL:
                quality = Mathf.RoundToInt(angle * 2.0f);

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
                break;

            case IndicatorShape.RECTANGULAR:
                quality = 1;

                mesh = new Mesh();
                mesh.vertices = new Vector3[4];
                mesh.triangles = new int[3 * 2];

                castTimeMesh = new Mesh();
                castTimeMesh.vertices = new Vector3[4];
                castTimeMesh.triangles = new int[3 * 2];

                normals = new Vector3[4];
                uv = new Vector2[4];

                for (int i = 0; i < uv.Length; i++)
                {
                    uv[i] = new Vector2(0, 0);
                }
                for (int i = 0; i < normals.Length; i++)
                {
                    normals[i] = new Vector3(0, 1, 0);
                }

                //vertices = new Vector3[4];
                //triangles = new int[3 * 2];

                mesh.uv = uv;
                mesh.normals = normals;

                castTimeMesh.uv = uv;
                castTimeMesh.normals = normals;
                break;

            default:
                break;
        }

    }

    public void DrawIndicator(Transform zoneStart, float angleOrWidth, float minRange, float maxRange)
    {
        float angleLookAt;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        Vector3 zonePositionProjected = zoneStart.position;
        zonePositionProjected.y += 10.0f;

        Vector3[] vertices = new Vector3[4 * quality];
        int[] triangles = new int[3 * 2 * quality];

        switch (shape)
        {
            case IndicatorShape.RADIAL:
                angleLookAt = GetForwardAngle(zoneStart);

                float angleStart = angleLookAt - angleOrWidth;
                float angleEnd = angleLookAt + angleOrWidth;
                float angleDelta = (angleEnd - angleStart) / quality;

                float angleCurrent = angleStart;
                float angleNext = angleStart + angleDelta;

                //vertices = new Vector3[4 * quality];
                //triangles = new int[3 * 2 * quality];

                for (int i = 0; i < quality; i++)
                {
                    Vector3 sphereCurrent = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleCurrent)), 0,
                                                        Mathf.Cos(Mathf.Deg2Rad * (angleCurrent)));

                    Vector3 sphereNext = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleNext)), 0,
                                                     Mathf.Cos(Mathf.Deg2Rad * (angleNext)));

                    posCurrentMin = zoneStart.position + sphereCurrent * minRange;
                    posCurrentMax = zoneStart.position + sphereCurrent * maxRange;

                    // When making further vertices, add more positions here such as zoneStart.position + sphereCurrent * (maxRange * 0.9)

                    posNextMin = zoneStart.position + sphereNext * minRange;
                    posNextMax = zoneStart.position + sphereNext * maxRange;

                    //posCurrentMin = zonePositionProjected + sphereCurrent * minRange;
                    //posCurrentMax = zonePositionProjected + sphereCurrent * maxRange;

                    //posNextMin = zonePositionProjected + sphereNext * minRange;
                    //posNextMax = zonePositionProjected + sphereNext * maxRange;

                    int a = 4 * i;
                    int b = 4 * i + 1;
                    int c = 4 * i + 2;
                    int d = 4 * i + 3;

                    vertices[a] = posCurrentMin;
                    vertices[b] = posCurrentMax;
                    vertices[c] = posNextMax;
                    vertices[d] = posNextMin;

                    triangles[6 * i] = a;
                    triangles[6 * i + 1] = b;   // b
                    triangles[6 * i + 2] = c;
                    triangles[6 * i + 3] = c;
                    triangles[6 * i + 4] = d;   // d
                    triangles[6 * i + 5] = a;

                    angleCurrent += angleDelta;
                    angleNext += angleDelta;
                }
                //for (int i = 0; i < uv.Length; i++)
                //{
                //uv[i] = new Vector2(vertices[i].x, vertices[i].z);
                //}

                // PROJECTION OF VERTICES ONTO SURFACE
                float mostRecentProjectedHeight = Mathf.NegativeInfinity;
                for (int i = 0; i < vertices.Length; i++)
                {
                    // Basic projection onto the surface below the actual vertice point
                    // To make this better, there need to be more vertices : a lattice able to draw circles, for better looking projection going up and down surfaces
                    // Extend the vertices and triangles array so I can turn each circle segment into a segment built up of slanted square pieces

                    zonePositionProjected = vertices[i];
                    zonePositionProjected.y += 50.0f;
                    
                    if (Physics.Raycast(zonePositionProjected, -Vector3.up, out RaycastHit hit, 55.0f, player.groundLayerMask))
                    {
                        vertices[i].y = hit.point.y + 0.05f;
                        mostRecentProjectedHeight = hit.point.y + 0.05f;
                    }
                    // Possible to do a second raycast upwards if the previous cast failed to check if there is a projectable surface above the point
                    // Need to use a layerMask for proper projection. We don't want to project ontop of everything, just the main surface
                    else
                    {
                        if (mostRecentProjectedHeight != Mathf.NegativeInfinity)
                        {
                            zonePositionProjected.y = mostRecentProjectedHeight;
                        }
                        else
                        {
                            zonePositionProjected.y -= 50.0f;
                        }
                    }
                }

                //mesh.uv = uv;
                mesh.vertices = vertices;
                mesh.triangles = triangles;

                // Bounds should be automatically recalculated when setting triangles
                mesh.RecalculateBounds();

                Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
                break;

            case IndicatorShape.RECTANGULAR:
                angleLookAt = GetForwardAngle(zoneStart);

                float halfAttackWidth = angleOrWidth * 0.5f;

                //vertices = new Vector3[4];
                //triangles = new int[3 * 2];

                for (int i = 0; i < 1; i++)
                {
                    posCurrentMin = zoneStart.position;
                    posCurrentMin.z -= halfAttackWidth;

                    posCurrentMax = zoneStart.position;
                    posCurrentMax.z -= halfAttackWidth;

                    posCurrentMax.x += maxRange;

                    posNextMin = zoneStart.position;
                    posNextMin.z += halfAttackWidth;

                    posNextMax = zoneStart.position;
                    posNextMax.z += halfAttackWidth;

                    posNextMax.x += maxRange;

                    int a = 4 * i;
                    int b = 4 * i + 1;
                    int c = 4 * i + 2;
                    int d = 4 * i + 3;

                    vertices[a] = posCurrentMin;
                    vertices[b] = posCurrentMax;
                    vertices[c] = posNextMax;
                    vertices[d] = posNextMin;

                    Quaternion qangle = Quaternion.AngleAxis(angleLookAt - 90.0f, Vector3.up);

                    vertices[a] -= zoneStart.position;
                    vertices[b] -= zoneStart.position;
                    vertices[c] -= zoneStart.position;
                    vertices[d] -= zoneStart.position;

                    vertices[a] = qangle * vertices[a];
                    vertices[b] = qangle * vertices[b];
                    vertices[c] = qangle * vertices[c];
                    vertices[d] = qangle * vertices[d];

                    vertices[a] += zoneStart.position;
                    vertices[b] += zoneStart.position;
                    vertices[c] += zoneStart.position;
                    vertices[d] += zoneStart.position;

                    triangles[6 * i] = a;
                    triangles[6 * i + 1] = d;
                    triangles[6 * i + 2] = c;
                    triangles[6 * i + 3] = c;
                    triangles[6 * i + 4] = b;
                    triangles[6 * i + 5] = a;

                    corner1 = vertices[a];
                    corner2 = vertices[b];
                    corner3 = vertices[c];
                    corner4 = vertices[d];
                }

                //mesh.uv = uv;
                mesh.vertices = vertices;
                mesh.triangles = triangles;

                // Bounds should be automatically recalculated when setting triangles
                mesh.RecalculateBounds();

                Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
                break;

            default:
                break;
        }
    }

    public void DrawCastTimeIndicator(Transform zoneStart, float angleOrWidth, float minRange, float maxRange, float drawPercent)
    {
        float angleLookAt;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        Vector3[] vertices = new Vector3 [4 * quality];
        int[] triangles = new int[3 * 2 * quality];

        switch (shape)
        {
            case IndicatorShape.RADIAL:
                angleLookAt = GetForwardAngle(zoneStart);

                float angleStart = angleLookAt - angleOrWidth;
                float angleEnd = angleLookAt + angleOrWidth;
                float angleDelta = (angleEnd - angleStart) / quality;

                float angleCurrent = angleStart;
                float angleNext = angleStart + angleDelta;

                //vertices = new Vector3[4 * quality];
                //triangles = new int[3 * 2 * quality];

                for (int i = 0; i < quality; i++)
                {
                    Vector3 sphereCurrent = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleCurrent)), 0,
                                                        Mathf.Cos(Mathf.Deg2Rad * (angleCurrent)));

                    Vector3 sphereNext = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleNext)), 0,
                                                     Mathf.Cos(Mathf.Deg2Rad * (angleNext)));

                    posCurrentMin = zoneStart.position + sphereCurrent * minRange;
                    posCurrentMax = zoneStart.position + sphereCurrent * (drawPercent * maxRange);

                    posNextMin = zoneStart.position + sphereNext * minRange;
                    posNextMax = zoneStart.position + sphereNext * (drawPercent * maxRange);

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
                break;

            case IndicatorShape.RECTANGULAR:
                angleLookAt = GetForwardAngle(zoneStart);

                float halfAttackWidth = angleOrWidth * 0.5f;

                //vertices = new Vector3[4];
                //triangles = new int[3 * 2];

                for (int i = 0; i < 1; i++)
                {


                    posCurrentMin = zoneStart.position;
                    posCurrentMin.z -= (halfAttackWidth * drawPercent);

                    posCurrentMax = zoneStart.position;
                    posCurrentMax.z -= (halfAttackWidth * drawPercent);

                    posCurrentMax.x += maxRange;


                    posNextMin = zoneStart.position;
                    posNextMin.z += (halfAttackWidth * drawPercent);

                    posNextMax = zoneStart.position;
                    posNextMax.z += (halfAttackWidth * drawPercent);

                    posNextMax.x += maxRange;


                    int a = 4 * i;
                    int b = 4 * i + 1;
                    int c = 4 * i + 2;
                    int d = 4 * i + 3;

                    vertices[a] = posCurrentMin;
                    vertices[b] = posCurrentMax;
                    vertices[c] = posNextMax;
                    vertices[d] = posNextMin;

                    Quaternion qangle = Quaternion.AngleAxis(angleLookAt - 90.0f, Vector3.up);

                    vertices[a] -= zoneStart.position;
                    vertices[b] -= zoneStart.position;
                    vertices[c] -= zoneStart.position;
                    vertices[d] -= zoneStart.position;

                    vertices[a] = qangle * vertices[a];
                    vertices[b] = qangle * vertices[b];
                    vertices[c] = qangle * vertices[c];
                    vertices[d] = qangle * vertices[d];

                    vertices[a] += zoneStart.position;
                    vertices[b] += zoneStart.position;
                    vertices[c] += zoneStart.position;
                    vertices[d] += zoneStart.position;

                    triangles[6 * i] = a;
                    triangles[6 * i + 1] = d;
                    triangles[6 * i + 2] = c;
                    triangles[6 * i + 3] = c;
                    triangles[6 * i + 4] = b;
                    triangles[6 * i + 5] = a;


                }

                castTimeMesh.vertices = vertices;
                castTimeMesh.triangles = triangles;

                castTimeMesh.RecalculateBounds();

                Graphics.DrawMesh(castTimeMesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
                break;

            default:
                break;
        }
    }

    float GetForwardAngle(Transform t)
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(t.forward.z, t.forward.x);
    }
}
