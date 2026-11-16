using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CadastroEventos.Models;

public class Evento : INotifyPropertyChanged
{
    private static readonly CultureInfo PtBR = new("pt-BR");

    private string _nome = string.Empty;
    private DateTime _dataInicio = DateTime.Today;
    private DateTime _dataTermino = DateTime.Today;
    private int _numeroParticipantes;
    private string _local = string.Empty;
    private decimal _custoPorParticipante;

    public string Nome
    {
        get => _nome;
        set { if (_nome == value) return; _nome = value; OnPropertyChanged(); }
    }

    public DateTime DataInicio
    {
        get => _dataInicio;
        set { if (_dataInicio == value) return; _dataInicio = value; OnPropertyChanged(); AtualizarCalculos(); }
    }

    public DateTime DataTermino
    {
        get => _dataTermino;
        set { if (_dataTermino == value) return; _dataTermino = value; OnPropertyChanged(); AtualizarCalculos(); }
    }

    public int NumeroParticipantes
    {
        get => _numeroParticipantes;
        set { if (_numeroParticipantes == value) return; _numeroParticipantes = value; OnPropertyChanged(); AtualizarCalculos(); }
    }

    public string Local
    {
        get => _local;
        set { if (_local == value) return; _local = value; OnPropertyChanged(); }
    }

    public decimal CustoPorParticipante
    {
        get => _custoPorParticipante;
        set { if (_custoPorParticipante == value) return; _custoPorParticipante = value; OnPropertyChanged(); AtualizarCalculos(); }
    }

    // ----- Propriedades calculadas -----

    /// <summary>Diferença entre as datas, calculada com DateTime e representada por um TimeSpan.</summary>
    public TimeSpan Duracao => DataTermino.Date - DataInicio.Date;

    /// <summary>Duração em dias, contando o dia de início e o de término (10/11 a 10/11 = 1 dia).</summary>
    public int DuracaoEmDias => Duracao < TimeSpan.Zero ? 0 : Duracao.Days + 1;

    public string DuracaoTexto => $"{DuracaoEmDias} dia(s)  (TimeSpan: {Duracao})";

    public decimal CustoTotal => NumeroParticipantes * CustoPorParticipante;

    public string CustoPorParticipanteFormatado => CustoPorParticipante.ToString("C", PtBR);

    public string CustoTotalFormatado => CustoTotal.ToString("C", PtBR);

    /// <summary>Retorna a mensagem de erro, ou null se os dados estiverem válidos.</summary>
    public string? Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome)) return "Informe o nome do evento.";
        if (string.IsNullOrWhiteSpace(Local)) return "Informe o local do evento.";
        if (DataTermino.Date < DataInicio.Date) return "A data de término não pode ser anterior à data de início.";
        if (NumeroParticipantes <= 0) return "O número de participantes deve ser maior que zero.";
        if (CustoPorParticipante < 0) return "O custo por participante não pode ser negativo.";
        return null;
    }

    private void AtualizarCalculos()
    {
        OnPropertyChanged(nameof(Duracao));
        OnPropertyChanged(nameof(DuracaoEmDias));
        OnPropertyChanged(nameof(DuracaoTexto));
        OnPropertyChanged(nameof(CustoTotal));
        OnPropertyChanged(nameof(CustoPorParticipanteFormatado));
        OnPropertyChanged(nameof(CustoTotalFormatado));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propriedade = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
}
