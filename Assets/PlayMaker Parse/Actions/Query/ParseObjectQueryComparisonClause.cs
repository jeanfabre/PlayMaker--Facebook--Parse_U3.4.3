// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Define Query comparison clause.")]
	public class ParseObjectQueryComparisonClause: FsmStateAction
	{
		public enum ParseComparisonClauses {EqualTo,NotEqualTo,LessThan,LessThanOrEqualTo,GreaterThan,GreaterThanOrEqualTo};

		[RequiredField]
		public FsmString queryId;

		public FsmString propertyKey;
		public ParseComparisonClauses where;
		public FsmVar propertyValue;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			queryId = null;
			
			where = ParseComparisonClauses.EqualTo;

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
			
			if (where == ParseComparisonClauses.EqualTo)
			{
				_query = _query.WhereEqualTo(propertyKey.Value,PlayMakerParseProxy.GetParseValueFromFsmVar(this.Fsm,propertyValue));
			}else if (where == ParseComparisonClauses.NotEqualTo)
			{
				_query = _query.WhereNotEqualTo(propertyKey.Value,PlayMakerParseProxy.GetParseValueFromFsmVar(this.Fsm,propertyValue));
			}else if (where == ParseComparisonClauses.LessThan)
			{
				_query = _query.WhereLessThan(propertyKey.Value,PlayMakerParseProxy.GetParseValueFromFsmVar(this.Fsm,propertyValue));
			}else if (where == ParseComparisonClauses.LessThanOrEqualTo)
			{
				_query = _query.WhereLessThanOrEqualTo(propertyKey.Value,PlayMakerParseProxy.GetParseValueFromFsmVar(this.Fsm,propertyValue));
			}else if (where == ParseComparisonClauses.GreaterThan)
			{
				_query = _query.WhereGreaterThan(propertyKey.Value,PlayMakerParseProxy.GetParseValueFromFsmVar(this.Fsm,propertyValue));
			}else if (where == ParseComparisonClauses.GreaterThanOrEqualTo)
			{
				_query = _query.WhereGreaterThanOrEqualTo(propertyKey.Value,PlayMakerParseProxy.GetParseValueFromFsmVar(this.Fsm,propertyValue));
			}

			PlayMakerParseProxy.CacheParseObjectQuery(queryId.Value,_query);
			
			Fsm.Event(successEvent);
			Finish();
		}
		
	}
}