using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Assemblage : MonoBehaviour
{
    [SerializeField] List<Attachable> compositeFragments; // Contains fragments belonging to this assemblage

    Dictionary<Attachable, Vector3> posByFragment = new(); // Stores original local positions of fragments relative assemblage
    Dictionary<Attachable, float> rotByFragment = new(); // Stores original local ritations of fragments relative assemblage

    List<Attachable> currentAttachables = new(); // Contains attachables currently attached to the assemblage during play
    
    [SerializeField] float posMargin = 0.1f;
    [SerializeField] float rotMargin = 10.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        InitializeFragments();
    }

    /// <summary>
    /// Populates the fragment dictionaries and sets all fragments to inactive to avoid collisions between them.
    /// </summary>
    private void InitializeFragments()
    {
        foreach (Attachable fragment in compositeFragments)
        {
            Transform tf = fragment.transform;

            posByFragment.Add(fragment, tf.localPosition);
            rotByFragment.Add(fragment, tf.localRotation.eulerAngles.z);

            fragment.gameObject.SetActive(false);
        }
    }

    public void AttachFragment(Attachable fragment)
    {
        fragment.Attach();
        currentAttachables.Add(fragment);

        Transform fragmentTf = fragment.transform;
        fragmentTf.parent = transform; // Must be set early for correct localTransform values

        if (!compositeFragments.Contains(fragment)) return;

        Vector3 destPos = posByFragment[fragment];
        float destRot = rotByFragment[fragment];

        float posDelta = CalcPosDiff(fragmentTf.localPosition, destPos); // This can also be used for scoring
        float rotDelta = CalcRotDiff(fragmentTf.localRotation.eulerAngles.z, destRot); // This can also be used for scoring

        // Check if perfect attach
        if (posDelta < posMargin && rotDelta < rotMargin)
        {
            fragmentTf.SetLocalPositionAndRotation(destPos, Quaternion.Euler(0, 0, destRot));
            posDelta = 0;
            rotDelta = 0;
        }
    }

    private float CalcPosDiff(Vector3 current, Vector3 target)
    {
        return Vector3.Magnitude(target - current);
    }

    private float CalcRotDiff(float current, float target)
    {
        float diff = Mathf.Abs(target - current);
        if (diff > 180) diff = 360 - diff; // Accounts for periodicity of rotation

        return diff;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Fragments")) return;

        AttachFragment(collision.gameObject.GetComponent<Attachable>());
    }

    /// <summary>
    /// Returns a copy of the list containing fragments belonging to this assemblage.
    /// </summary>
    /// <returns></returns>
    public List<Attachable> GetFragments()
    {
        return new List<Attachable>(compositeFragments);
    }
}
