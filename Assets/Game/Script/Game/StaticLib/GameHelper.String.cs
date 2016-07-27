using UnityEngine;
using System.Text;
using System.Collections;

public partial class GameHelper {
	public static string ToDay(int sec) {
		int d = (sec / (24 * 60 * 60));
		if (d > 0) {
			return System.Convert.ToString (d + 1);
		}

		return "0";
	}

	public static string ToTime(int sec) {
		int h = (sec / (60 * 60)) % 24;
		int m = (sec / 60) % 60;
		int s = sec % 60;

		StringBuilder sb = new StringBuilder ();

		if (h > 0) {
			if (h / 10 > 0) {
				sb.Append (System.Convert.ToInt32 (h));
			} else {
				sb.Append ("0");
				sb.Append (System.Convert.ToInt32 (h));
			}
		}

		if (sb.Length > 0) {
			sb.Append (":");
		} 

		if (m > 0 || h > 0) {
			if (m / 10 > 0) {
				sb.Append (System.Convert.ToInt32 (m));
			} else {
				sb.Append ("0");
				sb.Append (System.Convert.ToInt32 (m));
			}
		}

		if (sb.Length > 0) {
			sb.Append (":");
		} 

		if (s > 0 || (m > 0 || h > 0)) {
			if (s / 10 > 0) {
				sb.Append (System.Convert.ToInt32 (s));
			} else {
				sb.Append ("0");
				sb.Append (System.Convert.ToInt32 (s));
			}
		}

		return sb.ToString ();
	}

	public static string ToCostValue(int value) {
		StringBuilder stringBuilder = new StringBuilder ();

		if (value > 0) {
			int divide = 1000;
			int quotient = value;
			int remainder = 0;

			while (quotient > 0) {
				remainder = quotient % divide;
				quotient /= divide;
				if (quotient > 0) {
					stringBuilder.Insert (0, string.Format ("{0:D3}", remainder));
					stringBuilder.Insert (0, ",");
				} else {
					stringBuilder.Insert (0, string.Format("{0:D}", remainder));
				}
			}	
		} else {
			stringBuilder.Append ("0");
		}

		return stringBuilder.ToString ();
	}
}
