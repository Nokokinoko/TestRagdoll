using System;
using UnityEngine;

public class BoneRagdollable : MonoBehaviour 
{
	[Serializable]
	private class ManageBone
	{
		public bool Head;
		public bool Chest;
		public bool LeftArm;
		public bool RightArm;
		public bool Hips;
		public bool LeftLeg;
		public bool RightLeg;
	}
	
	[SerializeField] private ManageBone m_ManageBone;

	public bool IsInclude(HumanBodyBones pBone)
	{
		switch (pBone)
		{
			case HumanBodyBones.Hips:
				return m_ManageBone.Hips;
			case HumanBodyBones.LeftUpperLeg:
			case HumanBodyBones.LeftLowerLeg:
			case HumanBodyBones.LeftFoot:
			case HumanBodyBones.LeftToes:
				return m_ManageBone.LeftLeg;
			case HumanBodyBones.RightUpperLeg:
			case HumanBodyBones.RightLowerLeg:
			case HumanBodyBones.RightFoot:
			case HumanBodyBones.RightToes:
				return m_ManageBone.RightLeg;
			case HumanBodyBones.Spine:
			case HumanBodyBones.Chest:
			case HumanBodyBones.UpperChest:
				return m_ManageBone.Chest;
			case HumanBodyBones.Neck:
			case HumanBodyBones.Head:
			case HumanBodyBones.LeftEye:
			case HumanBodyBones.RightEye:
			case HumanBodyBones.Jaw:
				return m_ManageBone.Head;
			case HumanBodyBones.LeftShoulder:
			case HumanBodyBones.LeftUpperArm:
			case HumanBodyBones.LeftLowerArm:
			case HumanBodyBones.LeftHand:
				return m_ManageBone.LeftArm;
			case HumanBodyBones.RightShoulder:
			case HumanBodyBones.RightUpperArm:
			case HumanBodyBones.RightLowerArm:
			case HumanBodyBones.RightHand:
				return m_ManageBone.RightArm;
			case HumanBodyBones.LastBone:
				return false;
		}

		if (pBone.ToString().Contains("Left"))
		{
			return m_ManageBone.LeftArm;
		}
		if (pBone.ToString().Contains("Right"))
		{
			return m_ManageBone.RightArm;
		}
		
		return false;
	}
}
