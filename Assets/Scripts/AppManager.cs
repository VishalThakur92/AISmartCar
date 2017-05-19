using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour {


	[SerializeField]
	ZebraCrossingManager ZebraCrossingManagerRef;

	[SerializeField]
	GameObject AICarTargetObj;

	[SerializeField]
	GameObject AICarObj;

	bool CanSpawnTrarfficLight = true;

	bool IsRedLight = false;

	// Use this for initialization
	void Start () {
		Messenger.AddListener (Events.OnTrafficLightGreenSignal, OnTrafficLightGreenSignal);
	}



	/// <summary>
	/// Spawns the zebra crossing module.
	/// </summary>
	void ZebraCrossingBehaviour(){

		if (IsRedLight) {
			//Detach the AITarget Obj which does = Reposisition the AI Car Target Obj to a static Position
			AICarTargetObj.transform.SetParent (null);
		}
		//Spawn The Zebra Crossing Prefab At a legit Location
		ZebraCrossingManagerRef.transform.position = new Vector3(0 , 0 , AICarTargetObj.transform.position.z);

		//Initialize Zebra Crossing Behaviour
		if(IsRedLight)
			ZebraCrossingManagerRef.Initialize(ZebraCrossingManager.TrafficLight.red);
		else
			ZebraCrossingManagerRef.Initialize(ZebraCrossingManager.TrafficLight.Green);
	}




	void OnGUI(){
		if (!CanSpawnTrarfficLight)
			return;

		if (GUILayout.Button ("Introduce Red Light")) {
			IsRedLight = true;
			ZebraCrossingBehaviour ();
			CanSpawnTrarfficLight = false;
		}
		else if (GUILayout.Button ("Introduce Green Light")) {
			IsRedLight = false;
			ZebraCrossingBehaviour ();
		}
	}


	//--------Event Callbacks--------------

	void OnTrafficLightGreenSignal(){
		if (IsRedLight) {
			//Reparent AITarget Obj as child of AICar which does = Maintains the COntinous Movement of the AI car
			AICarTargetObj.transform.SetParent (AICarObj.transform);

			//Reposition the AITargetObj at a specified Distance from the AIcar
			AICarTargetObj.transform.localPosition = new Vector3 (0, 0, 200);
			//Continue Driving the Car


			AICarObj.GetComponent<UnityStandardAssets.Vehicles.Car.CarController> ().ResetBrakes ();

			AICarObj.GetComponent<UnityStandardAssets.Vehicles.Car.CarAIControl> ().m_Driving = true;
		}
		CanSpawnTrarfficLight = true;
	}
}
