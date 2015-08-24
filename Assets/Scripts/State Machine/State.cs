using UnityEngine;
using System.Collections;

namespace FiniteStateMachine {

	public class FSMState {
		public string name;

		public FSMState () {}

		public FSMState (string name) {
			this.name = name;
		}

		public virtual void Enter () {}
		public virtual void Update () {}
		public virtual void Exit () {}
	}

}