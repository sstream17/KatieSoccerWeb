using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR.Client;
using AOT;

public class SignalRLib
{

    private static SignalRLib instance;

    public SignalRLib()
    {
        instance = this;
    }

#if UNITY_EDITOR

    private HubConnection connection;

    public async void Init(string hubUrl, string hubListener)
    {
        connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        connection.On<string>(hubListener, (message) =>
        {
            OnMessageReceived(hubListener, message);
        });

        try
        {
            await connection.StartAsync();

            OnConnectionStarted(hubListener, "Connection Started");
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public async void InvokeMethod(string hubMethod, object argument)
    {
        await connection.InvokeAsync(hubMethod, argument);
    }

#elif UNITY_WEBGL

    [DllImport("__Internal")]
    private static extern void Connect(string url, Action<string, object> callback);

    [DllImport("__Internal")]
    private static extern void AddListener(string method, Action<string, object> callback);

    [DllImport("__Internal")]
    private static extern void Invoke(string method, object argument);

    [MonoPInvokeCallback(typeof(Action<string, object>))]
    public static void ConnectionCallback(string method, object argument)
    {
        OnConnectionStarted(method, argument);
    }

    [MonoPInvokeCallback(typeof(Action<string, object>))]
    public static void MessageCallback(string method, object argument)
    {
        OnMessageReceived(method, argument);
    }

    public void Init(string hubUrl, string hubListener)
    {
        Connect(hubUrl, ConnectionCallback);
    }

    public void InvokeMethod(string hubMethod, object argument)
    {
        Invoke(hubMethod, argument);
    }

#endif

    public event EventHandler<MessageEventArgs> MessageReceived;
    public event EventHandler<MessageEventArgs> ConnectionStarted;

    private static void OnMessageReceived(string method, object argument)
    {
        var args = new MessageEventArgs();
        args.Method = method;
        args.Argument = argument;
        instance.MessageReceived?.Invoke(instance, args);
    }

    private static void OnConnectionStarted(string method, object argument)
    {
        var args = new MessageEventArgs();
        args.Method = method;
        args.Argument = argument;
        instance.ConnectionStarted?.Invoke(instance, args);
    }

}

public class MessageEventArgs : EventArgs
{
    public string Method { get; set; }
    public object Argument { get; set; }
}
