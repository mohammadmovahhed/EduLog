using System.ComponentModel.DataAnnotations;

namespace EduLog.Web.Areas.AdminPanel.Models.Categories
{
    public class CategoryCreateViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} دسته‌بندی الزامی است")]
        [StringLength(100, ErrorMessage = "{0} نمی‌تواند بیش از 100 کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "{0} الزامی است")]
        [StringLength(100, ErrorMessage = "{0} نمی‌تواند بیش از 100 کاراکتر باشد")]
        [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} فقط می‌تواند شامل حروف کوچک، اعداد و خط تیره باشد")]
        public string Slug { get; set; }

        public string MetaTag { get; set; }

        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }

        public int? ParentId { get; set; }
    }
}