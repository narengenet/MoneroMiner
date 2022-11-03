using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Proyecto26;
using UnityEngine.Networking;
using Models;


//[Serializable]
//public class User
//{
//	public int id;
//	public string name;
//	public string username;
//	public string email;
//	public string phone;
//	public string website;

//	public override string ToString()
//	{
//		//return UnityEngine.JsonUtility.ToJson(this, true);
//		return name + " " + email;
//	}
//}

public class TestController : MonoBehaviour
{
	private readonly string basePath = "https://localhost:7249";
	private RequestHelper currentRequest;

	private void LogMessage(string title, string message)
	{
#if UNITY_EDITOR
		EditorUtility.DisplayDialog (title, message, "Ok");
#else
		Debug.Log(message);
#endif
	}

	public void Get()
	{
		RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";

		var usersRoute = basePath + "/user/getall";
		//RestClient.Get<User>(usersRoute).Then(firstUser => {
		//	EditorUtility.DisplayDialog("JSON", JsonUtility.ToJson(firstUser, true), "Ok");
		//      }).Then(res =>
		//      {
		//	EditorUtility.DisplayDialog("JSON2", JsonUtility.ToJson(firstUser, true), "Ok");
		//});

		//RestClient.Get<User>(usersRoute).Then(res => {
		//	EditorUtility.DisplayDialog("Response", res.name, "Ok");
		//});

		RestClient.GetArray<User>(usersRoute).Then(res =>
		{
			this.LogMessage("Users", JsonHelper.ArrayToJsonString<User>(res, true));
		});


	}







	public void AbortRequest()
	{
		if (currentRequest != null)
		{
			currentRequest.Abort();
			currentRequest = null;
		}
	}

}
