function GetDialogArguments() {
	return getRadWindow().ClientParameters;
}

function getRadWindow() {
	if (window.radWindow) {
		return window.radWindow;
	}

	if (window.frameElement && window.frameElement.radWindow) {
		return window.frameElement.radWindow;
	}

	return null;
}

var isRadWindow = true;

var radWindow = getRadWindow();

if (radWindow) {
	if (window.dialogArguments) {
		radWindow.Window = window;
	}
}

function scClose(media) {
	var returnValue = {
		media: media
	};

	getRadWindow().close(returnValue);

}

function scCancel() {
	getRadWindow().close();
}

function scNext() {
	var editorOptionsContainer = new EditorOptions();
	if (document.getElementById('ShowPlaylistHead').value != '') {
		document.getElementById('VideoHeadContainer').style.display = 'none';
		document.getElementById('PlaylistHeadContainer').style.display = 'block';
	} else {
		document.getElementById('VideoHeadContainer').style.display = 'block';
		document.getElementById('PlaylistHeadContainer').style.display = 'none';
	}
}

var AspectPreserver = function () {
	console.info('AspectPreserver called');
	var reload = function () {
		if ($("Width")) {
			$("Width").tainted = true;
		}
		this.retryCount = 0;
		this.hookEvents();
	};

	var hookEvents = function () {
		if ((!$("Width") || $("Width").tainted) && this.retryCount < 10) {
			this.retryCount++;
			setTimeout(this.hookEvents.bind(this), 50);
			return;
		}
		else if (this.retryCount >= 10) {
			console.warn("retry limit exceeded, bailing out");
			return;
		}

		this._originalWidth = $F($("Width"));
		this._originalHeight = $F($("Height"));

		$("Width").observe("blur", this.onWidthChange.bind(this));
		$("Height").observe("blur", this.onHeightChange.bind(this));
	};

	var onWidthChange = function () {
		var width = parseInt($F($("Width")));
		$("Height").value = Math.round(width / this._originalWidth * this._originalHeight);
	};

	var onHeightChange = function () {
		var height = parseInt($F($("Height")));
		$("Width").value = Math.round(height / this._originalHeight * this._originalWidth);
	}
}

function UpdateShortcode(key, value) {
	var shortcodeInput = document.getElementById('ShortcodeInput');

	if (shortcodeInput.value.includes(key)) {
		shortcodeInput.value = shortcodeInput.value.replace(key + '=\'' + /[^(?!.*(\'|\"))]/ + '\'', key + '=\'' + value + '\'');
	}
}
function UpdateHeight(widthInput, widthAspect, heightInput, heightAspect) {
	heightInput.value = Math.ceil((widthInput.value / widthAspect) * heightAspect);
}
function CalculateDimensions(aspectRatioList, widthInput, heightInput) {
	var widthHeight = aspectRatioList.value.split(':');
	var width = '';
	var height = '';
	if (widthHeight.length >= 2) {
		width = widthHeight[0];
		height = widthHeight[1];
	}
	heightInput.disabled = aspectRatioList.value !== 'Custom';
	if (width !== '' && height !== '') {
		UpdateHeight(widthInput, width, heightInput, height);
	}
}
function SetRadioSelection(input, group) {
	for (var i = 0; i < group.length; i++) {
		if (group[i].checked)
			input.value = group[i].value;
	}
}

var EditorOptions = function () {
	var aspectRatioSelect = document.getElementById('AspectRatioList');
	var widthInput = document.getElementById('WidthInput');
	var heightInput = document.getElementById('HeightInput');
	var embedInput = document.getElementById('EmbedInput');
	var sizingInput = document.getElementById('SizingInput');

	var embedRadiogroup = document.getElementsByName("EmbedRadiogroup");
	var sizingRadiogroup = document.getElementsByName("SizingRadiogroup");

	for (var i = 0; i < embedRadiogroup.length; i++) {
		embedRadiogroup[i].addEventListener("change", function () { SetRadioSelection(embedInput, embedRadiogroup); }, false);
	}
	for (var i = 0; i < sizingRadiogroup.length; i++) {
		sizingRadiogroup[i].addEventListener("change", function () { SetRadioSelection(sizingInput, sizingRadiogroup); }, false);
	}
	if (aspectRatioSelect !== null && widthInput !== null && heightInput !== null) {
		aspectRatioSelect.addEventListener("change", function () { CalculateDimensions(aspectRatioSelect, widthInput, heightInput); }, false);
		widthInput.addEventListener("change keyup", function () { CalculateDimensions(aspectRatioSelect, widthInput, heightInput); }, false);
	}
}

var scAspectPreserver = new AspectPreserver();