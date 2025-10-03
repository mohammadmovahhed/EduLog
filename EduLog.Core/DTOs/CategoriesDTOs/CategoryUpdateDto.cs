using System.ComponentModel.DataAnnotations;

namespace EduLog.CoreLayer.DTOs.CategoriesDTOs
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "آیدی دسته‌بندی الزامی است")]
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان دسته‌بندی الزامی است")]
        [StringLength(100, ErrorMessage = "عنوان نمی‌تواند بیش از 100 کاراکتر باشد")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Slug الزامی است")]
        [StringLength(100, ErrorMessage = "Slug نمی‌تواند بیش از 100 کاراکتر باشد")]
        [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "Slug فقط می‌تواند شامل حروف کوچک، اعداد و خط تیره باشد")]
        public string Slug { get; set; }

        [StringLength(200, ErrorMessage = "متا تگ نمی‌تواند بیش از 200 کاراکتر باشد")]
        public string MetaTag { get; set; }

        [StringLength(500, ErrorMessage = "توضیحات متا نمی‌تواند بیش از 500 کاراکتر باشد")]
        public string MetaDescription { get; set; }
    }
}