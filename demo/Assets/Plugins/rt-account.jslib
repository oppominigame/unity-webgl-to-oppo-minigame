var rt_account = {

  $RTAccountModule: {
      handleLoginCallback: function(callbackId) {
      return {
        success: function(res) {
          RTModule.onCallbackParameter(
            "RTAccount",
            callbackId,
            0,
            "_onLogin",
            res);
        },
        fail: function(data, code) {
          var args = {
            data:data,
            code:code
          };
          RTModule.onCallbackParameter(
            "RTAccount",
            callbackId,
            1,
            "_onLogin",
            args);
        },
        complete: function() {
          RTModule.onCallbackNoParameter(
            "RTAccount",
            callbackId,
            2,
            "_onLogin");
        }
      };
    }
  },

  RTGameLogin: function(forcelogin, appid, callbackId) {
    var args = RTAccountModule.handleLoginCallback(Pointer_stringify(callbackId));
    args.forceLogin = forcelogin;
    args.appid = Pointer_stringify(appid);
    RTModule.runtime.gameLogin(args);
  }
};


autoAddDeps(rt_account, "$RTAccountModule");
mergeInto(LibraryManager.library, rt_account);
