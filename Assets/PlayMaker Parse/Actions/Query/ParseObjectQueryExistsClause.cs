// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Define Query match clause.")]
	public class ParseObjectQueryExistsClause: FsmStateAction
	{
		public enum ParseExistsClauses {Exists,DoesNotExist};
		
		[RequiredField]
		public FsmString queryId;

		public ParseExistsClauses where;
		public FsmString propertyKey;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			queryId = null;
			
			where = ParseExistsClauses.Exists;
			
			propertyKey = null;
			
			successEvent = null;
			errorEvent = null;
		}
		
		public override string ErrorCheck()
		{
			if (! PlayMakerParseProxy.IsPropertyKeyValid(propertyKey.Value))
			{
				return "Parse Property must only contain alphanumeric or underscore characters, and must begin with a letter";
			}
			return "";
		}
		
		public override void OnEnter()
		{
			
			if (! PlayMakerParseProxy.IsPropertyKeyValid(propertyKey.Value))
			{
				LogError("Property Key name invalid");
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
			
			if (where == ParseExistsClauses.Exists)
			{
				_query = _query.WhereExists (propertyKey.Value);
			}else if (where == ParseExistsClauses.DoesNotExist)
			{
				_query = _query.WhereDoesNotExist(propertyKey.Value);
			}
			
			PlayMakerParseProxy.CacheParseObjectQuery(queryId.Value,_query);
			
			Fsm.Event(successEvent);
			Finish();
		}
		
	}
}