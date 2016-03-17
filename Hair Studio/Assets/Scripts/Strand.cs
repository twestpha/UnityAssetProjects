using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Strand {
	private List<Vector3> controlPoints;
	private List<Vector3> controlNormals;

	public Strand(){
		controlPoints = new List<Vector3>();
		controlNormals = new List<Vector3>();
	}

	public Vector3 getControlPointFromIndex(int index){
		return controlPoints[index];
	}

	public void AddControlPointAndNormal(Vector3 controlPoint, Vector3 controlNormal){
		controlPoints.Add(controlPoint);
		controlNormals.Add(controlNormal);
	}

	public int ControlPointsCount(){
		return controlPoints.Count;
	}

	public bool HasControlPoints(){
		return controlPoints.Count != 0;
	}
}
