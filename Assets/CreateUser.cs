using System.Threading.Tasks;

using UnityEngine;
using System.Collections;

using Parse;

public class CreateUser : MonoBehaviour {


	public string Name;
	public string Password;
	public string Email;


	void Start()
	{

	}
	void OnGUI()
	{

		if (GUILayout.Button("Sign up"))
		{
			SignUpUser();
		}

		if (GUILayout.Button("Sign in"))
		{
			SignInUser();
		}

	}

	void SignInUser()
	{
		Debug.Log("SignInUser");

		ParseUser user = null;
		ParseUser.LogInAsync(Email, Password).ContinueWith(
			t =>
			{
			  

			if (t.IsFaulted)
			{
				Debug.Log("failed");
			}

			user = t.Result;   

			Debug.Log("Your name is "+user.Get<string>("name"));


			return user.SaveAsync();
		}).Unwrap().ContinueWith(t =>
		                         {
			if (!t.IsFaulted)
			{
				// This succeeds, since this user was authenticated
				// on the device
				
				ParseUser.LogOut();
			}
		}).ContinueWith(t =>
		                {
			// Get the user from a non-authenticated method
			return ParseUser.Query.GetAsync(user.ObjectId);
		}).Unwrap().ContinueWith(t =>
		                         {
			user = t.Result;
			Debug.Log("Your name was "+user.Get<string>("name"));

		});


	}


	// Update is called once per frame
	void SignUpUser () {
	
		Debug.Log("SignUpUser");

		ParseUser user = new ParseUser()
		{
			Username = Email,
			Password = Password,
			Email = Email
		};

		user["name"] = Name;
		
		user.SignUpAsync().ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				// The signup failed. Check t.Exception to see why.
				Debug.Log("IS FAULTED : "+t.Exception.Message);
			}
			else if (t.IsCanceled)
			{
				Debug.Log("IS CANCELED : "+t.Exception.Message);
			}else{
				// signup was successful.
				Debug.Log("IS OK : ");
			}
		});
	}
}
