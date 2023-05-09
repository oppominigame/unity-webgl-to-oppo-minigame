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
  // Storage
  QGStorageSetItem: function (keyName, keyValue) {
    var keyNameStr = UTF8ToString(keyName);
    var keyValueStr = UTF8ToString(keyValue);
    localStorage.setItem(keyNameStr, keyValueStr);
    console.log("QGStorageSetItem success");
  },
  QGStorageGetItem: function (keyName) {
    var keyNameStr = UTF8ToString(keyName);
    var val = localStorage.getItem(keyNameStr);
    console.log("QGStorageGetItem111: " + val);
    if (val) {
      console.log("QGStorageGetItem: " + val);
    } else {
      console.log("QGStorageGetItem No data");
    }
  },
  QGStorageRemoveItem: function (keyName) {
    var keyNameStr = UTF8ToString(keyName);
    localStorage.removeItem(keyNameStr);
    console.log("QGStorageRemoveItem: " + keyNameStr);
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
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "LoginResponseCallback",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
          errCode: res.errCode,
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
        // If the icon does not exist, create an icon
        if (res == false) {
          qg.installShortcut({
            success: function () {
              // Perform user created icon rewards
              console.log("qg.installShortcut create icon success");
            },
            fail: function (err) {
              console.log("qg.installShortcut create icon fail");
            },
            complete: function () {
              console.log("qg.installShortcut create icon  called");
            },
          });
        } else {
          console.log("desktop icon has been created");
        }
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
          errCode: res.errCode,
        });
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

    qg.installShortcut({success: function (res) {
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
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res,
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
            console.log("gamePortalAd show fail with:" + error.errCode + "," + error.errMsg);
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
            "GameDrawerAd show fail with:" +
              error.errCode +
              "," +
              error.errMsg
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
    console.log("adIdStr11111111111111111111："+ adIdStr);
    var ad = mAdMap.get(adIdStr);
    console.log("ad1111111111111111111111111："+ ad);



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

  QGStorageSetIntSync: function (key, value) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);
    var valueStr = value + "";

    qg.setStorageSync({
      key: keyStr,
      value: valueStr,
    });
  },

  QGStorageGetIntSync: function (key, defaultValue) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);
    var defaultValueStr = defaultValue + "";

    var result = qg.getStorageSync({
      key: keyStr,
      default: defaultValueStr,
    });
    return parseInt(result);
  },

  QGStorageSetStringSync: function (key, value) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);
    var valueStr = UTF8ToString(value);

    qg.setStorageSync({
      key: keyStr,
      value: valueStr,
    });
  },

  QGStorageGetStringSync: function (key, defaultValue) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);
    var defaultValueStr = UTF8ToString(defaultValue);

    var result = qg.getStorageSync({
      key: keyStr,
      default: defaultValueStr,
    });

    var bufferSize = lengthBytesUTF8(result) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(result, buffer, bufferSize);

    return buffer;
  },

  QGStorageSetFloatSync: function (key, value) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);
    var valueStr = value + "";

    qg.setStorageSync({
      key: keyStr,
      value: valueStr,
    });
  },

  QGStorageGetFloatSync: function (key, defaultValue) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);
    var defaultValueStr = defaultValue + "";

    var result = qg.getStorageSync({
      key: keyStr,
      default: defaultValueStr,
    });
    return parseFloat(result);
  },

  QGStorageDeleteAllSync: function () {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    qg.clearStorageSync();
  },

  QGStorageDeleteKeySync: function (key) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);
    qg.deleteStorageSync({
      key: keyStr,
    });
  },

  QGStorageHasKeySync: function (key) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var keyStr = UTF8ToString(key);

    var result = qg.getStorageSync({
      key: keyStr,
    });

    return result === "" ? false : true;
  },
  // pay
  QGPay: function (param, success, fail, complete) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var paramStr = UTF8ToString(param);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);
    var completeID = UTF8ToString(complete);

    qg.pay({
      appId: paramStr.appId,
      token: paramStr.token,
      timestamp: paramStr.timestamp,
      orderNo: paramStr.orderNo,
      paySign: paramStr.paySign,
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
      },
      fail: function (err) {
        var json = JSON.stringify({
          callbackId: failID,
          data: err.data,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "PayResponseCallback",
          json
        );
      },
      complete: function () {
        var json = JSON.stringify({
          callbackId: completeID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "PayResponseCallback",
          json
        );
      },
    });
  },

  QGAccessFile: function (path) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var pathStr = UTF8ToString(path);
    var fs = qg.getFileSystemManager();
    var result = fs.access({
      path: pathStr,
      success: function (res) {
        console.log("QGAccessFile  have");
      },
      fail: function (err) {
        console.log("QGAccessFile  nohave");
      },
    });

    var bufferSize = lengthBytesUTF8(result) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(result, buffer, bufferSize);

    return buffer;
  },

  QGReadFile: function (uri, encoding, position, length, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var uriStr = UTF8ToString(uri);
    var encodingStr = UTF8ToString(encoding);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);

    qg.readFile({
      uri: uriStr,
      encoding: encodingStr,
      position: position,
      length: length,
      success: function (data) {
        if (encodingStr == "binary") {
          mFileData[successID] = data.text;
        }
        var json = JSON.stringify({
          callbackId: successID,
          textStr: data.text,
          encoding: encodingStr,
          byteLength: data.text.byteLength,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ReadFileResponseCallback",
          json
        );
      },
      fail: function (data, code) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: code,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "ReadFileResponseCallback",
          json
        );
      },
    });
  },

  QGGetFileBuffer: function (buffer, callBackId) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var callBackIdStr = UTF8ToString(callBackId);
    HEAPU8.set(new Uint8Array(mFileData[callBackIdStr]), buffer);
    delete mFileData[callBackIdStr];
  },

  QGWriteFile: function (filePath, data, encoding, success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }
    var fs = qg.getFileSystemManager();
    var qgDir = qg.env.USER_DATA_PATH;
    var localFilePath = qgDir + "/my/file.txt";
    // var localDir = `${qgDir}/my`;
    var filePathStr = UTF8ToString(filePath);
    var dataStrFinal = UTF8ToString(data);
    // var appendStr = UTF8ToString(append);
    var encodingStr = UTF8ToString(encoding);
    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);

    fs.writeFile({
      filePath: localFilePath,
      encoding: encodingStr,
      data: dataStrFinal,
      success: function (filePath) {
        var json = JSON.stringify({
          callbackId: successID,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "WriteFileResponseCallback",
          json
        );
      },
      fail: function (data, code) {
        var json = JSON.stringify({
          callbackId: failID,
          errCode: code,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "WriteFileResponseCallback",
          json
        );
      },
    });
  },
};

autoAddDeps(QgGameBridge, "$mAdMap");
autoAddDeps(QgGameBridge, "$CONSTANT");
autoAddDeps(QgGameBridge, "$mFileData");

mergeInto(LibraryManager.library, QgGameBridge);
