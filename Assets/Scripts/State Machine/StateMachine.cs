using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FiniteStateMachine {

	public class FSMMachine {
		private FSMState _currentState;
		private List<FSMState> _states;


		public FSMState initialCurrentState {
			set {
				if (_currentState == null) {
					_currentState = value;
					_currentState.Enter();
				}
				else {
					Debug.LogError("Current state already set.");
				}
			}
		}

	
		public FSMMachine () {
			_states = new List<FSMState>();
		}

		public void Update () {
			if (_currentState != null) {
				_currentState.Update();
			}
			else {
				Debug.LogError("Current state not set yet");
			}
		}

		public void AddState (FSMState newState) {
			foreach (FSMState state in _states) {
				if (state == newState || state.name.Equals(newState.name)) {
					return ;
				}
			}

			_states.Add(newState);
		}

		public void RemoveState (FSMState oldState) {
			_states.Remove (oldState);
		}

		public void RemoveState (string name) {
			FSMState oldState = null;
			foreach (FSMState state in _states) {
				if (state.name.Equals(name)) {
					oldState = state;
				}
			}

			if (oldState != null) {
				_states.Remove(oldState);
			}
		}

		public void ChangeState (string toState) {
			FSMState stateToChange = null;
			foreach (FSMState state in _states) {
				if (state.name.Equals(toState)) {
					stateToChange = state;
				}
			}

			if (stateToChange != null) {
				_currentState.Exit();
				_currentState = stateToChange;
				_currentState.Enter();
			}
			else {
				Debug.LogError("Invalid State name: " + toState);
			}
		}
	}

}