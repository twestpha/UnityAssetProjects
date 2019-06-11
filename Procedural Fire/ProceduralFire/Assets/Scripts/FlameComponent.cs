using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FlameComponent : MonoBehaviour {
    private const float SEGMENTS = 4.0f;

	// randomize position for all flame components (how to detect "last" or "first" one to update in the frame? just write it at the end of every frame?)
    // or just the first to Start() claims a core instance and writes it on a late update?
    // also personal noise
	// offset that into local space
	// lerp towards there iwith weight based on height?

    private float height;
    private float segmentHeight;

    private float timeUntilMove;

    private Vector3[] scratch;

    private Vector3 targeta;
    private Vector3 targetb;
    private Vector3 targetc;

    private Vector3 vela;
    private Vector3 velb;
    private Vector3 velc;

    private LineRenderer line;

    void Start(){
        line = GetComponent<LineRenderer>();

        scratch = new Vector3[4];

        height = (Random.value * 1.0f) + 0.5f;
        segmentHeight = height / SEGMENTS;

        timeUntilMove = Random.value + 0.25f;
    }

    void Update(){
        if(timeUntilMove > 0.0f){
            timeUntilMove -= Time.deltaTime;
        } else {
            timeUntilMove = Random.value + 0.25f;

            targeta = new Vector3(Random.value - 0.5f, 0.0f, Random.value - 0.5f) * 0.3f;
            targetb = new Vector3(Random.value - 0.5f, 0.0f, Random.value - 0.5f) * 0.2f;
            targetc = new Vector3(Random.value - 0.5f, 0.0f, Random.value - 0.5f) * 0.1f;
        }

        line.GetPositions(scratch);

        scratch[0] = Vector3.zero;

        // Why the fuck doesn't this simulate in world space?
        scratch[1] = Vector3.SmoothDamp(scratch[1] + transform.position, (targeta + transform.position) + (Vector3.up * segmentHeight * 1.0f), ref vela, 0.5f) - transform.position;
        scratch[2] = Vector3.SmoothDamp(scratch[2] + transform.position, (targeta + targetb + transform.position) + (Vector3.up * segmentHeight * 2.0f), ref velb, 0.7f) - transform.position;
        scratch[3] = Vector3.SmoothDamp(scratch[3] + transform.position, (targeta + targetb + targetc + transform.position) + (Vector3.up * segmentHeight * 3.0f), ref velc, 0.9f) - transform.position;

        line.SetPositions(scratch);
    }

    void LateUpdate(){

    }
}
