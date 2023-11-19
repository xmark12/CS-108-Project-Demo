using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //2D modification: made it easy to change values during runtime to do some tests.
    [SerializeField] LayerMask layerMask;
    [Range(0, 360)] [SerializeField] private float fov = 21f;
    [SerializeField] private int rayCount = 50;
    [SerializeField] public float viewDistance = 32;
    private Vector2 origin;
    float angle;

    private int layerGround;
    public bool playerDetected = false;

    //public HealthComponent health;
    //public EnemyBehave behave;

    void Start()
    {
        Renderer myrenderer = GetComponent<Renderer>();
        myrenderer.sortingLayerID = SortingLayer.layers[1].id;
        myrenderer.material.color = new Color(1f, 1f, 1f, 0.2f);
        origin = Vector2.zero;
        angle = 10f;

        layerGround = LayerMask.NameToLayer("Player");
    }

    void LateUpdate()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        float angleIncrease = fov / rayCount;
        int vertexIndex = 1;
        int triangleIndex = 0;
        vertices[0] = Vector3.zero;

        //raycast so the mesh does the thingy and changes depending on environment
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (!hit)
            {
                //if hit, go up to view distance
                vertex = GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //if hit, go up to point
                vertex = hit.point - origin;

                if (hit.transform.gameObject.layer == layerGround)
                {
                    playerDetected = true;
                }
                /*
                else if (hit.transform.gameObject.layer != layerGround)
                {
                    playerDetected = false;
                }
                */
            }
            vertices[vertexIndex] = vertex;
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        /*
        if (health.health <= 0 || behave.isAlerted)
        {
            Destroy(this.gameObject);
        }
        */
    }
    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);
    }

    public void SetPosition(Vector2 origin)
    {
        this.origin = origin;
    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
    }
}