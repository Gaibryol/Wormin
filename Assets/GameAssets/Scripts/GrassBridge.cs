using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

/*
 * Bends the grass blade
 * Object must implement IGrassBendable
 * CanBendGrass() must be overriden (prob using mass condition)
 * Animation lerps between two positions and is controlled by the bend parameter
 */

[RequireComponent(typeof(Animator), typeof(SpriteSkin))]
public class GrassBridge : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float bend = 0f;
    private Transform trackedObject;

    private Animator animator;
    private SpriteSkin skin;

    private Transform[] boneTransforms;

    [SerializeField] private int lastBoneIndex = 11;

    void Start()
    {
        animator = GetComponent<Animator>();
        skin = GetComponent<SpriteSkin>();

        boneTransforms = skin.boneTransforms;
    }


    void Update()
    {
        if (trackedObject == null || !trackedObject.GetComponent<IGrassBendable>().CanBendGrass()) 
        {
            ResetBend(Time.deltaTime);
            return; 
        }

        float distance = Vector2.Distance(boneTransforms[0].position, trackedObject.position);
        float boneDistance = Vector2.Distance(boneTransforms[0].position, boneTransforms[lastBoneIndex].position);
        float traveledPecent = distance / boneDistance;
        float bendTarget = Mathf.Lerp(0, 1, traveledPecent);
        bend = Mathf.MoveTowards(bend, bendTarget, Time.deltaTime);
        animator.SetFloat("bend", bend);
        
    }

    private void ResetBend(float deltaTime)
    {
        bend = Mathf.MoveTowards(bend, 0, deltaTime);
        animator.SetFloat("bend", bend);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGrassBendable grassBendable = collision.GetComponent<IGrassBendable>();
        if (grassBendable == null) { return; }
        Debug.Log("enter");
        trackedObject = collision.transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IGrassBendable grassBendable = collision.GetComponent<IGrassBendable>();
        if (grassBendable == null) { return; }
        trackedObject = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IGrassBendable grassBendable = collision.GetComponent<IGrassBendable>();
        if (grassBendable == null) { return; }
        trackedObject = null;
    }
}
