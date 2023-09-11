var rt_touch = {

  $RTTouchModule: {
    onTouchStart: function(res) {
      RTModule.sendMessage(
        "RTTouch",
        "_OnTouchStartCallback",
        res);
    },
    onTouchMove: function(res) {
      RTModule.sendMessage(
        "RTTouch",
        "_OnTouchMoveCallback",
        res);
    },    
    onTouchEnd: function(res) {
      RTModule.sendMessage(
        "RTTouch",
        "_OnTouchEndCallback",
        res);
    },
    onTouchCancel: function(res) {
      RTModule.sendMessage(
        "RTTouch",
        "_OnTouchCancelCallback",
        res);
    },
  },

  RTOnTouchStart: function() {
    console.log("jslib RTOnTouchStart");
    RTModule.runtime.UnityTouch.OnTouchStart(RTTouchModule.onTouchStart);
  },

  RTOffTouchStart: function() {
    console.log("jslib RTOffTouchStart");
    RTModule.runtime.UnityTouch.OffTouchStart();
  },
  RTOnTouchMove: function() {
    console.log("jslib RTOnTouchMove");
    RTModule.runtime.UnityTouch.OnTouchMove(RTTouchModule.onTouchMove);
  },

  RTOffTouchMove: function() {
    console.log("jslib RTOffTouchMove");
    RTModule.runtime.UnityTouch.OffTouchMove();
  },
  RTOnTouchEnd: function() {
    console.log("jslib RTOnTouchEnd");
    RTModule.runtime.UnityTouch.OnTouchEnd(RTTouchModule.onTouchEnd);
  },

  RTOffTouchEnd: function() {
    console.log("jslib RTOffTouchEnd");
    RTModule.runtime.UnityTouch.OffTouchEnd();
  },
  RTOnTouchCancel: function() {
    console.log("jslib RTOnTouchCancel");
    RTModule.runtime.UnityTouch.OnTouchCancel(RTTouchModule.onTouchCancel);
  },

  RTOffTouchCancel: function() {
    RTModule.runtime.UnityTouch.OffTouchCancel();
  }
};

autoAddDeps(rt_touch, "$RTTouchModule");
mergeInto(LibraryManager.library, rt_touch);
