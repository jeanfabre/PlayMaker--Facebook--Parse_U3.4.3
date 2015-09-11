// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Define Query result ordering.")]
	public class ParseObjectQueryOrderBy: FsmStateAction
	{
		[RequiredField]
		public FsmString queryId;
		
		[RequiredField]
		public FsmString orderBy;
		
		public FsmBool descending;
		
		public FsmString thenBy;

		public FsmBool thenByDescending;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			queryId = null;

			orderBy = null;
			descending = null;

			thenBy = new FsmString() {UseVariable=true};
			thenByDescending = null;

			successEvent = null;
			errorEvent = null;
		}
		
		public override string ErrorCheck()
		{
			if (! PlayMakerParseProxy.IsPropertyKeyValid(orderBy.Value))
			{
				return "Parse order by name must only contain alphanumeric or underscore characters, and must begin with a letter";
			}

			if (!thenBy.IsNone && !PlayMakerParseProxy.IsPropertyKeyValid(thenBy.Value))
			{
				return "Parse order by name must only contain alphanumeric or underscore characters, and must begin with a letter";
			}
			return "";
		}
		
		public override void OnEnter()
		{
			
			if (! PlayMakerParseProxy.IsPropertyKeyValid(orderBy.Value))
			{
				LogError("orderBy Class name invalid");
				Fsm.Event(errorEvent);
				Finish();
			}

			if (!thenBy.IsNone && ! PlayMakerParseProxy.IsPropertyKeyValid(thenBy.Value))
			{
				LogError("thenBy Class name invalid");
				Fsm.Event(errorEvent);
				Finish();
			}
			
			
			ParseQuery<ParseObject> _query = PlayMakerParseProxy.GetParseObjectQuery(queryId.Value);
			
			if (_query == null )
			{
				LogError("retrieving query failed");
				Fsm.Event(errorEvent);
				Finish();
			}
			
			if(descending.Value)
			{
				_query = _query.OrderBy(orderBy.Value);
			}else{
				_query = _query.OrderByDescending(orderBy.Value);
			}

			if (!thenBy.IsNone)
			{
				if(thenByDescending.Value)
				{
					_query = _query.ThenBy(thenBy.Value);
				}else{
					_query = _query.ThenByDescending(thenBy.Value);
				}
			}

			PlayMakerParseProxy.CacheParseObjectQuery(queryId.Value,_query);
			
			Fsm.Event(successEvent);
			Finish();
		}
		
	}
}