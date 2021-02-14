namespace dotnetpostgres.Api.ViewModels.Parameter.Response
{
    public class ParameterViewModel
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Value { get; set; }
        public byte Order { get; set; }
    }
}
