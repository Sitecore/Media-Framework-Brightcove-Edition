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

var scAspectPreserver = new AspectPreserver();