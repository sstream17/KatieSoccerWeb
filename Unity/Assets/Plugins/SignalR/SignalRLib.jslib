var SignalRLib = {

	$vars: {
		connection: null,
		handlerCallback: null,
		connectionCallback: null,
		invokeCallback: function (args, callback) {
			var sig = 'v';
			var messages = [];
			for (var i = 0; i < args.length; i++) {
				var message = args[i];
				var bufferSize = lengthBytesUTF8(message) + 1;
				var buffer = _malloc(bufferSize);
				stringToUTF8(message, buffer, bufferSize);
				messages.push(buffer);
				sig += 'i';
			}
			Runtime.dynCall(sig, callback, messages);
		}
	},

	InitJs: function (url) {
		var hubUrl = Pointer_stringify(url);

		vars.connection = new signalR.HubConnectionBuilder()
			.withUrl(hubUrl)
			.build();
	},

	AddHandlerJs: function (name, callback) {
		var handlerName = Pointer_stringify(name);
		vars.handlerCallback = callback;

		vars.connection.on(handlerName, function (payload) {
			vars.invokeCallback([handlerName, payload], vars.handlerCallback);
		});
	},

	ConnectJs: function (gameId, callback) {
		var gameIdString = Pointer_stringify(gameId);
		vars.connectionCallback = callback;

		var message = gameIdString;

		if (message === null) {
			message = vars.connection.connectionId;
		}

		vars.connection.start()
			.then(function () {
				vars.invokeCallback([message], vars.connectionCallback);
			}).catch(function (err) {
				return console.error(err.toString());
			});
	},

	SendToHubJs: function (method, payload) {
		var hubMethod = Pointer_stringify(method);
		var hubPayload = Pointer_stringify(payload);

		vars.connection.invoke(hubMethod, hubPayload)
			.catch(function (err) {
				return console.error(err.toString());
			});
	},

	Ready: function () {
		initializeGame();
	}
};

autoAddDeps(SignalRLib, '$vars');
mergeInto(LibraryManager.library, SignalRLib);
