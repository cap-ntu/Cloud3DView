using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using ModelLoaderFileExplorer;


public class TestSerialization : MonoBehaviour {

	// Use this for initialization
	void Start () {



		fsSerializer serializer = new fsSerializer();

		List<PairData> memoryCapacity =  new List<PairData> () {
			new PairData(6, 16), 
			new PairData(4, 8)};
		List<PairData> hdd =  new List<PairData> () {
			new PairData(4, 4), 
			new PairData(4, 8)};
		List<PairData> network =  new List<PairData> () {
			new PairData(2, 10), 
			new PairData(2, 1)};
		List<string> gpu =  new List<string> () {
			"GPU A",
			"GPU B",
			"GPU C"};

		ModelData modelData = new ModelData("Test Model",
		                                    true,
		                                    2,
		                                    4,
		                                    1000,
		                                    8,
		                                    1333,
		                                    memoryCapacity,
		                                    "RAID 0",
		                                    hdd,
		                                    network,
		                                    gpu,
		                                    3,
		                                    "Just a test",
		                                    System.DateTime.Now);

		fsData data;
		serializer.TrySerialize(modelData.GetType(), modelData, out data);

		string dataString = fsJsonPrinter.PrettyJson(data);
		data = fsJsonParser.Parse(dataString);

		Debug.Log(dataString);
		Debug.Log(modelData.ToString());

		object deserialized = null;
		serializer.TryDeserialize(data, typeof(ModelData), ref deserialized);

		ModelData newModelData = (ModelData) deserialized;
		Debug.Log(newModelData.ToString());

		PersistanceManager.StoreLocalModelData("Test Model", dataString, null);

	}
}
	
