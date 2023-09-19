namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel()
            {
                Expression = "0",
                Result = 0
            };
            foreach (IView button in buttonGrid.Children)
            {
                var buttonRefernce = (Button)button;
                buttonRefernce.Command = ((MainViewModel)BindingContext).ButtonClick;
                buttonRefernce.CommandParameter = buttonRefernce.Text;
            }
        }
    }
}