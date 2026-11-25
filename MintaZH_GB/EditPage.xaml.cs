namespace MintaZH_GB;

public partial class EditPage : ContentPage
{
	public EditPage(EditPageViewModel VM)
	{
		InitializeComponent();
		BindingContext=VM;
	}
}