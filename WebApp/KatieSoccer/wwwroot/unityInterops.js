var unityInstance;

window.instantiateGame = () => {
	unityInstance = UnityLoader.instantiate("unityContainer", "Build/WebGL.json", { onProgress: UnityProgress });
}

window.initializeGame = () => {
	unityInstance.SendMessage("Field", "SetTeamNames", '{"TeamOneName": "TeamOne", "TeamTwoName": "TeamTwo"}');
	unityInstance.SendMessage("Field", "SetTeamColors", '{"TeamOneColor": {"R": 1.0, "G": 0.69, "B": 0.56}, "TeamTwoColor": {"R": 0.65, "G": 0.65, "B": 0.93}}');
}