using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Data.Repositories;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public OrdersController(IOrderRepository orderRepository, IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
        }


        [Authorize(Roles = "employee")]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetAllAsync();
            if (orders.Count() == 0)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [Authorize(Roles = "user")]
        [HttpGet("userid")]
        public async Task<IActionResult> GetByUserId()
        {
            var orders = await _orderService.GetByUserIdAsync();
            if (orders.Count() == 0)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post([FromBody] OrderAddDto orderAddDto)
        {
            if (orderAddDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _orderService.AddAsync(orderAddDto);
            return StatusCode(201);
        }


        [HttpPut]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> Put([FromBody] OrderUpdateDto orderUpdateDto)
        {
            if (orderUpdateDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _orderService.UpdateAsync(orderUpdateDto);

            return StatusCode(201);
        }

    }
}