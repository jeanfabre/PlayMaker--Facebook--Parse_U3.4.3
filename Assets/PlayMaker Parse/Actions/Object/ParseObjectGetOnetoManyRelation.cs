// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Get a one to many relation from a parse Object. Leave ObjectId to none to target the current user.")]
	public class ParseObjectGetOneToManyRelation : FsmStateAction
	{
		[RequiredField]
		public FsmString objectId;
		
		[RequiredField]
		public FsmString propertyKey;
		
		public FsmString relationObjectId;

		public FsmBool fetchIfNeeded;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;

		private Task<ParseObject> _task;

		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			propertyKey = null;
			relationObjectId = null;
			fetchIfNeeded = null;
			
			successEvent = null;
			errorEvent = null;
		}
		
		public override void OnEnter()
		{
			ParseObject _object;
			
			if (objectId.IsNone)
			{
				_object = ParseUser.CurrentUser;
			}else{
				_object = PlayMakerParseProxy.GetParseObject(objectId.Value);
			}

			bool ok = true;

			if (_object.ContainsKey(propertyKey.Value))
			{
				ParseObject _relationObject = _object.Get<ParseObject>(propertyKey.Value);

				if (_relationObject!=null)
				{
					relationObjectId.Value = PlayMakerParseProxy.CacheParseObject(_relationObject);
				}else{
					ok = false;
				}

				if (fetchIfNeeded.Value)
				{
					_task = _relationObject.FetchIfNeededAsync();
					return; // we are asynch
				}

			}else{
				ok = false;
			}


			if (!ok )
			{
				Fsm.Event(errorEvent);
			}else{
				Fsm.Event(successEvent);
			}
			
			Finish();
			
		}

		public override void OnUpdate()
		{
			if (_task.IsFaulted || _task.IsCanceled)
			{
				Fsm.Event(errorEvent);
				Finish();
			}
			
			if (_task.IsCompleted)
			{
				PlayMakerParseProxy.CacheParseObject(_task.Result);

				Fsm.Event(successEvent);
				Finish();
			}

		}
	}
}