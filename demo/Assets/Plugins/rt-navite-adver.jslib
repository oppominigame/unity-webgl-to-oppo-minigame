var rt_native_adver = {

    $RTNativeAdverModule: {
        init: function() {
            RTNativeAdverModule.nativeAd = null;
            RTNativeAdverModule.objname = null;
        },

        onLoadError: function(res) {
            RTModule.sendMessage(
                RTNativeAdverModule.objname,
                "_onLoadErrorCallback",
                JSON.stringify(res));
        },

        onLoad: function(res) {
            RTModule.sendMessage(
                RTNativeAdverModule.objname,
                "_onLoadSuccCallback",
                JSON.stringify(res));
        },

        handleCreateNativeAdCallback: function(callbackId) {
            return {
                success: function(code) {
                    var args = {
                        code:code
                    };
                    RTModule.onCallbackParameter(
                        RTNativeAdverModule.objname,
                        callbackId,
                        0,
                        "_onCreateNativeAd",
                        args);
                },
                fail: function(data, code) {
                    var args = {
                        data:data,
                        code:code
                    };
                    RTModule.onCallbackParameter(
                        RTNativeAdverModule.objname,
                        callbackId,
                        1,
                        "_onCreateNativeAd",
                        args);
                },
                complete: function() {
                    RTModule.onCallbackNoParameter(
                        RTNativeAdverModule.objname,
                        callbackId,
                        2,
                        "_onCreateNativeAd");
                }
            };
        }
    },

    /* ????????? nativeAd???????? */
    RTCreateNativeAd: function(objname, adUnitId, callbackId) {
        var args = RTNativeAdverModule.handleCreateNativeAdCallback(Pointer_stringify(callbackId));
        args.adUnitId = Pointer_stringify(adUnitId);
        RTNativeAdverModule.nativeAd = RTModule.runtime.createNativeAd(args);
        RTNativeAdverModule.objname = Pointer_stringify(objname);
        console.log("chenkangad RTNativeAdverModule.nativeAd" + JSON.stringify(RTNativeAdverModule.nativeAd));
    },

    RTLoad: function() {
        RTNativeAdverModule.nativeAd.load();
    },

    RTOnLoadError: function() {
        RTNativeAdverModule.nativeAd.onError(RTNativeAdverModule.onLoadError);
    },


    RTOnLoad: function() {
        RTNativeAdverModule.nativeAd.onLoad(RTNativeAdverModule.onLoad);
    }
};

autoAddDeps(rt_native_adver, "$RTNativeAdverModule");
mergeInto(LibraryManager.library, rt_native_adver);