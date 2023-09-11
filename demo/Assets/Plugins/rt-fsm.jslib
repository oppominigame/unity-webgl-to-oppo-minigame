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
var rt_fsm = {

  $RTFSMModule: {

    init: function() {
      RTFSMModule.FSM = RTModule.runtime.getFileSystemManager();
      RTFSMModule.fileContentCache = {};
    },

    handleFileCallback: function(callbackId) {
      return {
        success: function(res) {
          RTModule.onCallbackParameter(
            "RTFileSystemManager",
            callbackId,
            0,
            "_onFile",
            res);
        },
        fail: function(res) {
          RTModule.onCallbackParameter(
            "RTFileSystemManager",
            callbackId,
            1,
            "_onFile",
            res);
        },
        complete: function() {
          RTModule.onCallbackNoParameter(
            "RTFileSystemManager",
            callbackId,
            2,
            "_onFile");
        }
      };
    }

  },

  RTCopyDataFromJS: function(callbackId, offset) {
    callbackId = Pointer_stringify(callbackId);
    HEAPU8.set(new Uint8Array(RTFSMModule.fileContentCache[callbackId]), offset);
    delete RTFSMModule.fileContentCache[callbackId];
  },

  RTAccess: function(callbackId, path) {
    var args = RTFSMModule.handleFileCallback(Pointer_stringify(callbackId));
    args.path = Pointer_stringify(path);
    RTFSMModule.FSM.access(args);
  },

  RTAccessSync: function(path) {
    var returnStr;
    try {
        returnStr = RTFSMModule.FSM.accessSync(Pointer_stringify(path));
        if (returnStr === undefined || returnStr === null) {
          returnStr = "";
        }
    } catch (e) {
      if (e === undefined) {
        returnStr = "unknown error";
      } else {
        returnStr = e;
      }
    }
    return RTModule.stringToBuffer(returnStr);
  },

  RTCopyFile: function(callbackId, srcPath, destPath) {
    var args = RTFSMModule.handleFileCallback(Pointer_stringify(callbackId));
    args.srcPath = Pointer_stringify(srcPath);
    args.destPath = Pointer_stringify(destPath);
    RTFSMModule.FSM.copyFile(args);
  },

  RTCopyFileSync: function(srcPath, destPath) {
    var returnStr;
    try {
        returnStr = RTFSMModule.FSM.copyFileSync(
          Pointer_stringify(srcPath),
          Pointer_stringify(destPath));
        if (returnStr === undefined || returnStr === null) {
          returnStr = "";
        }
    } catch (e) {
      if (e === undefined) {
        returnStr = "unknown error";
      } else {
        returnStr = e;
      }
    }
    return RTModule.stringToBuffer(returnStr);
  },

  RTReadFileBinary: function(callbackId, filePath) {
    callbackId = Pointer_stringify(callbackId);
    var args = RTModule.handleCallbackParameter(
      "RTFileSystemManager",
      callbackId,
      "_onFileReadBinary",
      function(res) {
        RTFSMModule.fileContentCache[callbackId] = res.data;
        return { errCode: res.errCode, errMsg: res.errMsg, data: res.data.byteLength };
      }
    );
    args.filePath = Pointer_stringify(filePath);
    args.encoding = "binary";
    RTFSMModule.FSM.readFile(args);
  },

  RTReadFileString: function(callbackId, filePath) {
    var args = RTModule.handleCallbackParameter(
      "RTFileSystemManager",
      Pointer_stringify(callbackId),
      "_onFileReadString");
    args.filePath = Pointer_stringify(filePath);
    args.encoding = "utf8";
    RTFSMModule.FSM.readFile(args);
  },

  RTReadFileStringSync: function(filePath) {
    var resObj = {};
    try {
      var result = RTFSMModule.FSM.readFileSync(Pointer_stringify(filePath), "utf8");
      if (result === undefined) {
        resObj.errCode = -1;
        resObj.errMsg = "unknown error";
      } else {
        resObj.errCode = 0;
        resObj.data = result;
      }
    } catch (error) {
      resObj.errCode = -1;
      resObj.errMsg = error;
    }
    return RTModule.stringToBuffer(JSON.stringify(resObj));
  },

  RTReadFileBinarySync: function(callbackId, filePath) {
    var resObj = {};
    callbackId = Pointer_stringify(callbackId);
    try {
      var result = RTFSMModule.FSM.readFileSync(Pointer_stringify(filePath), "binary");
      if (result === undefined) {
        resObj.errCode = -1;
        resObj.errMsg = "unknown error";
      } else {
        resObj.errCode = 0;
        resObj.data = result.byteLength;
        RTFSMModule.fileContentCache[callbackId] = result;
      }
    } catch (error) {
      resObj.errCode = -1;
      resObj.errMsg = error;
    }
    return RTModule.stringToBuffer(JSON.stringify(resObj));
  },

  RTUnlink: function(callbackId, filePath) {
    var args = RTFSMModule.handleFileCallback(Pointer_stringify(callbackId));
    args.filePath = Pointer_stringify(filePath);
    RTFSMModule.FSM.unlink(args);
  },

  RTUnlinkSync: function(filePath) {
    var returnStr;
    try {
        returnStr = RTFSMModule.FSM.unlinkSync(Pointer_stringify(filePath));
        if (returnStr === undefined || returnStr === null) {
          returnStr = "";
        }
    } catch (e) {
      if (e === undefined) {
        returnStr = "unknown error";
      } else {
        returnStr = e;
      }
    }
    return RTModule.stringToBuffer(returnStr);
  },

  RTWriteFileString: function(callbackId, filePath, data) {
    var args = RTFSMModule.handleFileCallback(Pointer_stringify(callbackId));
    args.filePath = Pointer_stringify(filePath);
    args.data = Pointer_stringify(data);
    args.encoding = "utf8";
    RTFSMModule.FSM.writeFile(args);
  },

  RTWriteFileBinary: function(callbackId, filePath, data, dataLength) {
    var args = RTFSMModule.handleFileCallback(Pointer_stringify(callbackId));
    args.filePath = Pointer_stringify(filePath);
    args.data = HEAPU8.slice(data, dataLength + data).buffer;
    args.encoding = "binary";
    RTFSMModule.FSM.writeFile(args);
  },

  RTWriteFileStringSync: function(filePath, data) {
    var returnStr;
    try {
      returnStr = RTFSMModule.FSM.writeFileSync(
        Pointer_stringify(filePath),
        Pointer_stringify(data),
        "utf8");
      if (returnStr === undefined || returnStr === null) {
        returnStr = "";
      }
    } catch (e) {
      if (e === undefined) {
        returnStr = "unknown error";
      } else {
        returnStr = e;
      }
    }
    return RTModule.stringToBuffer(returnStr);
  },

  RTWriteFileBinarySync: function(filePath, data, dataLength) {
    var returnStr;
    try {
      returnStr = RTFSMModule.FSM.writeFileSync(
        Pointer_stringify(filePath),
      HEAPU8.slice(data, dataLength + data),
      "binary");
      if (returnStr === undefined || returnStr === null) {
        returnStr = "";
      }
    } catch (e) {
      if (e === undefined) {
        returnStr = "unknown error";
      } else {
        returnStr = e;
      }
    }
    return RTModule.stringToBuffer(returnStr);
  }

};

autoAddDeps(rt_fsm, "$RTFSMModule");
mergeInto(LibraryManager.library, rt_fsm);
