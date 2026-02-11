
namespace EasyH.Unity.UI
{
    public interface IGUIFullScreen : IGUI
    {
        public void AddPopUp(IGUIPopUp popUp);
        public void ClosePopUp(IGUIPopUp popUp);
        public void AddPanel(IGUIPanel panel);
        public void ClosePanel();
    }
}