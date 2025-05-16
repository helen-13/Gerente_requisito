using System.ComponentModel.DataAnnotations;

namespace ServidoresAPI.Models;

public class Lotacao
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    public ICollection<Servidor> Servidores { get; set; }
} 