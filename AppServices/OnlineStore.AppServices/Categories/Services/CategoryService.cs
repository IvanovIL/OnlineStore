
using AutoMapper;
using OnlineStore.AppServices.Categories.Repositories;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.Contracts.Categories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Events;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace OnlineStore.AppServices.Categories.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IEventAccumulator _eventContainer;
        private readonly IDataTimeProvider _dataTimeProvider;
        

        public CategoryService(ICategoryRepository repository,
            IMapper mapper,
            IEventAccumulator eventContainer,
             IDataTimeProvider dataTimeProvider,
             IProductRepository productRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _eventContainer = eventContainer;
            _dataTimeProvider = dataTimeProvider;
            _productRepository = productRepository;
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto, CancellationToken cancellation)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _eventContainer.AddEvent(new AddProductEvent
            {
                eventDate = _dataTimeProvider.UtcNow,
                productName = "Добавлена новая категория " + category.Name,
            });
            await _repository.AddAsync(category, cancellation);
        }

        public async Task<ProductsListDto> findCatregoryAsync(int CategoryId, PagedRequest request, CancellationToken cancellation)
        {

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var totalCount = await _productRepository.GetCategoryTotalCountAsync(CategoryId, cancellation);

            if (totalCount == 0)
            {
                return new ProductsListDto
                {
                    PageNumber = 1,
                    TotalCount = totalCount,
                    PageSize = 1,
                    Result = []
                };
            }
            var products = await _productRepository.findCategoryAsync(CategoryId, new GetProductsRequest
            {
                Take = request.PageSize,
                Skip = (request.PageNumber - 1) * request.PageSize,
                IncludeCategory = true
            }, cancellation);

            var productList = _mapper.Map<List<ShortProductDto>>(products);

            return new ProductsListDto
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                Result = productList
            };

        }
     


        public async Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellation)
        {
            var result = await _repository.GetAllAsync(cancellation);

            return _mapper.Map<IReadOnlyCollection<CategoryDto>>(result);
        }


    }
}
