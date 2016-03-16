using UnityEngine;
using System.Collections;

 public class HairStudio : MonoBehaviour {

	 public Strand[] strands;

     void Start(){
     }

     void Update(){
     }

     public void AddMeshColliderComponent(){
         gameObject.AddComponent<MeshCollider>();
     }

     public void RemoveMeshColliderComponent(){
         DestroyImmediate(GetComponent<MeshCollider>());
     }
 }
