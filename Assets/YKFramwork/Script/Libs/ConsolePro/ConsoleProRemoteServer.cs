using UnityEngine;
using System;
using System.Net;
using System.Collections.Generic;

namespace FlyingWormConsole3
{
public class ConsoleProRemoteServer : MonoBehaviour
{
	#if !NETFX_CORE && !UNITY_WEBPLAYER && !UNITY_WP8 && !UNITY_METRO
	public class HTTPContext
	{
		public HttpListenerContext context;
		public string path;

		public string Command
		{
			get
			{
				return WWW.UnEscapeURL(context.Request.Url.AbsolutePath);
			}
		}

		public HttpListenerRequest Request
		{
			get
			{
				return context.Request;
			}
		}

		public HttpListenerResponse Response
		{
			get
			{
				return context.Response;
			}
		}

		public HTTPContext(HttpListenerContext inContext)
		{
			context = inContext;
		}

		public void RespondWithString(string inString)
		{
			Response.StatusDescription = "OK";
			Response.StatusCode = (int)HttpStatusCode.OK;

			if (!string.IsNullOrEmpty(inString))
			{
				Response.ContentType = "text/plain";

				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(inString);
				Response.ContentLength64 = buffer.Length;
				Response.OutputStream.Write(buffer,0,buffer.Length);
			}
		}
	}

	[System.SerializableAttribute]
	public class QueuedLog
	{
		public string message;
		public string stackTrace;
		public LogType type;
	}

	public int port = 51000;

	private static HttpListener listener = new HttpListener();
	
	[NonSerializedAttribute]
	public List<QueuedLog> logs = new List<QueuedLog>();

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		Debug.Log("Starting Console Pro Server on port : " + port);
		listener.Prefixes.Add("http://*:"+port+"/");
		listener.Start();
		listener.BeginGetContext(ListenerCallback, null);
	}

	#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6

	void OnEnable()
	{
		Application.RegisterLogCallback(LogCallback);
	}

	void Update()
	{
		Application.RegisterLogCallback(LogCallback);
	}

	void LateUpdate()
	{
		Application.RegisterLogCallback(LogCallback);
	}

	void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	#else

	void OnEnable()
	{
		Application.logMessageReceived += LogCallback;
	}

	void OnDisable()
	{
		Application.logMessageReceived -= LogCallback;
	}

	#endif

	public void LogCallback(string logString, string stackTrace, LogType type)
	{
		if(!logString.StartsWith("CPIGNORE"))
		{
			QueueLog(logString, stackTrace, type);
		}
	}

	void QueueLog(string logString, string stackTrace, LogType type)
	{
            Debug.Log("--------------");
		logs.Add(new QueuedLog() { message = logString, stackTrace = stackTrace, type = type } );
	}

	void ListenerCallback(IAsyncResult result)
	{
		HTTPContext context = new HTTPContext(listener.EndGetContext(result));

		HandleRequest(context);
		
		listener.BeginGetContext(new AsyncCallback(ListenerCallback), null);
	}

	void HandleRequest(HTTPContext context)
	{
		// Debug.LogError("HANDLE " + context.Request.HttpMethod);
		// Debug.LogError("HANDLE " + context.path);

		bool foundCommand = false;

		switch(context.Command)
		{
			case "/NewLogs":
			{
				foundCommand = true;

				if(logs.Count > 0)
				{
					string response = "";

					//  foreach(QueuedLog cLog in logs)
					for(int i = 0; i < logs.Count; i++)
					{					
						QueuedLog cLog = logs[i];
						response += "::::" + cLog.type;
						response += "||||" + cLog.message;
						response += ">>>>" + cLog.stackTrace + ">>>>";
					}

					context.RespondWithString(response);

					logs.Clear();
				}
				else
				{
					context.RespondWithString("");
				}
				break;
			}
		}

		if(!foundCommand)
		{
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			context.Response.StatusDescription = "Not Found";
		}

		context.Response.OutputStream.Close();
	}
	#endif
}
}