// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Execute a query and exactly one result. ")]
	public class ParseObjectQueryFirstAsync : FsmStateAction
	{
		[RequiredField]
		public FsmString queryId;
		
		public FsmBool firstOrDefault;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString resultObjectId;

		public FsmEvent successEvent;
		public FsmEvent noResultEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			queryId = "";
			
			firstOrDefault = false;
			
			successEvent = null;
			noResultEvent = null;
			errorEvent = null;
		}
		
		private Task<ParseObject> _task;
		
		public override void OnEnter()
		{

			ParseQuery<ParseObject> _query =  PlayMakerParseProxy.GetParseObjectQuery(queryId.Value);

			if (_query==null)
			{
				Fsm.Event(errorEvent);
				Finish();
			}else{
				if (firstOrDefault.Value)
				{
					_task = _query.FirstOrDefaultAsync();
				}else{
					_task = _query.FirstAsync();
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
					if (_e.Code == ParseException.ErrorCode.ObjectNotFound )
					{
						Fsm.Event(noResultEvent);
					}else{

						LogWarning(_e.Code+" "+_e.Message);

						Fsm.EventData.IntData = (int)_e.Code;
						Fsm.EventData.StringData = _e.Message;
						Fsm.Event(errorEvent);
					}
				}else{
					Fsm.Event(errorEvent);
				}
				Finish();

			}else if (_task.IsCompleted)
			{

				resultObjectId.Value = PlayMakerParseProxy.CacheParseObject(_task.Result);
				
				Fsm.Event(successEvent);
				Finish();
			}

		}
		
		
		
		
	}
}