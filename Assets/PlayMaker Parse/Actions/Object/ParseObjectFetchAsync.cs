// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Fetch an object with the data on Parse Server. Set objectId to none to target the current user. Use fetchOnlyIfNeeded to fetch only if data is available")]
	public class ParseObjectFetchAsync : FsmStateAction
	{
		public FsmString className;
		public FsmString objectId;

		public FsmBool fetchOnlyIfNeeded;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			className = null;
			objectId = null;

			fetchOnlyIfNeeded = true;

			successEvent = null;
			errorEvent = null;
		}
		
		private Task<ParseObject> _task;
		
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
				if (fetchOnlyIfNeeded.Value)
				{
					_task = _object.FetchIfNeededAsync();
				}else{
					_task = _object.FetchAsync();
				}
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
				PlayMakerParseProxy.CacheParseObject(_task.Result);

				Fsm.Event(successEvent);
				Finish();
			}

		}
		
		
		
		
	}
}