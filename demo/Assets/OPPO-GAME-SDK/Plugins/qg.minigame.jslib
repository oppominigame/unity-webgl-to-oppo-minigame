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
      title: "提示",
      content: "如需查看功能API接口详细日志信息打印，请先打开vConsole，点击vConsole进行查看。",
      showCancel: false,
      success: function(res) {
        if (res.confirm) {
          console.log("用户点击确定");
        } else if (res.cancel) {
          console.log("用户点击取消");
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
        console.log("获取网络状态：" + res.networkType);
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
        console.log("获取网络状态失败：" + err);
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
    //     "当前是否有网络：" + res.isConnected + "," + "网络类型为：" + res.networkType
    //   )
    // });
  },

  QGVibrateShort: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    qg.vibrateShort({
      type: "light", // heavy、medium、light
      success: function (res) {
        console.log("短振动-light");
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
        console.log("长振动");
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
        console.log("异步");
        console.log("手机品牌 brand：" + res.brand);
        console.log("系统语言 language：" + res.language);
        console.log("手机型号 model：" + res.model);
        console.log("状态栏/异形缺口高度 statusBarHeight：" + res.statusBarHeight);
        console.log("设备像素比 pixelRatio：" + res.pixelRatio);
        console.log("客户端平台 platformVersionName：" + res.platformVersionName);
        console.log("平台版本号 platformVersionCode：" + res.platformVersionCode);
        console.log("屏幕高度 screenHeight：" + res.screenHeight);
        console.log("屏幕宽度 screenWidth：" + res.screenWidth);
        console.log("系统版本 system：" + res.system);
        console.log("可使用窗口高度 windowHeight：" + res.windowHeight);
        console.log("可使用窗口宽度 windowWidth：" + res.windowWidth);
        console.log("系统当前主题 theme：" + res.theme);
        console.log("设备方向 deviceOrientation：" + res.deviceOrientation);
        console.log("版本号 COREVersion：" + res.COREVersion);
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
        console.log("异步获取系统信息失败")
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
      console.log("获取同步系统信息失败");
    }
    // console.log("同步");
    // console.log("手机品牌 brand：" + res.brand);
    // console.log("系统语言 language：" + res.language);
    // console.log("手机型号 model：" + res.model);
    // console.log("状态栏/异形缺口高度 statusBarHeight：" + res.statusBarHeight);
    // console.log("设备像素比 pixelRatio：" + res.pixelRatio);
    // console.log("客户端平台 platformVersionName：" + res.platformVersionName);
    // console.log("平台版本号 platformVersionCode：" + res.platformVersionCode);
    // console.log("屏幕高度 screenHeight：" + res.screenHeight);
    // console.log("屏幕宽度 screenWidth：" + res.screenWidth);
    // console.log("系统版本 system：" + res.system);
    // console.log("可使用窗口高度 windowHeight：" + res.windowHeight);
    // console.log("可使用窗口宽度 windowWidth：" + res.windowWidth);
    // console.log("系统当前主题 theme：" + res.theme);
    // console.log("设备方向 deviceOrientation：" + res.deviceOrientation);
    // console.log("版本号 COREVersion：" + res.COREVersion);
  },

  // vConsole
  QGSetEnableDebugTrue: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.setEnableDebug({
      enableDebug: true, // true 为打开，false 为关闭
      success: function () {
        // 以下语句将会在 vConsole 面板输出
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
      enableDebug: false, // true 为打开，false 为关闭
      success: function () {
        // 以下语句将会在 vConsole 面板输出
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
        console.log("创建目录成功：" + localDir)
      },
      fail: function (res) {
        console.log("创建目录失败：" + localDir + "，" + JSON.stringify(res))
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
        console.log("删除目录成功：" + localDir)
      },
      fail: function (res) {
        console.log("删除目录失败：" + localDir + "，" + JSON.stringify(res))
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
      console.log(localDir + "是否是目录：" + dirStat.isDirectory() +
        localFilePath + "是否是文件：" + fileStat.isFile())
    } catch (error) {
      console.log(error + ", 请创建目录、写入文件")
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
        console.log("重命名目录成功：" + localDir + "=>" + qgDir + "/new")
      },
      fail: function (res) {
        console.log("重命名目录失败：" + localDir + "，" + JSON.stringify(res))
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
            console.log("保存地址为: " + res.savedFilePath)
          },
          fail: function (res) {
            console.log("error：" + JSON.stringify(res))
          }
        })
      },
      fail: function (e) {
        console.log("选择图片失败：" + JSON.stringify(e))
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
        console.log("success：" + localDir + "，" + res.files)
      },
      fail: function (res) {
        console.log("error：" + localFilePath + "=>" + qgDir + "/newPath.txt，" + JSON.stringify(res))
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
        console.log("写入文件成功：" + localFilePath)
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
        console.log("读取文件成功：" + localFilePath + "，" + res.data)
      },
      fail: function (res) {
        console.log("读取文件失败：" + localFilePath + "，" + JSON.stringify(res))
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
        console.log("追加文件成功：" + localFilePath)
      },
      fail: function (res) {
        console.log("追加文件失败：" + localFilePath + "，" + JSON.stringify(res))
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
        console.log("复制文件成功：" + localFilePath + "=>" + qgDir + "/copy.txt")
      },
      fail: function (res) {
        console.log("复制文件失败：" + localFilePath + "，" + JSON.stringify(res))
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
        console.log("删除文件成功：" + localFilePath)
      },
      fail: function (res) {
        console.log("追删除文件失败：" + localFilePath + "，" + JSON.stringify(res))
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
        console.log("获取文件信息成功" + localFilePath + "：" + JSON.stringify(res))
      },
      fail: function (res) {
        console.log("获取文件信息失败" + localFilePath + "，" + JSON.stringify(res))
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
      // TODO: 替换自己的文件压缩包
      url: url,
      filePath: tempFilePath
    });
    fs.unzip({
      zipFilePath: tempFilePath,
      targetPath: qgDir,
      success: function (res) {
        console.log("解压文件成功：" + qgDir)
      },
      fail: function (res) {
        console.log("解压文件失败：" + qgDir + "，" + JSON.stringify(res))
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
        console.log("success: " + localFilePath + "，" + "文件大小：" + res.size + "字节")
      },
      fail: function (res) {
        console.log("error：" + localFilePath + "，" + JSON.stringify(res))
      }
    })
  },

  QGPlayAudio: function () {
    var innerAudioContext = qg.createInnerAudioContext()
    innerAudioContext.src = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3"
    innerAudioContext.play()
    // CONSTANT.InnerAudioContext.src = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3"
    // CONSTANT.InnerAudioContext.play()
    console.log("播放远程音频")
  },
  QGPauseAudio: function () {
    var innerAudioContext = qg.createInnerAudioContext()
    innerAudioContext.pause()
    innerAudioContext.stop()
    // CONSTANT.InnerAudioContext.pause()
    console.log("暂停音频")
  },
  QGOnAudioInterruptionBegin: function () {
    qg.onAudioInterruptionBegin(function () {
      console.log("onAudioInterruptionBegin success: " + new Date().toLocaleString());
    })
    console.log("监听qg.onAudioInterruptionBegin");
  },

  QGOffAudioInterruptionBegin: function () {
    qg.offAudioInterruptionBegin()
    console.log("取消监听qg.onAudioInterruptionBegin");
  },

  QGOnAudioInterruptionEnd: function () {
    qg.onAudioInterruptionEnd(function () {
      console.log("onAudioInterruptionEnd success: " + new Date().toLocaleString());
    })
    console.log("监听qg.onAudioInterruptionEnd");
  },

  QGOffAudioInterruptionEnd: function () {
    qg.offAudioInterruptionEnd()
    console.log("取消监听qg.onAudioInterruptionEnd");
  },

  QGOnError: function () {
    qg.onError(function (res) {
      console.log("onError success: " + res.message.slice(0, 149));
    })
    console.log("监听全局错误事件");
  },

  QGOffError: function () {
    qg.offError()
    console.log("取消监听全局错误事件");
  },

  QGDispatchError: function () {
    console.log("模拟触发Error");
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
              console.log("调起创建桌面图标弹窗成功：" + JSON.stringify(res));
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
              console.log("调起创建桌面图标弹窗失败：" + JSON.stringify(err));
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
          console.log('桌面图标已创建')
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
  //   console.log("请求隐藏BannerAD : HOME");
  //   if (!!this.bannerAd) {
  //       this.bannerAd.hide();
  //   }
  // },
  // // destroyBannerAd
  // QGdestroyBannerAd: function () {
  //   console.log("请求毁掉BannerAD : HOME");
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
      console.log("暂无数据");
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
        console.log("支付成功：" + JSON.stringify(res));
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
        console.log("支付失败：" + JSON.stringify(err));
      },
    });
  },
};

autoAddDeps(QgGameBridge, "$mAdMap");
autoAddDeps(QgGameBridge, "$CONSTANT");
autoAddDeps(QgGameBridge, "$mFileData");

mergeInto(LibraryManager.library, QgGameBridge);
