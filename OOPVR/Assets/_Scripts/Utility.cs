using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOPVR {
	public static class Utility {

		public static List<int> AllIndexsOf(this string str, string value) {
			List<int> indexes = new List<int> ();
			for (int index = 0;; index += value.Length) {
				index = str.IndexOf (value, index);
				if (index == -1)
					return indexes;
				indexes.Add (index);
			}
		}
	}
}
