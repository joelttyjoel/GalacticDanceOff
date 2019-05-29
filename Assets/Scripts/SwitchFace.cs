using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFace : MonoBehaviour {

    [SerializeField]
    private Material[] faceMaterials = new Material[1];

    private Animator anim;

    private SkinnedMeshRenderer faceRenderer;

    private int faceInt;

    Material[] mat;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponentInParent<Animator>();

        faceRenderer = GetComponent<SkinnedMeshRenderer>();
        mat = faceRenderer.materials;

        for (int i = 0; i < mat.Length; i++)
        {
            if (mat[i].name.Contains("Face"))
            {
                faceInt = i;
                break;
            }
        }
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        int i = anim.GetInteger("SelectState");
		if (faceMaterials[i])
        {
            mat[faceInt] = faceMaterials[i];
            faceRenderer.materials = mat;
        }
	}
}
