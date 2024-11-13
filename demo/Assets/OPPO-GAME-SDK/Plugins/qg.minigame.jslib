var QgGameBridge = {
  $CONSTANT: {
    ACTION_CALL_BACK_CLASS_NAME_DEFAULT: "QGMiniGameManager",
    ACTION_CALL_BACK_METHORD_NAME_DEFAULT: "DefaultResponseCallback",
    ACTION_CALL_BACK_METHORD_NAME_AD_ERROR: "AdOnErrorCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_LOAD: "AdOnLoadCallBack",
    //ACTION_CALL_BACK_METHORD_NAME_AD_SHOW: 'AdOnShowCallBack',
    ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE: "AdOnCloseCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_HIDE: "AdOnHideCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE_REWARDED:
      "RewardedVideoAdOnCloseCallBack",
    ACTION_CALL_BACK_METHORD_NAME_AD_LOAD_NATIVE: "NativeAdOnLoadCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_PLAY: "pdOnPlayCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_CANPLAY: "pdOnCanPlayCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_PAUSE: "pdOnPauseCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_STOP: "pdOnStopCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_ENDED: "pdOnEndedCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_TIMEUPDATE: "pdOnTimeUpdateCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_ERROR: "pdOnErrorCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_WAITING: "pdOnWaitingCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_SEEKING: "pdOnSeekingCallBack",
    ACTION_CALL_BACK_METHORD_NAME_PD_SEEKED: "pdOnSeekedCallBack",
  },
  $mAdMap: {},

  $mFileData: {},

  $mKeyBoardData: null,

  $property: {
    cameraObj: null,
    cameraImageCallback: null,
    cameraData: null,
    cameraArPoseCallback: null,
    cameraArPose: null,
    deviceMotionChangeData: null,
    deviceMotionChangeCallback: null,
    cameraObjYuv: null,
    cameraImageYuvCallback: null,
    cameraYuvData: null,
    cameraArPlaneCallback: null,
    cameraArPlane: null,
    readFileData: null,
  },

  QGLog: function () {
    var originalConsoleLog = console.log;

    window.UnityLog = function (message) {
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "HandleLogMessage",
        message
      );
    };
    console.log = function (message) {
      UnityLog(message);
    };

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    mAdMap.set("QGLog", originalConsoleLog);
  },

  QGLogClose: function () {
    var originalConsoleLog = mAdMap.get("QGLog");
    if (originalConsoleLog) {
      console.log = originalConsoleLog;
    }
  },

  QGShowModal: function (param, success, fail, complete) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.showModal({
      title: paramData.title,
      content: paramData.content,
      showCancel: paramData.showCancel,
      cancelText: paramData.cancelText,
      cancelColor: paramData.cancelColor,
      confirmText: paramData.confirmText,
      confirmColor: paramData.confirmColor,
      success: function (res) {
        if (res.confirm) {
          console.log("??????", JSON.stringify(res.confirm));
        } else if (res.cancel) {
          console.log("??????", JSON.stringify(res.cancel));
        }
        var json = JSON.stringify({
          callbackId: successID,
          data: res,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ShowModalCallback",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          data: res,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ShowModalCallback",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          data: res,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ShowModalCallback",
          json
        );
      },
    });
  },

  QGShowToast: function (param) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    qg.showToast({
      title: paramData.title,
      icon: paramData.iconType,
      duration: paramData.durationTime,
    });

    var qgDir = qg.env.USER_DATA_PATH;
    console.log("qgDir::" + qgDir);
  },

  QGShowLoading: function (param) {
    var paramStr = UTF8ToString(param);
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.showLoading({
      title: paramStr,
    });
  },

  QGHideLoading: function (success) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    qg.hideLoading({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGSetTimeout: function (times, callback) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var callbackID = UTF8ToString(callback);
    var func = function () {
      var json = JSON.stringify({
        callbackId: callbackID,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
    };
    setTimeout(func, times);
  },

  QGGetNetworkType: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    qg.getNetworkType({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res,
        });
        console.log(json);
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetNetworkTypeCallback",
          json
        );
        console.log("???????" + res.networkType);
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetNetworkTypeCallback",
          json
        );
        console.log("?????????" + err);
      },
    });
  },

  QGOnNetworkStatusChange: function (callback) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var callbackID = Pointer_stringify(callback);
    var func = function (data) {
      var json = JSON.stringify({
        callbackId: callbackID,
        isConnected: data.isConnected,
        networkType: data.networkType,
      });
      console.log(data);
      console.log(json);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnNetworkStatusChangeResponseCallback",
        json
      );
    };
    qg.onNetworkStatusChange(func);
    // qg.onNetworkStatusChange(function (res) {
    //   console.log(
    //     "????????" + res.isConnected + "," + "??????" + res.networkType
    //   )
    // });
  },

  QGVibrateShort: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    qg.vibrateShort({
      type: "light", // heavy?medium?light
      success: function (res) {
        console.log("???-light");
      },
      fail: function (res) {},
    });
  },

  QGVibrateLong: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    qg.vibrateLong({
      success: function (res) {
        console.log("???");
      },
      fail: function (res) {},
    });
  },

  QGGetSystemInfo: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    qg.getSystemInfo({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res,
        });
        console.log(json);
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "SystemInfo",
          json
        );
        console.log("??");
        console.log("???? brand?" + res.brand);
        console.log("???? language?" + res.language);
        console.log("???? model?" + res.model);
        console.log("???/?????? statusBarHeight?" + res.statusBarHeight);
        console.log("????? pixelRatio?" + res.pixelRatio);
        console.log("????? platformVersionName?" + res.platformVersionName);
        console.log("????? platformVersionCode?" + res.platformVersionCode);
        console.log("???? screenHeight?" + res.screenHeight);
        console.log("???? screenWidth?" + res.screenWidth);
        console.log("???? system?" + res.system);
        console.log("??????? windowHeight?" + res.windowHeight);
        console.log("??????? windowWidth?" + res.windowWidth);
        console.log("?????? theme?" + res.theme);
        console.log("???? deviceOrientation?" + res.deviceOrientation);
        console.log("??? COREVersion?" + res.COREVersion);
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "SystemInfo",
          json
        );
        console.log("??????????");
      },
    });
  },

  QGGetSystemInfoSync: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var res = qg.getSystemInfoSync();
    var returnStr = JSON.stringify(res);
    console.log(returnStr);
    if (returnStr) {
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } else {
      console.log("??????????");
    }
    // console.log("??");
    // console.log("???? brand?" + res.brand);
    // console.log("???? language?" + res.language);
    // console.log("???? model?" + res.model);
    // console.log("???/?????? statusBarHeight?" + res.statusBarHeight);
    // console.log("????? pixelRatio?" + res.pixelRatio);
    // console.log("????? platformVersionName?" + res.platformVersionName);
    // console.log("????? platformVersionCode?" + res.platformVersionCode);
    // console.log("???? screenHeight?" + res.screenHeight);
    // console.log("???? screenWidth?" + res.screenWidth);
    // console.log("???? system?" + res.system);
    // console.log("??????? windowHeight?" + res.windowHeight);
    // console.log("??????? windowWidth?" + res.windowWidth);
    // console.log("?????? theme?" + res.theme);
    // console.log("???? deviceOrientation?" + res.deviceOrientation);
    // console.log("??? COREVersion?" + res.COREVersion);
  },

  // vConsole
  QGSetEnableDebugTrue: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.setEnableDebug({
      enableDebug: true, // true ????false ???
      success: function () {
        // ??????? vConsole ????
        console.log("test consol log");
        console.info("test console info");
        console.warn("test consol warn");
        console.debug("test consol debug");
        console.error("test consol error");
      },
      complete: function () {},
      fail: function () {},
    });
  },

  QGSetEnableDebugFalse: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.setEnableDebug({
      enableDebug: false, // true ????false ???
      success: function () {
        // ??????? vConsole ????
        console.log("test consol log");
        console.info("test console info");
        console.warn("test consol warn");
        console.debug("test consol debug");
        console.error("test consol error");
      },
      complete: function () {},
      fail: function () {},
    });
  },

  QGShowKeyboard: function (id, param, success, fail, complete) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    mKeyBoardData = UTF8ToString(id);

    qg.showKeyboard({
      defaultValue: paramData.defaultValue,
      maxLength: paramData.maxLength,
      multiple: paramData.multiple,
      confirmHold: paramData.confirmHold,
      confirmType: paramData.confirmType,
      success: function () {
        var json = JSON.stringify({
          callbackId: successID,
        });
        console.log(json);
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ShowKeyboardResponseCallback",
          json
        );
        console.log("show keyboard success");
      },
      fail: function () {
        var json = JSON.stringify({
          callbackId: failID,
        });
        console.log(json);
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ShowKeyboardResponseCallback",
          json
        );
        console.log("show keyboard fail");
      },
    });
  },

  QGOnKeyboardInput: function (callback) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    // var callbackID = Pointer_stringify(callback);
    var callbackID = UTF8ToString(callback);
    var func = function (data) {
      var json = JSON.stringify({
        callbackId: callbackID,
        value: data.value,
        keyboardId: mKeyBoardData,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnKeyboardInputResponseCallback",
        json
      );
    };
    qg.onKeyboardInput(func);
  },

  QGOffKeyboardInput: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.offKeyboardInput();
  },

  QGOnKeyboardConfirm: function (callback) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    // var callbackID = Pointer_stringify(callback);
    var callbackID = UTF8ToString(callback);

    var func = function (data) {
      var json = JSON.stringify({
        callbackId: callbackID,
        value: data.value,
        keyboardId: mKeyBoardData,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnKeyboardInputResponseCallback",
        json
      );
    };
    qg.onKeyboardConfirm(func);
  },

  QGOffKeyboardConfirm: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.offKeyboardConfirm();
  },

  QGOnKeyboardComplete: function (callback) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    // var callbackID = Pointer_stringify(callback);
    var callbackID = UTF8ToString(callback);

    var func = function (data) {
      var json = JSON.stringify({
        callbackId: callbackID,
        value: data.value,
        keyboardId: mKeyBoardData,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnKeyboardInputResponseCallback",
        json
      );
    };
    qg.onKeyboardComplete(func);
  },

  QGOffKeyboardComplete: function (callback) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.offKeyboardComplete();
  },

  QGHideKeyboard: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.hideKeyboard();
  },

  QGMkdir: function (dirPath, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(dirPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.mkdir({
      dirPath: localDir,
      encoding: "utf8",
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("mkdir success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("mkdir fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("mkdir complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGMkdirSync: function (dirPath, recursivebl, success, fail) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(dirPath);
    var recursive = UTF8ToString(recursivebl) === "true";
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      fs.mkdirSync(localDir, recursive);
      var json = JSON.stringify({
        callbackId: successID,
      });
      console.log("mkdirSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("mkdirSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return false;
    }
  },

  QGRmdir: function (dirPath, recursivebl, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(dirPath);
    var recursive = UTF8ToString(recursivebl) === "true";
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.rmdir({
      dirPath: localDir,
      recursive: recursive,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("rmdir success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("rmdir fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("rmdir complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGRmdirSync: function (dirPath, recursivebl, success, fail) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(dirPath);
    var recursive = UTF8ToString(recursivebl) === "true";
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      fs.rmdirSync(localDir, recursive);
      var json = JSON.stringify({
        callbackId: successID,
      });
      console.log("rmdirSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("rmdirSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return false;
    }
  },

  QGUnlink: function (dirPath, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(dirPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    console.log("QGUnlink dirPath::: ", localDir);
    fs.unlink({
      filePath: localDir,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("unlink success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("unlink fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("unlink complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGUnlinkSync: function (dirPath, success, fail) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(dirPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    console.log("QGUnlinkSync dirPath::: ", localDir);
    try {
      fs.unlinkSync(localDir);
      var json = JSON.stringify({
        callbackId: successID,
      });
      console.log("unlinkSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("unlinkSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return false;
    }
  },

  QGIsExist: function () {
    var qgDir = qg.env.USER_DATA_PATH;
    var localFilePath = qgDir + "/my/file.txt";
    var localDir = qgDir + "/my";
    var fs = qg.getFileSystemManager();
    try {
      var dirStat = fs.statSync(localDir, false);
      var fileStat = fs.statSync(localFilePath, false);
      console.log(
        localDir +
          "??????" +
          dirStat.isDirectory() +
          localFilePath +
          "??????" +
          fileStat.isFile()
      );
    } catch (error) {
      console.log(error + ", ??????????");
    }
  },

  // QGRename: function () {
  //   var qgDir = qg.env.USER_DATA_PATH;
  //   var localFilePath = qgDir + "/my/file.txt";
  //   var localDir = qgDir + "/my";
  //   var fs = qg.getFileSystemManager();
  //   fs.rename({
  //     oldPath: localDir,
  //     newPath: qgDir + "/new/" + Math.random(),
  //     success: function () {
  //       console.log("????????" + localDir + "=>" + qgDir + "/new");
  //     },
  //     fail: function (res) {
  //       console.log("????????" + localDir + "?" + JSON.stringify(res));
  //     },
  //   });
  // },

  // QGSaveFile: function () {
  //   var qgDir = qg.env.USER_DATA_PATH;
  //   var localFilePath = qgDir + "/my/file.txt";
  //   var localDir = qgDir + "/my";
  //   var fs = qg.getFileSystemManager();
  //   qg.chooseImage({
  //     count: 1,
  //     sizeType: ["original"],
  //     sourceType: ["album"],
  //     success: function (imgRes) {
  //       fs.saveFile({
  //         filePath: localFilePath,
  //         tempFilePath: imgRes.tempFilePaths[0],
  //         success: function (res) {
  //           console.log("?????: " + res.savedFilePath);
  //         },
  //         fail: function (res) {
  //           console.log("error?" + JSON.stringify(res));
  //         },
  //       });
  //     },
  //     fail: function (e) {
  //       console.log("???????" + JSON.stringify(e));
  //     },
  //   });
  // },

  QGRename: function (oldPath, newPath, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localOldPath = qgDir + UTF8ToString(oldPath);
    var localNewPath = qgDir + UTF8ToString(newPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.rename({
      oldPath: localOldPath,
      newPath: localNewPath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("rename success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("rename fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("rename complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGRenameSync: function (oldPath, newPath, success, fail) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localOldPath = qgDir + UTF8ToString(oldPath);
    var localNewPath = qgDir + UTF8ToString(newPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      fs.renameSync(localOldPath, localNewPath);
      var json = JSON.stringify({
        callbackId: successID,
      });
      console.log("renameSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("renameSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return false;
    }
  },

  // QGReadDir: function () {
  //   var qgDir = qg.env.USER_DATA_PATH;
  //   var localFilePath = qgDir + "/my/file.txt";
  //   var localDir = qgDir + "/my";
  //   var fs = qg.getFileSystemManager();
  //   fs.readdir({
  //     dirPath: localDir,
  //     success: function (res) {
  //       console.log("success?" + localDir + "?" + res.files);
  //     },
  //     fail: function (res) {
  //       console.log(
  //         "error?" +
  //           localFilePath +
  //           "=>" +
  //           qgDir +
  //           "/newPath.txt?" +
  //           JSON.stringify(res)
  //       );
  //     },
  //   });
  // },

  QGReadDir: function (path, success, fail, complete) {
    var localDirPath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.readdir({
      dirPath: localDirPath,
      success: function (res) {
        var files = res.files;
        var filesStr = files.join(", ");
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
          filesStr: filesStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "OnReadDirSuccessCallBack",
          json
        );
        console.log("readdir success:", json);
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("readdir fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("readdir complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGReadDirSync: function (path, success, fail) {
    var localDirPath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      var res = fs.readdirSync(localDirPath);
      var filesStr = res.join(", ");
      console.log("readdirSync success:", res);
      var json = JSON.stringify({
        callbackId: successID,
        errCode: res.errCode,
        errMsg: res.errMsg,
        filesStr: filesStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnReadDirSuccessCallBack",
        json
      );
      var bufferSize = lengthBytesUTF8(filesStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(filesStr, buffer, bufferSize);
      return buffer;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errCode: error.errCode,
        errMsg: error.errMsg,
      });
      console.log("readdirSync fail: " + JSON.stringify(error));
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return null;
    }
  },

  QGWriteFile: function (
    path,
    param,
    paramEncoding,
    appendbl,
    success,
    fail,
    complete
  ) {
    var paramStr = UTF8ToString(param);
    var localFilePath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var encoding = UTF8ToString(paramEncoding);
    var append = UTF8ToString(appendbl) === "true";
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    var FileData;
    if (encoding === "binary") {
      var uint8ArrayData = new Uint8Array(paramStr.split(",").map(Number));
      FileData = uint8ArrayData.buffer;
    } else if (encoding === "utf8") {
      FileData = paramStr;
    } else {
      console.log(
        "This data type is not supported, please pass in utf8 or binary"
      );
      return;
    }

    fs.writeFile({
      filePath: localFilePath,
      data: FileData,
      encoding: encoding,
      append: append,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("writeFile success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("writeFile fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("writeFile complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGWriteFileSync: function (
    path,
    param,
    paramEncoding,
    appendbl,
    success,
    fail
  ) {
    var paramStr = UTF8ToString(param);
    var localFilePath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var encoding = UTF8ToString(paramEncoding);
    var append = UTF8ToString(appendbl) === "true";
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    var FileData;
    if (encoding === "binary") {
      var uint8ArrayData = new Uint8Array(paramStr.split(",").map(Number));
      FileData = uint8ArrayData.buffer;
    } else if (encoding === "utf8") {
      FileData = paramStr;
    } else {
      console.log(
        "This data type is not supported, please pass in utf8 or binary"
      );
      return false;
    }
    try {
      fs.writeFileSync(localFilePath, FileData, encoding, append);
      var json = JSON.stringify({
        callbackId: successID,
      });
      console.log("writeFileSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("writeFileSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return false;
    }
  },

  QGReadFile: function (path, paramEncoding, success, fail, complete) {
    var localFilePath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var encoding = UTF8ToString(paramEncoding);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    if (encoding === "binary" || encoding === "utf8") {
    } else {
      console.log(
        "This ReadFile data type is not supported, please pass in utf8 or binary"
      );
      return;
    }

    fs.readFile({
      filePath: localFilePath,
      encoding: encoding,
      success: function (res) {
        property.readFileData = res.data;
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
          encoding: encoding,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "OnReadFileSuccessCallBack",
          json
        );
        console.log("readFile success:", JSON.stringify(res));
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("readFile fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("readFile complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGReadFileByteArray: function (QGReadFileDataCallback) {
    if (!QGReadFileDataCallback) {
      return;
    }
    if (!property.readFileData) {
      return;
    }
    var dataView = new Uint8Array(property.readFileData);
    var dataLen = dataView.length;
    if (!dataLen) {
      return;
    }
    var buffer = _malloc(dataLen);
    HEAPU8.set(dataView, buffer);
    dynCall("vii", QGReadFileDataCallback, [buffer, dataLen]);
    property.readFileData = null;
    _free(buffer);
  },

  QGReadFileUtf8: function (QGReadFileDataCallback) {
    if (!QGReadFileDataCallback) {
      return;
    }
    if (!property.readFileData) {
      return;
    }
    var readFileDataUtf8String = allocateUTF8(property.readFileData);
    dynCall("vi", QGReadFileDataCallback, [readFileDataUtf8String]);
    property.readFileData = null;
  },

  QGReadFileSync: function (path, paramEncoding, success, fail) {
    var localFilePath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var encoding = UTF8ToString(paramEncoding);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    if (encoding === "binary" || encoding === "utf8") {
    } else {
      console.log(
        "This ReadFileSync data type is not supported, please pass in utf8 or binary"
      );
      return null;
    }
    try {
      var res = fs.readFileSync(localFilePath, encoding);
      property.readFileData = res;
      var json = JSON.stringify({
        callbackId: successID,
        encoding: encoding,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnReadFileSuccessCallBack",
        json
      );
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errCode: error.errCode,
        errMsg: error.errMsg,
      });
      console.log("readFileSync fail: " + JSON.stringify(error));
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
    }
  },

  QGAppendFile: function (path, param, paramEncoding, success, fail, complete) {
    var paramStr = UTF8ToString(param);
    var localFilePath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var encoding = UTF8ToString(paramEncoding);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    var FileData;
    if (encoding === "binary") {
      var uint8ArrayData = new Uint8Array(paramStr.split(",").map(Number));
      FileData = uint8ArrayData.buffer;
    } else if (encoding === "utf8") {
      FileData = paramStr;
    } else {
      console.log(
        "This data type is not supported, please pass in utf8 or binary"
      );
      return;
    }
    fs.appendFile({
      filePath: localFilePath,
      data: FileData,
      encoding: encoding,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("appendFile success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("appendFile fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("appendFile complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGAppendFileSync: function (path, param, paramEncoding, success, fail) {
    var paramStr = UTF8ToString(param);
    var localFilePath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var encoding = UTF8ToString(paramEncoding);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    var FileData;
    if (encoding === "binary") {
      var uint8ArrayData = new Uint8Array(paramStr.split(",").map(Number));
      FileData = uint8ArrayData.buffer;
    } else if (encoding === "utf8") {
      FileData = paramStr;
    } else {
      console.log(
        "This data type is not supported, please pass in utf8 or binary"
      );
      return false;
    }
    try {
      fs.appendFileSync(localFilePath, FileData, encoding);
      var json = JSON.stringify({
        callbackId: successID,
      });
      console.log("appendFileSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("appendFileSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return;
    }
  },

  QGCopyFile: function (srcPath, destPath, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localSrcPath = qgDir + UTF8ToString(srcPath);
    var localDestPath = qgDir + UTF8ToString(destPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.copyFile({
      srcPath: localSrcPath,
      destPath: localDestPath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("copyFile success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("copyFile fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("copyFile complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGCopyFileSync: function (srcPath, destPath, success, fail) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localSrcPath = qgDir + UTF8ToString(srcPath);
    var localDestPath = qgDir + UTF8ToString(destPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      fs.copyFileSync(localSrcPath, localDestPath);
      var json = JSON.stringify({
        callbackId: successID,
      });
      console.log("copyFileSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("copyFileSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return false;
    }
  },

  QGSaveFile: function (tempFilePath, filePath, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localTempFilePath = qgDir + UTF8ToString(tempFilePath);
    var localFilePath = qgDir + UTF8ToString(filePath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.saveFile({
      tempFilePath: localTempFilePath,
      filePath: localFilePath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.savedFilePath,
        });
        console.log("saveFile success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("saveFile fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("saveFile complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGSaveFileSync: function (tempFilePath, filePath, success, fail) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localTempFilePath = qgDir + UTF8ToString(tempFilePath);
    var localFilePath = qgDir + UTF8ToString(filePath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      var savedFilePath = fs.saveFileSync(localTempFilePath, localFilePath);
      var json = JSON.stringify({
        callbackId: successID,
        errMsg: savedFilePath,
      });
      console.log("saveFileSync success");
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      var bufferSize = lengthBytesUTF8(savedFilePath) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(savedFilePath, buffer, bufferSize);
      return buffer;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: error,
      });
      console.log("saveFileSync fail: " + error);
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return null;
    }
  },

  QGRemoveSavedFile: function (filePath, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localFilePath = qgDir + UTF8ToString(filePath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.removeSavedFile({
      filePath: localFilePath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("removeSavedFile success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("removeSavedFile fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("removeSavedFile complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGStat: function (path, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localPath = qgDir + UTF8ToString(path);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.stat({
      path: localPath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
          mode: res.stat.mode,
          size: res.stat.size,
          lastAccessedTime: res.stat.lastAccessedTime,
          lastModifiedTime: res.stat.lastModifiedTime,
          isDirectory: res.stat.isDirectory(),
          isFile: res.stat.isFile(),
        });
        console.log("stat success res: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "OnStatSuccessCallBack",
          json
        );
        console.log("stat success json:", json);
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("stat fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("stat complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGStatSync: function (path, recursivebl, success, fail) {
    var localPath = qg.env.USER_DATA_PATH + UTF8ToString(path);
    var recursive = UTF8ToString(recursivebl) === "true";
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      console.log(
        "statSync -js localPath: ",
        localPath,
        ",recursive: ",
        recursive
      );
      var res = fs.statSync(localPath, recursive);
      console.log("statSync -js success res: " + JSON.stringify(res));
      var json = JSON.stringify({
        callbackId: successID,
        mode: res.mode,
        size: res.size,
        lastAccessedTime: res.lastAccessedTime,
        lastModifiedTime: res.lastModifiedTime,
        isDirectory: res.isDirectory(),
        isFile: res.isFile(),
      });

      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnStatSuccessCallBack",
        json
      );
      console.log("statSync success json:", json);
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
        errCode: error.errCode,
        errMsg: error.errMsg,
      });
      console.log("statSync fail: " + JSON.stringify(error));
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      return null;
    }
  },

  QGUploadFile: function (param, success, fail) {
    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var tempFilePath = qg.env.USER_DATA_PATH + paramData.path;
    qg.uploadFile({
      url: paramData.url,
      filePath: tempFilePath,
      name: paramData.name,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        console.log("success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        console.log("fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGDownLoadFile: function (param, success, fail) {
    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var tempFilePath = qg.env.USER_DATA_PATH + paramData.path;
    qg.downloadFile({
      url: paramData.url,
      filePath: tempFilePath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        console.log("success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        console.log("fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGUnzip: function (zipFilePath, targetPath, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localZipFilePath = qgDir + UTF8ToString(zipFilePath);
    var localTargetPath = qgDir + UTF8ToString(targetPath);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.unzip({
      zipFilePath: localZipFilePath,
      targetPath: localTargetPath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("unzip success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("unzip fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("unzip complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGGetFileInfo: function (filename, success, fail) {
    var filePath = UTF8ToString(filename);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var qgDir = qg.env.USER_DATA_PATH;
    var localFilePath = qgDir + filePath;
    var fs = qg.getFileSystemManager();
    fs.getFileInfo({
      filePath: localFilePath,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errMsg: res.size,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        console.log("error?" + localFilePath + "?" + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGCreateVideo: function (adId, param) {
    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var video = qg.createVideo({
      x: paramData.ParamX,
      y: paramData.ParamY,
      width: paramData.ParamWidth,
      height: paramData.ParamHeight,
      src: paramData.url,
      poster: paramData.poster,
      playbackRate: 1.0,
      objectFit: "contain",
      autoplay: false,
    });

    video.onPlay(function () {
      console.log("video play");
    });
    video.play();
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    var adIdStr = UTF8ToString(adId);
    mAdMap.set(adIdStr, video);
  },

  QGPlayAudio: function (playerId, param) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var innerAudioContext = qg.createInnerAudioContext();
    innerAudioContext.startTime = paramData.startTime ? paramData.startTime : 0;
    innerAudioContext.autoplay = paramData.autoplay
      ? paramData.autoplay
      : false;
    innerAudioContext.loop = paramData.loop ? paramData.loop : false;
    innerAudioContext.src = paramData.url;
    innerAudioContext.play();
    console.log("QGPlayAudio -js url: ", paramData.url);
    innerAudioContext.volume = paramData.volume;
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    var pdIdStr = UTF8ToString(playerId);
    mAdMap.set(pdIdStr, innerAudioContext);
  },

  QGAudioPlayerVolume: function (playerId, param) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var playerIdStr = UTF8ToString(playerId);
    var pd = mAdMap.get(playerIdStr);
    if (pd) {
      console.log("QGAudioPlayerVolume", param);
      pd.volume = param;
    }
  },

  QGAudioPlayerLoop: function (playerId, param) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var playerIdStr = UTF8ToString(playerId);
    var pd = mAdMap.get(playerIdStr);
    if (pd) {
      pd.loop = param;
    }
  },
  // QGPauseAudio: function () {
  //   var innerAudioContext = qg.createInnerAudioContext();
  //   innerAudioContext.pause();
  //   innerAudioContext.stop();
  //   // CONSTANT.InnerAudioContext.pause()
  //   console.log("????");
  // },
  QGOnAudioInterruptionBegin: function () {
    qg.onAudioInterruptionBegin(function () {
      console.log(
        "onAudioInterruptionBegin success: " + new Date().toLocaleString()
      );
    });
    console.log("??qg.onAudioInterruptionBegin");
  },

  QGOffAudioInterruptionBegin: function () {
    qg.offAudioInterruptionBegin();
    console.log("????qg.onAudioInterruptionBegin");
  },

  QGOnAudioInterruptionEnd: function () {
    qg.onAudioInterruptionEnd(function () {
      console.log(
        "onAudioInterruptionEnd success: " + new Date().toLocaleString()
      );
    });
    console.log("??qg.onAudioInterruptionEnd");
  },

  QGOffAudioInterruptionEnd: function () {
    qg.offAudioInterruptionEnd();
    console.log("????qg.onAudioInterruptionEnd");
  },

  QGOnError: function () {
    qg.onError(function (res) {
      console.log("onError success: " + res.message.slice(0, 149));
    });
    console.log("????????");
  },

  QGOffError: function () {
    qg.offError();
    console.log("??????????");
  },

  QGDispatchError: function () {
    console.log("????Error");
    throw Error("dispatch Error");
  },

  QGLogin: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);

    qg.login({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        console.log(json);
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "LoginResponseCallback",
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "LoginResponseCallback",
          json
        );
      },
    });
  },

  QGHasShortcutInstalled: function (success, fail, complete) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.hasShortcutInstalled({
      success: function (res) {
        console.log("qg.hasShortcutInstalled success:", JSON.stringify(res));
        var json = JSON.stringify({
          callbackId: successID,
          hasShortcutInstalled: res,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "HasShortcutInstalledCallback",
          json
        );
      },
      fail: function (res) {
        console.log("qg.hasShortcutInstalled fail: " + JSON.stringify(res));
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        console.log("qg.hasShortcutInstalled complete: " + JSON.stringify(res));
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGInstallShortcut: function (success, fail, complete) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.installShortcut({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
          errMsg: res.errMsg,
          code: res.code,
          errCode: res.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "InstallShortcutCallback",
          json
        );
      },
      fail: function (res) {
        console.log("qg.installShortcut fail: " + JSON.stringify(res));
        var json = JSON.stringify({
          callbackId: failID,
          data: res.data,
          errMsg: res.errMsg,
          code: res.code,
          errCode: res.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "InstallShortcutCallback",
          json
        );
      },
      complete: function (res) {
        console.log("qg.installShortcut complete: " + JSON.stringify(res));
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },
  // bannerAd
  QGCreateBannerAd: function (adId, adUnitId, style) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = UTF8ToString(posId);
    var adUnitIdStr = UTF8ToString(adUnitId);
    var styleStr = UTF8ToString(style);
    var adIdStr = UTF8ToString(adId);

    var bannerAd;
    if (styleStr) {
      bannerAd = qg.createBannerAd({
        adUnitId: adUnitIdStr,
        style: JSON.parse(styleStr),
      });
    } else {
      bannerAd = qg.createBannerAd({
        adUnitId: adUnitIdStr,
      });
    }
    if (bannerAd) {
      mAdMap.set(adIdStr, bannerAd);
      bannerAd.onLoad(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        console.log("banner load success");
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD,
          json
        );
      });
      bannerAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adId,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR,
          json
        );
      });
      // bannerAd.show();
    }
  },
  // showBannerAd
  // QGshowBannerAd: function () {
  //   if (!!this.bannerAd) {
  //     this.bannerAd.show();
  // } else {
  //     this.QGCreateBannerAd(true);
  // }
  // },
  // // hideBannerAd
  // QGhideBannerAd: function () {
  //   console.log("????BannerAD : HOME");
  //   if (!!this.bannerAd) {
  //       this.bannerAd.hide();
  //   }
  // },
  // // destroyBannerAd
  // QGdestroyBannerAd: function () {
  //   console.log("????BannerAD : HOME");
  //   if (!!this.bannerAd) {
  //       this.bannerAd.destroy();
  //       this.bannerAd = null;
  //   }
  // },

  // RewardedVideoAd
  QGCreateRewardedVideoAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = UTF8ToString(posId);
    var adUnitIdStr = UTF8ToString(adUnitId);
    var adIdStr = UTF8ToString(adId);

    var rewardedVideoAd = qg.createRewardedVideoAd({
      adUnitId: adUnitIdStr,
    });
    if (rewardedVideoAd) {
      mAdMap.set(adIdStr, rewardedVideoAd);

      rewardedVideoAd.onLoad(function () {
        console.log("rewardedVideoAd onload success");
        // rewardedVideoAd.show();
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD,
          json
        );
      });

      rewardedVideoAd.onClose(function (rec) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          isEnded: rec.isEnded,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE_REWARDED,
          json
        );
      });
      rewardedVideoAd.onError(function (err) {
        console.error(" rewardedVideoAd.onError = " + JSON.stringify(err));
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR,
          json
        );
      });
    }
  },
  // InterstitialAd
  QGCreateInterstitialAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = UTF8ToString(posId);
    var adUnitIdStr = UTF8ToString(adUnitId);
    var adIdStr = UTF8ToString(adId);

    var interstitialAd = qg.createInterstitialAd({
      adUnitId: adUnitIdStr,
    });
    if (interstitialAd) {
      mAdMap.set(adIdStr, interstitialAd);
      interstitialAd.onLoad(function () {
        console.log("Interstitial onload success");
        // interstitialAd.show();
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD,
          json
        );
      });
      interstitialAd.onClose(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE,
          json
        );
      });
      interstitialAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR,
          json
        );
      });
    }
  },
  // CustomAd
  QGCreateCustomAd: function (adId, adUnitId, style) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = UTF8ToString(posId);
    var adUnitIdStr = UTF8ToString(adUnitId);
    var adIdStr = UTF8ToString(adId);
    var styleStr = UTF8ToString(style);

    var customAd = qg.createCustomAd({
      adUnitId: adUnitIdStr,
      style: styleStr ? JSON.parse(styleStr) : {},
    });
    if (customAd) {
      mAdMap.set(adIdStr, customAd);
      // customAd.show();
      customAd.onLoad(function (rec) {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD,
          json
        );
      });
      customAd.onShow();
      // customAd.onClose(function () {
      //   var json = JSON.stringify({
      //     callbackId: adIdStr,
      //   });
      //   unityInstance.SendMessage(
      //     CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      //     CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE,
      //     json
      //   );
      // });
      customAd.onHide(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_HIDE,
          json
        );
      });
      customAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR,
          json
        );
      });
    }
  },
  // GameBannerAd
  QGCreateGameBannerAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = UTF8ToString(posId);
    var adUnitIdStr = UTF8ToString(adUnitId);
    var adIdStr = UTF8ToString(adId);

    var gameBannerAd = qg.createGameBannerAd({
      adUnitId: adUnitIdStr,
    });
    if (gameBannerAd) {
      mAdMap.set(adIdStr, gameBannerAd);
      // gameBannerAd
      //   .show()
      //   .then(function () {
      //     console.log("show success");
      //   })
      //   .catch(function (error) {
      //     console.log("show fail with:" + error.errCode + "," + error.errMsg);
      //   });
      gameBannerAd.onLoad(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD,
          json
        );
      });
      // gameBannerAd.onClose(function () {
      //   var json = JSON.stringify({
      //     callbackId: adIdStr,
      //   });
      //   unityInstance.SendMessage(
      //    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      //    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE,
      //    json
      //  );
      // });
      gameBannerAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR,
          json
        );
      });
    }
  },
  // GamePortalAd
  QGCreateGamePortalAd: function (adId, adUnitId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = UTF8ToString(posId);
    var adUnitIdStr = UTF8ToString(adUnitId);
    var adIdStr = UTF8ToString(adId);
    // var imageStr = UTF8ToString(image);

    var gamePortalAd = qg.createGamePortalAd({
      adUnitId: adUnitIdStr,
    });
    if (gamePortalAd) {
      mAdMap.set(adIdStr, gamePortalAd);
      // gamePortalAd
      //   .load()
      //   .then(function () {
      //     console.log("load success");
      //   })
      //   .catch(function (error) {
      //     console.log("load fail with:" + error.errCode + "," + error.errMsg);
      //   });

      // gamePortalAd
      //   .show()
      //   .then(function () {
      //     console.log("show success");
      //   })
      //   .catch(function (error) {
      //     console.log("show fail with:" + error.errCode + "," + error.errMsg);
      //   });
      gamePortalAd.onLoad(function () {
        console.log("gamePortalAd onload success");
        // gamePortalAd
        //   .show()
        //   .then(function () {
        //     console.log("gamePortalAd show success");
        //   })
        //   .catch(function (error) {
        //     console.log(
        //       "gamePortalAd show fail with:" +
        //         error.errCode +
        //         "," +
        //         error.errMsg
        //     );
        //   });
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD,
          json
        );
      });
      gamePortalAd.onClose(function () {
        var json = JSON.stringify({
          callbackId: adIdStr,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE,
          json
        );
      });
      gamePortalAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR,
          json
        );
      });
    }
  },
  // GameDrawerAd
  QGCreateGameDrawerAd: function (adId, adUnitId, style) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    // var posIdStr = UTF8ToString(posId);
    var adUnitIdStr = UTF8ToString(adUnitId);
    var adIdStr = UTF8ToString(adId);
    var styleStr = UTF8ToString(style);

    var GameDrawerAd;
    if (styleStr) {
      GameDrawerAd = qg.createGameDrawerAd({
        adUnitId: adUnitIdStr,
        style: JSON.parse(styleStr),
      });
    } else {
      GameDrawerAd = qg.createGameDrawerAd({
        adUnitId: adUnitIdStr,
      });
    }
    if (GameDrawerAd) {
      mAdMap.set(adIdStr, GameDrawerAd);
      // GameDrawerAd.show()
      //   .then(function () {
      //     console.log("GameDrawerAd show success");
      //   })
      //   .catch(function (error) {
      //     console.log(
      //       "GameDrawerAd show fail with:" + error.errCode + "," + error.errMsg
      //     );
      //   });
      GameDrawerAd.onShow(function () {
        console.log("GameDrawerAd onShow success");
      });
      GameDrawerAd.onError(function (err) {
        var json = JSON.stringify({
          callbackId: adIdStr,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR,
          json
        );
      });
    }
  },

  QGShowAd: function (adId, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var adIdStr = UTF8ToString(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      ad.show()
        .then(function () {
          var json = JSON.stringify({
            callbackId: successID,
          });
          unityInstance.SendMessage(
            CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
            CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
            json
          );
        })
        .catch(function (err) {
          var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg;
          var errCodeValue = !err
            ? ""
            : err.data
            ? err.data.errCode
            : err.errCode;
          var json = JSON.stringify({
            callbackId: failID,
            errMsg: errMsgStr,
            errCode: errCodeValue,
          });
          unityInstance.SendMessage(
            CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
            CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
            json
          );
        });
    } else {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: "ad is undefined",
        errCode: 404,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
    }
  },

  QGHideAd: function (adId, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var adIdStr = UTF8ToString(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      ad.hide();
      // .then(function () {
      //   var json = JSON.stringify({
      //     callbackId: successID,
      //   });
      //   unityInstance.SendMessage(
      //     CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      //     CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
      //     json
      //   );
      // })
      // .catch(function (err) {
      //   var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg;
      //   var errCodeValue = !err
      //     ? ""
      //     : err.data
      //     ? err.data.errCode
      //     : err.errCode;
      //   var json = JSON.stringify({
      //     callbackId: failID,
      //     errMsg: errMsgStr,
      //     errCode: errCodeValue,
      //   });
      //   unityInstance.SendMessage(
      //     CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      //     CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
      //     json
      //   );
      // });
      // ad.onHide(function () {
      //   var json = JSON.stringify({
      //     callbackId: adIdStr,
      //   });
      //   unityInstance.SendMessage(
      //     CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      //     CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_HIDE,
      //     json
      //   );
      // });
    } else {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: "ad is undefined",
        errCode: 404,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
    }
  },

  QGLoadAd: function (adId, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var adIdStr = UTF8ToString(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      ad.load()
        .then(function () {
          var json = JSON.stringify({
            callbackId: successID,
          });
          unityInstance.SendMessage(
            CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
            CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
            json
          );
        })
        .catch(function (err) {
          var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg;
          var errCodeValue = !err
            ? ""
            : err.data
            ? err.data.errCode
            : err.errCode;
          var json = JSON.stringify({
            callbackId: failID,
            errMsg: errMsgStr,
            errCode: errCodeValue,
          });
          unityInstance.SendMessage(
            CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
            CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
            json
          );
        });
    } else {
      var json = JSON.stringify({
        callbackId: failID,
        errMsg: "ad is undefined",
        errCode: 404,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
    }
  },

  QGDestroyAd: function (adId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    var adIdStr = UTF8ToString(adId);
    var ad = mAdMap.get(adIdStr);
    if (ad) {
      ad.destroy();
      mAdMap.delete(adIdStr);
    }
  },

  QGIsShow: function (adId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }

    var adIdStr = UTF8ToString(adId);

    var ad = mAdMap.get(adIdStr);

    if (ad) {
      return ad.isShow();
    }
  },

  // Storage
  QGStorageSetItem: function (keyName, keyValue) {
    var keyNameStr = UTF8ToString(keyName);
    var keyValueStr = UTF8ToString(keyValue);
    localStorage.setItem(keyNameStr, keyValueStr);
    console.log("QGStorageSetItem success");
  },
  QGStorageGetItem: function (keyName) {
    var keyNameStr = UTF8ToString(keyName);
    var returnStr = localStorage.getItem(keyNameStr);
    console.log("QGStorageGetItem", typeof returnStr);
    if (returnStr) {
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      console.log("QGStorageGetItem -js", buffer);
      return buffer;
    } else {
      console.log("????");
    }
  },
  QGStorageRemoveItem: function (keyName) {
    var keyNameStr = UTF8ToString(keyName);
    localStorage.removeItem(keyNameStr);
    console.log("QGStorageRemoveItem: " + keyNameStr);
  },

  // pay
  QGPayOld: function (param, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var paramObj = JSON.parse(paramStr);

    qg.getSystemInfo({
      success: function (res) {
        if (
          !res.platformVersionCode ||
          res.platformVersionCode == null ||
          res.platformVersionCode == "undefined"
        ) {
          var json = JSON.stringify({
            errMsg:
              "Failed to obtain the fast application engine version. Procedure",
          });
          unityInstance.SendMessage(
            CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
            "PayResponseFailCallback",
            json
          );
        } else {
          var xhr = new XMLHttpRequest();
          xhr.open(
            "POST",
            "https://jits.open.oppomobile.com/jitsopen/api/pay/v1.0/preOrder"
          );

          xhr.withCredentials = true;
          // xhr.setRequestHeader("Accept", "application/json");
          xhr.setRequestHeader("Content-Type", "application/json");
          xhr.setRequestHeader("charset", "UTF-8");

          var dataObject = {
            appId: paramObj.appId,
            openId: paramObj.openId,
            timestamp: paramObj.timestamp,
            sign: paramObj.sign,
            productName: paramObj.productName,
            productDesc: paramObj.productDesc,
            count: paramObj.count,
            price: paramObj.price,
            attach: paramObj.attach,
            currency: paramObj.currency,
            cpOrderId: paramObj.cpOrderId,
            appVersion: paramObj.appVersion,
            engineVersion: res.platformVersionCode.toString(),
            callBackUrl: paramObj.callBackUrl,
          };

          console.log("xhr.send::: ", JSON.stringify(dataObject));
          xhr.send(JSON.stringify(dataObject));
          xhr.onreadystatechange = function () {
            console.log("readyState: ", xhr.readyState);
            console.log("status: ", xhr.status);
            console.log("response: ", JSON.stringify(xhr.response));
            if (xhr.readyState == 4 && xhr.status == 200) {
              var data = JSON.parse(xhr.response).data;
              qg.pay({
                appId: paramObj.appId,
                token: paramObj.openId,
                timestamp: paramObj.timestamp,
                orderNo: data.orderNo,
                paySign: paramObj.paySign,
                success: function (res) {
                  var json = JSON.stringify({
                    callbackId: successID,
                    data: res.data,
                  });
                  unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "PayResponseCallback",
                    json
                  );
                  console.log("?????" + JSON.stringify(res));
                },
                fail: function (err) {
                  var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: err.errMsg,
                    errCode: err.errCode,
                    data: err,
                  });
                  console.log(json);
                  console.log("fail json" + json);
                  unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "PayResponseFailCallback",
                    json
                  );
                  console.log("?????" + JSON.stringify(err));
                },
              });
            }
          };
        }
      },
      fail: function (err) {
        var json = JSON.stringify({
          errMsg: err,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "PayResponseFailCallback",
          json
        );
        console.log("?????" + JSON.stringify(err));
      },
    });
  },

  QGPay: function (param, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var paramObj = JSON.parse(paramStr);

    qg.pay({
      appId: paramObj.appId,
      token: paramObj.openId,
      timestamp: paramObj.timestamp,
      paySign: paramObj.paySign,
      orderNo: paramObj.orderNo,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "PayResponseCallback",
          json
        );
        console.log("?????" + JSON.stringify(res));
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: err.errMsg,
          errCode: err.errCode,
          data: err,
        });
        console.log(json);
        console.log("fail json" + json);
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "PayResponseFailCallback",
          json
        );
        console.log("?????" + JSON.stringify(err));
      },
    });
  },

  QGPayTest: function (param, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var paramObj = JSON.parse(paramStr);

    qg.getSystemInfo({
      success: function (res) {
        if (
          !res.platformVersionCode ||
          res.platformVersionCode == null ||
          res.platformVersionCode == "undefined"
        ) {
          var json = JSON.stringify({
            errMsg:
              "Failed to obtain the fast application engine version. Procedure",
          });
          unityInstance.SendMessage(
            CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
            "PayResponseFailCallback",
            json
          );
        } else {
          var xhr = new XMLHttpRequest();
          xhr.open(
            "POST",
            "https://jits.open.oppomobile.com/jitsopen/api/pay/demo/preOrder"
          );
          xhr.setRequestHeader("Accept", "application/json");
          xhr.setRequestHeader("charset", "UTF-8");

          var dataObject = {
            openId: paramObj.openId,
            deviceInfo: paramObj.deviceInfo,
            model: res.model,
            ip: paramObj.ip,
            productName: paramObj.productName,
            productDesc: paramObj.productDesc,
            count: paramObj.count,
            price: paramObj.price,
            currency: paramObj.currency,
            cpOrderId: paramObj.cpOrderId,
            attach: paramObj.attach,
            appVersion: paramObj.appVersion,
            engineVersion: res.platformVersionCode.toString(),
            callBackUrl: paramObj.callBackUrl,
          };

          console.log("xhr.send::: ", JSON.stringify(dataObject));
          xhr.send(JSON.stringify(dataObject));
          xhr.onreadystatechange = function () {
            console.log("readyState: ", xhr.readyState);
            console.log("status: ", xhr.status);
            console.log("response: ", JSON.stringify(xhr.response));
            if (xhr.readyState == 4 && xhr.status == 200) {
              var data = JSON.parse(xhr.response).data;
              qg.pay({
                appId: paramObj.appId,
                token: paramObj.openId,
                timestamp: data.timestamp,
                orderNo: data.orderNo,
                paySign: data.paySign,
                success: function (res) {
                  var json = JSON.stringify({
                    callbackId: successID,
                    data: res.data,
                  });
                  unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "PayResponseCallback",
                    json
                  );
                  console.log("?????" + JSON.stringify(res));
                },
                fail: function (err) {
                  var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: err.errMsg,
                    errCode: err.errCode,
                    data: err,
                  });
                  console.log(json);
                  console.log("fail json" + json);
                  unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "PayResponseFailCallback",
                    json
                  );
                  console.log("?????" + JSON.stringify(err));
                },
              });
            }
          };
        }
      },
      fail: function (err) {
        var json = JSON.stringify({
          errMsg: err,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "PayResponseFailCallback",
          json
        );
        console.log("?????" + JSON.stringify(err));
      },
    });
  },

  QGPlayMedia: function (playerId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    console.log("playerId: ", playerId);
    var pdIdStr = UTF8ToString(playerId);
    var pd = mAdMap.get(pdIdStr);
    if (!pd || pd == null || pd == "undefined") {
      console.log("innerAudioContext is null");
      return;
    }

    pd.play();
    pd.onCanplay(function () {
      console.log("onCanplay success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_CANPLAY,
        json
      );
    });

    pd.onPlay(function () {
      console.log("onPlay success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_PLAY,
        json
      );
    });

    pd.onPause(function () {
      console.log("onPause success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_PAUSE,
        json
      );
    });

    pd.onStop(function () {
      console.log("onStop success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_STOP,
        json
      );
    });

    pd.onEnded(function () {
      console.log("onEndedfunction success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_ENDED,
        json
      );
    });

    pd.onTimeUpdate(function () {
      console.log("onTimeUpdate success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_TIMEUPDATE,
        json
      );
    });

    pd.onError(function () {
      console.log("onError success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_ERROR,
        json
      );
    });

    pd.onWaiting(function () {
      console.log("onWaiting success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_WAITING,
        json
      );
    });

    pd.onSeeking(function () {
      console.log("onSeeking success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_SEEKING,
        json
      );
    });

    pd.onSeeked(function () {
      console.log("onSeeked success");
      var json = JSON.stringify({
        callbackId: pdIdStr,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_SEEKED,
        json
      );
    });
  },

  QGPauseMedia: function (playerId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    var pdIdStr = UTF8ToString(playerId);
    var pd = mAdMap.get(pdIdStr);
    if (pd) {
      pd.pause();
    }
  },

  QGStopMedia: function (playerId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    var pdIdStr = UTF8ToString(playerId);
    var pd = mAdMap.get(pdIdStr);
    if (pd) {
      pd.stop();
    }
  },

  QGDestroyMedia: function (playerId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    var pdIdStr = UTF8ToString(playerId);
    var pd = mAdMap.get(pdIdStr);
    if (pd) {
      pd.destroy();
    }
  },

  QGSeekMedia: function (playerId, time) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (!(mAdMap instanceof Map)) {
      mAdMap = new Map();
    }
    var pdIdStr = UTF8ToString(playerId);
    var pd = mAdMap.get(pdIdStr);
    if (pd) {
      pd.seek(time);
    }
  },
  QGExitApplication: function (data, success, fail, complete) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);

    qg.exitApplication({
      data: data,
      success: function () {
        var json = JSON.stringify({
          callbackId: successID,
        });
        console.log("exitApplication success: ");
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function () {
        var json = JSON.stringify({
          callbackId: failID,
        });
        console.log("exitApplication fail: ");
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function () {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        console.log("exitApplication complete: ");
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGGetManifestInfo: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    qg.getManifestInfo({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
          data: JSON.parse(res.manifest),
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ManifestInfo",
          json
        );
        console.log("QGGetManifestInfo -js ", json);
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ManifestInfo",
          json
        );
        console.log("getManifestInfo err", JSON.stringify(err));
      },
    });
  },

  QGGetProvider: function (callback) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var callbackID = UTF8ToString(callback);
    var provider = qg.getProvider();
    var tempData = { provider: provider };
    var json = JSON.stringify({
      callbackId: callbackID,
      data: tempData,
    });
    unityInstance.SendMessage(
      CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
      "ProviderInfo",
      json
    );
    console.log("QGGetProvider", provider);
  },

  QGSetPreferredFramesPerSecond: function (fps) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    if (typeof fps == "number") {
      qg.setPreferredFramesPerSecond(fps);
      console.log("Change the render frame rate to ", fps);
    }
  },

  QGStartARCamera: function (imageCallback) {
    property.cameraObj = qg.createARCamera();
    property.cameraObj
      .start()
      .then(function (data) {
        console.log("QGStartARCamera -js SuccessCallback: ");
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "OnARCameraSuccess",
          "Success"
        );
        property.cameraData = data;
      })
      .catch(function (err) {
        console.log("QGStartARCamera -js FailCallback: ", JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "OnARCameraFail",
          JSON.stringify(err)
        );
      });
    property.cameraImageCallback = imageCallback;
  },

  QGStartARCameraYuv: function (imageCallback) {
    property.cameraObjYuv = qg.createARCameraYuv();
    property.cameraObjYuv
      .start()
      .then(function (data) {
        console.log("QGStartARCameraYuv -js SuccessCallback: ");
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "OnARCameraYuvSuccess",
          "Success"
        );
        property.cameraYuvData = data;
      })
      .catch(function (err) {
        console.log(
          "QGStartARCameraYuv -js FailCallback: ",
          JSON.stringify(err)
        );
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "OnARCameraYuvFail",
          JSON.stringify(err)
        );
      });
    property.cameraImageYuvCallback = imageCallback;
  },

  QGDestroyARCamera: function () {
    if (property.cameraObj != null) {
      property.cameraObj.destroy();
    }
    property.cameraObj = null;
    property.cameraData = null;
    property.cameraImageCallback = null;
  },

  QGDestroyARCameraYuv: function () {
    if (property.cameraObjYuv != null) {
      property.cameraObjYuv.destroy();
    }
    property.cameraObjYuv = null;
    property.cameraYuvData = null;
    property.cameraImageYuvCallback = null;
  },

  QGRequireARCameraImage: function () {
    if (!property.cameraImageCallback) {
      return;
    }
    if (!property.cameraData) {
      return;
    }
    var dataView = new Uint8Array(property.cameraData.data);
    var dataLen = dataView.length;
    if (!dataLen) {
      return;
    }
    var buffer = _malloc(dataLen);
    HEAPU8.set(dataView, buffer);
    dynCall("viiii", property.cameraImageCallback, [
      buffer,
      dataLen,
      property.cameraData.width,
      property.cameraData.height,
    ]);
    _free(buffer);
  },

  QGRequireARCameraImageYuv: function () {
    if (!property.cameraImageYuvCallback) {
      return;
    }
    if (!property.cameraYuvData) {
      return;
    }
    var dataView = new Uint8Array(property.cameraYuvData.data);
    var dataLen = dataView.length;

    if (!dataLen) {
      return;
    }
    var buffer = _malloc(dataLen);
    HEAPU8.set(dataView, buffer);
    dynCall("vii", property.cameraImageYuvCallback, [buffer, dataLen]);
    _free(buffer);
  },

  QGCreateOppoARPose: function (ArPoseCallback) {
    qg.createARPoseDetector()
      .then(function (res) {
        console.log("createARPoseDetector-js", JSON.stringify(res));
        property.cameraArPose = res;
      })
      .catch(function (err) {});
    property.cameraArPoseCallback = ArPoseCallback;
  },

  QGRequireARPose: function () {
    if (!property.cameraArPoseCallback) {
      return;
    }
    if (!property.cameraArPose) {
      return;
    }

    var posStr = property.cameraArPose.positionMatrix.join(",");
    var rotStr = property.cameraArPose.rotationMatrix.join(",");

    var posString = allocateUTF8(posStr);
    var rotSring = allocateUTF8(rotStr);
    dynCall("vii", property.cameraArPoseCallback, [posString, rotSring]);
  },

  QGCreateOppoARPlane: function (ArPlaneCallback) {
    qg.createARPlaneDetector()
      .then(function (res) {
        property.cameraArPlane = res;
        console.log("createARPlaneDetector-js", JSON.stringify(res));
      })
      .catch(function (err) {});
    property.cameraArPlaneCallback = ArPlaneCallback;
  },

  QGRequireARPlane: function () {
    if (!property.cameraArPlaneCallback) {
      return;
    }
    if (!property.cameraArPlane) {
      return;
    }

    var projectionMatrixStr = property.cameraArPlane.projectionMatrix.join(",");
    var viewMatrixStr = property.cameraArPlane.viewMatrix.join(",");
    var modelMatrixStr = property.cameraArPlane.modelMatrix.join(",");
    var modelViewMatrixStr = property.cameraArPlane.modelViewMatrix.join(",");
    var modelViewProjectionMatrixStr =
      property.cameraArPlane.modelViewProjectionMatrix.join(",");
    var planeAngleUvMatrixStr =
      property.cameraArPlane.planeAngleUvMatrix.join(",");
    var normalVectorStr = property.cameraArPlane.normalVector.join(",");
    var msgStr = property.cameraArPlane.msg;

    var projectionMatrixString = allocateUTF8(projectionMatrixStr);
    var viewMatrixString = allocateUTF8(viewMatrixStr);
    var modelMatrixString = allocateUTF8(modelMatrixStr);
    var modelViewMatrixString = allocateUTF8(modelViewMatrixStr);
    var modelViewProjectionMatrixString = allocateUTF8(
      modelViewProjectionMatrixStr
    );
    var planeAngleUvMatrixString = allocateUTF8(planeAngleUvMatrixStr);
    var normalVectorString = allocateUTF8(normalVectorStr);
    var msgString = allocateUTF8(msgStr);
    property.cameraArPlane = null;
    dynCall("viiiiiiii", property.cameraArPlaneCallback, [
      projectionMatrixString,
      viewMatrixString,
      modelMatrixString,
      modelViewMatrixString,
      modelViewProjectionMatrixString,
      planeAngleUvMatrixString,
      normalVectorString,
      msgString,
    ]);
  },

  QGSetUserCloudStorage: function (param, success, fail, complete) {
    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var tempKey = paramData.key;
    var tempValue = paramData.value;
    var kvDataItem = {};
    kvDataItem[tempKey] = tempValue;
    qg.setUserCloudStorage({
      KVDataList: [kvDataItem],
      success: function (res) {
        console.log("QGSetUserCloudStorage success -js", res);

        var json = JSON.stringify({
          callbackId: successID,
          errMsg: res.errMsg,
          code: res.code,
          errCode: res.errCode,
          data: res.data,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "SetUserCloudStorageCallBack",
          json
        );
      },
      fail: function (res) {
        console.log("QGSetUserCloudStorage fail -js", res);
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
          code: res.code,
          errCode: res.errCode,
          data: res.data,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "SetUserCloudStorageCallBack",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "SetUserCloudStorageCallBack",
          json
        );
      },
    });
  },

  QGGetUserCloudStorage: function (param, success, fail, complete) {
    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.getUserCloudStorage({
      keyList: [paramStr],
      success: function (res) {
        var tempKVDataList = JSON.parse(res.KVDataList);
        var cloudStorageValue = tempKVDataList[0][paramStr];
        if (cloudStorageValue) {
          var keyValue = {
            key: paramStr,
            value: cloudStorageValue,
          };
          var json = JSON.stringify({
            callbackId: successID,
            data: keyValue,
          });
          unityInstance.SendMessage(
            CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
            "GetUserCloudStorageCallBack",
            json
          );
        } else {
          console.log("There is no such value");
        }
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetUserCloudStorageCallBack",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetUserCloudStorageCallBack",
          json
        );
      },
    });
  },

  QGRemoveUserCloudStorage: function (param) {
    var paramStr = UTF8ToString(param);
    qg.removeUserCloudStorage({
      keyList: [paramStr],
      success: function (res) {},
      fail: function (res) {},
      complete: function (res) {},
    });
  },

  QGGetBatteryInfo: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.getBatteryInfo({
      success: function (res) {
        var BatteryInfo = {
          level: res.level,
          isCharging: res.isCharging,
        };
        var json = JSON.stringify({
          callbackId: successID,
          data: BatteryInfo,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetBatteryInfoCallBack",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetBatteryInfoCallBack",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetBatteryInfoCallBack",
          json
        );
      },
    });
  },

  QGGetBatteryInfoSync: function () {
    var batteryInfo = qg.getBatteryInfoSync();
    var json = JSON.stringify({
      level: batteryInfo.level,
      isCharging: batteryInfo.isCharging,
    });

    if (json) {
      var bufferSize = lengthBytesUTF8(json) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(json, buffer, bufferSize);
      return buffer;
    } else {
      console.log("??????????");
    }
  },

  QGGetDeviceId: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.getDeviceId({
      success: function (res) {
        var DeviceId = {
          deviceId: res.deviceId,
        };
        var json = JSON.stringify({
          callbackId: successID,
          data: DeviceId,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetDeviceIdCallBack",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetDeviceIdCallBack",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetDeviceIdCallBack",
          json
        );
      },
    });
  },

  QGGetScreenBrightness: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.getScreenBrightness({
      success: function (res) {
        var value = {
          value: res.value,
        };
        var json = JSON.stringify({
          callbackId: successID,
          data: value,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetScreenBrightnessCallBack",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetScreenBrightnessCallBack",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetScreenBrightnessCallBack",
          json
        );
      },
    });
  },

  QGSetScreenBrightness: function (param, success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.setScreenBrightness({
      value: param,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGSetKeepScreenOn: function (param, success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.setKeepScreenOn({
      keepScreenOn: param,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGGetLocation: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.getLocation({
      success: function (res) {
        var data = {
          latitude: res["latitude"],
          longitude: res["longitude"],
          speed: res["speed"],
          accuracy: res["accuracy"],
          altitude: res["altitude"],
          verticalAccuracy: res["verticalAccuracy"],
          horizontalAccuracy: res["horizontalAccuracy"],
        };
        var json = JSON.stringify({
          callbackId: successID,
          data: data,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetLocationCallBack",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetLocationCallBack",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          data: null,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetLocationCallBack",
          json
        );
      },
    });
  },

  QGOnAccelerometerChange: function (success) {
    var successID = UTF8ToString(success);
    qg.onAccelerometerChange(function (x, y, z) {
      var param = {
        QgParamX: x,
        QgParamY: y,
        QgParamZ: z,
      };
      var json = JSON.stringify({
        callbackId: successID,
        data: param,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnAccelerometerChangeCallBack",
        json
      );
      console.log("QGOnAccelerometerChange -js:", json);
    });
  },

  QGStartAccelerometer: function (param, success, fail, complete) {
    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.startAccelerometer({
      interval: paramStr,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGStopAccelerometer: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.stopAccelerometer({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGSetClipboardData: function (param, success, fail, complete) {
    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.setClipboardData({
      data: paramStr,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGGetClipboardData: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.getClipboardData({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetClipboardDataCallBack",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetClipboardDataCallBack",
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "GetClipboardDataCallBack",
          json
        );
      },
    });
  },

  QGStartCompass: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.startCompass({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGStopCompass: function (success, fail, complete) {
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    qg.stopCompass({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGOnCompassChange: function (success) {
    var successID = UTF8ToString(success);
    qg.onCompassChange(function (res) {
      var json = JSON.stringify({
        callbackId: successID,
        data: res.direction,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        "OnCompassChangeCallBack",
        json
      );
      console.log("QGOnCompassChange -js:", json);
    });
  },

  QGStartDeviceMotionListening: function (param) {
    var paramStr = UTF8ToString(param);
    qg.startDeviceMotionListening({
      interval: paramStr,
      success: function (res) {
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "StartDeviceMotionListeningSuccess",
          "Success"
        );
        console.log("startDeviceMotionListening success ", JSON.stringify(res));
      },
      fail: function (err) {
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "StartDeviceMotionListeningFail",
          "Fail"
        );
        console.log("startDeviceMotionListening fail ", JSON.stringify(err));
      },
      complete: function () {
        console.log("startDeviceMotionListening complete ");
      },
    });
  },

  QGStopDeviceMotionListening: function () {
    qg.stopDeviceMotionListening({
      success: function (res) {
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "StopDeviceMotionListeningSuccess",
          "Success"
        );
        console.log("stopDeviceMotionListening success ", JSON.stringify(res));
      },
      fail: function (err) {
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "StopDeviceMotionListeningFail",
          "Fail"
        );
        console.log("stopDeviceMotionListening fail ", JSON.stringify(err));
      },
      complete: function () {
        console.log("stopDeviceMotionListening complete ");
      },
    });
  },

  QGOnDeviceMotionChange: function (deviceMotionChangeCallback) {
    qg.onDeviceMotionChange(function (res) {
      // console.log("onDeviceMotionChange", JSON.stringify(res));
      property.deviceMotionChangeData = res;
    });
    property.deviceMotionChangeCallback = deviceMotionChangeCallback;
  },

  QGGetDeviceMotionChange: function () {
    if (!property.deviceMotionChangeCallback) {
      return;
    }
    var alphaStr = property.deviceMotionChangeData.alpha.toString();
    var betaStr = property.deviceMotionChangeData.beta.toString();
    var gammaStr = property.deviceMotionChangeData.gamma.toString();
    var alphaString = allocateUTF8(alphaStr);
    var betaString = allocateUTF8(betaStr);
    var gammaString = allocateUTF8(gammaStr);
    dynCall("viii", property.deviceMotionChangeCallback, [
      alphaString,
      betaString,
      gammaString,
    ]);
  },

  QGOffDeviceMotionChange: function () {
    qg.offDeviceMotionChange();
  },

  QGAccess: function (filename, success, fail, complete) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(filename);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);
    var fs = qg.getFileSystemManager();
    fs.access({
      path: localDir,
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("access success: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: err.errCode,
          errMsg: err.errMsg,
        });
        console.log("access fail: " + JSON.stringify(err));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
      complete: function (res) {
        var json = JSON.stringify({
          callbackId: completeID,
          errCode: res.errCode,
          errMsg: res.errMsg,
        });
        console.log("access complete: " + JSON.stringify(res));
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
          json
        );
      },
    });
  },

  QGAccessSync: function (filename, success, fail) {
    var qgDir = qg.env.USER_DATA_PATH;
    var localDir = qgDir + UTF8ToString(filename);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var fs = qg.getFileSystemManager();
    try {
      fs.accessSync(localDir);
      var json = JSON.stringify({
        callbackId: successID,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      console.log("accessSync success");
      return true;
    } catch (error) {
      var json = JSON.stringify({
        callbackId: failID,
      });
      unityInstance.SendMessage(
        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
        json
      );
      console.log("accessSync fail: " + error);
      return false;
    }
  },
};

autoAddDeps(QgGameBridge, "$mAdMap");
autoAddDeps(QgGameBridge, "$CONSTANT");
autoAddDeps(QgGameBridge, "$mFileData");
autoAddDeps(QgGameBridge, "$property");

mergeInto(LibraryManager.library, QgGameBridge);
