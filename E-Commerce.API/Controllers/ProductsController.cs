﻿using System.Net;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
        public async Task<ActionResult<CommonResponse<GetProductDTO>>> AddProduct(AddProductDTO product)
        {
            var response = new CommonResponse<GetProductDTO>();
            var result = await _productService.AddProduct(product);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommonResponse<GetProductDTO>>> GetProductById(int id)
        {
            var response = new CommonResponse<GetProductDTO>();
            var result = await _productService.GetProductById(id);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetProductDTO>>>> GetAllProducts()
        {
            var response = new CommonResponse<IEnumerable<GetProductDTO>>();
            var result = await _productService.GetAllProducts();
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetProductDTO>>>> SearchProducts(string name)
        {
            var response = new CommonResponse<IEnumerable<GetProductDTO>>();
            var result = await _productService.SearchProducts(name);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet("filter-by-category")]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetProductDTO>>>> GetProductsByCategory(int categoryId)
        {
            var response = new CommonResponse<IEnumerable<GetProductDTO>>();
            var result = await _productService.GetProductsByCategory(categoryId);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet("filter-by-price")]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetProductDTO>>>> FilterByPriceRange(double from, double to)
        {
            var response = new CommonResponse<IEnumerable<GetProductDTO>>();
            var result = await _productService.FilterByPriceRange(from, to);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
        public async Task<ActionResult<CommonResponse<string>>> DeleteProduct(int id)
        {
            var response = new CommonResponse<string>();
            var result = await _productService.DeleteProduct(id);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = "Product Deleted Successfully";
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
            
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]

        public async Task<ActionResult<CommonResponse<GetProductDTO>>> UpdateProduct(int id,UpdateProductDTO product)
        {
            var response = new CommonResponse<GetProductDTO>();
            product.Id = id;
            var result = await _productService.UpdateProduct(product);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

    }
}