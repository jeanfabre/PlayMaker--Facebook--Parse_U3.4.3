using System;
using System.Text.RegularExpressions;


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HutongGames.PlayMaker;

using Parse;

public class PlayMakerParseProxy : MonoBehaviour {


	static public Dictionary<string,ParseObject> LocalParseObjects;

	static public Dictionary<string,ParseQuery<ParseObject>> LocalParseObjectQueries;

	static public PlayMakerParseProxy _instance;


	public void Start()
	{
		_instance = this;

	}

	static public void ClearCache()
	{
		LocalParseObjects = new Dictionary<string, ParseObject>();
		LocalParseObjectQueries = new Dictionary<string, ParseQuery<ParseObject>>();
	}


	#region QUERIES
	static public ParseQuery<ParseObject> GetParseObjectQuery(string queryId)
	{
		
		if (LocalParseObjectQueries!=null && ! string.IsNullOrEmpty(queryId) )
		{
			return LocalParseObjectQueries[queryId];
		}
		
		return null;
	}
	
	static public bool CacheParseObjectQuery(string queryId, ParseQuery<ParseObject> parseQuery)
	{
		if (LocalParseObjectQueries==null)
		{
			LocalParseObjectQueries = new Dictionary<string, ParseQuery<ParseObject>>();
		}
		
		if (parseQuery!=null && ! string.IsNullOrEmpty(queryId))
		{
			LocalParseObjectQueries[queryId] = parseQuery;
		}else{
			return false;
		}
		
		return true;
	}
	
	static public void ForgetParseObjectQuery(string queryId)
	{
		if (LocalParseObjectQueries!=null && !string.IsNullOrEmpty(queryId))
		{
			LocalParseObjectQueries.Remove(queryId);
		}
	}

	#endregion QUERIES



	#region OBJECTS

	static public ParseObject GetParseObject(string objectId)
	{

		if (LocalParseObjects!=null && ! string.IsNullOrEmpty(objectId) )
		{
			return LocalParseObjects[objectId];
		}

		return null;
	}

	static public string CacheParseObject(ParseObject parseObject)
	{
		if (LocalParseObjects==null)
		{
			LocalParseObjects = new Dictionary<string, ParseObject>();
		}

		if (parseObject!=null && !string.IsNullOrEmpty(parseObject.ObjectId))
		{
			LocalParseObjects[parseObject.ObjectId] = parseObject;
		}else{
			return null;
		}

		return parseObject.ObjectId;
	}

	static public void ForgetParseObject(ParseObject parseObject)
	{
		if (LocalParseObjects!=null && parseObject!=null)
		{
			LocalParseObjects.Remove(parseObject.ObjectId);
		}
	}

	#endregion OBJECTS


	#region TOOLS

	// check that is starts with an alpha char
	static private Regex _nameValidationStartRegex = new Regex(@"^[a-zA-Z]");

	// check that is only contains alphanumeric or underscores
	static private Regex _nameValidationContentRegex =  new Regex(@"^[a-zA-Z0-9_]*$");

	static public bool IsPropertyKeyValid(string propertyKey )
	{
		// Must only contain alphanumeric or underscore characters, and must begin with a letter

		if (string.IsNullOrEmpty(propertyKey))
		{
			return false;
		}
		
		return  _nameValidationStartRegex.IsMatch(propertyKey) && _nameValidationContentRegex.IsMatch(propertyKey);
	}


	static public object GetParseValueFromFsmVar(Fsm fsm,FsmVar fsmVar)
	{
		PlayMakerUtils.RefreshValueFromFsmVar(fsm,fsmVar);
		
