// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("log in a User")]
	public class ParseUserLogIn : FsmStateAction
	{

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString email;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString password;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool isLoggedIn;

		
		public override void Reset()
		{
			email = null;
			password = null;

			successEvent = null;
			errorEvent = null;

			isLoggedIn = null;
		}

		private Task _task;

		public override void OnEnter()
		{
			_task = ParseUser.LogInAsync(email.Value, password.Value);
		}

		public override void OnUpdate()
		{

			if (_task.IsFaulted || _task.IsCanceled)
			{
				isLoggedIn.Value = false;

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
				isLoggedIn.Value = true;
				Fsm.Event(successEvent);
				Finish();
			}
		}

		

		
	}
}