using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Strand {
	private List<Vector3> controlPoints;
	private List<Vector3> controlNormals;

	// Static Resources
	static Texture2D inputTexture;
	static Material wireframeMaterial;

	public Strand(){
		controlPoints = new List<Vector3>();
		controlNormals = new List<Vector3>();

		inputTexture = (Texture2D)Resources.Load("GUI/blue_pellet");
		wireframeMaterial = (Material)Resources.Load("GUI/wireframe_material");
	}

	public void AddControlPointAndNormal(Vector3 controlPoint, Vector3 controlNormal){
		controlPoints.Add(controlPoint);
		controlNormals.Add(controlNormal);
	}

	public bool HasControlPoints(){
		return controlPoints.Count != 0;
	}

	public void drawWireframeToGUI(){
		// Drawing Wireframe
		GL.PushMatrix();
			// might still need this...?
			// GL.MultMatrix(gameObject.transform.localToWorldMatrix);
			wireframeMaterial.SetPass(0);
			GL.Begin(GL.LINES);
				for(int i = 1; i < controlPoints.Count; ++i){
					GL.Vertex(controlPoints[i-1]);
					GL.Vertex(controlPoints[i]);
					Debug.Log((i - 1) + "-->" + i);
				}
			GL.End();
		GL.PopMatrix();

		// Drawing Pellet
		for(int i=0; i < controlPoints.Count; ++i){
			Vector3 screenPos = Camera.current.WorldToScreenPoint(controlPoints[i]);
			Vector3 screenCoords = getTrueScreenCoordinates(screenPos);

			GUI.DrawTexture(new Rect(screenCoords.x, screenCoords.y, 5, 5), inputTexture);
		}
	}

	static public Vector2 getTrueScreenCoordinates(Vector3 screenPosition){
		return new Vector2(screenPosition.x - 3, Screen.height - screenPosition.y - 40);
	}
}
