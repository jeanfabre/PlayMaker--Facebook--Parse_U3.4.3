// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
using System;
using System.Threading.Tasks;

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Creates a parse object.")]
	public class ParseObjectCreate : FsmStateAction
	{
		[RequiredField]
		public FsmString className;

		public FsmString property;
		
		public FsmVar value;

		public bool useAcl;

		public FsmString AclOwnerObjectId;
		public FsmBool AclReadAccess;
		public FsmBool AclWriteAccess;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;

		[UIHint(UIHint.Variable)]
		public FsmString objectId;

		private Task _task;
		private ParseObject _object;

		public override void Reset()
		{
			className = null;

			property = null;
			value = null;

			useAcl = false;
			AclOwnerObjectId = new FsmString {UseVariable=true};
			AclReadAccess = false;
			AclWriteAccess = false;

			successEvent = null;
			errorEvent = null;

			objectId = null;
		}
		
		public override string ErrorCheck()
		{
			if (! PlayMakerParseProxy.IsPropertyKeyValid(className.Value))
			{
				return "Parse Class name must only contain alphanumeric or underscore characters, and must begin with a letter";
			}

			if (! PlayMakerParseProxy.IsPropertyKeyValid(property.Value))
			{
				return "Parse Property name must only contain alphanumeric or underscore characters, and must begin with a letter";
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

			_object = new ParseObject(className.Value);

			if (useAcl)
			{
				ParseUser _aclOwner;

				if (AclOwnerObjectId.IsNone)
				{
					_aclOwner = ParseUser.CurrentUser;
				}else{
					_aclOwner = (ParseUser)PlayMakerParseProxy.GetParseObject(AclOwnerObjectId.Value);
				}

				if (_aclOwner!=null)
				{
					_object.ACL = new ParseACL(_aclOwner)
					{
						PublicReadAccess = AclReadAccess.Value,
						PublicWriteAccess = AclWriteAccess.Value
					};

				}else{
					LogError("ACL Owner is null.If you target the current user, make sure a user is logged in");
					Fsm.Event(errorEvent);
					Finish();
				}

			}

			bool ok = PlayMakerParseProxy.SetParsePropertyFromFsmVar(_object,property.Value,this.Fsm,value);
			
			if (!ok )
			{
				LogError("Set parse property failed");
				Fsm.Event(errorEvent);
				Finish();
			}

			_task = _object.SaveAsync();
			
		}

		public override void OnUpdate()
		{
			
			if (_task.IsFaulted || _task.IsCanceled)
			{
				ParseException _e = PlayMakerParseProxy.GetParseException(_task.Exception);
				string _eMessage = "";
				if (_e!=null)
				{
					_eMessage = "ErrorCode:"+_e.Code+" ErrorMessage:"+_e.Message;
					Fsm.EventData.IntData = (int)_e.Code;
					Fsm.EventData.StringData = _e.Message;
					Fsm.Event(errorEvent);
					
				}else{
					Fsm.Event(errorEvent);
				}


				LogError("Parse SaveAsync failed :"+_eMessage);
			
				Finish();
			}
			
			if (_task.IsCompleted)
			{
				objectId.Value = _object.ObjectId;
				PlayMakerParseProxy.CacheParseObject(_object);
				Fsm.Event(successEvent);
				Finish();
			}
			
		}
	}
}