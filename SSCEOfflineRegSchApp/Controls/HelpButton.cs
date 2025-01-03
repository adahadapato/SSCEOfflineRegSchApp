
using System.Windows;
using System.Windows.Controls;

namespace SSCEOfflineRegSchApp
{
    public class HelpButton : CaptionButton
    {
        static HelpButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HelpButton), new FrameworkPropertyMetadata(typeof(HelpButton)));
        }

        protected override void OnClick()
        {
            base.OnClick();
            Window w = Window.GetWindow(this);
            if (w != null)
            {
               Grid grd=((MainWindow)System.Windows.Application.Current.MainWindow).contentGrid as Grid;
               Tools.SafeGuiWpf.AddUserControlToGrid<Pages.InstructionsPage>(grd);
            }
        }
    }
}
