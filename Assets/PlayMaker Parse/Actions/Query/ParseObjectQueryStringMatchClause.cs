// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Define Query clause on strings.")]
	public class ParseObjectQueryStringMatchClause: FsmStateAction
	{
		public enum ParseStringMatchClauses {Contains,StartsWith,EndsWith,Matches};
		
		[RequiredField]
		public FsmString queryId;

		public FsmString propertyKey;
		public ParseStringMatchClauses where;
		public FsmString propertyValue;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			queryId = null;
			
			where = ParseStringMatchClauses.Contains;
			
			propertyKey = null;
			propertyValue = null;
			
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
			
			if (where == ParseStringMatchClauses.Contains)
			{
				_query = _query.WhereContains(propertyKey.Value,propertyValue.Value);
			}else if (where == ParseStringMatchClauses.StartsWith)
			{
				_query = _query.WhereStartsWith(propertyKey.Value,propertyValue.Value);
			}else if (where == ParseStringMatchClauses.EndsWith)
			{
				_query = _query.WhereEndsWith(propertyKey.Value,propertyValue.Value);
			}else if (where == ParseStringMatchClauses.Matches)
			{
				_query = _query.WhereMatches(propertyKey.Value,propertyValue.Value,null);
			}
			
			PlayMakerParseProxy.CacheParseObjectQuery(queryId.Value,_query);
			
			Fsm.Event(successEvent);
			Finish();
		}
		
	}
}