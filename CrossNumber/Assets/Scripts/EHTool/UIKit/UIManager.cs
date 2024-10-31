using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EHTool.UIKit {

    public class UIManager : Singleton<UIManager> {

        class GUIData {
            internal string name;
            internal string path;

            internal void Read(XmlNode node)
            {
                name = node.Attributes["name"].Value;
                path = node.Attributes["path"].Value;
            }
        }
        
        public IGUIFullScreen NowDisplay { get; private set; }

        IDictionary<string, GUIData> _dic;
        IList<IGUIFullScreen> uiStack;

        private GUIMessageBox _msgBox;

        public void OpenFullScreen(IGUIFullScreen newData)
        {
            if (NowDisplay != null)
            {
                NowDisplay.SetOff();
            }

            uiStack.Add(newData);

            NowDisplay = newData;
            NowDisplay.SetOn();

        }

        public void CloseFullScreen(IGUIFullScreen closeFullScreen)
        {
            if (uiStack.Count < 1)
                return;

            uiStack.Remove(closeFullScreen);

            if (NowDisplay == closeFullScreen)
            {
                NowDisplay = uiStack[uiStack.Count - 1];
                NowDisplay.SetOn();
            }

        }

        protected override void OnCreate()
        {
            NowDisplay = null;
            uiStack = new List<IGUIFullScreen>();

            _dic = new Dictionary<string, GUIData>();
            XmlDocument xmlDoc = AssetOpener.ReadXML("GUIInfor");

            XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

            for (int i = 0; i < nodes.Count; i++)
            {
                GUIData guiData = new GUIData();
                guiData.Read(nodes[i]);

                _dic.Add(guiData.name, guiData);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            NowDisplay = null;
            uiStack = new List<IGUIFullScreen>();

        }

        public T OpenGUI<T>(string guiName) where T : Component, IGUI
        {

            string path = Instance._dic[guiName].path;

            GameObject retGO = AssetOpener.ImportGameObject(path);
            retGO.GetComponent<IGUI>().Open();

            return retGO.GetComponent<T>();
        }

        public void DisplayMessage(string messageContent)
        {
            if (_msgBox == null)
            {
                _msgBox = OpenGUI<GUIMessageBox>("MessageBox");
            }

            _msgBox.SetMessage(messageContent);
        }

    }
}
