using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FanSection
{
	public Vector3 point0;
	public Vector3 point1;

	public FanSection(Vector3 p0, Vector3 p1)
	{
		point0 = p0;
		point1 = p1;
	}
}

//画出一个扇形
public class CFanMesh
{
	#region Mesh Members
	private Vector3[] vertices;
	private Color[] colors;
	private Vector2[] uv;
    private int[] triangles;
	#endregion

    public bool BuildFan(GameObject FanDispObject, ref List<FanSection> sections)
	{
        // We need at least 2 sections to create the line
        if (sections == null || sections.Count < 2)
		{
			return false;
		}

        MeshFilter meshFilter = FanDispObject.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null)
        {
            return false;
        }

		// Rebuild the mesh	
        meshFilter.mesh.Clear();
		//
        int nElementCount = sections.Count * 2;
        if (vertices == null || vertices.GetLength(0) != nElementCount)
        {
            vertices = new Vector3[nElementCount];
        }

        if (colors == null || colors.GetLength(0) != nElementCount)
        {
            colors = new Color[nElementCount];
        }

        if (uv == null || uv.GetLength(0) != nElementCount)
        {
            uv = new Vector2[nElementCount];
        }

        nElementCount = (sections.Count - 1) * 2 * 3;
        if (triangles == null || triangles.GetLength(0) != nElementCount)
        {
            triangles = new int[nElementCount];
        }

        //
        FanSection currentSection = sections[0];
		//
		// Use matrix instead of transform.TransformPoint for performance reasons
        Matrix4x4 localSpaceTransform = FanDispObject.transform.worldToLocalMatrix;

		// Generate vertex, uv and colors
		for (var i = 0; i < sections.Count; i++)
		{
			currentSection = sections[i];

			vertices[i * 2 + 0] = localSpaceTransform.MultiplyPoint(currentSection.point1);
			vertices[i * 2 + 1] = localSpaceTransform.MultiplyPoint(currentSection.point0);

            // Calculate u for texture uv and color interpolation
            float u = ((float)i) / ((float)(sections.Count - 1));
            uv[i * 2 + 0] = new Vector2(u, 0);
			uv[i * 2 + 1] = new Vector2(u, 1);

			// fade colors out over time
            //Color interpolatedColor = Color.Lerp(startColor, endColor, u);
            colors[i * 2 + 0] = Color.white;
            colors[i * 2 + 1] = Color.white;
		}

		// Generate triangles indices
		for (int i = 0; i < triangles.Length / 6; i++)
		{
			triangles[i * 6 + 0] = i * 2;
			triangles[i * 6 + 1] = i * 2 + 1;
			triangles[i * 6 + 2] = i * 2 + 2;

			triangles[i * 6 + 3] = i * 2 + 2;
			triangles[i * 6 + 4] = i * 2 + 1;
			triangles[i * 6 + 5] = i * 2 + 3;
		}

		// Assign to mesh	
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.colors = colors;
        meshFilter.mesh.uv = uv;
        meshFilter.mesh.triangles = triangles;

        return true;
	}
}


