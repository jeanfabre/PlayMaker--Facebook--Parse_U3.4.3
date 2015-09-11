// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Creates a query for a given parse object.")]
	public class ParseObjectCreateQuery: FsmStateAction
	{
		[RequiredField]
		public FsmString queryId;

		[RequiredField]
		public FsmString className;

		public FsmString include;

		public FsmInt limit;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;

		public override void Reset()
		{
			className = null;
			include = new FsmString() {UseVariable=true};
			queryId = "MyQuery";
			limit = new FsmInt() {UseVariable=true};

			successEvent = null;
			errorEvent = null;
		}
		
		public override string ErrorCheck()
		{
			if (! PlayMakerParseProxy.IsPropertyKeyValid(className.Value))
			{
				return "Parse Class name must only contain alphanumeric or underscore characters, and must begin with a letter";
			}

			return "";
		}
		
		public override void OnEnter()
		{
			
			if (! PlayMakerParseProxy.IsPropertyKeyValid(className.Value))
			{
				LogError("Class name invalid");
				Fsm.Event(errorEvent);
				Finish();
			}


			ParseQuery<ParseObject> _query = ParseObject.GetQuery(className.Value);

			if (_query == null )
			{
				LogError("Set parse property failed");
				Fsm.Event(errorEvent);
				Finish();
			}

			if (! limit.IsNone)
			{
				_query = _query.Limit(limit.Value);
			}
			 
			if (!string.IsNullOrEmpty(include.Value))
			{
				_query = _query.Include(include.Value);
			}


			bool ok = PlayMakerParseProxy.CacheParseObjectQuery(queryId.Value,_query);

			if (!ok)
			{
				LogError("Parse Query could not be cached");
				Fsm.Event(errorEvent);
				Finish();
			}

			Fsm.Event(successEvent);
			Finish();
		}

	}
}