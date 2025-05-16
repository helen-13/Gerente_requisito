namespace ServidoresAPI.DTOs;

public class ServidorDto
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int OrgaoId { get; set; }
    public int LotacaoId { get; set; }
    public string Sala { get; set; }
}

public class ServidorResponseDto : ServidorDto
{
    public int Id { get; set; }
    public bool Ativo { get; set; }
    public string OrgaoNome { get; set; }
    public string LotacaoNome { get; set; }
}