using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Domain.Entities
{
    public class BaseEntity
    {
        public virtual uint Id { get; set; }

        [Column("ESTADO_REGISTRO")]
        public string RecordStatus { get; set; }

        [Column("USUARIO_CREACION")]
        public string CreationUser { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime CreationDate { get; set; }

        [Column("TERMINAL_CREACION")]
        public string CreationTerminal { get; set; }

        [Column("USUARIO_MODIFICACION")]
        public string? ModificationUser { get; set; }

        [Column("FECHA_MODIFICACION")]
        public DateTime? ModificationDate { get; set; }

        [Column("TERMINAL_MODIFICACION")]
        public string? ModificationTerminal { get; set; }
    }
}
