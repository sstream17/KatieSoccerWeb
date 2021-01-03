let unityInstance;

window.instantiateGame = (gameId) => {
	unityInstance = UnityLoader.instantiate("unityContainer", "Build/WebGL.json", { onProgress: UnityProgress });
}

window.initializeGame = () => {
	let gameId = localStorage.getItem("gameId");
	unityInstance.SendMessage("Field", "SetupSignalR", gameId);
}

window.disposeGame = () => {
	if (unityInstance) {
		unityInstance.Quit();
	}
}