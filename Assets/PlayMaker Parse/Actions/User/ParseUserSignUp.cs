// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Sign up a User. Leave Username to none if you want to use the email as the username.")]
	public class ParseUserSignUp : FsmStateAction
	{
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString email;

		[UIHint(UIHint.Variable)]
		public FsmString username;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString password;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool isSignedUp;
		
		public override void Reset()
		{
			email = null;
			username = new FsmString {UseVariable=true};
			password = null;
			
			successEvent = null;
			errorEvent = null;
			
			isSignedUp = null;
		}
		
		private Task _task;
		
		public override void OnEnter()
		{
			ParseUser user = new ParseUser()
			{
				Username = username.IsNone ? email.Value : username.Value,
				Password = password.Value,
				Email = email.Value
			};
		
			_task = user.SignUpAsync();
		}
		
		public override void OnUpdate()
		{
			
			if (_task.IsFaulted || _task.IsCanceled)
			{
				isSignedUp.Value = false;

				ParseException _e = PlayMakerParseProxy.GetParseException(_task.Exception);
				if (_e!=null)
				{
					Fsm.EventData.IntData = (int)_e.Code;
					Fsm.EventData.StringData = _e.Message;
					Fsm.Event(errorEvent);
				
				}else{
					Fsm.Event(errorEvent);
				}
				Finish();

			}else if (_task.IsCompleted)
			{
				isSignedUp.Value = true;
				Fsm.Event(successEvent);
				Finish();
			}

		}
		
	}
}