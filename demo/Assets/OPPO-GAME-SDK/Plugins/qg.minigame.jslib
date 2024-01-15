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
  },
  $mAdMap: {},

  $mFileData: {},

  QGShowModal: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.showModal({
      title: "??",
      content: "??????API???????????????vConsole???vConsole?????",
      showCancel: false,
      success: function(res) {
        if (res.confirm) {
          console.log("??????");
        } else if (res.cancel) {
          console.log("??????");
        }
      },
    });
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
        networkType: data.networkType
      })
      console.log(data);
      console.log(json);
      unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "OnNetworkStatusChangeResponseCallback", json);
    }
    qg.onNetworkStatusChange(func)
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
      fail: function (res) { },
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
      fail: function (res) { },
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
        console.log("??????????")
      },
    });
  },

  QGGetSystemInfoSync: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var res = qg.getSystemInfoSync()
    var returnStr = JSON.stringify(res)
    console.log(returnStr)
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
      complete: function () { },
      fail: function () { },
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
      complete: function () { },
      fail: function () { },
    });
  },

  QGShowKeyboard: function (param, success, fail, complete) {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = UTF8ToString(param);
    var paramData = JSON.parse(paramStr);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);

    qg.showKeyboard({
      defaultValue: paramData.defaultValue,
      maxLength: paramData.maxLength,
      multiple: paramData.multiple,
      confirmHold: paramData.confirmHold,
      confirmType: paramData.confirmType,
      success: function () {
        var json = JSON.stringify({
          callbackId: successID,
        })
        console.log(json);
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,"ShowKeyboardResponseCallback", json);
        console.log("show keyboard success");
      },
      fail: function () {
        var json = JSON.stringify({
          callbackId: failID,
        })
        console.log(json);
        unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "ShowKeyboardResponseCallback", json);
        console.log("show keyboard fail");
      }
    })
  },


  QGOnKeyboardInput: function (callback) {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var callbackID = Pointer_stringify(callback);

    var func = function (data) {
      var json = JSON.stringify({
        callbackId: callbackID,
        value: data.value

      })
      unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "OnKeyboardInputResponseCallback", json);
    };

    qg.onKeyboardInput(func);
  },

  QGOffKeyboardInput: function () {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.offKeyboardInput();
  },

  QGOnKeyboardConfirm: function (callback) {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var callbackID = Pointer_stringify(callback);

    var func = function (data) {
      var json = JSON.stringify({
        callbackId: callbackID,
        value: data.value

      })
      unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "OnKeyboardInputResponseCallback", json);
    };
    qg.onKeyboardConfirm(func);
  },

  QGOffKeyboardConfirm: function (callback) {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.offKeyboardConfirm();
  },

  QGOnKeyboardComplete: function (callback) {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var callbackID = Pointer_stringify(callback);

    var func = function (data) {
      var json = JSON.stringify({
        callbackId: callbackID,
        value: data.value

      })
      unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "OnKeyboardInputResponseCallback", json);
    };
    qg.onKeyboardComplete(func);
  },

  QGOffKeyboardComplete: function (callback) {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.offKeyboardComplete();
  },

  QGHideKeyboard: function () {
    if (typeof (qg) == 'undefined') {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.hideKeyboard();
  },

  QGMkdir: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.mkdir({
      dirPath: localDir,
      encoding: "utf8",
      success: function () {
        console.log("???????" + localDir)
      },
      fail: function (res) {
        console.log("???????" + localDir + "?" + JSON.stringify(res))
      }
    })
  },

  QGRmdir: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.rmdir({
      dirPath: localDir,
      success: function () {
        console.log("???????" + localDir)
      },
      fail: function (res) {
        console.log("???????" + localDir + "?" + JSON.stringify(res))
      }
    })
  },

  QGIsExist: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    try {
      var dirStat = fs.statSync(localDir, false)
      var fileStat = fs.statSync(localFilePath, false)
      console.log(localDir + "??????" + dirStat.isDirectory() +
        localFilePath + "??????" + fileStat.isFile())
    } catch (error) {
      console.log(error + ", ??????????")
    }
  },

  QGRename: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.rename({
      oldPath: localDir,
      // newPath: `${qgDir}/new${Math.random()}`,
      newPath: qgDir + "/new/" + Math.random(),
      success: function () {
        console.log("????????" + localDir + "=>" + qgDir + "/new")
      },
      fail: function (res) {
        console.log("????????" + localDir + "?" + JSON.stringify(res))
      }
    })
  },

  QGSaveFile: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    qg.chooseImage({
      count: 1,
      sizeType: ['original'],
      sourceType: ['album'],
      success: function (imgRes) {
        fs.saveFile({
          filePath: localFilePath,
          tempFilePath: imgRes.tempFilePaths[0],
          success: function (res) {
            console.log("?????: " + res.savedFilePath)
          },
          fail: function (res) {
            console.log("error?" + JSON.stringify(res))
          }
        })
      },
      fail: function (e) {
        console.log("???????" + JSON.stringify(e))
      }
    })
  },

  QGReadDir: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.readdir({
      dirPath: localDir,
      success: function (res) {
        console.log("success?" + localDir + "?" + res.files)
      },
      fail: function (res) {
        console.log("error?" + localFilePath + "=>" + qgDir + "/newPath.txt?" + JSON.stringify(res))
      }
    })
  },

  QGWriteFile: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    var data = "Hello world."
    var encoding = "utf8"
    fs.writeFile({
      filePath: localFilePath,
      data: data,
      encoding: encoding,
      success: function () {
        console.log("???????" + localFilePath)
      },
      fail: function (res) {
        console.log(JSON.stringify(res))
      }
    })
  },

  QGReadFile: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.readFile({
      filePath: localFilePath,
      data: "Hello world.",
      encoding: "utf8",
      success: function (res) {
        console.log("???????" + localFilePath + "?" + res.data)
      },
      fail: function (res) {
        console.log("???????" + localFilePath + "?" + JSON.stringify(res))
      }
    })
  },

  QGAppendFile: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.appendFile({
      filePath: localFilePath,
      data: "Hello world.",
      encoding: "utf8",
      success: function () {
        console.log("???????" + localFilePath)
      },
      fail: function (res) {
        console.log("???????" + localFilePath + "?" + JSON.stringify(res))
      }
    })
  },

  QGCopyFile: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.copyFile({
      srcPath: localFilePath,
      destPath: qgDir + "/copy.txt",
      success: function () {
        console.log("???????" + localFilePath + "=>" + qgDir + "/copy.txt")
      },
      fail: function (res) {
        console.log("???????" + localFilePath + "?" + JSON.stringify(res))
      }
    })
  },

  QGRemoveSavedFile: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.removeSavedFile({
      filePath: localFilePath,
      success: function () {
        console.log("???????" + localFilePath)
      },
      fail: function (res) {
        console.log("????????" + localFilePath + "?" + JSON.stringify(res))
      }
    })
  },

  QGStat: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    fs.stat({
      path: localFilePath,
      success: function (res) {
        console.log("????????" + localFilePath + "?" + JSON.stringify(res))
      },
      fail: function (res) {
        console.log("????????" + localFilePath + "?" + JSON.stringify(res))
      }
    })
  },

  QGUnzip: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var localDir = qgDir + "/my"
    var fs = qg.getFileSystemManager()
    var tempFilePath = qg.env.USER_DATA_PATH + "/test.zip"
    var url = "https://cdofs.oppomobile.com/cdo-activity/static/201905/08/da1f253b1854d1c6353ec79c3e3e8145.zip"
    qg.downloadFile({
      // TODO: ??????????
      url: url,
      filePath: tempFilePath
    });
    fs.unzip({
      zipFilePath: tempFilePath,
      targetPath: qgDir,
      success: function (res) {
        console.log("???????" + qgDir)
      },
      fail: function (res) {
        console.log("???????" + qgDir + "?" + JSON.stringify(res))
      }
    })
  },

  QGGetFileInfo: function () {
    var qgDir = qg.env.USER_DATA_PATH
    var localFilePath = qgDir + "/my/file.txt"
    var fs = qg.getFileSystemManager()
    fs.getFileInfo({
      filePath: localFilePath,
      success: function (res) {
        console.log("success: " + localFilePath + "?" + "?????" + res.size + "??")
      },
      fail: function (res) {
        console.log("error?" + localFilePath + "?" + JSON.stringify(res))
      }
    })
  },

  QGPlayAudio: function () {
    var innerAudioContext = qg.createInnerAudioContext()
    innerAudioContext.src = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3"
    innerAudioContext.play()
    // CONSTANT.InnerAudioContext.src = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3"
    // CONSTANT.InnerAudioContext.play()
    console.log("??????")
  },
  QGPauseAudio: function () {
    var innerAudioContext = qg.createInnerAudioContext()
    innerAudioContext.pause()
    innerAudioContext.stop()
    // CONSTANT.InnerAudioContext.pause()
    console.log("????")
  },
  QGOnAudioInterruptionBegin: function () {
    qg.onAudioInterruptionBegin(function () {
      console.log("onAudioInterruptionBegin success: " + new Date().toLocaleString());
    })
    console.log("??qg.onAudioInterruptionBegin");
  },

  QGOffAudioInterruptionBegin: function () {
    qg.offAudioInterruptionBegin()
    console.log("????qg.onAudioInterruptionBegin");
  },

  QGOnAudioInterruptionEnd: function () {
    qg.onAudioInterruptionEnd(function () {
      console.log("onAudioInterruptionEnd success: " + new Date().toLocaleString());
    })
    console.log("??qg.onAudioInterruptionEnd");
  },

  QGOffAudioInterruptionEnd: function () {
    qg.offAudioInterruptionEnd()
    console.log("????qg.onAudioInterruptionEnd");
  },

  QGOnError: function () {
    qg.onError(function (res) {
      console.log("onError success: " + res.message.slice(0, 149));
    })
    console.log("????????");
  },

  QGOffError: function () {
    qg.offError()
    console.log("??????????");
  },

  QGDispatchError: function () {
    console.log("????Error");
    throw Error('dispatch Error')
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

  QGHasShortcutInstalled: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);

    qg.hasShortcutInstalled({
      success: function (res) {
        if (res == false) {
          qg.installShortcut({
            success: function (res) {
              console.log("?????????????" + JSON.stringify(res));
              var json = JSON.stringify({
                callbackId: successID,
                data: res,
              });
              console.log(res);
              console.log(res.data);
              console.log(json);
              unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                "HasShortcutInstalledResponseCallback",
                json
              );
            },
            fail: function (err) {
              console.log("?????????????" + JSON.stringify(err));
              var json = JSON.stringify({
                callbackId: failID,
                data: err,
              });
              console.log(err);
              console.log(json);
              unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                "HasShortcutInstalledResponseCallback",
                json
              );
            },
          });
        } else {
          console.log('???????')
        }
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: err.errMsg,
          errCode: err.errCode,
        });
        console.log(err);
        console.log(json);
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ShortcutResponseCallback",
          json
        );
      },
    });
  },

  QGInstallShortcut: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);

    qg.installShortcut({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res,
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
          errMsg: err,
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
      bannerAd.show();
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
      rewardedVideoAd.load();
      rewardedVideoAd.onLoad(function () {
        console.log("rewardedVideoAd onload success");
        rewardedVideoAd.show();
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
        interstitialAd.show();
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
      gameBannerAd
        .show()
        .then(function () {
          console.log("show success");
        })
        .catch(function (error) {
          console.log("show fail with:" + error.errCode + "," + error.errMsg);
        });
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
      gamePortalAd
        .load()
        .then(function () {
          console.log("load success");
        })
        .catch(function (error) {
          console.log("load fail with:" + error.errCode + "," + error.errMsg);
        });
      gamePortalAd.onLoad(function () {
        console.log("gamePortalAd onload success");
        gamePortalAd
          .show()
          .then(function () {
            console.log("gamePortalAd show success");
          })
          .catch(function (error) {
            console.log(
              "gamePortalAd show fail with:" +
              error.errCode +
              "," +
              error.errMsg
            );
          });
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
      GameDrawerAd.show()
        .then(function () {
          console.log("GameDrawerAd show success");
        })
        .catch(function (error) {
          console.log(
            "GameDrawerAd show fail with:" + error.errCode + "," + error.errMsg
          );
        });
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
      ad.hide()
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
    if (returnStr) {
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
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
  QGPay: function (param, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var paramObj = JSON.parse(paramStr)

    qg.pay({
      appId: paramObj.appId,
      token: paramObj.token,
      timestamp: paramObj.timestamp,
      orderNo: paramObj.orderNo,
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
          data: err
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
};

autoAddDeps(QgGameBridge, "$mAdMap");
autoAddDeps(QgGameBridge, "$CONSTANT");
autoAddDeps(QgGameBridge, "$mFileData");

mergeInto(LibraryManager.library, QgGameBridge);
