using System.ComponentModel.DataAnnotations;

namespace ServidoresAPI.Models;

public class Servidor
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [MaxLength(20)]
    public string Telefone { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    public int OrgaoId { get; set; }
    public Orgao Orgao { get; set; }

    [Required]
    public int LotacaoId { get; set; }
    public Lotacao Lotacao { get; set; }

    [MaxLength(20)]
    public string Sala { get; set; }

    public bool Ativo { get; set; } = true;
}