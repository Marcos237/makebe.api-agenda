namespace api.makebe.agenda.domain.Entidades
{
    public class Endereco
    {
            public int Id { get; set; }                     
            public string? Logradouro { get; set; }          
            public int Numero { get; set; }                 
            public string? Complemento { get; set; }        
            public string? CEP { get; set; }                
            public string? Estado { get; set; }             
            public string? Cidade { get; set; }             
            public bool Status { get; set; }                
            public DateTime DataCadastro { get; set; }      
            public DateTime? DataAtualizacao { get; set; }  
    }
}
