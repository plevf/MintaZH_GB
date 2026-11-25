using System.Threading.Tasks;

namespace MintaZH_GB
{
    public partial class MainPage : ContentPage
    {
      
        public MainPage(MainPageViewModel VM)
        {
            InitializeComponent();
            BindingContext = VM;
            VM.LoadFromJson("students.json");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as MainPageViewModel).SaveToJson("students.json");
        }
    }
}
