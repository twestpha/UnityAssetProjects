using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Strand {
	private List<Vector3> controlPoints;
	private List<Vector3> controlNormals;

	// Make these static
	string texture;
	Texture2D inputTexture;

	public Strand(){
		controlPoints = new List<Vector3>();
		controlNormals = new List<Vector3>();

		texture = "GUI/blue_pellet";
		inputTexture = (Texture2D)Resources.Load(texture);
	}

	public void AddControlPointAndNormal(Vector3 controlPoint, Vector3 controlNormal){
		controlPoints.Add(controlPoint);
		controlNormals.Add(controlNormal);
	}

	public bool HasControlPoints(){
		return controlPoints.Count != 0;
	}

	public void drawWireframeToGUI(){

		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[controlPoints.Count];
		mesh.triangles = new int[controlPoints.Count * 3];
		mesh.colors = new Color[controlPoints.Count];

		for(int i=0; i < controlPoints.Count; ++i){
			Vector3 screenPos = Camera.current.WorldToScreenPoint(controlPoints[i]);
			GUI.DrawTexture(new Rect(screenPos.x - 3, Screen.height - screenPos.y - 40, 5, 5), inputTexture);

			mesh.vertices[i] = controlPoints[i];
			mesh.colors[i] = Color.black;
			if(i > 0){
				mesh.triangles[i + 0] = i;
				mesh.triangles[i + 1] = i - 1;
				mesh.triangles[i + 2] = i;
			}
		}

		Graphics.DrawMeshNow(mesh, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
	}
}
