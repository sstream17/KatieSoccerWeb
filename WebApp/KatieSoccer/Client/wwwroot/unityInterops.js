let unityInstance;

window.instantiateGame = (gameId) => {
	unityInstance = UnityLoader.instantiate("unityContainer", "Build/WebGL.json", { onProgress: UnityProgress });
	unityInstance.SendMessage("Field", "SetGameId", gameId);
}

window.initializeGame = () => {
	const name = localStorage.getItem("name");
	const color = localStorage.getItem("color");
	const gameData = {
		PlayerOne: {
			IsLocal: true,
			Name: name,
			Color: color
		},
		PlayerTwo: {
			IsLocal: true,
			Name: "Player 2",
			Color: "#42147B"
		}
	};
	unityInstance.SendMessage("Field", "InitializeGame", JSON.stringify(gameData));
}

window.disposeGame = () => {
	if (unityInstance) {
		unityInstance.Quit();
	}
}