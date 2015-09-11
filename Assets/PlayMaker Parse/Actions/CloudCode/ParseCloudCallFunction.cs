// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Call a function on Cloud Code")]
	public class ParseCloudCallFunction : FsmStateAction
	{
		
		public FsmString function;


		public FsmString result;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			function = "hello";
			successEvent = null;
			errorEvent = null;
		}
		
		private  Task<string> _task;
		
		public override void OnEnter()
		{
			_task = ParseCloud.CallFunctionAsync<string>(function.Value, new Dictionary<string, object>());
		}


		public override void OnUpdate()
		{
			
			if (_task.IsFaulted || _task.IsCanceled)
			{
				Fsm.Event(errorEvent);
				Finish();
			}else if (_task.IsCompleted)
			{

				string _result = _task.Result;

				this.result.Value = _result;

				Fsm.Event(successEvent);
				Finish();
			}
			
		}

		
		
		
	}
}