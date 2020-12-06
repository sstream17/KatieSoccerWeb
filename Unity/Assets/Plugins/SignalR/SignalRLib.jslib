var SignalRLib = {

	$vars: {
		connection: null,
		cnxCallback: null,
		msgCallback: null,
		sendMessage: function (message, callback) {
			var bufferSize = lengthBytesUTF8(message) + 1;
			var buffer = _malloc(bufferSize);
			stringToUTF8(message, buffer, bufferSize);
			Runtime.dynCall('vi', callback, [buffer]);
		}
	},

	Connect: function (url, callback) {
		var hubUrl = Pointer_stringify(url);

		vars.cnxCallback = callback;

		vars.connection = new signalR.HubConnectionBuilder()
			.withUrl(hubUrl)
			.build();

		vars.connection.start()
			.then(function () {
				vars.sendMessage('Connection Started', vars.cnxCallback);
			}).catch(function (err) {
				return console.error(err.toString());
			});
	},

	AddHubListener: function (listener, callback) {
		var hubListener = Pointer_stringify(listener);

		vars.connection.on(hubListener, function (message) {
			vars.sendMessage(message, callback);
		});
	},

	Invoke: function (method, message) {
		var hubMethod = Pointer_stringify(method);
		var hubMessage = Pointer_stringify(message);

		vars.connection.invoke(hubMethod, hubMessage)
			.catch(function (err) {
				return console.error(err.toString());
			});
	}

};

autoAddDeps(SignalRLib, '$vars');
mergeInto(LibraryManager.library, SignalRLib);
