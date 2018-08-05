using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtility {

	public static float ANIM_LENGTH = 1.2f;

	public static float PLAYER_ANIM_LENGTH = 3f;

	private static AnimationCurve xCurve;
	private static AnimationCurve yCurve;
	private static AnimationCurve zCurve;
	private static Keyframe[] ks;

	public static AnimationCurve[] moveToParent (Transform beginning, float xOffset, float yOffset, float zOffset)
	{
		
		ks = new Keyframe[2];

		ks[0] = new Keyframe(0, beginning.localPosition.x);
		ks[1] = new Keyframe(ANIM_LENGTH, 0 + xOffset);

		xCurve = new AnimationCurve(ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks[0] = new Keyframe(0, beginning.localPosition.y);
		ks[1] = new Keyframe(ANIM_LENGTH, 0 + yOffset);

		yCurve = new AnimationCurve(ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks[0] = new Keyframe(0, beginning.localPosition.z);
		ks[1] = new Keyframe(ANIM_LENGTH, 0 + zOffset);

		zCurve = new AnimationCurve(ks);
		zCurve.postWrapMode = WrapMode.Once;

		AnimationCurve[] curves = new AnimationCurve[3];
		curves [0] = xCurve;
		curves [1] = yCurve;
		curves [2] = zCurve;
		return curves;
	}

	public static AnimationCurve[] movePlayer(Transform player, Vector3 destination) {
		ks = new Keyframe[2];

		ks[0] = new Keyframe(0, player.position.x);
		ks[1] = new Keyframe(PLAYER_ANIM_LENGTH, destination.x);

		xCurve = new AnimationCurve(ks);
		xCurve.postWrapMode = WrapMode.Once;

		// Keep the players 'height'
		ks[0] = new Keyframe(0, player.position.y);
		ks[1] = new Keyframe(PLAYER_ANIM_LENGTH, player.position.y);

		yCurve = new AnimationCurve(ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks[0] = new Keyframe(0, player.position.z);
		ks[1] = new Keyframe(PLAYER_ANIM_LENGTH, destination.z);

		zCurve = new AnimationCurve(ks);
		zCurve.postWrapMode = WrapMode.Once;

		AnimationCurve[] curves = new AnimationCurve[3];
		curves [0] = xCurve;
		curves [1] = yCurve;
		curves [2] = zCurve;
		return curves;
	}
}
