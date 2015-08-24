//----------------------------------------------
// © 2013 Tinbabu
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace RMI
{
	/**
	\english
	\brief Represents foundation for RMI.
	
	Use RMITask from a class that inherits from this.
	\endenglish
	*/
	public class RMIManagerHelper : MonoBehaviour
	{
		/**
		\english
		\brief Initalize RMI.

		\warning Make sure that you call before and after use of RMI.
		\endenglish
		*/
		[DllImport("RMICore")]
		public static extern void RMIInitalize ();

		void Awake ()
		{
			RMIInitalize ();
		}

		void OnDestroy ()
		{
			RMIInitalize ();
		}
	}
}
