// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Get an object data from the parse server who's objectId is already known.")]
	public class ParseObjectGetAsync : FsmStateAction
	{
		[RequiredField]
		public FsmString className;

		[RequiredField]
		public FsmString objectId;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;

		private Task<ParseObject> _task;

		public override void Reset()
		{
			className = null;
			objectId = null;

			successEvent = null;
			errorEvent = null;
		}
		


		public override string ErrorCheck()
		{
			if (! PlayMakerParseProxy.IsPropertyKeyValid(className.Value))
			{
				return "Parse Class must only contain alphanumeric or underscore characters, and must begin with a letter";
			}
			return "";
		}

		public override void OnEnter()
		{

			if (! PlayMakerParseProxy.IsPropertyKeyValid(className.Value))
			{
				LogWarning("Parse Class must only contain alphanumeric or underscore characters, and must begin with a letter");
				Fsm.Event(errorEvent);
				Finish();
				return;
			}

			_task = ParseObject.GetQuery(className.Value).GetAsync(objectId.Value);

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