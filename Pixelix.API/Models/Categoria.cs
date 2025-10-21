using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pixelix.API.Models;

[Table("Categoria")]
public class Categoria
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Por favor, informe o Nome")]
    [StringLength(50, ErrorMessage = "O Nome deve possuir no máximo 50 carácteres")]
    public string Nome { get; set; }

    [StringLength(200)]
    public string Foto { get; set; }

    // colocar cor na categoria

    [StringLength(20, ErrorMessage = "A cor deve ter no máximo 20 caracteres")]
    public string Cor { get; set; }
}
