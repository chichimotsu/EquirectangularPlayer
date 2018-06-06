using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EQRSphere : MonoBehaviour {

    [Range(8, 128)]
    public int DivisionNum = 64;

    public float Radius_m;
    public Material SphereMaterial;

    MeshFilter mesh_filter;
    MeshRenderer mesh_renderer;

    Mesh sphere_mesh;
    Vector3[] vertices;
    int[] indices;
    Vector2[] uvs;

    void MakeMesh()
    {
        int kDivLon = DivisionNum * 2;
        int kDivLat = DivisionNum;

        vertices = new Vector3[(kDivLon + 1) * (kDivLat + 1)];
        uvs = new Vector2[(kDivLon + 1) * (kDivLat + 1)];
        for (int lat = 0; lat <= kDivLat; lat++)
        {
            var lat_rad = Mathf.PI / 2f - Mathf.PI / kDivLat * lat;
            for (int lon = 0; lon <= kDivLon; lon++)
            {
                var lon_rad = (lon != kDivLon) ? (2f * Mathf.PI / kDivLon * lon) : 0f;

                var idx = lat * (kDivLon + 1) + lon;
                vertices[idx].x = Radius_m * Mathf.Cos(lon_rad) * Mathf.Cos(lat_rad);
                vertices[idx].z = Radius_m * Mathf.Sin(lon_rad) * Mathf.Cos(lat_rad);
                vertices[idx].y = Radius_m * Mathf.Sin(lat_rad);

                uvs[idx].x = 1f - (float)lon / kDivLon;
                uvs[idx].y = 1f - (float)lat / kDivLat;
            }
        }

        indices = new int[3 * kDivLon * kDivLat * 2]; // each rectangles have 2 triangles.
        for (int y = 0; y < kDivLat; y++)
        {
            for (int x = 0; x < kDivLon; x++)
            {
                var idx_tl = y * (kDivLon + 1) + x;
                var idx_tr = idx_tl + 1;
                var idx_bl = (y + 1) * (kDivLon + 1) + x;
                var idx_br = idx_bl + 1;

                var idx = (y * kDivLon + x) * 6;

                // 1st tri.
                indices[idx + 0] = idx_tl;
                indices[idx + 1] = idx_bl;
                indices[idx + 2] = idx_tr;

                // 2nd tri.
                indices[idx + 3] = idx_tr;
                indices[idx + 4] = idx_bl;
                indices[idx + 5] = idx_br;
            }
        }

        sphere_mesh = new Mesh()
        {
            vertices = vertices,
            triangles = indices,
            uv = uvs,
        };
        //sphere_mesh.SetIndices(indices, MeshTopology.LineStrip, 0);   // For Debug.
        sphere_mesh.RecalculateNormals();

        return;
    }

	// Use this for initialization
	void Start ()
    {
        MakeMesh();

        mesh_filter = gameObject.GetComponent<MeshFilter>();
        if (!mesh_filter) mesh_filter = gameObject.AddComponent<MeshFilter>();
        mesh_filter.mesh = sphere_mesh;

        mesh_renderer = gameObject.GetComponent<MeshRenderer>();
        if (!mesh_renderer) mesh_renderer = gameObject.AddComponent<MeshRenderer>();
        mesh_renderer.sharedMaterial = SphereMaterial;

        return;
	}
	
	// Update is called once per frame
	void Update ()
    {
        return;
	}
}
