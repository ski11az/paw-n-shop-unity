using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Assemblage : MonoBehaviour
{
    private struct Pose
    {
        public Pose(Vector3 pos, float rot)
        {
            Pos = pos;
            Rot = rot;
        }

        public Vector3 Pos { get; }
        public float Rot { get; }
    }

    [SerializeField] List<Attachable> compositeFragments; // Contains fragments belonging to this assemblage

    List<Attachable> currentAttachables = new(); // Contains attachables currently attached to the assemblage during play

    Dictionary<Attachable, Pose> poseByFragment = new(); // Stores original local positions and rotations of fragments relative assemblage
    
    [SerializeField] float posMargin = 0.1f;
    [SerializeField] float rotMargin = 10.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        InitializeFragments();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Fragments")) return;

        AttachFragment(collision.collider.GetComponent<Attachable>(), collision.otherCollider.GetComponent<Attachable>());
    }

    /// <summary>
    /// Returns a copy of the list containing fragments belonging to this assemblage.
    /// </summary>
    /// <returns></returns>
    public List<Attachable> GetFragments()
    {
        return new List<Attachable>(compositeFragments);
    }

    /// <summary>
    /// Populates the fragment dictionaries and sets all fragments to inactive to avoid collisions between them.
    /// </summary>
    private void InitializeFragments()
    {
        foreach (Attachable fragment in compositeFragments)
        {
            Transform tf = fragment.transform;

            poseByFragment.Add(fragment, new Pose(tf.localPosition, tf.localEulerAngles.z));

            fragment.gameObject.SetActive(false);
        }
    }

    private void AttachFragment(Attachable incomingFragment, Attachable collidedFragment)
    {
        incomingFragment.Attach();
        currentAttachables.Add(incomingFragment);

        Transform incomingFragmentTf = incomingFragment.transform;
        incomingFragmentTf.parent = transform; // Must be set early for correct localTransform values

        if (!compositeFragments.Contains(incomingFragment)) return;

        Pose incomingPose = new(incomingFragment.transform.localPosition, incomingFragment.transform.localEulerAngles.z);

        List<Pose> destPoses = new();

        destPoses.Add(poseByFragment[incomingFragment]);

        if (poseByFragment.ContainsKey(collidedFragment))
        {
            //destPoses.Add(new Pose(GetRelativeLocalPosition(incomingFragment, collidedFragment), GetRelativeLocalRotation(incomingFragment, collidedFragment)));
            destPoses.Add(GetRelativeLocalPose(incomingFragment, collidedFragment));
        }

        foreach (Pose destPose in destPoses)
        {
            if (IsPerfectAttach(incomingPose, destPose))
            {
                incomingFragmentTf.SetLocalPositionAndRotation(destPose.Pos, Quaternion.Euler(0, 0, destPose.Rot));
                break;
            }
        }
    }

    private Pose GetRelativeLocalPose(Attachable incomingFragment, Attachable collidedFragment)
    {
        // Calculate relative position
        Vector3 ogIncomingPos = poseByFragment[incomingFragment].Pos;
        Vector3 ogCollidedPos = poseByFragment[collidedFragment].Pos;

        Vector3 relativePos = ogIncomingPos - ogCollidedPos; // From frame of collided

        Vector3 relativePosWorld = collidedFragment.transform.TransformPoint(relativePos);
        Vector3 relativePosLocal = transform.InverseTransformPoint(relativePosWorld);

        // Calculate relative rotation
        float ogIncomingRot = poseByFragment[incomingFragment].Rot;
        float ogCollidedRot = poseByFragment[collidedFragment].Rot;

        float relativeRot = ogIncomingRot - ogCollidedRot; // From frame of collided

        float relativeRotLocal = collidedFragment.transform.localEulerAngles.z + relativeRot;

        return new Pose(relativePosLocal, relativeRotLocal);
    }

    private bool IsPerfectAttach(Pose pose1, Pose pose2)
    {
        float posDelta = CalcPosDiff(pose1.Pos, pose2.Pos); // This can also be used for scoring
        float rotDelta = CalcRotDiff(pose1.Rot, pose2.Rot); // This can also be used for scoring
        return posDelta < posMargin && rotDelta < rotMargin;
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
}
