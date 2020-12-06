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

    public async void Init(string hubUrl)
    {
        connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        try
        {
            await connection.StartAsync();

            OnConnectionStarted("Connection Started");
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void AddListener(string hubListener)
    {
        connection.On<string>(hubListener, (message) =>
        {
            OnMessageReceived($"{message} from {hubListener}");
        });
    }

    public async void InvokeMethod(string hubMethod, string argument)
    {
        await connection.InvokeAsync(hubMethod, argument);
    }

#elif UNITY_WEBGL

    [DllImport("__Internal")]
    private static extern void Connect(string url, Action<string> callback);

    [DllImport("__Internal")]
    private static extern void AddHubListener(string method, Action<string> callback);

    [DllImport("__Internal")]
    private static extern void Invoke(string method, object argument);

    [MonoPInvokeCallback(typeof(Action<string>))]
    public static void ConnectionCallback(string message)
    {
        OnConnectionStarted(message);
    }

    [MonoPInvokeCallback(typeof(Action<string>))]
    public static void MessageCallback(string method)
    {
        OnMessageReceived(method);
    }

    public void Init(string hubUrl)
    {
        Connect(hubUrl, ConnectionCallback);
    }

    public void AddListener(string hubListener)
    {
        AddHubListener(hubListener, MessageCallback);
    }

    public void InvokeMethod(string hubMethod, string argument)
    {
        Invoke(hubMethod, argument);
    }

#endif

    public event EventHandler<MessageEventArgs> MessageReceived;
    public event EventHandler<MessageEventArgs> ConnectionStarted;

    private static void OnMessageReceived(string message)
    {
        var args = new MessageEventArgs
        {
            Message = message
        };

        instance.MessageReceived?.Invoke(instance, args);
    }

    private static void OnConnectionStarted(string message)
    {
        var args = new MessageEventArgs
        {
            Message = message
        };

        instance.ConnectionStarted?.Invoke(instance, args);
    }

}

public class MessageEventArgs : EventArgs
{
    public string Message { get; set; }
}
