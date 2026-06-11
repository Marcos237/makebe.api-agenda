namespace PesquisarVitrineEvent
{
    public class PesquisarVitrineResponse : IPesquisarVitrineResponse
    {
        public List<ItemVitrineResponse> Itens { get; set; } = [];
    }
}
