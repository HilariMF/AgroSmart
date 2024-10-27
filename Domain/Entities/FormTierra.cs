using Domain.Common;

namespace Domain.Entities
{
    public class FormTierra : BaseEntity
    {
        public string TipoDeSuelo { get; set; }
        public string HumedadTerreno { get; set; }
        public string Drenaje { get; set; }
        public string LuzSolar { get; set; }
        public string TipoDeRiego { get; set; }
        public string TipoDeFertilizacion { get; set; }
        public bool ProblemasDePlagas { get; set; }
        public string TamanoTerreno { get; set; }
        public string UsuarioId { get; set; }  // Relacionado con el usuario registrado
    }
}
