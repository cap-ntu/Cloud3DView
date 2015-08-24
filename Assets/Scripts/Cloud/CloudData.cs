using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AVOSCloud;
using Parse;

namespace ModelLoaderFileExplorer {
	
	public static class CloudData {

		public static void SaveInCloud (ModelData data, Action successAction, Action<string> failedAction) {
			switch (CloudSettings.instance.serviceProvider) {
			case ServiceProvider.Parse:
				SaveInCloudP(data, successAction, failedAction);
				break;
			case ServiceProvider.LeanCloud:
				SaveInCloudLC(data, successAction, failedAction);
				break;
			}
		}

		public static void GetUserModelDataListFromCloud (Action<List<ModelData>> successAction, Action<string> failedAction) {
			switch (CloudSettings.instance.serviceProvider) {
			case ServiceProvider.Parse:
				GetUserModelDataListFromCloudP(successAction, failedAction);
				break;
			case ServiceProvider.LeanCloud:
				GetUserModelDataListFromCloudLC(successAction, failedAction);
				break;
			}
		}

		public static void GetModelDataListFromCloud (bool publicOnly, bool includeUsername, Action<List<ModelData>> successAction, Action<string> failedAction) {
			switch (CloudSettings.instance.serviceProvider) {
			case ServiceProvider.Parse:
				GetModelDataListFromCloudP(publicOnly, includeUsername, successAction, failedAction);
				break;
			case ServiceProvider.LeanCloud:
				GetModelDataListFromCloudLC(publicOnly, includeUsername, successAction, failedAction);
				break;
			}
		}


		private static void SaveInCloudLC (ModelData data, Action successAction, Action<string> failedAction) {
			AVObject avData = new AVObject("ServerData");

			bool isNewModel = true;
			if (data.objectId != null && !data.objectId.Equals("")) {
				avData.ObjectId = data.objectId;
				isNewModel = false;
			}
			
			avData["name"] = data.name;
			avData["is_private"] = data.isPrivate;
			avData["unit_size"] = data.unitSize;
			avData["cpu_count"] = data.cpuCoresCount;
			avData["cpu_hz"] = data.cpuHz;
			avData["hdd_count"] = data.hddBaysCount;
			avData["memory_speed"] = data.memorySpeed;
			avData["memory_capacity"] = PairData.ToArrayOfInts(data.memoryCapacity);
			avData["raid"] = data.raid;
			avData["hdd"] = PairData.ToArrayOfInts(data.hdd);
			avData["network"] = PairData.ToArrayOfInts(data.network);
			avData["gpu"] = data.gpu;

			avData.SaveAsync().ContinueWith(t => {
				if (t.IsFaulted || t.IsCanceled) {
					if (failedAction != null) {
						var dataString = "";
						foreach (var key in t.Exception.Data.Keys) {
							dataString += t.Exception.Data[key] + "\n";
						}
						failedAction(t.Exception.Message + "\n" + dataString);
					}
				}
				else {
					if (isNewModel) {
						var userToServers = new AVObject("UserServers");
						userToServers["user"] = AVUser.CurrentUser;
						userToServers["model"] = avData;

						userToServers.SaveAsync().ContinueWith(t2 => {
							if (t2.IsFaulted || t2.IsCanceled) {
								Debug.Log("User To Servers Failed");
								if (failedAction != null) {
									failedAction(t.Exception.Message);
								}
							}
							else {
								if (successAction != null) {
									successAction();
								}		
							}
						});
					}
					else {
						if (successAction != null) {
							successAction();
						}
					}
				}
			});
		}


		private static void GetUserModelDataListFromCloudLC (Action<List<ModelData>> successAction, Action<string> failedAction) {
			var query = new AVQuery<AVObject>("UserServers");
			query = query.Include("model");
//			query = query.WhereEqualTo("user", AVUser.CurrentUser.ObjectId);

			query.FindAsync().ContinueWith(t => {
				if (t.IsFaulted || t.IsCanceled) {
					if (failedAction != null) {
						failedAction(t.Exception.Message);
					}
				}
				else {
					if (successAction != null) {
						IEnumerable<AVObject> avDatas = t.Result;

						var results = new List<ModelData>();
						
						foreach (var avData in avDatas) {
							
							var avDataForModel = avData.Get<AVObject>("model");
							var data = new ModelData();
							
							data.objectId = avDataForModel.ObjectId;
							
							data.name = avDataForModel.Get<string>("name");
							data.isPrivate = avDataForModel.Get<bool>("is_private");
//							data.unitSize = avDataForModel.Get<int>("unit_size");
//							data.cpuCoresCount = avDataForModel.Get<int>("cpu_count");
//							data.cpuHz = avDataForModel.Get<int>("cpu_hz");
//							data.hddBaysCount = avDataForModel.Get<int>("hdd_count");
//							data.memorySpeed = avDataForModel.Get<int>("memory_speed");
//							
//							data.memoryCapacity = PairData.FromListOfIntsToList(avDataForModel.Get<IList<int>>("memory_capacity"));
//							
//							data.raid = avDataForModel.Get<string>("raid");
//							
//							data.hdd = PairData.FromListOfIntsToList(avDataForModel.Get<IList<int>>("hdd"));
//							data.gpu = (List<string>) (avData.Get<IList<string>>("gpu"));
							
							results.Add(data);
						}
						
						successAction(results);
					}
				}
			});
		}


