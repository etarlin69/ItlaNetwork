using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class SavePostViewModel : IValidatableObject
{
    public int Id { get; set; }

    [MaxLength(800)]
    public string? Content { get; set; }

    public IFormFile? ImageFile { get; set; }

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Content) && ImageFile == null && string.IsNullOrWhiteSpace(VideoUrl))
        {
            yield return new ValidationResult(
                "Debes escribir algo, subir una imagen o ingresar un video.",
                new[] { nameof(Content), nameof(ImageFile), nameof(VideoUrl) });
        }
    }
}
