using UnityEngine;
using System.Collections;
using FiniteStateMachine.Unity;

namespace ModelLoaderFileExplorer {

	public class LoadingState : StateComponent {
		public UIProgressBar progressBar;
		public UIWindowButton windowButton;
		public float fakeLoadTime = 3;

		private float time;


		protected override void Enter () {
			base.Enter ();

			progressBar.value = 0;
		}

		protected override void Execute () {
			base.Execute ();

			progressBar.value += Random.Range(Time.deltaTime / fakeLoadTime * 0.5f, Time.deltaTime / fakeLoadTime * 1.5f);
			if (progressBar.value >= 1) {
				progressBar.value = 1;

				time += Time.deltaTime;
				if (time > 1) {
					windowButton.WindowAction();
				}
			}


		}

		protected override void Exit () {
			base.Exit ();
		}
	}

}