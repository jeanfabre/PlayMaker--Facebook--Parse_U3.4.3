// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Start a request procedure to reset a user's password. Requires the email of this user account.")]
	public class ParseUserRequestPasswordReset : FsmStateAction
	{
		
		public FsmString email;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			email = null;
			
			successEvent = null;
			errorEvent = null;
		}
		
		private Task _task;

		public override void OnEnter()
		{
			_task = ParseUser.RequestPasswordResetAsync(email.Value);
		}

		public override void OnUpdate()
		{
			
			if (_task.IsFaulted || _task.IsCanceled)
			{
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
				Fsm.Event(successEvent);
				Finish();
			}
		}
		
		
		
		
	}
}