// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Save an object online on Parse Server. set objectId ")]
	public class ParseObjectSaveAsync : FsmStateAction
	{

		public FsmString objectId;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			objectId = new FsmString {UseVariable =true};

			successEvent = null;
			errorEvent = null;
		}
		
		private Task _task;
		
		public override void OnEnter()
		{

			ParseObject _object;
			
			if (objectId.IsNone)
			{
				_object = ParseUser.CurrentUser;
			}else{
				_object = PlayMakerParseProxy.GetParseObject(objectId.Value);
			}

			if (_object==null)
			{
				Fsm.Event(errorEvent);
				Finish();
			}else{
				_task = _object.SaveAsync();
			}
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
				}
				
				Fsm.Event(errorEvent);
				Finish();
				
			}else if (_task.IsCompleted)
			{
				Fsm.Event(successEvent);
				Finish();
			}

		}
		
		
		
		
	}
}