﻿using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

public class Produto : IValidatableObject
{
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter entre 5 e 80 caracteres", MinimumLength = 5)]
    [FirstLetterBig]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
    public string? Descricao { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(8,2)")]
    [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if(primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult(
                    "A primeira letra do nome do produto deve ser maiúscula",
                    new[] { nameof(this.Nome) }
                    );
            }
        }

        if (this.Estoque <= 0)
        {
            yield return new ValidationResult(
                    "O estoque deve ser maior que zero",
                    new[] { nameof(this.Estoque) }
                    );
        }
    }
}
