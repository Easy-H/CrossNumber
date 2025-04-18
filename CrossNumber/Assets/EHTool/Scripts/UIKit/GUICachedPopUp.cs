namespace EHTool.UIKit {

    public class GUICachedPopUp : GUIWindow, IGUIPopUp {

        public override void Open()
        {
            base.Open();

            PopUpAction();
        }

        protected void PopUpAction()
        {
            if (UIManager.Instance.NowDisplay == null)
            {
                return;
            }

            UIManager.Instance.NowDisplay.AddPopUp(this);

        }

        public override void Close()
        {
            UIManager.Instance.NowDisplay?.ClosePopUp(this);
            base.Close();
        }
    }

}