		if (fsmVar.Type == VariableType.Bool)
		{
			return fsmVar.boolValue;
			
		}else if (fsmVar.Type == VariableType.Float)
		{
			return fsmVar.floatValue;
			
		}else if (fsmVar.Type == VariableType.Int)
		{
			return fsmVar.intValue;
			
		}else if (fsmVar.Type == VariableType.String)
		{
			return fsmVar.stringValue;
			
		}else if (fsmVar.Type == VariableType.Color)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.colorValue);
		}else if (fsmVar.Type == VariableType.GameObject)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.gameObjectValue);
		}else if (fsmVar.Type == VariableType.Material)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.materialValue);
		}else if (fsmVar.Type == VariableType.Quaternion)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.quaternionValue);
		}else if (fsmVar.Type == VariableType.Rect)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.rectValue);
		}else if (fsmVar.Type == VariableType.Texture)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.textureValue);
		}else if (fsmVar.Type == VariableType.Vector2)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.vector2Value);
		}
		else if (fsmVar.Type == VariableType.Vector3)
		{
			return PlayMakerUtils.ParseValueToString(fsmVar.vector3Value);
		}

		return false;
	}

	static public bool SetParsePropertyFromFsmVar(ParseObject _object,string property,Fsm fsm,FsmVar fsmVar)
	{
		if (_object==null )
		{
			Debug.Log("Parse Object null");
			return false;
		}

		if (string.IsNullOrEmpty(property) )
		{
			Debug.Log("property null");
			return false;
		}

		if (! _object.ContainsKey(property))
		{
		//	return false;
		}

		PlayMakerUtils.RefreshValueFromFsmVar(fsm,fsmVar);
		
	
		if (fsmVar.Type == VariableType.Bool)
		{
			_object[property] = fsmVar.boolValue;

		}else if (fsmVar.Type == VariableType.Float)
		{
			_object[property] = fsmVar.floatValue;

		}else if (fsmVar.Type == VariableType.Int)
		{
			_object[property] = fsmVar.intValue;

		}else if (fsmVar.Type == VariableType.String)
		{
			_object[property] = fsmVar.stringValue;

		}else if (fsmVar.Type == VariableType.Color)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.colorValue);
		}else if (fsmVar.Type == VariableType.GameObject)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.gameObjectValue);
		}else if (fsmVar.Type == VariableType.Material)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.materialValue);
		}else if (fsmVar.Type == VariableType.Quaternion)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.quaternionValue);
		}else if (fsmVar.Type == VariableType.Rect)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.rectValue);
		}else if (fsmVar.Type == VariableType.Texture)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.textureValue);
		}else if (fsmVar.Type == VariableType.Vector2)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.vector2Value);
		}
		else if (fsmVar.Type == VariableType.Vector3)
		{
			_object[property] = PlayMakerUtils.ParseValueToString(fsmVar.vector3Value);
		}

		return true;
	}

	static public bool GetParsePropertyToFsmVar(ParseObject _object,string property,Fsm fsm,FsmVar fsmVar)
	{
		if (_object==null || string.IsNullOrEmpty(property) )
		{
			return false;
		}
		
		if (! _object.ContainsKey(property))
		{
			return false;
		}


		if (fsmVar.Type == VariableType.Bool)
		{
			PlayMakerUtils.ApplyValueToFsmVar(fsm,fsmVar,_object[property]);
			
		}else if (fsmVar.Type == VariableType.Float)
		{
			PlayMakerUtils.ApplyValueToFsmVar(fsm,fsmVar,_object[property]);
			
		}else if (fsmVar.Type == VariableType.Int)
		{
			PlayMakerUtils.ApplyValueToFsmVar(fsm,fsmVar,_object[property]);
			
		}else if (fsmVar.Type == VariableType.String)
		{
			PlayMakerUtils.ApplyValueToFsmVar(fsm,fsmVar,_object[property]);
			
		}else if (fsmVar.Type == VariableType.Color)
		{
			fsmVar.colorValue = (Color) PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}else if (fsmVar.Type == VariableType.GameObject)
		{
			fsmVar.gameObjectValue = (GameObject) PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}else if (fsmVar.Type == VariableType.Material)
		{
			fsmVar.materialValue = (Material)PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}else if (fsmVar.Type == VariableType.Quaternion)
		{
			fsmVar.quaternionValue = (Quaternion)PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}else if (fsmVar.Type == VariableType.Rect)
		{
			fsmVar.rectValue = (Rect)PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}else if (fsmVar.Type == VariableType.Texture)
		{
			fsmVar.textureValue = (Texture2D)PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}else if (fsmVar.Type == VariableType.Vector2)
		{
			fsmVar.vector2Value = (Vector2)PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}else if (fsmVar.Type == VariableType.Vector3)
		{
			fsmVar.vector3Value = (Vector3)PlayMakerUtils.ParseValueFromString( (string)_object[property] );
		}
		
		return true;
	}

	#endregion TOOLS

	#region EXCEPTIONS

	public static ParseException GetParseException(AggregateException e)
	{

		if(	e!= null && e is AggregateException 
		   	&& 
		   (((AggregateException)e).InnerExceptions.Count > 0 
		 	&& 
		 ((AggregateException)e).InnerExceptions[0] is ParseException))
		{
			AggregateException ae= (AggregateException)e;
			ParseException pe = (ParseException)ae.InnerExceptions[0];

			Debug.Log("error: ["+ae.InnerExceptions.Count + "]: " + pe.Code + "(" + (int)pe.Code + ") - " + pe.Message);

			return pe;
		}

		return null;
	}

	#endregion


}
