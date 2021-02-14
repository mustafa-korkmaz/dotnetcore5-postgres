using System.ComponentModel.DataAnnotations;

namespace dotnetpostgres.Dal.Entities
{
    public class Parameter : EntityBase
    {
        /// <summary>
        /// parameterTypes.Id FK
        /// </summary>
        [Required]
        public int TypeId { get; set; }

        public virtual ParameterType Type { get; set; } //navigation

        [Required]
        [MaxLength(100)]
        public string Value { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public byte Order { get; set; }

        [Required]
        public bool IsSystem { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
