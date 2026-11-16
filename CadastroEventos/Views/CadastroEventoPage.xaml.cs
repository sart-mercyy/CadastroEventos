using CadastroEventos.Models;

namespace CadastroEventos.Views;

public partial class CadastroEventoPage : ContentPage
{
    private readonly Evento _evento = new();

    public CadastroEventoPage()
    {
        InitializeComponent();
        BindingContext = _evento;
    }

    private async void OnCadastrarClicked(object? sender, EventArgs e)
    {
        string? erro = _evento.Validar();

        if (erro is not null)
        {
            await DisplayAlert("Dados inválidos", erro, "OK");
            return;
        }

        await Navigation.PushAsync(new ResumoEventoPage(_evento));
    }
}
