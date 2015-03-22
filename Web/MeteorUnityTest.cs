using UnityEngine;
using System.Collections;
using Extensions;

public class Thing : MonoBehaviour
{

	public class DocumentType : Meteor.MongoDocument
	{
		public string title;
		public string director;
	}

	private GUIText guiText ;

	void Awake() {
		Application.logMessageReceived += HandleLog;
		Debug.Log("Whatup");
	}

	void Start ()
	{
		guiText = GetComponent<GUIText> ();
		StartCoroutine (MeteorExample ());
	}

	public void HandleLog (string logString, string stackTrace, LogType type)
	{
		guiText.text += logString + "\n";
	}

	IEnumerator MeteorExample ()
	{
		var production = false;
		
		var collection = Meteor.Collection<DocumentType>.Create ("movies");
		collection.DidAddRecord += (string id, DocumentType document) => {
			Debug.Log(document.Serialize ());
			//Debug.Log("hello");
        };

		// Connect to the meteor server. Yields when you're connected
		if (production) {
			yield return Meteor.Connection.Connect ("ws://gamestart.meteor.com:80/websocket");
		} else {
			yield return Meteor.Connection.Connect ("ws://127.0.0.1:3000/websocket");
		}

		yield return (Coroutine)Meteor.Subscription.Subscribe ("allMovies");

		yield return new WaitForSeconds (2);
		// Create a method call that returns a string
		var methodCall = Meteor.Method<string>.Call ("helloWorld");

		// // //yield return (Coroutine)methodCall;

		// Get the value returned by the method.
		//Debug.Log(methodCall.Response);
		Debug.Log("boom");
	}

	IEnumerator MeteorExampleOld ()
	{
		Debug.Log ("Starting...");
		
		var production = false;
		
		// Connect to the meteor server. Yields when you're connected
		if (production) {
			yield return Meteor.Connection.Connect ("wss://productionserver.com/websocket");
		} else {
			yield return Meteor.Connection.Connect ("ws://localhost:3000/websocket");
		}


		// Login
		//yield return Meteor.Accounts.LoginAsGuest ();

		/*
		// Create a collection
		var collection = Meteor.Collection<DocumentType>.Create ("todos");

		// Add some handlers
		collection.DidAddRecord += (string id, DocumentType document) => {
			Debug.Log(string.Format("Document added:\n{0}", document.Serialize()));
		};
		*/
		
		// Subscribe
		var subscription = Meteor.Subscription.Subscribe ("todos");
		// The convention to turn something into a connection is to cast it to a Coroutine
		yield return (Coroutine)subscription;

		/*
		
		// Create a method call that returns a string
		var methodCall = Meteor.Method<string>.Call ("getStringMethod", 1, 3, 4);
		
		// Execute the method. This will yield until all the database sideffects have synced.
		yield return (Coroutine)methodCall;
		
		// Get the value returned by the method.
		Debug.Log (string.Format ("Method response:\n{0}", methodCall.Response));

		*/
	}

}