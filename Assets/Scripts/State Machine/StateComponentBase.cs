using UnityEngine;
using System.Collections;

namespace FiniteStateMachine.Internal {

	public class StateComponentBase : MonoBehaviour {

		protected static StateComponentBase s_currentState;

		protected virtual void OnEnable () {Enter();}
		protected virtual void OnDisable () {Exit();}
		protected virtual void Update () {Execute();}

		protected virtual void Enter () {}
		protected virtual void Execute () {}
		protected virtual void Exit () {}
	}

}