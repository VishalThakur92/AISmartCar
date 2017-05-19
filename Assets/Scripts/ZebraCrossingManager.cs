using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZebraCrossingManager : MonoBehaviour {

	[SerializeField]
	float TrafficLightTimer;

	[SerializeField]
	MeshRenderer TrafficLightMeshObj;

	public enum TrafficLight
	{
		red ,
		Green
	
	}



	public void Initialize(TrafficLight light){
		if (light == TrafficLight.red) {
			StartCoroutine (TrafficLightBehaviour ());

			StartCoroutine (PedestriansBehaviour ());
		}
		else {//Signal Green lights

			TrafficLightMeshObj.materials[1].mainTextureOffset= new Vector2(0 , 0);
			TrafficLightMeshObj.materials[2].mainTextureOffset= new Vector2(1 , 0);
		
		}
			
	}


	IEnumerator TrafficLightBehaviour(){
		//Material Index Ref - 
		//[1] = Red signal
		//[2] = Green signal
		//Signal Red Light for x seconds
		TrafficLightMeshObj.materials[1].mainTextureOffset = new Vector2(1 , 0);
		TrafficLightMeshObj.materials[2].mainTextureOffset= new Vector2(0 , 0);

		//Wait for Traffic Light Time
		yield return new WaitForSeconds(TrafficLightTimer);

		//Signal Green Lights
		TrafficLightMeshObj.materials[1].mainTextureOffset= new Vector2(0 , 0);
		TrafficLightMeshObj.materials[2].mainTextureOffset= new Vector2(1 , 0);

		//BroadCast the Green Traffic Light Signal
		Messenger.Broadcast (Events.OnTrafficLightGreenSignal);

		StopCoroutine (TrafficLightBehaviour());
	}




	IEnumerator PedestriansBehaviour(){
		//Wait For few Seconds Before The Pedestrians Start to cross the Road
		yield return new WaitForSeconds(5);

		//Spawn Pedestrians
		GameObject Pedestrians = Instantiate(Resources.Load("peds" , typeof(GameObject)) as GameObject);
		Pedestrians.transform.SetParent(transform);
		Pedestrians.transform.localPosition = new Vector3 (12, 0, 0);

		float Time = TrafficLightTimer - 5;

		while(Time > 0){
		//Pedestrians Cross the Road
			Pedestrians.transform.localPosition = Vector3.Lerp(Pedestrians.transform.localPosition,  new Vector3(-23.5f , 0, 0) , .0035f);
			Time -= 0.018f;
			yield return null;
		}


		//Pedestrians Have Crossed the Road
		//Destroy Pedestrians
		Destroy(Pedestrians.gameObject);

		StopCoroutine (PedestriansBehaviour());
	}


}