		private static void GetModelDataListFromCloudLC (bool publicOnly, bool includeUsername, Action<List<ModelData>> successAction, Action<string> failedAction) {
			var query = new AVQuery<AVObject>("ServerData");

			if (publicOnly) {
				query = query.WhereEqualTo("is_private", false);
			}

			query = query.OrderByDescending("updateAt");

			query.FindAsync().ContinueWith(t => {
				if (t.IsFaulted || t.IsCanceled) {
					if (failedAction != null) {
						failedAction(t.Exception.Message);
					}
				}
				else {
					if (successAction != null) {
						IEnumerable<AVObject> avDatas = t.Result;
						var results = new List<ModelData>();

						foreach (var avData in avDatas) {
							var data = new ModelData();

							data.objectId = avData.ObjectId;

							data.name = avData.Get<string>("name");
							data.isPrivate = avData.Get<bool>("is_private");
							data.unitSize = avData.Get<int>("unit_size");
							data.cpuCoresCount = avData.Get<int>("cpu_count");
							data.cpuHz = avData.Get<int>("cpu_hz");
							data.hddBaysCount = avData.Get<int>("hdd_count");
							data.memorySpeed = avData.Get<int>("memory_speed");

							data.memoryCapacity = PairData.FromListOfIntsToList(avData.Get<IList<int>>("memory_capacity"));

							data.raid = avData.Get<string>("raid");

							data.hdd = PairData.FromListOfIntsToList(avData.Get<IList<int>>("hdd"));
//							data.gpu = (List<string>) (avData.Get<IList<string>>("gpu"));

							results.Add(data);
						}

						successAction(results);
					}
				}
			});
		}


		private static void SaveInCloudP (ModelData data, Action successAction, Action<string> failedAction) {
			ParseObject parseData = new ParseObject("ServerData");

			bool isNewModel = true;
			if (data.objectId != null && !data.objectId.Equals("")) {
				parseData.ObjectId = data.objectId;
				isNewModel = false;
			}
			
			parseData["name"] = data.name;
			parseData["is_private"] = data.isPrivate;
			parseData["unit_size"] = data.unitSize;
			parseData["cpu_count"] = data.cpuCoresCount;
			parseData["cpu_hz"] = data.cpuHz;
			parseData["hdd_count"] = data.hddBaysCount;
			parseData["memory_speed"] = data.memorySpeed;
			parseData["memory_capacity"] = PairData.ToArrayOfInts(data.memoryCapacity);
			parseData["raid"] = data.raid;
			parseData["hdd"] = PairData.ToArrayOfInts(data.hdd);
			parseData["network"] = PairData.ToArrayOfInts(data.network);
			parseData["gpu"] = data.gpu;
			
			parseData.SaveAsync().ContinueWith(t => {	// 1. Save basic data
				if (t.IsFaulted || t.IsCanceled) {
					if (failedAction != null) {
						var dataString = "";
						foreach (var key in t.Exception.Data.Keys) {
							dataString += t.Exception.Data[key] + "\n";
						}
						failedAction(t.Exception.Message + "\n" + dataString);
					}
				}
				else {
					if (isNewModel) {
						var userToServers = new ParseObject("UserServers");
						userToServers["user"] = ParseUser.CurrentUser;
						userToServers["model"] = parseData;
						
						userToServers.SaveAsync().ContinueWith(t2 => {	// 2. Save user information
							if (t2.IsFaulted || t2.IsCanceled) {
								Debug.Log("User To Servers Failed");
								if (failedAction != null) {
									failedAction(t2.Exception.Message);
								}
							}
							else {
								var parts = data.modelPath.Split('.');
								var file = new ParseFile("model." + parts[parts.Length-1], File.ReadAllBytes(data.modelPath));
								parseData["model_file"] = file;
								parseData.SaveAsync().ContinueWith(t3 => {	// 3. save model file
									if (t3.IsFaulted || t3.IsCanceled) {
										Debug.Log("User To Servers Failed");
										if (failedAction != null) {
											failedAction(t3.Exception.Message);
										}
									}
									else {
										if (successAction != null) {
											successAction();
										}
									}
								});
							}
						});
					}
					else {
						if (successAction != null) {
							successAction();
						}
					}
				}
			});
		}

