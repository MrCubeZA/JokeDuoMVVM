using ChuckNorrisJokesApp.Services;
using ChuckNorrisJokesApp.ViewModels;

namespace ChuckNorrisJokesApp.Views;

public partial class JokesView : ContentPage
{
	public JokesView(JokesViewModel viewModel)
	{

		InitializeComponent();

		BindingContext = viewModel;

		
	}
}