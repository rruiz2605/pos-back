using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS.Domain.Entities
{
    [Table("CLIENTE")]
    public class Client : BaseEntity
    {
        [Column("ID_CLIENTE"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override uint Id { get; set; }
        [Column("NOMBRE_COMPLETO")]
        public string FullName { get; set; } = string.Empty;
        [Column("NUMERO_CELULAR")]
        public string? CellphoneNumber { get; set; }
        [Column("NUMERO_CELULAR_BUSQUEDA")]
        public string? CellphoneNumberSearch { get; set; }
    }
}
