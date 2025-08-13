using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class filemanager : MonoBehaviour
{
    public Button comebackbtn;

    public Button downLoadFilebtn;

    public Button getFileInfobtn;

    public Button accessbtn;
    public Button accessSyncbtn;
    public Button statbtn;
    public Button statSyncbtn;
    public Button unzipbtn;
    public Button writeFilebtn;
    public Button writeFileSyncbtn;
    public Button appendFilebtn;

    public Button appendFileSyncbtn;

    public Button copyFilebtn;

    public Button copyFileSyncbtn;

    public Button renamebtn;

    public Button renameSyncbtn;

    public Button saveFilebtn;

    public Button saveFileSyncbtn;

    public Button removeSavedFilebtn;

    public Button unlinkbtn;

    public Button unlinkSyncbtn;

    public Button mkdirbtn;

    public Button mkdirSyncbtn;

    public Button rmdirbtn;

    public Button rmdirSyncbtn;

    public Button readFilebtn;

    public Button readFileSyncbtn;

    public Button readdirbtn;

    public Button readdirSyncbtn;

    public Text loginMessage;

    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        downLoadFilebtn.onClick.AddListener(downLoadFilefunc);
        getFileInfobtn.onClick.AddListener(getFileInfofunc);
        accessbtn.onClick.AddListener(accessfunc);
        accessSyncbtn.onClick.AddListener(accessSyncfunc);
        statbtn.onClick.AddListener(statfunc);
        statSyncbtn.onClick.AddListener(statSyncfunc);
        unzipbtn.onClick.AddListener(unzipfunc);

        writeFilebtn.onClick.AddListener(writeFilefunc);
        writeFileSyncbtn.onClick.AddListener(writeFileSyncfunc);
        appendFilebtn.onClick.AddListener(appendFilefunc);
        appendFileSyncbtn.onClick.AddListener(appendFileSyncfunc);
        copyFilebtn.onClick.AddListener(copyFilefunc);
        copyFileSyncbtn.onClick.AddListener(copyFileSyncfunc);
        renamebtn.onClick.AddListener(renamefunc);
        renameSyncbtn.onClick.AddListener(renameSyncfunc);

        saveFilebtn.onClick.AddListener(saveFilefunc);
        saveFileSyncbtn.onClick.AddListener(saveFileSyncfunc);

        removeSavedFilebtn.onClick.AddListener(removeSavedFilefunc);

        unlinkbtn.onClick.AddListener(unlinkfunc);
        unlinkSyncbtn.onClick.AddListener(unlinkSyncfunc);
        mkdirbtn.onClick.AddListener(mkdirfunc);
        mkdirSyncbtn.onClick.AddListener(mkdirSyncfunc);
        rmdirbtn.onClick.AddListener(rmdirfunc);
        rmdirSyncbtn.onClick.AddListener(rmdirSyncfunc);
        readFilebtn.onClick.AddListener(readFilefunc);
        readFileSyncbtn.onClick.AddListener(readFileSyncfunc);

        readdirbtn.onClick.AddListener(readdirfunc);
        readdirSyncbtn.onClick.AddListener(readdirSyncfunc);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    public void downLoadFilefunc()
    {
        QG.DownLoadFile(new DownLoadFileParam()
        {
            path = "/test.zip",
            url = "https://cdofs.oppomobile.com/cdo-activity/static/201905/08/da1f253b1854d1c6353ec79c3e3e8145.zip",
        }, (success) =>
        {
            Debug.Log("QG.DownLoadFile success = " + JsonUtility.ToJson(success));
            loginMessage.text = "QG.DownLoadFile success = " + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.DownLoadFile fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "QG.DownLoadFile fail = " + JsonUtility.ToJson(fail);
        }
       );
    }

    public void getFileInfofunc()
    {
        string filename = "/test.zip";
        QG.GetFileInfo(filename, (success) =>
      {
          Debug.Log("QG.GetFileInfo success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.GetFileInfo success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.GetFileInfo fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.GetFileInfo fail = " + JsonUtility.ToJson(fail);
      }
     );
    }

    public void accessfunc()
    {
        string filename = "/test.zip";
        QG.Access(filename, (success) =>
      {
          Debug.Log("QG.Access success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.Access success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.Access fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.Access fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.Access complete = " + JsonUtility.ToJson(complete));
      }
     );
    }


    public void accessSyncfunc()
    {
        string filename = "/test.zip";
        bool isAccessSync = QG.AccessSync(filename, (success) =>
           {
               Debug.Log("QG.AccessSync success = " + JsonUtility.ToJson(success));
           },
           (fail) =>
           {
               Debug.Log("QG.AccessSync fail = " + JsonUtility.ToJson(fail));
           }
          );
        loginMessage.text = "QG.AccessSync 同步判斷文件/目錄是否存在 = " + isAccessSync;
    }

    public void statfunc()
    {
        string path = "/test.zip";
        QG.Stat(path, (success) =>
      {
          StatResponse res = JsonUtility.FromJson<StatResponse>(JsonUtility.ToJson(success));
          Debug.Log("QG.Stat success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.Stat success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.Stat fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.Stat fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.Stat complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void statSyncfunc()
    {
        string path = "/test.zip";
        bool recursive = false;
        StatResponse res = QG.StatSync(path, recursive, (success) =>
        {
            StatResponse statResponse = JsonUtility.FromJson<StatResponse>(JsonUtility.ToJson(success));
            Debug.Log("QG.StatSync success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.StatSync fail = " + JsonUtility.ToJson(fail));
        }
       );
        if (res == null)
        {
            loginMessage.text = "QG.StatSync fail";
            return;
        }
        loginMessage.text = "QG.StatSync success\n mode:" + res.mode + "\n size:" + res.size + "\n lastAccessedTime:" + res.lastAccessedTime + "\n lastModifiedTime:" + res.lastModifiedTime + "\n isDirectory:" + res.isDirectory + "\n isFile:" + res.isFile;
    }

    public void unzipfunc()
    {
        string zipFilePath = "/test.zip";
        string targetPath = "/";
        QG.Unzip(zipFilePath, targetPath, (success) =>
      {
          Debug.Log("QG.Unzip success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.Unzip success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.Unzip fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.Unzip fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.Unzip complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void appendFilefunc()
    {
        string filename = "/myfile.txt";
        string append_data = "appendFile data";
        byte[] append_byteArray = new byte[] { 1, 2, 3, 4 };
        QG.AppendFile(filename, append_byteArray, (success) =>
      {
          Debug.Log("QG.AppendFile success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.AppendFile success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.AppendFile fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.AppendFile fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.AppendFile complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void appendFileSyncfunc()
    {
        string filename = "/myfile.txt";
        string append_data = "appendFile data";
        byte[] append_byteArray = new byte[] { 1, 2, 3, 4 };
        bool isAppendFileSync = QG.AppendFileSync(filename, append_byteArray, (success) =>
         {
             Debug.Log("QG.AppendFileSync success = " + JsonUtility.ToJson(success));
         },
         (fail) =>
         {
             Debug.Log("QG.AppendFileSync fail = " + JsonUtility.ToJson(fail));
         }
        );
        loginMessage.text = "QG.AppendFileSync 是否同步在文件結尾追加內容 = " + isAppendFileSync;
    }


    public void copyFilefunc()
    {
        string srcPath = "/myfile.txt";
        string destPath = "/myfileCopy.txt";
        QG.CopyFile(srcPath, destPath, (success) =>
      {
          Debug.Log("QG.CopyFile success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.CopyFile success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.CopyFile fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.CopyFile fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.CopyFile complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void copyFileSyncfunc()
    {
        string srcPath = "/myfile.txt";
        string destPath = "/myfileCopySync.txt";
        bool isCopyFileSync = QG.CopyFileSync(srcPath, destPath, (success) =>
          {
              Debug.Log("QG.CopyFileSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.CopyFileSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        loginMessage.text = "QG.CopyFileSync 是否同步拷貝目錄 = " + isCopyFileSync;
    }

    public void renamefunc()
    {
        string oldPath = "/myfile.txt";
        string newPath = "/new1/rename.txt";
        QG.Rename(oldPath, newPath, (success) =>
      {
          Debug.Log("QG.Rename success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.Rename success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.Rename fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.Rename fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.Rename complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void renameSyncfunc()
    {
        string oldPath = "/myfile.txt";
        string newPath = "/new2/renameSync.txt";
        bool isRenameSync = QG.RenameSync(oldPath, newPath, (success) =>
          {
              Debug.Log("QG.RenameSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.RenameSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        loginMessage.text = "QG.RenameSync 是否同步重命名文件 = " + isRenameSync;
    }

    public void saveFilefunc()
    {
        string filePath = "/myfile.txt";
        string tempFilePath = "/myfileCopy.txt";
        QG.SaveFile(tempFilePath, filePath, (success) =>
      {
          Debug.Log("QG.SaveFile success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.SaveFile success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.SaveFile fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.SaveFile fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.SaveFile complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void saveFileSyncfunc()
    {
        string filePath = "/myfile.txt";
        string tempFilePath = "/myfileCopySync.txt";
        string SaveFileSyncStr = QG.SaveFileSync(tempFilePath, filePath, (success) =>
          {
              Debug.Log("QG.SaveFileSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.SaveFileSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        loginMessage.text = "QG.SaveFileSync 同步保存臨時文件路徑 = " + SaveFileSyncStr;
    }

    public void removeSavedFilefunc()
    {
        string filePath = "/myfileCopy.txt";
        QG.RemoveSavedFile(filePath, (success) =>
      {
          Debug.Log("QG.RemoveSavedFile success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.RemoveSavedFile success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.RemoveSavedFile fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.RemoveSavedFile fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.RemoveSavedFile complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void unlinkfunc()
    {
        string dirPath = "/myfileCopy.txt";
        QG.Unlink(dirPath, (success) =>
      {
          Debug.Log("QG.Unlink success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.Unlink success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.Unlink fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.Unlink fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.Unlink complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void unlinkSyncfunc()
    {
        string dirPath = "/myfileCopySync.txt";
        bool isUnlinkSync = QG.UnlinkSync(dirPath, (success) =>
       {
           Debug.Log("QG.UnlinkSync success = " + JsonUtility.ToJson(success));
       },
       (fail) =>
       {
           Debug.Log("QG.UnlinkSync fail = " + JsonUtility.ToJson(fail));
       }
      );
        loginMessage.text = "QG.UnlinkSync 是否同步刪除文件 = " + isUnlinkSync;
    }

    public void mkdirfunc()
    {
        string dirPath = "/oppoNew";
        QG.Mkdir(dirPath, (success) =>
        {
            Debug.Log("QG.Mkdir success = " + JsonUtility.ToJson(success));
            loginMessage.text = "QG.Mkdir success = " + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.Mkdir fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "QG.Mkdir fail = " + JsonUtility.ToJson(fail);
        },
         (complete) =>
        {
            Debug.Log("QG.Mkdir complete = " + JsonUtility.ToJson(complete));
        }
       );
    }

    public void mkdirSyncfunc()
    {
        string dirPath = "/oppo2/mydirNew";
        bool recursive = true;
        bool isMkdirSync = QG.MkdirSync(dirPath, recursive, (success) =>
       {
           Debug.Log("QG.MkdirSync success = " + JsonUtility.ToJson(success));
       },
       (fail) =>
       {
           Debug.Log("QG.MkdirSync fail = " + JsonUtility.ToJson(fail));
       }
      );
        loginMessage.text = "QG.MkdirSync 是否同步創建目錄 = " + isMkdirSync;
    }

    public void rmdirfunc()
    {
        string dirPath = "/oppoNew";
        bool recursive = true;
        QG.Rmdir(dirPath, recursive, (success) =>
        {
            Debug.Log("QG.Rmdir success = " + JsonUtility.ToJson(success));
            loginMessage.text = "QG.Rmdir success = " + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.Rmdir fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "QG.Rmdir fail = " + JsonUtility.ToJson(fail);
        },
         (complete) =>
        {
            Debug.Log("QG.Rmdir complete = " + JsonUtility.ToJson(complete));
        }
       );
    }

    public void rmdirSyncfunc()
    {
        string dirPath = "/oppo2";
        bool recursive = true;
        bool isRmdirSync = QG.RmdirSync(dirPath, recursive, (success) =>
        {
            Debug.Log("QG.RmdirSync success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.RmdirSync fail = " + JsonUtility.ToJson(fail));
        }
       );
        loginMessage.text = "QG.RmdirSync 是否同步刪除目錄 = " + isRmdirSync;
    }

    public void readFilefunc()
    {
        QG.DownLoadFile(new DownLoadFileParam()
        {
            path = "/database2",
            url = "https://openfs.oppomobile.com/open/res/201907/31/5f27f86a3cc84b02a8baaf0a4a3066ab.png", //替换成自己的文件
        }, (success) =>
        {
            string filename = "/database2";
            string encoding = "binary"; //utf8 binary
            QG.ReadFile(filename, encoding, (readSuccess) =>
          {
              ReadFileResponse res = JsonUtility.FromJson<ReadFileResponse>(JsonUtility.ToJson(readSuccess));
              Debug.Log("QG.ReadFile success = " + JsonUtility.ToJson(readSuccess));
              if (res.encoding == "utf8")
              {
                  loginMessage.text = "QG.ReadFile success = " + JsonUtility.ToJson(readSuccess) + "\n >>>>>>>> \n encoding:" + res.encoding + "\n dataUtf8:" + res.dataUtf8;
              }
              else if (res.encoding == "binary")
              {
                  loginMessage.text = "QG.ReadFile success = \n encoding:" + res.encoding + "\n dataUtf8:" + res.dataUtf8 + "\n dataBytes[0]:" + res.dataBytes[0] + "\n dataBytes[end]:" + res.dataBytes[res.dataBytes.Length - 1] + "\n res.dataBytes.Length: " + res.dataBytes.Length;
              }
          },
          (fail) =>
          {
              Debug.Log("QG.ReadFile fail = " + JsonUtility.ToJson(fail));
              loginMessage.text = "QG.ReadFile fail = " + JsonUtility.ToJson(fail);
          },
           (complete) =>
          {
              Debug.Log("QG.ReadFile complete = " + JsonUtility.ToJson(complete));
          }
         );
        },
       (fail) =>
       {
           Debug.Log("QG.DownLoadFile fail = " + JsonUtility.ToJson(fail));
           loginMessage.text = "QG.DownLoadFile fail = " + JsonUtility.ToJson(fail);
       }
      );
    }

    public void readFileSyncfunc()
    {
        QG.DownLoadFile(new DownLoadFileParam()
        {
            path = "/database3",
            url = "https://openfs.oppomobile.com/open/res/201907/31/5f27f86a3cc84b02a8baaf0a4a3066ab.png",  //替换成自己的文件
        }, (success) =>
        {
            string filename = "/database3";
            string encoding = "binary"; //utf8 binary
            ReadFileResponse res = QG.ReadFileSync(filename, encoding, (readSuccess) =>
           {
               ReadFileResponse readFileResponse = JsonUtility.FromJson<ReadFileResponse>(JsonUtility.ToJson(readSuccess));
               if (readFileResponse.encoding == "utf8")
               {
                   Debug.Log("QG.ReadFileSync utf8 success = " + JsonUtility.ToJson(readSuccess));
               }
               else if (readFileResponse.encoding == "binary")
               {
                   Debug.Log("QG.ReadFileSync binary success = " + JsonUtility.ToJson(readSuccess));
               }
           },
           (fail) =>
           {
               Debug.Log("QG.ReadFileSync fail = " + JsonUtility.ToJson(fail));
           }
          );
            if (res == null)
            {
                loginMessage.text = "QG.ReadFileSync fail";
                return;
            }
            if (res.encoding == "utf8")
            {
                loginMessage.text = "QG.ReadFileSync success = \n >>>>>>>> \n encoding:" + res.encoding + "\n dataUtf8:" + res.dataUtf8;
            }
            else if (res.encoding == "binary")
            {
                loginMessage.text = "QG.ReadFileSync success =  \n >>>>>>>> \n encoding:" + res.encoding + "\n dataUtf8:" + res.dataUtf8 + "\n dataBytes[0]:" + res.dataBytes[0] + "\n dataBytes[end]:" + res.dataBytes[res.dataBytes.Length - 1] + "\n res.dataBytes.Length:" + res.dataBytes.Length;
            }
        },
        (fail) =>
        {
            Debug.Log("QG.DownLoadFile fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "QG.DownLoadFile fail = " + JsonUtility.ToJson(fail);
        }
       );
    }

    public void readdirfunc()
    {
        string dirPath = "/";
        QG.ReadDir(dirPath, (success) =>
      {
          ReadDirResponse res = JsonUtility.FromJson<ReadDirResponse>(JsonUtility.ToJson(success));
          Debug.Log("QG.ReadDir success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.ReadDir success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.ReadDir fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.ReadDir fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.ReadDir complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void readdirSyncfunc()
    {
        string dirPath = "/";
        ReadDirResponse res = QG.ReadDirSync(dirPath, (success) =>
          {
              ReadDirResponse readDirResponse = JsonUtility.FromJson<ReadDirResponse>(JsonUtility.ToJson(success));
              Debug.Log("QG.ReadDirSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.ReadDirSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        if (res == null)
        {
            loginMessage.text = "QG.ReadDirSync fail";
            return;
        }
        loginMessage.text = "QG.ReadDirSync success\n >>>>>>>> \n files[0]:" + res.files[0] + "\n files[end]:" + res.files[res.files.Length - 1];
    }

    public void writeFilefunc()
    {
        string filename = "/myfile.txt";
        string append_data = "writeFile data";
        bool append = false;
        byte[] append_byteArray = new byte[] { 1, 2, 3, 4 };
        QG.WriteFile(filename, append_byteArray, append, (success) =>
      {
          Debug.Log("QG.WriteFile success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.WriteFile success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.WriteFile fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.WriteFile fail = " + JsonUtility.ToJson(fail);
      },
       (complete) =>
      {
          Debug.Log("QG.WriteFile complete = " + JsonUtility.ToJson(complete));
      }
     );
    }

    public void writeFileSyncfunc()
    {
        string filename = "/myfile.txt";
        string append_data = "writeFileSync data";
        bool append = true;
        byte[] append_byteArray = new byte[] { 11, 22, 33, 44 };
        bool isWriteFileSync = QG.WriteFileSync(filename, append_byteArray, append, (success) =>
          {
              Debug.Log("QG.WriteFileSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.WriteFileSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        loginMessage.text = "QG.WriteFileSync 是否同步寫入文件 = " + isWriteFileSync;
    }
}