		private static void GetUserModelDataListFromCloudP (Action<List<ModelData>> successAction, Action<string> failedAction) {
			var query = new ParseQuery<ParseObject>("UserServers");
			query = query.Include("model");
			//			query = query.WhereEqualTo("user", ParseUser.CurrentUser.ObjectId);
			
			query.FindAsync().ContinueWith(t => {
				if (t.IsFaulted || t.IsCanceled) {
					if (failedAction != null) {
						failedAction(t.Exception.Message);
					}
				}
				else {
					if (successAction != null) {
						IEnumerable<ParseObject> parseDatas = t.Result;
						
						var results = new List<ModelData>();
						
						foreach (var parseData in parseDatas) {
							
							var parseDataForModel = parseData.Get<ParseObject>("model");
							var data = new ModelData();
							
							data.objectId = parseDataForModel.ObjectId;
							data.modelFile = parseDataForModel.Get<ParseFile>("model_file").Name;
							data.modelUrl = parseDataForModel.Get<ParseFile>("model_file").Url.AbsoluteUri;
							
							data.name = parseDataForModel.Get<string>("name");
							data.isPrivate = parseDataForModel.Get<bool>("is_private");
							data.unitSize = parseDataForModel.Get<int>("unit_size");
							data.cpuCoresCount = parseDataForModel.Get<int>("cpu_count");
							data.cpuHz = parseDataForModel.Get<int>("cpu_hz");
							data.hddBaysCount = parseDataForModel.Get<int>("hdd_count");
							data.memorySpeed = parseDataForModel.Get<int>("memory_speed");
							
							data.memoryCapacity = PairData.FromListOfIntsToList(parseDataForModel.Get<IList<int>>("memory_capacity"));
							
							data.raid = parseDataForModel.Get<string>("raid");
							
							data.hdd = PairData.FromListOfIntsToList(parseDataForModel.Get<IList<int>>("hdd"));
//							data.gpu = (List<string>) (parseDataForModel.Get<IList<string>>("gpu"));
							var gpus = (parseDataForModel.Get<IList<string>>("gpu"));
							data.gpu = new List<string>();
							foreach (var gpu in gpus) {
								data.gpu.Add(gpu);
							}

							results.Add(data);
						}
						
						successAction(results);
					}
				}
			});
		}


		private static void GetModelDataListFromCloudP (bool publicOnly, bool includeUsername, Action<List<ModelData>> successAction, Action<string> failedAction) {
			var serverDataQuery = new ParseQuery<ParseObject>("ServerData");
			
			if (publicOnly) {
				serverDataQuery = serverDataQuery.WhereEqualTo("is_private", false);
			}

			var query = new ParseQuery<ParseObject>("UserServers");
			query = query.WhereMatchesQuery("model", serverDataQuery);
			query = query.Include("model");
			query = query.Include("user");
			
			query = query.OrderByDescending("updateAt");
			
			query.FindAsync().ContinueWith(t => {
				if (t.IsFaulted || t.IsCanceled) {
					if (failedAction != null) {
						failedAction(t.Exception.Message);
					}
				}
				else {
					if (successAction != null) {
						IEnumerable<ParseObject> parseDatas = t.Result;
						var results = new List<ModelData>();
						
						foreach (var parseData in parseDatas) {
							var data = new ModelData();

							var parseModelData = parseData.Get<ParseObject>("model");
							var parseUserData = parseData.Get<ParseUser>("user");

							data.objectId = parseModelData.ObjectId;
							data.username = parseUserData.Username;
							data.modelFile = parseModelData.Get<ParseFile>("model_file").Name;
							data.modelUrl = parseModelData.Get<ParseFile>("model_file").Url.AbsoluteUri;

							
							data.name = parseModelData.Get<string>("name");
							data.isPrivate = parseModelData.Get<bool>("is_private");
							data.unitSize = parseModelData.Get<int>("unit_size");
							data.cpuCoresCount = parseModelData.Get<int>("cpu_count");
							data.cpuHz = parseModelData.Get<int>("cpu_hz");
							data.hddBaysCount = parseModelData.Get<int>("hdd_count");
							data.memorySpeed = parseModelData.Get<int>("memory_speed");
							
							data.memoryCapacity = PairData.FromListOfIntsToList(parseModelData.Get<IList<int>>("memory_capacity"));
							
							data.raid = parseModelData.Get<string>("raid");
							
							data.hdd = PairData.FromListOfIntsToList(parseModelData.Get<IList<int>>("hdd"));
							var gpus = (parseModelData.Get<IList<string>>("gpu"));
							data.gpu = new List<string>();
							foreach (var gpu in gpus) {
								data.gpu.Add(gpu);
							}
							
							results.Add(data);
						}
						
						successAction(results);
					}
				}
			});
		}
	}

}