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
            OnTurnReceived(message);
        });

        try
        {
            await connection.StartAsync();

            OnConnectionStarted("54321");  // TODO: Replace with GameId
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public async void SendMessage(string hubMethod, string hubMessage)
    {
        await connection.InvokeAsync(hubMethod, hubMessage);
    }

#elif UNITY_WEBGL

    [DllImport("__Internal")]
    private static extern void Connect(string url, string listener, Action<string> cnx, Action<string> msg);

    [DllImport("__Internal")]
    private static extern void Invoke(string method, string message);

    [MonoPInvokeCallback(typeof(Action<string>))]
    public static void ConnectionCallback(string message)
    {
        OnConnectionStarted(message);
    }

    [MonoPInvokeCallback(typeof(Action<string>))]
    public static void TurnCallback(string message)
    {
        OnTurnReceived(message);
    }

    public void Init(string hubUrl, string hubListener)
    {
        Connect(hubUrl, hubListener, ConnectionCallback, TurnCallback);
    }

    public void SendMessage(string hubMethod, string hubMessage)
    {
        Invoke(hubMethod, hubMessage);
    }

#endif

    public event EventHandler<DataEventArgs> TurnReceived;
    public event EventHandler<DataEventArgs> ConnectionStarted;

    private static void OnTurnReceived(string message)
    {
        var args = new DataEventArgs();
        args.Data = message;
        instance.TurnReceived?.Invoke(instance, args);
    }

    private static void OnConnectionStarted(string message)
    {
        var args = new DataEventArgs();
        args.Data = message;
        instance.ConnectionStarted?.Invoke(instance, args);
    }

}

public class DataEventArgs : EventArgs
{
    public string Data { get; set; }
}