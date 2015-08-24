using UnityEngine;
using System;
using System.Collections;
using AVOSCloud;
using Parse;

namespace Cloud {
	public static class CloudUser {

		public static void Login (string username, string password, Action successAction, Action<string> failedAction) {
			switch (CloudSettings.instance.serviceProvider) {
			case ServiceProvider.Parse:
				LoginP(username, password, successAction, failedAction);
				break;
			case ServiceProvider.LeanCloud:
				LoginLC(username, password, successAction, failedAction);
				break;
			}
		}

		public static void Logout () {
			switch (CloudSettings.instance.serviceProvider) {
			case ServiceProvider.Parse:
				LogoutP();
				break;
			case ServiceProvider.LeanCloud:
				LogoutLC();
				break;
			}
		}

		private static void LoginLC (string username, string password, Action successAction, Action<string> failedAction) {
			Debug.Log(username);
			Debug.Log(password);
			AVUser.LogInAsync(username, password).ContinueWith(t => {
				if (!t.IsFaulted && !t.IsCanceled) {
					if (successAction != null) successAction();
					Debug.Log("Login Success");
				}
				else {
					if (failedAction != null) failedAction(t.Exception.Message);
					Debug.Log("Login Failed");
				}
			});
		}

		private static void LogoutLC () {
			AVUser.LogOut();
		}

		private static void LoginP (string username, string password, Action successAction, Action<string> failedAction) {
			Debug.Log(username);
			Debug.Log(password);
			ParseUser.LogInAsync(username, password).ContinueWith(t => {
				if (!t.IsFaulted && !t.IsCanceled) {
					if (successAction != null) successAction();
					Debug.Log("Login Success");
				}
				else {
					if (failedAction != null) failedAction(t.Exception.Message);
					Debug.Log("Login Failed");
				}
			});
		}
		
		private static void LogoutP () {
			ParseUser.LogOut();
		}
	}

}