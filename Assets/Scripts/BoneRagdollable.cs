using UnityEngine;

public class BoneRagdollable : MonoBehaviour 
{
	[SerializeField][Uneditable] private bool m_Head = true;
	[SerializeField][Uneditable] private bool m_Chest = true;
	[SerializeField][Uneditable] private bool m_LeftArm = true;
	[SerializeField][Uneditable] private bool m_RightArm = true;
	[SerializeField][Uneditable] private bool m_Hips = true;
	[SerializeField][Uneditable] private bool m_LeftLeg = false;
	[SerializeField][Uneditable] private bool m_RightLeg = false;

	public bool IsInclude(HumanBodyBones pBone)
	{
		switch (pBone)
		{
			case HumanBodyBones.Hips:
				return m_Hips;
			case HumanBodyBones.LeftUpperLeg:
			case HumanBodyBones.LeftLowerLeg:
			case HumanBodyBones.LeftFoot:
			case HumanBodyBones.LeftToes:
				return m_LeftLeg;
			case HumanBodyBones.RightUpperLeg:
			case HumanBodyBones.RightLowerLeg:
			case HumanBodyBones.RightFoot:
			case HumanBodyBones.RightToes:
				return m_RightLeg;
			case HumanBodyBones.Spine:
			case HumanBodyBones.Chest:
			case HumanBodyBones.UpperChest:
				return m_Chest;
			case HumanBodyBones.Neck:
			case HumanBodyBones.Head:
			case HumanBodyBones.LeftEye:
			case HumanBodyBones.RightEye:
			case HumanBodyBones.Jaw:
				return m_Head;
			case HumanBodyBones.LeftShoulder:
			case HumanBodyBones.LeftUpperArm:
			case HumanBodyBones.LeftLowerArm:
			case HumanBodyBones.LeftHand:
				return m_LeftArm;
			case HumanBodyBones.RightShoulder:
			case HumanBodyBones.RightUpperArm:
			case HumanBodyBones.RightLowerArm:
			case HumanBodyBones.RightHand:
				return m_RightArm;
			case HumanBodyBones.LastBone:
				return false;
		}

		if (pBone.ToString().Contains("Left"))
		{
			return m_LeftArm;
		}
		if (pBone.ToString().Contains("Right"))
		{
			return m_RightArm;
		}
		
		return false;
	}
}
