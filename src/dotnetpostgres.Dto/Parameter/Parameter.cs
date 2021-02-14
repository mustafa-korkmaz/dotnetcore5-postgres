
namespace dotnetpostgres.Dto.Parameter
{
    public class Parameter : DtoBase
    {
        /// <summary>
        /// parameterTypes.Id FK
        /// </summary>
        public int TypeId { get; set; }

        public virtual ParameterType Type { get; set; } //navigation

        public string Value { get; set; }

        public string Description { get; set; }

        public byte Order { get; set; }

        public bool IsSystem { get; set; }

        public bool IsDeleted { get; set; }
    }
}
