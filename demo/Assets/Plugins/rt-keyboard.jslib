/*******************************************************************************
Xiamen Yaji Software Co., Ltd., (the “Licensor”) grants the user (the “Licensee”
) non-exclusive and non-transferable rights to use the software according to
the following conditions:
a.  The Licensee shall pay royalties to the Licensor, and the amount of those
    royalties and the payment method are subject to separate negotiations
    between the parties.
b.  The software is licensed for use rather than sold, and the Licensor reserves
    all rights over the software that are not expressly granted (whether by
    implication, reservation or prohibition).
c.  The open source codes contained in the software are subject to the MIT Open
    Source Licensing Agreement (see the attached for the details);
d.  The Licensee acknowledges and consents to the possibility that errors may
    occur during the operation of the software for one or more technical
    reasons, and the Licensee shall take precautions and prepare remedies for
    such events. In such circumstance, the Licensor shall provide software
    patches or updates according to the agreement between the two parties. the
    Licensor will not assume any liability beyond the explicit wording of this
    Licensing Agreement.
e.  Where the Licensor must assume liability for the software according to
    relevant laws, the Licensor’s entire liability is limited to the annual
    royalty payable by the Licensee.
f.  The Licensor owns the portions listed in the root directory and subdirectory
    (if any) in the software and enjoys the intellectual property rights over
    those portions. As for the portions owned by the Licensor, the Licensee
    shall not:
    i.  Bypass or avoid any relevant technical protection measures in the
        products or services;
    ii. Release the source codes to any other parties;
    iii.Disassemble, decompile, decipher, attack, emulate, exploit or
        reverse-engineer these portion of code;
    iv. Apply it to any third-party products or services without Licensor’s
        permission;
    v.  Publish, copy, rent, lease, sell, export, import, distribute or lend any
        products containing these portions of code;
    vi. Allow others to use any services relevant to the technology of these
        codes; and
    vii.Conduct any other act beyond the scope of this Licensing Agreement.
g.  This Licensing Agreement terminates immediately if the Licensee breaches
    this Agreement. The Licensor may claim compensation from the Licensee where
    the Licensee’s breach causes any damage to the Licensor.
h.  The laws of the People's Republic of China apply to this Licensing Agreement.
i.  This Agreement is made in both Chinese and English, and the Chinese version
    shall prevail the event of conflict.

*******************************************************************************/
var rt_keyboard = {

  $RTKeyboardModule: {

    onKeyboardInput: function(res) {
      RTModule.sendMessage(
        "RTKeyboard",
        "_onKeyboardInputCallback",
        res.value);
    },

    onKeyboardConfirm: function(res) {
      RTModule.sendMessage(
        "RTKeyboard",
        "_onKeyboardConfirmCallback",
        res.value);
    },

    onKeyboardComplete: function(res) {
      RTModule.sendMessage(
        "RTKeyboard",
        "_onKeyboardCompleteCallback",
        res.value);
    }

  },

  RTHideKeyboard: function(callbackId) {
    var args = RTModule.handleCallbackNoParameter(
      "RTKeyboard",
      Pointer_stringify(callbackId));
    RTModule.runtime.hideKeyboard(args);
  },

  RTShowKeyboard: function(callbackId, defaultValue, maxLength, multiple, confirmHold, confirmType) {
    var args = RTModule.handleCallbackNoParameter("RTKeyboard", Pointer_stringify(callbackId));
    args.defaultValue = Pointer_stringify(defaultValue);
    args.maxLength = maxLength;
    args.multiple = (multiple == 1);
    args.confirmHold = (confirmHold == 1);
    args.confirmType = Pointer_stringify(confirmType);
    RTModule.runtime.showKeyboard(args);
  },

  RTUpdateKeyboard: function(callbackId, value) {
    var args = RTModule.handleCallbackParameter(
      "RTKeyboard",
      Pointer_stringify(callbackId),
      "_onUpdateKeyboard",
      function(res) {
        return { message: res.errMsg };
      },
      function(res) {
        return { message: res.errMsg };
      }
    );
    args.value = Pointer_stringify(value);
    RTModule.runtime.updateKeyboard(args);
  },

  RTOnKeyboardInput: function() {
    RTModule.runtime.onKeyboardInput(RTKeyboardModule.onKeyboardInput);
  },

  RTOffKeyboardInput: function() {
    RTModule.runtime.offKeyboardInput(RTKeyboardModule.onKeyboardInput);
  },

  RTOnKeyboardConfirm: function() {
    RTModule.runtime.onKeyboardConfirm(RTKeyboardModule.onKeyboardConfirm);
  },

  RTOffKeyboardConfirm: function() {
    RTModule.runtime.offKeyboardConfirm(RTKeyboardModule.onKeyboardConfirm);
  },

  RTOnKeyboardComplete: function() {
    RTModule.runtime.onKeyboardComplete(RTKeyboardModule.onKeyboardComplete);
  },

  RTOffKeyboardComplete: function() {
    RTModule.runtime.offKeyboardComplete(RTKeyboardModule.onKeyboardComplete);
  }

};

autoAddDeps(rt_keyboard, "$RTKeyboardModule");
mergeInto(LibraryManager.library, rt_keyboard);
