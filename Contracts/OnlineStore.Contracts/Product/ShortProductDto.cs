using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Contracts.Product
{
    /// <summary>
    /// Краткая информация о товаре
    /// </summary>
    public sealed class ShortProductDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [HiddenInput(DisplayValue=false)]
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Display(Name = "Название")]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        /// <summary>
        /// Доступное количество
        /// </summary>
        [Display(Name = "Количество")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Главное изображение
        /// </summary>
        [HiddenInput(DisplayValue=false)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Список изображений
        /// </summary>
        public string[] ImagesUrls { get; set; } = [];

        /// <summary>
        /// Признак удаление товара
        /// </summary>
        public bool IsDeleted { get; set; }


    }
}
