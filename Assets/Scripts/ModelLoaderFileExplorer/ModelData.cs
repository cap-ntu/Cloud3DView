using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FullSerializer;


namespace ModelLoaderFileExplorer {

	public class ModelData {

		public string name;
		public bool isPrivate;
		public float unitSize;
		public int cpuCoresCount;
		public float cpuHz;
		public int hddBaysCount;
		public int memorySpeed;
		public List<PairData> memoryCapacity;
		public string raid;
		public List<PairData> hdd;
		public List<PairData> network;
		public List<string> gpu;
	
		public DateTime timeStamp;
		public float fileSize;
		public string details;

		public string modelFile;
		public string modelUrl;
		public string modelPath;
		public string objectId;
		public string username;


		private static readonly fsSerializer _serializer = new fsSerializer();


		public ModelData () {
			memoryCapacity = new List<PairData>();
			hdd = new List<PairData>();
			network = new List<PairData>();
			gpu = new List<string>();

			timeStamp = DateTime.Now;
		}

		public ModelData (string name,
		                  bool isPrivate,
		                  float unitSize,
		                  int cpuCoresCount,
		                  float cpuHz,
		                  int hddBaysCount,
		                  int memorySpeed,
		                  List<PairData> memoryCapacity,
		                  string raid,
		                  List<PairData> hdd,
		                  List<PairData> network,
		                  List<string> gpu,
		                  float fileSize,
		                  string details,
		                  DateTime timeStamp) {
			this.name = name;
			this.isPrivate = isPrivate;
			this.unitSize = unitSize;
			this.cpuCoresCount = cpuCoresCount;
			this.cpuHz = cpuHz;
			this.hddBaysCount = hddBaysCount;
			this.memorySpeed = memorySpeed;
			this.memoryCapacity = memoryCapacity;
			this.raid = raid;
			this.hdd = hdd;
			this.network = network;
			this.gpu = gpu;

			this.fileSize = fileSize;
			this.details = details;
			this.timeStamp = timeStamp;
		}

		public override string ToString () {
			string memoryCapacityString = GetListString<PairData>(memoryCapacity);
			string hddString = GetListString<PairData>(hdd);
			string networkString = GetListString<PairData>(network);
			string gpuString = GetListString<String>(gpu);
			return string.Format ("[ModelData] \n" +
			                      " Name: {0} \n" +
			                      " Unit Size: {1} \n" +
			                      " CPU Cores Count: {2} \n" +
			                      " CPU Hz: {3} \n" +
			                      " HDD Bays Count: {4} \n" +
			                      " Memory Speed: {5} \n" +
			                      " Memory Caacity: {6} \n" +
			                      " RAID: {7} \n" +
			                      " HDD: {8} \n" +
			                      " Network: {9} \n" +
			                      " GPU: {10} \n" +
			                      " \n" +
			                      " File Size: {11} \n" +
			                      " Details: {12} \n" +
			                      " Time Stamp: {13} \n" + 
			                      " Is Private: {14} \n",
			                      name,
			                      unitSize,
			                      cpuCoresCount,
			                      cpuHz,
			                      hddBaysCount,
			                      memorySpeed,
			                      memoryCapacityString,
			                      raid,
			                      hddString,
			                      networkString,
			                      gpuString, 
			                      fileSize,
			                      details,
			                      timeStamp.ToString(),
			                      isPrivate);
		}

		public void Serialize () {
			fsData data;
			_serializer.TrySerialize<ModelData>(this, out data).AssertSuccessWithoutWarnings();

			PersistanceManager.StoreLocalModelData(name, fsJsonPrinter.PrettyJson(data), modelPath);
		}

		public void MakeCloud () {
			CloudData.SaveInCloud(this, () => {
				Debug.Log("MakeCould: Success");
			},
			(string s) => {
				Debug.Log("MakeCould: Failed");
				Debug.Log("Exception: " + s);
			});
		}

		public void AddToUser () {
			objectId = null;

			CloudData.SaveInCloud(this, () => {
				Debug.Log("Save In Cloud: Success");
			}, 
			(string s) => {
				Debug.Log("Save In Cloud: Failed");
				Debug.Log("Exception: " + s);
			});
		}

		public static ModelData Deserialize (string content) {
			fsData data = fsJsonParser.Parse(content);
			ModelData modelData = null;
			_serializer.TryDeserialize(data, ref modelData).AssertSuccessWithoutWarnings();

			return modelData;
		}

		private static string GetListString<T> (List<T> objList) {
			string result = "";
			foreach (T obj in objList) {
				result += "  " + obj.ToString() + "\n";
			}
			return result;
		}
	}

	public struct PairData {
		public int count;
		public int data;

		public PairData (int count, int data) {
			this.count = count;
			this.data = data;
		}

		public override string ToString (){
			return count + " x " + data;
		}

		public static int[] ToArrayOfInts (List<PairData> datas) {
			var arr = new int[2 * datas.Count];
			for (int i=0; i<datas.Count; i++) {
				arr[2*i] = datas[i].count;
				arr[2*i+1] = datas[i].data;
			}
			return arr;
		}

		public static List<PairData> FromArrOfIntsToList (int[] arr) {
			var list = new List<PairData>();

			for (int i=0; i<arr.Length; i+=2) {
				int count = arr[i];
				int data = arr[i+1];
				list.Add(new PairData(count, data));
			}
			return list;
		}

		public static List<PairData> FromListOfIntsToList (IList<int> listInt) {
			var list = new List<PairData>();
			
			for (int i=0; i<listInt.Count; i+=2) {
				int count = listInt[i];
				int data = listInt[i+1];
				list.Add(new PairData(count, data));
			}
			return list;
		}
	}

}