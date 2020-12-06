let unityInstance;

const hexToRGB = (hex) => {
	// Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
	var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
	hex = hex.replace(shorthandRegex, function (_m, r, g, b) {
		return r + r + g + g + b + b;
	});

	var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
	return result ? {
		r: parseInt(result[1], 16) / 255,
		g: parseInt(result[2], 16) / 255,
		b: parseInt(result[3], 16) / 255
	} : null;
}

window.instantiateGame = () => {
	unityInstance = UnityLoader.instantiate("unityContainer", "Build/WebGL.json", { onProgress: UnityProgress });
}

window.initializeGame = () => {
	const name = localStorage.getItem("name");
	const colorHex = localStorage.getItem("color");
	const color = hexToRGB(colorHex);
	unityInstance.SendMessage("Field", "SetTeamNames", `{"TeamOneName": "${name}", "TeamTwoName": "TeamTwo"}`);
	unityInstance.SendMessage("Field", "SetTeamColors", `{"TeamOneColor": {"R": ${color.r}, "G": ${color.g}, "B": ${color.b}}, "TeamTwoColor": {"R": 0.65, "G": 0.65, "B": 0.93}}`);
}

window.disposeGame = () => {
	if (unityInstance) {
		unityInstance.Quit();
	}
}