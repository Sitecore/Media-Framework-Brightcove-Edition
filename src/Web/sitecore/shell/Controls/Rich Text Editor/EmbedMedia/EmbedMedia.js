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
		media:media
	};
	
	getRadWindow().close(returnValue);

}

function scCancel() {
  getRadWindow().close();
}

var AspectPreserver = function () {
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
		shortcodeInput.value = shortcodeInput.value.replace(key + '=\'' + /[^(?!.*(\'|\"))]/  + '\'', key + '=\'' + value + '\'');
	}
}
function UpdateHeight(widthInput, widthAspect, heightInput, heightAspect) {
	heightInput.value = (widthInput.value / widthAspect) * heightAspect;
}
function UpdateAspectRatio(aspectRatioList, widthInput, heightInput) {
	var widthHeight = aspectRatioList.value.split(':');
	var width = '';
	var height = '';
	if (widthHeight.length >= 2) {
		width = widthHeight[0];
		height = widthHeight[1];
	}
	heightInput.disabled = aspectRatioList.value !== 'custom';
	if (width !== '' && height !== '') {
		UpdateHeight(widthInput, width, heightInput, height);
	}
}
function GenerateIframeEmbed() {
	var shortcodeInput = document.getElementById('ShortcodeInput');

	shortcodeInput.value = '';
}
function GenerateJavascriptEmbed() {
	var shortcodeInput = document.getElementById('ShortcodeInput');

}

var EditorOptions = function () {
	var aspectRatioSelect = document.getElementById('AspectRatioList');
	var widthInput = document.getElementById('WidthInput');
	var heightInput = document.getElementById('HeightInput');

	aspectRatioSelect.addEventListener("change", UpdateAspectRatio(aspectRatioSelect, widthInput, heightInput));
	widthInput.addEventListener("change", UpdateAspectRatio(aspectRatioSelect, widthInput, heightInput));
}

var scAspectPreserver = new AspectPreserver();
var editorOptionsContainer = new EditorOptions();