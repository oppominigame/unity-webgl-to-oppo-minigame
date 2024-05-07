using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace QGMiniGame
{
    public class ShaderListener : MonoBehaviour
    {
        private List<GameObject> gameObjects;
        private List<string> shaderNames;
        private List<string> AssetsNames;
        private List<string> pageList;

        public Button startBtn;
        public Button endBtn;
        public Slider slider;
        public Text sliderText;
        public GameObject sliderObj;
        public Text errorShaderNum;
        public Text yeshu;
        public Text SceneTime;
        public Text ProgreeTime;

        public GameObject svc;
        public Text contentText;

        public Button previousBtn;
        public Button nextBtn;

        private string sliderNum;
        private int PageCount = 0;
        private int PageMaxCount = 0;

        private float timer = 0.0f;
        private bool isTimerRunning = false;
        private bool onceResourceSence = false;

        //---------------------------------------初始化组件-------------------------------------
        private void Awake()
        {
            endBtn.gameObject.SetActive(false);
            svc.SetActive(false);
            startBtn.gameObject.SetActive(true);
            errorShaderNum.gameObject.SetActive(false);
            gameObjects = new List<GameObject>();
            AssetsNames = new List<string>();
            // 启动计时器
            onceResourceSence = true;
            SceneTime.gameObject.SetActive(true);
            StartTimer();
        }

        private void OnEnable()
        {
            // 注册事件监听
            Camera.onPostRender += OnSceneRenderComplete;
        }

        private void OnSceneRenderComplete(Camera camera)
        {
            if (onceResourceSence)
            {
                StopTimer();
                Debug.Log("场景启动时间: " + timer);
                SceneTime.text = "场景启动时间: " + timer + "秒";
                timer = 0.0f;
                onceResourceSence = false;
            }
        }
        //---------------------------------------监听事件---------------------------------------
        private void Start()
        {
            startBtn.onClick.AddListener(StartClick);
            previousBtn.onClick.AddListener(previousClick);
            nextBtn.onClick.AddListener(nextClick);
            endBtn.onClick.AddListener(endClick);
            slider.value = 0;
            sliderNum = "0%";
            sliderText.text = sliderNum;
        }

        private void FixedUpdate()
        {
            if (isTimerRunning)
            {
                // 更新计时器
                timer += Time.deltaTime;

                // 打印当前计时器值
                ProgreeTime.text = "[" + timer.ToString("F2") + "s] 异常检测中...";
            }
        }
        //---------------------------------------点击事件---------------------------------------
        //开始测试
        private void StartClick()
        {
            StartTimer();
            SceneTime.gameObject.SetActive(false);
            AssetsNames = GetComponent<TempPrefabName>()._prefabNames;
            if (AssetsNames.Count <= 0)
            {
                showTip("没有可检测的资源", "none", 2000);
                endBtn.gameObject.SetActive(true);
                return;
            }
            startBtn.gameObject.SetActive(false);
            sliderObj.SetActive(true);
            StartCoroutine(LoadPrefabs());
        }

        //上一页
        private void previousClick()
        {
            RefreshSlidingText(false);
        }

        //下一页
        private void nextClick()
        {
            RefreshSlidingText(true);
        }

        private void endClick()
        {
            DefaultEntryNextScene();
        }

        //---------------------------------------点击逻辑---------------------------------------
        //加载检测资源
        IEnumerator LoadPrefabs()
        {
            //开启 console.log 监听
            QG.Log();
            // 初始化进度条
            slider.maxValue = AssetsNames.Count;
            for (int i = 0; i < AssetsNames.Count; i++)
            {
                // 实例化预制体
                UnityEngine.Object obj = Resources.Load(AssetsNames[i]);
                GameObject gameObject = Instantiate(obj) as GameObject;
                gameObject.transform.SetParent(this.transform, false);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.SetActive(true);
                gameObjects.Add(gameObject);
                if (i > 0)
                {
                    gameObjects[i - 1].SetActive(false);
                }
                if (i == AssetsNames.Count - 1)
                {
                    gameObjects[i].SetActive(false);
                }
                float ff = i;
                float fff = AssetsNames.Count;
                sliderNum = "加载资源: " + i + "/" + AssetsNames.Count + "  " + Math.Round(ff / fff, 2) * 100 + "%";
                // 更新进度条
                slider.value = i + 1;
                sliderText.text = sliderNum;
                // 等待一帧以减少CPU负担
                yield return null;
            }
            Resources.UnloadUnusedAssets();
            QG.LogClose();
            StopTimer();
            timer = 0.0f;
            isShowUI();
            StopCoroutine(LoadPrefabs());
        }

        //读取数据写入文本 
        IEnumerator GetAllShaderAssets()
        {
            errorShaderNum.gameObject.SetActive(true);
            errorShaderNum.text = "异常shader数量: 0";
            int shaderCount = 0;
            shaderNames = new List<string>();
            pageList = new List<string>();
            List<string> logMessage = QGMiniGameManager.Instance.LogMessage;
            //日志报错示例："ERROR: Shader Custom/GeometryStab shader is not supported" or "Custom/GeometryStab shader is not supported"
            Regex regex = new Regex(@"ERROR: Shader (.*?) shader is not supported|(.*?) shader is not supported");

            for (int i = 0; i < logMessage.Count; i++)
            {
                Debug.Log("捕捉到日志： " + logMessage[i]);
                Match match = regex.Match(logMessage[i]);

                if (match.Success)
                {
                    string extracted_string = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
                    Debug.Log("捕捉到ShaderError： " + extracted_string);
                    shaderNames.Add(extracted_string);
                }
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                StringBuilder tempSb = new StringBuilder();
                StringBuilder tempErrorSb = new StringBuilder();
                ShaderAssets shaderAssets = gameObjects[i].GetComponent<ShaderAssets>();
                bool siErrorShader = false;
                for (int j = 0; j < shaderAssets.shaderList.Count; j++)
                {
                    string shaderName = shaderAssets.shaderList[j].name;
                    if (shaderNames.Contains(shaderName))
                    {
                        shaderAssets.errorShaderData.Add(shaderName);
                        tempErrorSb.AppendLine("Shader: " + shaderName);
                        siErrorShader = true;
                        shaderCount++;
                    }
                }

                if (siErrorShader)
                {
                    if (shaderAssets.objectType == 1)
                    {
                        tempSb.AppendLine("场景");
                        tempSb.AppendLine("场景名: " + shaderAssets.sceneName);
                        tempSb.AppendLine("场景路径: " + shaderAssets.scenePath);
                    }
                    else if (shaderAssets.objectType == 2)
                    {
                        tempSb.AppendLine("预制");
                        tempSb.AppendLine("预制名: " + shaderAssets.prefabName);
                        tempSb.AppendLine("预制路径: " + shaderAssets.prefabPath);
                        tempSb.AppendLine("预制AssetBundle名: " + shaderAssets.prefabABName);
                    }
                    else if (shaderAssets.objectType == 3)
                    {
                        tempSb.AppendLine("AssetBundle");
                        tempSb.AppendLine("预制AssetBundle名: " + shaderAssets.prefabABName);
                        tempSb.AppendLine("预制名: " + shaderAssets.prefabName);
                    }
                    else if (shaderAssets.objectType == 4)
                    {
                        tempSb.AppendLine("天空盒");
                        tempSb.AppendLine("天空盒材质名: " + shaderAssets.materialList[0]);
                        tempSb.AppendLine("天空盒Shader路径: " + shaderAssets.shaderList[0]);
                    }
                    tempSb.AppendLine("异常shader: ");
                    tempSb.AppendLine(tempErrorSb.ToString());
                    tempSb.AppendLine("   ");
                    pageList.Add(tempSb.ToString());
                }

                float ff = i;
                float fff = gameObjects.Count;
                sliderNum = "检测异常: " + i + "/" + gameObjects.Count + "  " + Math.Round(ff / fff, 2) * 100 + "%";
                slider.value = i + 1;
                sliderText.text = sliderNum;
                errorShaderNum.text = "异常shader数量: " + shaderCount;
                yield return null;
            }
            if (shaderCount > 0)
            {
                showTip("发现异常Shader", "success", 2000);
                sliderObj.SetActive(false);
                svc.SetActive(true);
                StringBuilder contentTextSb = new StringBuilder();
                PageCount = 1;
                if (pageList.Count % 20 == 0)
                {
                    PageMaxCount = pageList.Count / 20;
                }
                else
                {
                    PageMaxCount = pageList.Count / 20 + 1;
                }

                for (global::System.Int32 i = 0; i < pageList.Count; i++)
                {
                    if (i <= PageCount * 20 && i >= ((PageCount - 1) * 20))
                    {
                        contentTextSb.AppendLine(pageList[i]);
                    }
                }
                yeshu.gameObject.SetActive(PageMaxCount > 1);
                yeshu.text = "(" + PageCount + "/" + PageMaxCount + ")";
                contentText.text = contentTextSb.ToString();
                nextBtn.gameObject.SetActive(PageMaxCount > 1);
                previousBtn.gameObject.SetActive(false);
            }
            else
            {
                showTip("没有异常的资源", "success", 2000);
                svc.SetActive(false);
                sliderObj.SetActive(false);
                endBtn.gameObject.SetActive(true);
            }
            StopTimer();
            timer = 0.0f;
            errorShaderNum.gameObject.SetActive(false);
            StopCoroutine(GetAllShaderAssets());
        }

        //刷新滑动文本
        private void RefreshSlidingText(bool bl)
        {
            PageCount = bl ? PageCount + 1 : PageCount - 1;
            nextBtn.gameObject.SetActive(PageCount < PageMaxCount);
            previousBtn.gameObject.SetActive(PageCount > 1);
            if (PageCount < 1 || PageCount > PageMaxCount)
            {
                return;
            }

            StringBuilder contentTextSb = new StringBuilder();
            for (global::System.Int32 i = 0; i < pageList.Count; i++)
            {
                if (i <= PageCount * 20 && i >= ((PageCount - 1) * 20))
                {
                    contentTextSb.AppendLine(pageList[i]);
                }
            }
            contentText.text = contentTextSb.ToString();
            yeshu.text = "(" + PageCount + "/" + PageMaxCount + ")";
        }

        private void isShowUI()
        {
            if (gameObjects.Count <= 0)
            {
                showTip("没有异常的资源", "success", 2000);
                svc.SetActive(false);
                sliderObj.SetActive(false);
                endBtn.gameObject.SetActive(true);
                return;
            }
            else
            {
                StartTimer();
                StartCoroutine(GetAllShaderAssets());
            }
        }

        //---------------------------------------工具---------------------------------------
        //提示框
        private void showTip(string tex, string type, int time)
        {
            QG.ShowToast(new ShowToastParam
            {
                title = tex,
                iconType = type,
                durationTime = time,
            });
        }

        //进入下个场景
        private void DefaultEntryNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
            string nextSceneName = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
            if (nextSceneName == string.Empty)
            {
                return;
            }
            SceneManager.LoadScene(nextSceneName);
        }

        //计时器
        public void StartTimer()
        {
            // 启动计时器
            timer = 0.0f;
            isTimerRunning = true;
        }

        public void StopTimer()
        {
            // 停止计时器
            isTimerRunning = false;
        }
    }
}